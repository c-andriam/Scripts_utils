# Projet Inception — Guide complet pas à pas avec explications

Ce guide vous accompagne étape par étape pour réaliser le projet Inception, en explicitant les exigences du sujet et ce qu’il faut réellement faire à chaque étape.

---

## 1. Préparer l’environnement

### **Exigences du sujet**
- Tout doit être fait dans une machine virtuelle (VM).
- Objectif : garantir l’isolation de votre projet et sa reproductibilité.

### **Ce qu’il faut faire**
- Installez une VM (VirtualBox, VMware, QEMU, etc.) sur votre machine physique.
- Privilégiez une distribution Linux légère, par exemple Ubuntu Server, Debian ou Alpine.
- Toutes les commandes et installations seront faites _uniquement_ dans cette VM.

---

## 2. Organiser l’arborescence des fichiers

### **Exigences du sujet**
- Tous les fichiers de configuration doivent être dans un dossier `srcs/`.
- Un dossier `secrets/` pour stocker mots de passe, credentials, etc.
- Un fichier `Makefile` à la racine pour automatiser le build et le lancement.
- Respecter une structure de projet précise (voir exemple de structure dans le sujet).

### **Ce qu’il faut faire**
- Créez les dossiers et fichiers suivants :
    ```
    .
    ├── Makefile
    ├── secrets/
    │   ├── credentials.txt
    │   ├── db_password.txt
    │   └── db_root_password.txt
    └── srcs/
        ├── .env
        ├── docker-compose.yml
        └── requirements/
            ├── nginx/
            │   ├── Dockerfile
            │   ├── conf/
            │   └── tools/
            ├── wordpress/
            │   ├── Dockerfile
            │   ├── conf/
            │   └── tools/
            └── mariadb/
                ├── Dockerfile
                ├── conf/
                └── tools/
    ```
- Mettez les informations sensibles dans `secrets/` et ignorez ce dossier dans `.gitignore`.
- Le dossier `srcs/requirements/` contiendra un sous-dossier par service (NGINX, WordPress, MariaDB, etc.), chacun avec son propre Dockerfile et configurations associées.
- `srcs/.env` contiendra les variables d’environnement à injecter dans vos containers.

---

## 3. Écrire et configurer les Dockerfiles

### **Exigences du sujet**
- Chaque service doit avoir un Dockerfile _écrit par vous_.
- Basé sur l’avant-dernière version stable de Debian ou Alpine.
- Interdiction d’utiliser des images toutes faites pour les services autres qu’Alpine/Debian.
- Pas de mot de passe dans le Dockerfile.

### **Ce qu’il faut faire**
- Pour chaque service (nginx, wordpress, mariadb), créez un Dockerfile dans son sous-dossier.
- Utilisez `FROM alpine:3.xx` ou `FROM debian:xx` (avant-dernière version stable).
- Installez et configurez chaque logiciel dans le Dockerfile (ex : NGINX, PHP-FPM, MariaDB).
- Utilisez des variables d’environnement pour injecter la configuration sensible.
- Ne stockez jamais de mots de passe ou credentials dans le Dockerfile.

---

## 4. Rédiger le docker-compose.yml

### **Exigences du sujet**
- Tous les services doivent être orchestrés via docker-compose.
- Chaque image Docker doit porter le nom du service.
- Chaque service tourne dans un container dédié.
- Un réseau Docker personnalisé doit connecter les containers.
- Les containers doivent redémarrer automatiquement en cas de crash (`restart: always`).

### **Ce qu’il faut faire**
- Créez `srcs/docker-compose.yml` qui va décrire :
  - Les trois services : nginx, wordpress, mariadb.
  - Les volumes pour la base de données et les fichiers WordPress.
  - Le network Docker personnalisé.
  - Les variables d’environnement à injecter (possiblement via `.env`).
  - Le mapping des volumes sur `/home/<login>/data` de la VM.
  - Le restart automatique.
- **Exemple simplifié :**
    ```yaml
    version: '3.8'
    services:
      nginx:
        build: ./requirements/nginx
        image: nginx
        ports:
          - "443:443"
        volumes:
          - /home/<login>/data/wordpress:/var/www/html
        networks:
          - inception
        restart: always
        env_file: .env
      wordpress:
        build: ./requirements/wordpress
        image: wordpress
        volumes:
          - /home/<login>/data/wordpress:/var/www/html
        networks:
          - inception
        restart: always
        env_file: .env
      mariadb:
        build: ./requirements/mariadb
        image: mariadb
        volumes:
          - /home/<login>/data/mariadb:/var/lib/mysql
        networks:
          - inception
        restart: always
        env_file: .env
    networks:
      inception:
        driver: bridge
    ```

---

## 5. Configurer chaque service en détail

### **NGINX**
- **But du sujet :** Unique point d’entrée, reverse proxy, écoute seulement sur 443 avec TLSv1.2 ou 1.3.
- **À faire :**
  - Générer un certificat SSL (auto-signé ou via Let’s Encrypt).
  - Configurer NGINX pour accepter uniquement TLSv1.2/1.3.
  - Servir comme reverse proxy pour WordPress (PHP-FPM).
  - Pas d’accès HTTP (80 fermé).
  - Rediriger `login.42.fr` vers WordPress.
  - NGINX ne doit contenir que la configuration web, pas de gestion de base de données ni PHP.

### **WordPress + PHP-FPM**
- **But du sujet :** Fournir le site WordPress, exécution PHP via FPM, pas de NGINX ici.
- **À faire :**
  - Installer WordPress, PHP-FPM et extensions nécessaires dans le Dockerfile.
  - Configurer le fichier `wp-config.php` pour utiliser les variables d’environnement (DB_HOST, DB_USER, DB_PASSWORD, etc.).
  - Monter le volume `/var/www/html` pour la persistance des fichiers WordPress.

### **MariaDB**
- **But du sujet :** Fournir la base de données à WordPress, pas de NGINX ici.
- **À faire :**
  - Installer MariaDB dans le Dockerfile.
  - Démarrer le service MariaDB.
  - Créer la base de données et deux utilisateurs (un admin, un user) via un script d’initialisation.
  - L’admin **ne doit pas** avoir un login contenant "admin", "administrator", etc.
  - Monter le volume `/var/lib/mysql` pour la persistance.

---

## 6. Configurer la persistance des données

### **Exigences du sujet**
- Deux volumes : un pour la DB (MariaDB), un pour les fichiers WordPress.
- Ces volumes doivent être mappés sur `/home/<login>/data` de la machine hôte.

### **Ce qu’il faut faire**
- Dans `docker-compose.yml`, déclarez :
    - `/home/<login>/data/mariadb` <-> `/var/lib/mysql` (MariaDB)
    - `/home/<login>/data/wordpress` <-> `/var/www/html` (WordPress)
- Créez ces dossiers sur la VM si besoin.

---

## 7. Configurer le réseau Docker

### **Exigences du sujet**
- Utiliser un réseau Docker personnalisé.
- Interdiction d’utiliser `network: host`, `--link` ou `links:`.
- La ligne `networks:` doit être présente dans `docker-compose.yml`.

### **Ce qu’il faut faire**
- Dans le `docker-compose.yml`, déclarez un network (ex : `inception`).
- Ajoutez chaque service à ce network.
- Utilisez les noms de services comme hostname dans les variables d’environnement (ex : DB_HOST=mariadb).

---

## 8. Sécuriser l’infrastructure

### **Exigences du sujet**
- Aucun mot de passe ne doit être visible dans les Dockerfiles.
- Utilisation des variables d’environnement obligatoire.
- Stockage des variables sensibles dans `.env` et secrets dans `secrets/`.
- Le fichier `.env` doit être ignoré par git.

### **Ce qu’il faut faire**
- Placez toutes les variables sensibles dans le fichier `.env` (ex : DB_PASSWORD, DOMAIN_NAME, etc.).
- Placez les secrets (passwords, credentials) dans des fichiers texte dans `secrets/`.
- Ajoutez `.env` et `secrets/` dans `.gitignore`.
- Injectez les variables d’environnement dans vos containers via le paramètre `env_file: .env` dans `docker-compose.yml` ou via la directive `ENV` dans vos Dockerfiles (sans jamais y mettre de valeurs sensibles).
- Pour encore plus de sécurité, renseignez-vous sur [Docker secrets](https://docs.docker.com/engine/swarm/secrets/) (optionnel mais recommandé).

---

## 9. Configurer le nom de domaine

### **Exigences du sujet**
- Le nom de domaine utilisé sera `<login>.42.fr` (exemple : `c-andriam.42.fr`).
- Il doit pointer vers l’adresse IP locale de la VM.

### **Ce qu’il faut faire**
- Ajoutez la ligne suivante dans `/etc/hosts` (sur la VM et/ou votre poste local) :
    ```
    127.0.0.1    c-andriam.42.fr
    ```
- Utilisez la variable d’environnement DOMAIN_NAME pour injecter ce nom dans vos templates de configuration (nginx notamment).

---

## 10. Respecter les contraintes et bonnes pratiques

### **À faire absolument**
- **Interdictions :**
  - Utiliser le tag `latest` pour les images Docker.
  - Mettre des mots de passe ou credentials dans les Dockerfiles ou dans le dépôt git.
  - Utiliser des "hacky patch" (ex : `tail -f`, `sleep infinity`, `while true`, etc.) pour garder les containers vivants.
- **Obligations :**
  - Utiliser les bonnes pratiques d’écriture de Dockerfile (PID 1, CMD approprié, pas de process zombie).
  - Chaque service a son propre Dockerfile.
  - Les containers doivent redémarrer automatiquement (`restart: always`).

---

## 11. Rédiger le Makefile (Automatisation)

### **Exigences du sujet**
- Le Makefile doit permettre de builder et lancer toute l’application (via docker-compose).

### **Ce qu’il faut faire**
- Un Makefile minimal :
    ```makefile
    all:
    	docker-compose -f srcs/docker-compose.yml up --build -d

    down:
    	docker-compose -f srcs/docker-compose.yml down

    clean:
    	docker system prune -af
    ```

---

## 12. Tester l’infrastructure

### **Ce qu’il faut vérifier**
- Accès au site WordPress via `https://<login>.42.fr`.
- Administration WordPress fonctionnelle, base MariaDB bien reliée.
- Persistance des données après arrêt/redémarrage des containers.
- Les containers redémarrent seuls en cas de crash.
- Aucun mot de passe n’est visible dans les Dockerfiles, ni dans le git.
- Respect strict de la structure de fichiers attendue.

---

## 13. (Bonus) Ajouter des services supplémentaires

### **Exigences du sujet**
- Chaque bonus doit être dans son propre Dockerfile et son propre conteneur.
- Les bonus ne sont pris en compte **que si la partie obligatoire est parfaite**.

### **Exemples de bonus**
- Redis cache pour WordPress.
- Serveur FTP pointant vers le volume WordPress.
- Petit site statique (autre langage que PHP).
- Adminer.
- Autre service utile (à justifier lors de la soutenance).

---

## 14. Exemples d’arborescence attendue

```
.
├── Makefile
├── secrets/
│   ├── credentials.txt
│   ├── db_password.txt
│   └── db_root_password.txt
└── srcs/
    ├── .env
    ├── docker-compose.yml
    └── requirements/
        ├── nginx/
        │   ├── Dockerfile
        │   ├── conf/
        │   └── tools/
        ├── wordpress/
        │   ├── Dockerfile
        │   ├── conf/
        │   └── tools/
        └── mariadb/
            ├── Dockerfile
            ├── conf/
            └── tools/
```

Les volumes de données doivent être montés sur `/home/<login>/data` sur votre VM.

---

## 15. Récapitulatif des grandes étapes à réaliser

1. **Préparer la VM**
2. **Créer la structure de dossiers et fichiers**
3. **Écrire tous les Dockerfiles nécessaires**
4. **Rédiger le docker-compose.yml**
5. **Configurer chaque service (nginx, wordpress, mariadb)**
6. **Gérer la persistance des données (volumes)**
7. **Configurer le réseau Docker**
8. **Sécuriser l’infrastructure (variables, secrets, .env, etc.)**
9. **Configurer le nom de domaine local**
10. **Respecter toutes les contraintes du sujet**
11. **Rédiger un Makefile d’automatisation**
12. **Tester l’ensemble de l’infrastructure**
13. **Bonus : ajouter d’autres services si tout le reste est parfait**

---

**Besoin d’un exemple, d’un template ou d’une explication sur une étape précise ? N’hésitez pas à demander !**