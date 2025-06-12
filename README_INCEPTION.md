# Inception : Guide Complet et Documentation

Ce projet de l'école 42, nommé **Inception**, a pour objectif principal de vous initier à l'administration système et à la virtualisation de services en utilisant Docker et Docker Compose. À travers ce guide, vous apprendrez à créer, configurer et orchestrer plusieurs conteneurs Docker (Nginx, WordPress, MariaDB, et d'autres en bonus) au sein d'une machine virtuelle. Ce document sert à la fois de tutoriel pas à pas et de documentation de référence pour le projet.

**Objectifs d'apprentissage :**
*   Maîtrise des concepts fondamentaux de Docker : images, conteneurs, Dockerfiles, volumes, réseaux.
*   Utilisation de Docker Compose pour l'orchestration d'applications multi-conteneurs.
*   Configuration d'un serveur web Nginx, incluant SSL/TLS.
*   Déploiement d'un site WordPress avec une base de données MariaDB.
*   Compréhension des interactions entre les services conteneurisés.
*   Notions de sécurité et de bonnes pratiques en matière de conteneurisation.

## 1. Prérequis et Conseils Importants Avant de Commencer

Avant de plonger dans le projet, veuillez lire attentivement ces conseils :

1.  **Procédez Étape par Étape :** N'essayez pas de configurer tous les conteneurs (Nginx, WordPress, MariaDB) simultanément. Vous risqueriez de vous perdre et de ne pas bien assimiler leur fonctionnement. Suivez l'ordre suggéré : Nginx d'abord, puis WordPress, et enfin MariaDB.
2.  **Compréhension Individuelle des Services :** Assurez-vous de comprendre le rôle de chaque service avant de les combiner.
    *   **Commencez par Nginx :** Affichez une simple page `index.html`.
        *   Apprenez d'abord comment construire une image Docker et l'exécuter **sans utiliser `docker-compose`**.
        *   Servez une page HTML simple via HTTP (`http://localhost:80` ou un autre port).
        *   Mettez en place SSL/TLS pour servir la même page via HTTPS (`https://votredomaine.42.fr`).
    *   **Poursuivez avec WordPress et MariaDB :** C'est à ce stade que `docker-compose` deviendra réellement indispensable pour gérer les dépendances et la configuration de l'ensemble.
3.  **Bases en Ligne de Commande :** Une familiarité avec le terminal Linux et les commandes de base est fortement recommandée.
4.  **Notions de Réseau :** Comprendre les concepts de base comme les adresses IP, les ports, et le DNS vous sera utile.
5.  **Sauvegardez Régulièrement :** Surtout vos fichiers de configuration et Dockerfiles.
6.  **Lisez les Logs :** Les logs sont vos meilleurs amis pour le débogage (`docker logs <container_id>`, `docker-compose logs <service_name>`).

*   Les deux dépôts GitHub qui m'ont beaucoup aidé pour le projet (expérience de vbachele) : [llescure/42_Inception](https://github.com/llescure/42_Inception) et [42cursus/inception par malatini](https://github.com/42cursus/inception).
*   Ce dépôt GitHub qui m'a aidé pour les bonus (expérience de vbachele) : [twagger/inception](https://github.com/twagger/inception).

Si vous avez des questions : n'hésitez pas à me contacter, je serai ravi de vous répondre ! Mon nom d'utilisateur Discord : vbachele#7949

## 2. Sommaire Détaillé

1.  [Prérequis et Conseils Importants](#1-prérequis-et-conseils-importants-avant-de-commencer)
2.  [Sommaire Détaillé](#2-sommaire-détaillé)
3.  [Comprendre les Concepts Clés de la Conteneurisation](#3-comprendre-les-concepts-clés-de-la-conteneurisation)
    *   [Docker : Images, Conteneurs, Moteur et CLI](#docker--images-conteneurs-moteur-et-cli)
    *   [Dockerfile : Recettes pour vos Images](#dockerfile--recettes-pour-vos-images)
    *   [Docker Compose : Orchestration Simplifiée](#docker-compose--orchestration-simplifiée)
    *   [Volumes Docker : Persistance des Données](#volumes-docker--persistance-des-données)
    *   [Réseaux Docker : Communication Isolée](#réseaux-docker--communication-isolée)
    *   [Variables d'Environnement : Configuration Flexible](#variables-denvironnement--configuration-flexible)
    *   [SSL/TLS et HTTPS : Sécuriser les Communications](#ssltls-et-https--sécuriser-les-communications)
    *   [Fichier `.env` : Gérer les Configurations Sensibles](#fichier-env--gérer-les-configurations-sensibles)
4.  [Installation de Docker et Docker Compose](#4-installation-de-docker-et-docker-compose)
    *   [Sur macOS](#sur-macos)
    *   [Sur Linux](#sur-linux)
    *   [Sur Windows](#sur-windows)
5.  [Commandes Essentielles Docker et Docker Compose](#5-commandes-essentielles-docker-et-docker-compose)
    *   [Gestion des Conteneurs Docker](#gestion-des-conteneurs-docker)
    *   [Gestion des Images Docker](#gestion-des-images-docker)
    *   [Gestion des Volumes Docker](#gestion-des-volumes-docker)
    *   [Gestion des Réseaux Docker](#gestion-des-réseaux-docker)
    *   [Commandes Docker Compose](#commandes-docker-compose)
6.  [Le Projet Inception : Construction Pas à Pas](#6-le-projet-inception--construction-pas-à-pas)
    *   [Structure des Fichiers du Projet](#structure-des-fichiers-du-projet)
    *   [NGINX : Serveur Web et Reverse Proxy](#nginx--serveur-web-et-reverse-proxy)
    *   [MARIADB : Base de Données Robuste](#mariadb--base-de-données-robuste)
    *   [WORDPRESS : Système de Gestion de Contenu](#wordpress--système-de-gestion-de-contenu)
    *   [DOCKER COMPOSE : Assemblage Final](#docker-compose--assemblage-final)
7.  [Les Bonus : Étendre les Fonctionnalités](#7-les-bonus--étendre-les-fonctionnalités)
    *   [REDIS : Cache d'Objets Performant](#redis--cache-dobjets-performant)
    *   [VSFTPD : Serveur FTP Sécurisé](#vsftpd--serveur-ftp-sécurisé)
    *   [ADMINER : Interface Web pour MariaDB](#adminer--interface-web-pour-mariadb)
    *   [HUGO : Site Statique Personnalisé](#hugo--site-statique-personnalisé)
    *   [Portfolio : Page Web Statique Avancée](#portfolio--page-web-statique-avancée)
8.  [Considérations de Sécurité](#8-considérations-de-sécurité)
9.  [Dépannage et Logs](#9-dépannage-et-logs)
10. [Conclusion](#10-conclusion)

## 3. Comprendre les Concepts Clés de la Conteneurisation

### Docker : Images, Conteneurs, Moteur et CLI

*   **Docker :** Docker est une plateforme ouverte qui automatise le déploiement, la mise à l'échelle et la gestion d'applications en les empaquetant dans des **conteneurs**.
*   **Image Docker :** Une image est un modèle léger, autonome et exécutable qui inclut tout ce dont une application a besoin pour fonctionner : le code, les environnements d'exécution, les outils système, les bibliothèques et les paramètres. Les images sont souvent basées sur d'autres images (concept d'héritage) et sont construites à partir d'un `Dockerfile`. Elles sont immuables : une fois construites, elles ne changent pas.
*   **Conteneur Docker :** Un conteneur est une instance en cours d'exécution d'une image Docker. Vous pouvez créer, démarrer, arrêter, déplacer ou supprimer des conteneurs. Chaque conteneur est isolé des autres et de la machine hôte, bien qu'il partage le noyau du système d'exploitation de l'hôte. Cette isolation garantit que l'application s'exécute de manière cohérente, quel que soit l'environnement.
*   **Moteur Docker (Docker Engine) :** C'est l'application client-serveur sous-jacente qui construit et exécute les conteneurs. Il se compose d'un démon (le `dockerd` process) qui gère les objets Docker (images, conteneurs, réseaux, volumes) et d'une API REST que les clients peuvent utiliser pour interagir avec le démon.
*   **CLI Docker (Docker Command Line Interface) :** C'est l'outil principal (`docker`) que vous utilisez pour interagir avec le démon Docker via des commandes en ligne (ex: `docker run`, `docker build`).

### Dockerfile : Recettes pour vos Images

Un `Dockerfile` est un fichier texte qui contient une série d'instructions décrivant comment construire une image Docker spécifique. Docker lit ces instructions séquentiellement pour assembler l'image.

*   **Couches (Layers) :** Chaque instruction dans un Dockerfile crée une nouvelle "couche" dans l'image. Ces couches sont empilées les unes sur les autres. Si une instruction change, seules les couches suivantes sont reconstruites, ce qui rend les builds plus rapides grâce à un système de cache.
*   **Cache de Build :** Docker met en cache chaque couche. Si votre Dockerfile n'a pas changé, ou si les changements n'affectent que les dernières couches, la reconstruction de l'image peut être très rapide.
*   **Exemple de structure simple d'un Dockerfile :**
    ```dockerfile
    # 1. Choisir une image de base
    FROM debian:buster-slim

    # 2. Définir des variables d'environnement (optionnel)
    ENV APP_USER=monapp \
        APP_HOME=/opt/monapp

    # 3. Installer des dépendances
    RUN apt-get update && \
        apt-get install -y nginx curl && \
        rm -rf /var/lib/apt/lists/*

    # 4. Créer un utilisateur et un répertoire pour l'application (bonnes pratiques)
    RUN useradd -ms /bin/bash ${APP_USER} && \
        mkdir -p ${APP_HOME}

    # 5. Copier les fichiers de l'application dans l'image
    COPY ./mon_app_sources/ ${APP_HOME}/

    # 6. Définir le propriétaire des fichiers (bonnes pratiques)
    RUN chown -R ${APP_USER}:${APP_USER} ${APP_HOME}

    # 7. Définir le répertoire de travail
    WORKDIR ${APP_HOME}

    # 8. Changer d'utilisateur (bonnes pratiques, si possible)
    USER ${APP_USER}

    # 9. Exposer un port (informatif, ne publie pas le port sur l'hôte)
    EXPOSE 8080

    # 10. Définir la commande à exécuter au démarrage du conteneur
    CMD ["nginx", "-g", "daemon off;"]
    ```
*   **Bonnes Pratiques pour les Dockerfiles :**
    *   Utiliser un fichier `.dockerignore` pour exclure les fichiers inutiles du contexte de build.
    *   Minimiser le nombre de couches en combinant les commandes `RUN` (ex: `apt-get update && apt-get install`).
    *   Nettoyer les caches d'installation (ex: `rm -rf /var/lib/apt/lists/*`).
    *   Utiliser des images de base officielles et minimales (ex: `alpine`).
    *   Construire des images en plusieurs étapes (multi-stage builds) pour réduire la taille finale de l'image.
    *   Ne pas exécuter les processus en tant que `root` dans le conteneur si possible.

### Docker Compose : Orchestration Simplifiée

Docker Compose est un outil permettant de définir et d'exécuter des applications Docker multi-conteneurs.

*   **Orchestration :** Il simplifie la gestion de plusieurs conteneurs qui doivent fonctionner ensemble (comme un serveur web, une application et une base de données).
*   **Fichier `docker-compose.yml` :** Vous utilisez un fichier de configuration au format YAML (par défaut `docker-compose.yml`) pour décrire tous les services de votre application, ainsi que leurs dépendances, réseaux, volumes et configurations.
    *   **`version` :** Spécifie la version de la syntaxe Docker Compose (ex: `'3.8'`, `'3.9'`).
    *   **`services` :** Définit chaque conteneur de votre application (ex: `nginx`, `wordpress`, `mariadb`).
        *   `image` : Nom de l'image Docker à utiliser.
        *   `build` : Chemin vers un Dockerfile pour construire une image personnalisée.
        *   `container_name` : Nom personnalisé pour le conteneur.
        *   `ports` : Mappage des ports entre l'hôte et le conteneur (ex: `"443:443"`).
        *   `volumes` : Montage de volumes pour la persistance des données ou le partage de fichiers.
        *   `networks` : Connexion du service à des réseaux Docker spécifiques.
        *   `environment` : Définition de variables d'environnement.
        *   `depends_on` : Spécifie les dépendances entre services.
        *   `restart` : Politique de redémarrage du conteneur (ex: `always`, `on-failure`).
    *   **`networks` (global) :** Permet de définir des réseaux personnalisés.
    *   **`volumes` (global) :** Permet de définir des volumes nommés.
*   **Commande unique :** Avec une seule commande (ex: `docker-compose up`), vous pouvez créer et démarrer tous les services définis.

### Volumes Docker : Persistance des Données

Par défaut, les données à l'intérieur d'un conteneur sont éphémères : si le conteneur est supprimé, ses données le sont aussi. Les volumes Docker permettent de faire persister les données.

*   **Volumes Nommés :** Créés et gérés par Docker (`docker volume create ...`). Ils sont stockés dans une partie du système de fichiers de l'hôte gérée par Docker. C'est la méthode recommandée pour la persistance des données de services comme les bases de données.
    *   Exemple dans `docker-compose.yml` :
        ```yaml
        services:
          mariadb:
            volumes:
              - db_data:/var/lib/mysql # db_data est un volume nommé
        volumes:
          db_data: # Déclaration du volume nommé
        ```
*   **Bind Mounts (Montages Liés) :** Permettent de monter un fichier ou un répertoire de la machine hôte directement dans un conteneur. Utile pour le développement (monter le code source) ou pour fournir des fichiers de configuration.
    *   Exemple dans `docker-compose.yml` :
        ```yaml
        services:
          nginx:
            volumes:
              - ./nginx.conf:/etc/nginx/nginx.conf:ro # :ro pour read-only
              - ./html_files:/usr/share/nginx/html
        ```

### Réseaux Docker : Communication Isolée

Docker permet de créer des réseaux virtuels pour isoler les conteneurs ou leur permettre de communiquer entre eux.

*   **Réseau `bridge` (par défaut) :** Si vous ne spécifiez pas de réseau, Docker Compose crée un réseau `bridge` par défaut pour votre application. Les services sur ce même réseau peuvent communiquer entre eux en utilisant leurs noms de service comme noms d'hôte (ex: WordPress peut se connecter à MariaDB via l'hôte `mariadb`).
*   **Isolation :** Les conteneurs sur des réseaux différents sont isolés, sauf si des configurations spécifiques sont mises en place.
*   **Définition dans `docker-compose.yml` :**
    ```yaml
    services:
      wordpress:
        networks:
          - app_network
      mariadb:
        networks:
          - app_network
    networks:
      app_network: # Définit un réseau personnalisé
        driver: bridge
    ```

### Variables d'Environnement : Configuration Flexible

Les variables d'environnement sont un moyen courant de configurer les applications à l'intérieur des conteneurs sans avoir à modifier leur code ou leurs images. Elles peuvent être utilisées pour passer des mots de passe, des noms d'utilisateur, des options de configuration, etc.

*   **Définition dans `Dockerfile` :** `ENV MA_VARIABLE=valeur` (valeur par défaut).
*   **Définition dans `docker-compose.yml` :**
    ```yaml
    services:
      mariadb:
        environment:
          - MYSQL_ROOT_PASSWORD=supersecret
          - MYSQL_DATABASE=wordpress_db
          - MYSQL_USER=wp_user
          - MYSQL_PASSWORD=wp_pass
    ```
*   **Utilisation d'un fichier `.env` (voir ci-dessous).**

### SSL/TLS et HTTPS : Sécuriser les Communications

*   **SSL/TLS (Secure Sockets Layer / Transport Layer Security) :** Protocoles cryptographiques conçus pour sécuriser les communications sur un réseau informatique. TLS est le successeur de SSL.
*   **HTTPS (HTTP Secure) :** Utilisation de HTTP sur une connexion chiffrée par TLS. Cela garantit la confidentialité (les données ne peuvent pas être lues par des tiers), l'intégrité (les données ne peuvent pas être modifiées) et l'authentification (prouve l'identité du serveur auquel vous vous connectez, via un certificat).
*   **Certificats SSL/TLS :** Fichiers numériques qui lient une clé cryptographique aux détails d'une organisation ou d'un individu. Pour HTTPS, le serveur web (Nginx) a besoin d'un certificat et d'une clé privée correspondante.
*   **Pour ce projet :** Vous générerez probablement des certificats **auto-signés**. Ils fournissent le chiffrement mais ne sont pas approuvés par une autorité de certification reconnue, donc votre navigateur affichera un avertissement de sécurité (que vous pouvez ignorer pour le développement local).

### Fichier `.env` : Gérer les Configurations Sensibles

Un fichier nommé `.env` (à la racine de votre projet Docker Compose) peut être utilisé pour stocker des variables d'environnement. Docker Compose charge automatiquement ce fichier et substitue les variables dans votre `docker-compose.yml`.

*   **Avantages :**
    *   Sépare la configuration de la définition des services.
    *   Permet de ne pas commiter de secrets (mots de passe, clés API) dans votre `docker-compose.yml` (ajoutez `.env` à votre `.gitignore`).
*   **Exemple de fichier `.env` :**
    ```env
    # Configuration MariaDB
    MYSQL_DATABASE=wordpress_project
    MYSQL_USER=inception_user
    MYSQL_PASSWORD=inception_password
    MYSQL_ROOT_PASSWORD=root_strong_password

    # Domaine (pour Nginx et WordPress)
    DOMAIN_NAME=c-andriam.42.fr
    ```
*   **Utilisation dans `docker-compose.yml` :**
    ```yaml
    services:
      mariadb:
        environment:
          MYSQL_ROOT_PASSWORD: ${MYSQL_ROOT_PASSWORD}
          MYSQL_DATABASE: ${MYSQL_DATABASE}
          MYSQL_USER: ${MYSQL_USER}
          MYSQL_PASSWORD: ${MYSQL_PASSWORD}
    ```

## 4. Installation de Docker et Docker Compose

### Sur macOS
1.  **Docker Desktop :** La méthode la plus simple est de télécharger et d'installer [Docker Desktop for Mac](https://docs.docker.com/desktop/install/mac-install/) depuis le site officiel de Docker. Il inclut Docker Engine, la CLI Docker, et Docker Compose.
2.  **Installation :** Suivez les instructions du programme d'installation.
3.  **Vérification :** Ouvrez un terminal et tapez :
    ```bash
    docker --version
    docker-compose --version
    docker run hello-world 
    ```
    Si `hello-world` s'exécute, l'installation est réussie.

### Sur Linux
1.  **Docker Engine :** Suivez les instructions officielles pour votre distribution Linux (ex: Ubuntu, Debian, Fedora) sur le [site de Docker](https://docs.docker.com/engine/install/). Cela implique généralement d'ajouter le dépôt Docker, puis d'installer le paquet `docker-ce`.
2.  **Post-installation :** Ajoutez votre utilisateur au groupe `docker` pour exécuter les commandes Docker sans `sudo` :
    ```bash
    sudo usermod -aG docker $USER
    # Déconnectez-vous et reconnectez-vous pour que les changements prennent effet.
    ```
3.  **Docker Compose :** Suivez les [instructions d'installation de Docker Compose](https://docs.docker.com/compose/install/) (souvent un binaire à télécharger).
4.  **Vérification :**
    ```bash
    docker --version
    docker-compose --version
    docker run hello-world
    ```

### Sur Windows
1.  **Docker Desktop :** Téléchargez et installez [Docker Desktop for Windows](https://docs.docker.com/desktop/install/windows-install/). Il utilise WSL 2 (Windows Subsystem for Linux version 2) comme backend, qui doit être activé.
2.  **Installation :** Suivez les instructions, y compris l'activation de WSL 2 si nécessaire.
3.  **Vérification :** Ouvrez PowerShell ou CMD et tapez :
    ```bash
    docker --version
    docker-compose --version
    docker run hello-world
    ```

*   **Arrêter les services locaux conflictuels :** Sur n'importe quel système, si vous avez des services locaux (comme Nginx, Apache, MySQL/MariaDB) qui tournent déjà sur les ports que vous comptez utiliser pour Inception (ex: 80, 443, 3306), vous devrez les arrêter pour éviter les conflits de ports.
    Sur Linux (exemple) :
    ```bash
    sudo systemctl stop nginx
    sudo systemctl stop apache2
    sudo systemctl stop mysql
    # Ou avec service:
    # sudo service nginx stop
    ```

## 5. Commandes Essentielles Docker et Docker Compose

### Gestion des Conteneurs Docker
```bash
# Lister les conteneurs en cours d'exécution
docker ps

# Lister tous les conteneurs (en cours et arrêtés)
docker ps -a

# Démarrer un conteneur à partir d'une image (exemple simple)
# -d: mode détaché (arrière-plan)
# -p <port_hôte>:<port_conteneur>: mapper un port
# --name <nom_conteneur>: donner un nom au conteneur
docker run -d -p 8080:80 --name mon_nginx nginx:latest

# Arrêter un conteneur en cours d'exécution
docker stop <nom_conteneur_ou_id>

# Démarrer un conteneur arrêté
docker start <nom_conteneur_ou_id>

# Redémarrer un conteneur
docker restart <nom_conteneur_ou_id>

# Supprimer un conteneur arrêté (ne peut pas être supprimé s'il est en cours)
docker rm <nom_conteneur_ou_id>

# Supprimer un conteneur en cours d'exécution (forcer)
docker rm -f <nom_conteneur_ou_id>

# Supprimer tous les conteneurs arrêtés
docker rm $(docker ps -a -q)

# Afficher les logs d'un conteneur
docker logs <nom_conteneur_ou_id>

# Afficher les logs en temps réel (suivre)
docker logs -f <nom_conteneur_ou_id>

# Exécuter une commande dans un conteneur en cours (ex: ouvrir un shell)
docker exec -it <nom_conteneur_ou_id> bash  # ou sh pour Alpine

# Inspecter un conteneur (informations détaillées en JSON)
docker inspect <nom_conteneur_ou_id>
```

### Gestion des Images Docker
```bash
# Lister les images Docker disponibles localement
docker images

# Télécharger une image depuis un registre (par défaut Docker Hub)
docker pull nginx:latest

# Construire une image à partir d'un Dockerfile
# -t <nom_image>:<tag>: nommer et taguer l'image
# .: contexte de build (répertoire courant)
docker build -t mon_app:1.0 .

# Supprimer une image (doit être inutilisée par des conteneurs)
docker rmi <nom_image_ou_id>

# Supprimer une image (forcer, même si utilisée)
docker rmi -f <nom_image_ou_id>

# Supprimer les images "dangling" (sans tag, souvent des couches intermédiaires)
docker rmi $(docker images -q -f dangling=true)

# Supprimer toutes les images inutilisées
docker image prune -a
```

### Gestion des Volumes Docker
```bash
# Lister les volumes
docker volume ls

# Créer un volume nommé
docker volume create mon_volume_data

# Inspecter un volume (informations détaillées)
docker volume inspect mon_volume_data

# Supprimer un volume (doit être inutilisé)
docker volume rm mon_volume_data

# Supprimer tous les volumes inutilisés (attention !)
docker volume prune
```

### Gestion des Réseaux Docker
```bash
# Lister les réseaux
docker network ls

# Créer un réseau (bridge par défaut)
docker network create mon_reseau_app

# Inspecter un réseau
docker network inspect mon_reseau_app

# Supprimer un réseau
docker network rm mon_reseau_app

# Supprimer tous les réseaux inutilisés
docker network prune
```

### Commandes Docker Compose
(Exécutez ces commandes depuis le répertoire contenant votre fichier `docker-compose.yml`)
```bash
# Construire les images et démarrer tous les services en mode détaché
docker-compose up -d --build

# Démarrer les services (sans reconstruire les images, sauf si elles n'existent pas)
docker-compose up -d

# Arrêter les services (les conteneurs sont arrêtés mais pas supprimés)
docker-compose stop

# Arrêter et supprimer les conteneurs, réseaux, et (optionnellement) volumes
docker-compose down
# Pour supprimer aussi les volumes nommés définis dans docker-compose.yml :
docker-compose down -v

# Lister les conteneurs des services du projet
docker-compose ps

# Afficher les logs de tous les services
docker-compose logs

# Afficher les logs d'un service spécifique en temps réel
docker-compose logs -f <nom_du_service> # ex: docker-compose logs -f nginx

# Exécuter une commande dans un service en cours
docker-compose exec <nom_du_service> <commande> # ex: docker-compose exec wordpress bash

# Construire (ou reconstruire) les images des services
docker-compose build
# Reconstruire sans utiliser le cache
docker-compose build --no-cache <nom_du_service_optionnel>

# Valider et afficher la configuration interprétée de docker-compose.yml
docker-compose config
```

## 6. Le Projet Inception : Construction Pas à Pas

### Structure des Fichiers du Projet
Une structure de projet organisée est essentielle. Voici une suggestion :
```
inception/
├── .env                   # Variables d'environnement (à ajouter à .gitignore)
├── docker-compose.yml     # Fichier principal d'orchestration
├── Makefile               # Optionnel, pour simplifier les commandes
├── srcs/                  # Répertoire contenant les configurations et Dockerfiles
│   ├── nginx/
│   │   ├── Dockerfile
│   │   └── conf/
│   │       └── default.conf   # Configuration Nginx
│   │   └── tools/
│   │       └── script_nginx.sh # Optionnel, pour la génération de certificats, etc.
│   ├── mariadb/
│   │   ├── Dockerfile
│   │   └── tools/
│   │       ├── init_db.sh     # Script d'initialisation de la DB
│   │       └── wordpress.sql  # Optionnel, dump SQL initial
│   └── wordpress/
│       ├── Dockerfile
│       └── tools/
│           ├── entrypoint.sh  # Script d'entrée pour WordPress
│           └── www.conf       # Configuration PHP-FPM
└── ... (autres répertoires pour les bonus)
```
**Important :** N'oubliez pas de créer un fichier `.gitignore` à la racine de votre projet pour exclure les fichiers sensibles ou inutiles (comme `.env`, les dossiers `node_modules` si vous en aviez, les fichiers `data/` des volumes, etc.).
```gitignore
.env
data/
# Ajoutez d'autres fichiers/dossiers à ignorer
```

### NGINX : Serveur Web et Reverse Proxy

Nginx sera notre porte d'entrée. Il servira les fichiers statiques de WordPress, agira comme reverse proxy pour l'application WordPress (PHP-FPM), et gérera les connexions SSL/TLS.

1.  **Rôle de Nginx dans Inception :**
    *   Servir le contenu statique (HTML, CSS, JS, images).
    *   Terminaison SSL/TLS (déchiffrer HTTPS et passer en HTTP à WordPress si besoin).
    *   Transmettre les requêtes dynamiques pour les fichiers PHP au service WordPress (PHP-FPM).
    *   Héberger potentiellement d'autres services (Adminer, site statique Hugo) via des configurations de `location`.

2.  **Dockerfile Nginx (`srcs/nginx/Dockerfile`) :**
    ```dockerfile
    FROM debian:buster-slim

    # Mettre à jour les paquets et installer Nginx et OpenSSL (pour les certificats)
    RUN apt-get update && \
        apt-get install -y nginx openssl procps && \
        rm -rf /var/lib/apt/lists/*

    # Copier le fichier de configuration Nginx personnalisé
    COPY ./conf/default.conf /etc/nginx/sites-available/default
    # Il est souvent préférable de copier dans sites-available et de créer un lien symbolique
    # vers sites-enabled, ou de copier directement dans conf.d/default.conf

    # Optionnel: Copier un script pour générer des certificats SSL auto-signés
    # COPY ./tools/generate_ssl.sh /usr/local/bin/generate_ssl.sh
    # RUN chmod +x /usr/local/bin/generate_ssl.sh && /usr/local/bin/generate_ssl.sh votredomaine.42.fr

    # Créer les répertoires pour les certificats SSL (si non fait par le script)
    RUN mkdir -p /etc/nginx/ssl/
    # Vous devrez générer et copier vos certificats ici ou via un volume

    # Exposer les ports HTTP et HTTPS
    EXPOSE 80
    EXPOSE 443

    # Commande pour démarrer Nginx en premier plan
    CMD ["nginx", "-g", "daemon off;"]
    ```
    *   **Certificats SSL :** Pour `votredomaine.42.fr`, vous devez générer un certificat SSL auto-signé et sa clé privée.
        ```bash
        # Exemple de commande OpenSSL (à exécuter sur votre hôte ou dans un script)
        openssl req -x509 -nodes -days 365 -newkey rsa:2048 \
            -keyout ./srcs/nginx/conf/votredomaine.42.fr.key \
            -out ./srcs/nginx/conf/votredomaine.42.fr.crt \
            -subj "/C=FR/ST=IDF/L=Paris/O=42/OU=c-andriam/CN=c-andriam.42.fr"
        ```
        Ces fichiers (`.key` et `.crt`) devront ensuite être copiés dans l'image Nginx ou montés via un volume.

3.  **Configuration Nginx (`srcs/nginx/conf/default.conf`) :**
    ```nginx
    server {
        listen 80;
        server_name c-andriam.42.fr www.c-andriam.42.fr; # Remplacez par votre domaine du .env

        # Redirection de HTTP vers HTTPS
        return 301 https://$host$request_uri;
    }

    server {
        listen 443 ssl http2; # http2 pour de meilleures performances
        server_name c-andriam.42.fr www.c-andriam.42.fr; # Remplacez par votre domaine du .env

        # Chemins vers vos certificats SSL (ajustez si nécessaire)
        ssl_certificate /etc/nginx/ssl/c-andriam.42.fr.crt;
        ssl_certificate_key /etc/nginx/ssl/c-andriam.42.fr.key;

        # Paramètres SSL recommandés
        ssl_protocols TLSv1.2 TLSv1.3;
        ssl_prefer_server_ciphers off;
        ssl_ciphers 'ECDHE-ECDSA-AES128-GCM-SHA256:ECDHE-RSA-AES128-GCM-SHA256:ECDHE-ECDSA-AES256-GCM-SHA384:ECDHE-RSA-AES256-GCM-SHA384';

        root /var/www/html; # Racine des fichiers de WordPress
        index index.php index.html index.htm;

        location / {
            try_files $uri $uri/ /index.php?$args;
        }

        # Passer les scripts PHP au conteneur WordPress (PHP-FPM)
        # 'wordpress' est le nom du service WordPress dans docker-compose.yml
        # 9000 est le port sur lequel PHP-FPM écoute dans le conteneur WordPress
        location ~ \.php$ {
            include fastcgi_params;
            fastcgi_pass wordpress:9000; # Communication inter-conteneurs
            fastcgi_index index.php;
            fastcgi_param SCRIPT_FILENAME $document_root$fastcgi_script_name;
            fastcgi_param PATH_INFO $fastcgi_path_info;
        }

        # Bloquer l'accès aux fichiers .htaccess (sécurité)
        location ~ /\.ht {
            deny all;
        }

        # Optimisations pour les fichiers statiques (optionnel)
        location ~* \.(css|js|gif|jpe?g|png)$ {
            expires 1M;
            access_log off;
            add_header Cache-Control "public";
        }
    }
    ```

4.  **Configurer le domaine local (`/etc/hosts`) sur votre machine hôte :**
    Pour que votre navigateur reconnaisse `c-andriam.42.fr` (ou le domaine que vous avez choisi) comme pointant vers votre machine locale (localhost), modifiez le fichier `hosts` de votre système (nécessite des droits admin) :
    ```
    127.0.0.1   c-andriam.42.fr www.c-andriam.42.fr
    ```

### MARIADB : Base de Données Robuste

MariaDB stockera toutes les données de WordPress (articles, utilisateurs, configurations, etc.).

1.  **Rôle de MariaDB :** Fournir un service de base de données relationnelle fiable pour WordPress.

2.  **Dockerfile MariaDB (`srcs/mariadb/Dockerfile`) :**
    Vous pouvez utiliser l'image officielle `mariadb` qui est très bien configurée, ou en construire une à partir de Debian pour plus de contrôle (comme souvent demandé à 42).
    *   **Option 1 : Utiliser l'image officielle (plus simple pour la configuration initiale) :**
        Pas besoin de Dockerfile si vous utilisez directement `image: mariadb:10.9` (ou une version stable) dans `docker-compose.yml` et que vous passez les variables d'environnement.
    *   **Option 2 : Construire à partir de Debian (exemple) :**
        ```dockerfile
        FROM debian:buster-slim

        RUN apt-get update && \
            apt-get install -y mariadb-server procps && \
            rm -rf /var/lib/apt/lists/*

        # Copier un script d'initialisation
        COPY ./tools/init_db.sh /usr/local/bin/init_db.sh
        RUN chmod +x /usr/local/bin/init_db.sh

        # Copier une configuration MariaDB personnalisée (optionnel)
        # COPY ./conf/my.cnf /etc/mysql/mariadb.conf.d/99-custom.cnf

        # Le répertoire de données de MariaDB
        VOLUME /var/lib/mysql

        EXPOSE 3306

        # Le script d'initialisation s'occupera de démarrer mysqld après la config
        ENTRYPOINT ["/usr/local/bin/init_db.sh"]
        CMD ["mysqld", "--user=mysql", "--console"] # Commande par défaut après init
        ```

3.  **Script d'Initialisation (`srcs/mariadb/tools/init_db.sh`) :**
    Ce script est crucial si vous construisez votre propre image ou si vous avez besoin d'une initialisation plus complexe que ce que permettent les variables d'environnement de l'image officielle.
    ```bash
    #!/bin/bash
    set -e # Arrête le script en cas d'erreur

    # Vérifier si la base de données est déjà initialisée
    if [ -d "/var/lib/mysql/$MYSQL_DATABASE" ]; then
        echo "MariaDB: Base de données '$MYSQL_DATABASE' déjà initialisée."
    else
        echo "MariaDB: Initialisation de la base de données..."

        # Initialiser le répertoire de données MariaDB
        mysql_install_db --user=mysql --datadir=/var/lib/mysql

        # Démarrer mysqld temporairement en mode bootstrap pour la configuration
        mysqld --user=mysql --bootstrap --skip-networking=0 <<EOF
    FLUSH PRIVILEGES;
    ALTER USER 'root'@'localhost' IDENTIFIED BY '${MYSQL_ROOT_PASSWORD}';
    CREATE DATABASE IF NOT EXISTS \`${MYSQL_DATABASE}\`;
    CREATE USER IF NOT EXISTS '${MYSQL_USER}'@'%' IDENTIFIED BY '${MYSQL_PASSWORD}';
    GRANT ALL PRIVILEGES ON \`${MYSQL_DATABASE}\`.* TO '${MYSQL_USER}'@'%';
    FLUSH PRIVILEGES;
    EOF
        # Si vous avez un fichier wordpress.sql à importer au premier lancement :
        # (Assurez-vous que mysqld est accessible; cela peut nécessiter de le démarrer normalement)
        # mysqld_safe --user=mysql --datadir=/var/lib/mysql &
        # PID=$!
        # sleep 5 # Attendre que le serveur démarre
        # mysql -u root -p${MYSQL_ROOT_PASSWORD} ${MYSQL_DATABASE} < /path/to/wordpress.sql
        # kill $PID
        # wait $PID

        echo "MariaDB: Initialisation terminée."
    fi

    # Lancer mysqld normalement (sera la commande principale du conteneur)
    echo "MariaDB: Démarrage du serveur MariaDB..."
    exec mysqld --user=mysql --console --skip-networking=0
    ```
    **Variables d'environnement requises (à définir dans `.env` et `docker-compose.yml`) :**
    *   `MYSQL_DATABASE`
    *   `MYSQL_USER`
    *   `MYSQL_PASSWORD`
    *   `MYSQL_ROOT_PASSWORD`

4.  **Persistance des Données :** Il est **impératif** d'utiliser un volume Docker nommé pour stocker les données de MariaDB (`/var/lib/mysql`) afin qu'elles survivent aux redémarrages et suppressions de conteneurs.

### WORDPRESS : Système de Gestion de Contenu

WordPress est l'application PHP qui générera notre site web. Elle a besoin de Nginx pour être servie et de MariaDB pour stocker ses données.

1.  **Rôle de WordPress :** Fournir l'interface d'administration et le moteur du site.

2.  **Dockerfile WordPress (`srcs/wordpress/Dockerfile`) :**
    ```dockerfile
    # Utiliser une image PHP-FPM officielle est une bonne base
    FROM php:7.4-fpm-alpine # Alpine est léger, ajustez la version de PHP si besoin

    # Installer les dépendances et extensions PHP nécessaires pour WordPress
    # mysqli pour la base de données, gd pour les images, curl, etc.
    # wp-cli pour la gestion en ligne de commande
    RUN apk update && apk add --no-cache \
        mariadb-client \
        curl \
        gd \
        freetype-dev \
        libjpeg-turbo-dev \
        libpng-dev \
        zip \
        unzip \
    && docker-php-ext-configure gd --with-freetype --with-jpeg \
    && docker-php-ext-install -j$(nproc) gd mysqli opcache pdo pdo_mysql \
    && rm -rf /var/cache/apk/*

    # Installer WP-CLI
    RUN curl -O https://raw.githubusercontent.com/wp-cli/builds/gh-pages/phar/wp-cli.phar && \
        chmod +x wp-cli.phar && \
        mv wp-cli.phar /usr/local/bin/wp

    # Créer le répertoire pour WordPress et définir les permissions
    RUN mkdir -p /var/www/html && \
        chown -R www-data:www-data /var/www/html # www-data est l'utilisateur PHP-FPM par défaut

    # Définir le répertoire de travail
    WORKDIR /var/www/html

    # Copier la configuration PHP-FPM personnalisée (www.conf)
    COPY ./tools/www.conf /usr/local/etc/php-fpm.d/www.conf

    # Copier le script d'entrée
    COPY ./tools/entrypoint.sh /usr/local/bin/entrypoint.sh
    RUN chmod +x /usr/local/bin/entrypoint.sh

    # Exposer le port sur lequel PHP-FPM écoute (pour Nginx)
    EXPOSE 9000

    ENTRYPOINT ["/usr/local/bin/entrypoint.sh"]
    CMD ["php-fpm", "-F"] # -F pour forcer php-fpm à rester au premier plan
    ```

3.  **Configuration PHP-FPM (`srcs/wordpress/tools/www.conf`) :**
    C'est un fichier de configuration pour le pool de processus PHP-FPM.
    ```ini
    [www]
    user = www-data
    group = www-data

    ; Écouter sur un socket TCP/IP. Nginx s'y connectera.
    ; '0.0.0.0:9000' écoute sur toutes les interfaces du conteneur sur le port 9000.
    listen = 0.0.0.0:9000

    ; Alternative : écouter sur un socket Unix (plus performant si Nginx et PHP-FPM sont sur le même hôte/conteneur)
    ; listen = /run/php/php7.4-fpm.sock
    ; listen.owner = www-data
    ; listen.group = www-data
    ; listen.mode = 0660

    pm = dynamic
    pm.max_children = 5
    pm.start_servers = 2
    pm.min_spare_servers = 1
    pm.max_spare_servers = 3
    pm.max_requests = 500 ; Redémarrer les processus enfants après X requêtes (évite les fuites mémoire)

    ; Rediriger stdout/stderr des workers vers les logs principaux de FPM
    catch_workers_output = yes
    clear_env = no ; Permet de passer les variables d'environnement aux scripts PHP
    ```

4.  **Script d'Entrée WordPress (`srcs/wordpress/tools/entrypoint.sh`) :**
    Ce script s'exécute au démarrage du conteneur WordPress. Il télécharge WordPress, le configure avec WP-CLI, et s'assure que la base de données est prête.
    ```bash
    #!/bin
    set -e

    # Attendre que MariaDB soit disponible (simple test de connexion)
    echo "WordPress Entrypoint: Attente de MariaDB sur ${WORDPRESS_DB_HOST}:${WORDPRESS_DB_PORT:-3306}..."
    # Un script plus robuste utiliserait `nc` ou un client mysql pour vérifier la connexion
    # Ici, on suppose que depends_on dans docker-compose suffit pour un démarrage ordonné,
    # mais une boucle de vérification est plus sûre.
    # Exemple simple (nécessite mariadb-client dans l'image WordPress):
    # while ! mysqladmin ping -h"${WORDPRESS_DB_HOST}" --silent; do
    #     echo "MariaDB n'est pas encore prête - en attente..."
    #     sleep 1
    # done
    # echo "MariaDB est prête."


    # Si wp-config.php n'existe pas, alors on configure WordPress
    if [ ! -f "/var/www/html/wp-config.php" ]; then
        echo "WordPress Entrypoint: Configuration de WordPress..."

        # Télécharger WordPress (si le volume est vide)
        if [ ! -f "/var/www/html/index.php" ]; then
            echo "WordPress Entrypoint: Téléchargement de WordPress core..."
            wp core download --path=/var/www/html --allow-root --version=${WORDPRESS_VERSION:-latest}
        fi

        # Créer wp-config.php
        # WP-CLI va tenter de se connecter à la DB ici, donc la DB doit être prête.
        # Utilisation de --skip-check pour éviter l'erreur si la DB n'est pas encore totalement initialisée
        # mais les tables WordPress n'existent pas.
        wp config create --path=/var/www/html --allow-root --skip-check \
            --dbname="${WORDPRESS_DB_NAME}" \
            --dbuser="${WORDPRESS_DB_USER}" \
            --dbpass="${WORDPRESS_DB_PASSWORD}" \
            --dbhost="${WORDPRESS_DB_HOST}" \
            --dbprefix="${WORDPRESS_TABLE_PREFIX:-wp_}"

        # Installer WordPress (crée les tables dans la base de données)
        # Cette commande échouera si la base de données ou l'utilisateur n'existent pas
        # ou si les identifiants sont incorrects.
        if ! $(wp core is-installed --allow-root); then
            echo "WordPress Entrypoint: Installation de WordPress core..."
            wp core install --path=/var/www/html --allow-root \
                --url="https://c-andriam.42.fr" \
                --title="${WORDPRESS_SITE_TITLE:-Mon Site Inception}" \
                --admin_user="${WORDPRESS_ADMIN_USER:-admin}" \
                --admin_password="${WORDPRESS_ADMIN_PASSWORD:-password}" \
                --admin_email="${WORDPRESS_ADMIN_EMAIL:-admin@example.com}" \
                --skip-email
        else
            echo "WordPress Entrypoint: WordPress est déjà installé."
        fi

        # Définir les permissions pour les fichiers WordPress
        chown -R www-data:www-data /var/www/html

        echo "WordPress Entrypoint: Configuration terminée."
    else
        echo "WordPress Entrypoint: wp-config.php trouvé, WordPress semble déjà configuré."
    fi

    # Exécuter la commande CMD par défaut (php-fpm -F)
    echo "WordPress Entrypoint: Démarrage de PHP-FPM..."
    exec "$@"
    ```
    **Variables d'environnement requises (à définir dans `.env` et `docker-compose.yml`) :**
    *   `WORDPRESS_DB_HOST` (ex: `mariadb`)
    *   `WORDPRESS_DB_NAME`
    *   `WORDPRESS_DB_USER`
    *   `WORDPRESS_DB_PASSWORD`
    *   `WORDPRESS_SITE_TITLE`
    *   `WORDPRESS_ADMIN_USER`
    *   `WORDPRESS_ADMIN_PASSWORD`
    *   `WORDPRESS_ADMIN_EMAIL`
    *   `WORDPRESS_TABLE_PREFIX` (optionnel)
    *   `WORDPRESS_VERSION` (optionnel)

5.  **Gestion des Fichiers WordPress :** Utilisez un volume Docker nommé pour `/var/www/html` afin que les fichiers de WordPress (core, thèmes, plugins, uploads) persistent.

### DOCKER COMPOSE : Assemblage Final

Le fichier `docker-compose.yml` est le chef d'orchestre. Il définit comment tous nos services (Nginx, WordPress, MariaDB) sont construits, configurés, et connectés.

**`docker-compose.yml` :**
```yaml
version: '3.8' # Ou une version plus récente compatible

services:
  nginx:
    build:
      context: ./srcs/nginx
      dockerfile: Dockerfile
    container_name: inception_nginx
    hostname: nginx # Nom d'hôte à l'intérieur du réseau Docker
    ports:
      - "443:443" # Exposer HTTPS seulement
    volumes:
      - wordpress_files:/var/www/html # Volume partagé pour les fichiers WordPress
      # Si vous générez les certificats sur l'hôte avant de build Nginx:
      - ./srcs/nginx/conf/c-andriam.42.fr.crt:/etc/nginx/ssl/c-andriam.42.fr.crt:ro
      - ./srcs/nginx/conf/c-andriam.42.fr.key:/etc/nginx/ssl/c-andriam.42.fr.key:ro
    networks:
      - inception_network
    depends_on:
      - wordpress # Nginx a besoin que WordPress (PHP-FPM) soit démarré
    restart: always # Redémarrer toujours ce service

  wordpress:
    build:
      context: ./srcs/wordpress
      dockerfile: Dockerfile
    container_name: inception_wordpress
    hostname: wordpress
    volumes:
      - wordpress_files:/var/www/html # Persistance des fichiers WordPress
    networks:
      - inception_network
    environment:
      # Variables pour la connexion à la base de données
      WORDPRESS_DB_HOST: mariadb # Nom du service MariaDB
      WORDPRESS_DB_NAME: ${MYSQL_DATABASE} # Utilise la variable du fichier .env
      WORDPRESS_DB_USER: ${MYSQL_USER}
      WORDPRESS_DB_PASSWORD: ${MYSQL_PASSWORD}
      # Variables pour l'installation de WordPress (via WP-CLI dans entrypoint.sh)
      WORDPRESS_SITE_TITLE: "Inception par c-andriam"
      WORDPRESS_ADMIN_USER: ${WP_ADMIN_USER:-admin} # Valeur par défaut si non défini dans .env
      WORDPRESS_ADMIN_PASSWORD: ${WP_ADMIN_PASS:-password}
      WORDPRESS_ADMIN_EMAIL: ${WP_ADMIN_EMAIL:-user@example.com}
      WORDPRESS_TABLE_PREFIX: "wp_"
    depends_on:
      - mariadb # WordPress a besoin que MariaDB soit démarré et prêt
    restart: always

  mariadb:
    build:
      context: ./srcs/mariadb
      dockerfile: Dockerfile
    container_name: inception_mariadb
    hostname: mariadb
    volumes:
      - mariadb_data:/var/lib/mysql # Persistance des données MariaDB
    networks:
      - inception_network
    environment:
      MYSQL_DATABASE: ${MYSQL_DATABASE}
      MYSQL_USER: ${MYSQL_USER}
      MYSQL_PASSWORD: ${MYSQL_PASSWORD}
      MYSQL_ROOT_PASSWORD: ${MYSQL_ROOT_PASSWORD}
    restart: always
    # healthcheck: # Optionnel, pour s'assurer que MariaDB est vraiment prêt
    #   test: ["CMD", "mysqladmin", "ping", "-h", "localhost", "-p${MYSQL_ROOT_PASSWORD}"]
    #   interval: 10s
    #   timeout: 5s
    #   retries: 5

# Définition des réseaux
networks:
  inception_network:
    driver: bridge # Type de réseau par défaut, permet la communication par nom de service

# Définition des volumes nommés
volumes:
  wordpress_files: # Pour les fichiers du site WordPress (core, thèmes, plugins, uploads)
    driver: local # Pilote de volume par défaut
  mariadb_data:    # Pour les données de la base MariaDB
    driver: local
```
**Fichier `.env` (exemple à créer à la racine du projet) :**
```env
# Domaine
DOMAIN_NAME=c-andriam.42.fr

# MariaDB Configuration
MYSQL_DATABASE=wordpress_db
MYSQL_USER=wpuser
MYSQL_PASSWORD=wppassword
MYSQL_ROOT_PASSWORD=rootpassword

# WordPress Admin Configuration
WP_ADMIN_USER=c-andriam
WP_ADMIN_PASS=SuperSecurePassword123!
WP_ADMIN_EMAIL=c-andriam@student.42.fr
```
N'oubliez pas d'ajouter `.env` à votre `.gitignore` !

**Utilisation :**
1.  Créez votre fichier `.env`.
2.  Générez vos certificats SSL (par exemple, dans `./srcs/nginx/conf/`).
3.  Depuis la racine de votre projet Inception, exécutez :
    ```bash
    docker-compose up --build -d
    ```
4.  Accédez à `https://c-andriam.42.fr` (ou votre domaine) dans votre navigateur.

## 7. Les Bonus : Étendre les Fonctionnalités
(Cette section reprend la structure de votre README original pour les bonus, avec des clarifications si besoin.)

### REDIS : Cache d'Objets Performant
*   **Définition :** Redis est un serveur de structure de données en mémoire, utilisé comme base de données clé-valeur, cache et courtier de messages. Pour WordPress, il sert principalement de cache d'objets persistant pour accélérer le site.
*   **Configuration :**
    1.  **Dockerfile Redis :** Base Alpine, installation de `redis`, copie de `redis.conf` et d'un script d'entrée.
    2.  **`redis.conf` :** Configurer `bind 0.0.0.0`, `protected-mode no` (si `bind` n'est pas `127.0.0.1`), `port 6379`, `requirepass VOTRE_MOT_DE_PASSE_REDIS`.
    3.  **Service Docker Compose :** Ajouter un service `redis` avec son image, port exposé (si besoin pour débogage externe, sinon non), volume pour la persistance (optionnel pour un cache), et variables d'environnement (pour le mot de passe).
    4.  **Modification WordPress :**
        *   Installer l'extension PHP `php-redis` dans le Dockerfile WordPress.
        *   Dans `wp-config.php` (via le script d'entrée WordPress ou manuellement) :
            ```php
            define('WP_REDIS_HOST', 'redis'); // Nom du service Redis
            define('WP_REDIS_PORT', '6379');
            define('WP_REDIS_PASSWORD', 'VOTRE_MOT_DE_PASSE_REDIS');
            define('WP_CACHE_KEY_SALT', 'c-andriam.42.fr_'); // Préfixe unique
            // define('WP_REDIS_CLIENT', 'phpredis'); // Recommandé
            ```
        *   Utiliser WP-CLI dans le script d'entrée WordPress pour installer et activer le plugin "Redis Object Cache" :
            ```bash
            wp plugin install redis-cache --activate --allow-root
            wp redis enable --allow-root
            ```
*   **Vérification :** Connectez-vous au CLI Redis (`docker-compose exec redis redis-cli`, puis `AUTH ...`, `PING`). Dans l'admin WordPress, vérifiez l'état du plugin Redis Object Cache.

### VSFTPD : Serveur FTP Sécurisé
*   **Définition :** vsftpd (Very Secure FTP Daemon) est un serveur FTP/FTPS pour les systèmes Unix. Permet de transférer des fichiers vers/depuis le volume WordPress.
*   **Configuration :**
    1.  **Dockerfile vsftpd :** Base Debian/Alpine, installation de `vsftpd`, création d'un utilisateur FTP, copie de `vsftpd.conf`.
    2.  **`vsftpd.conf` :**
        *   `listen=NO`, `listen_ipv6=NO`
        *   `anonymous_enable=NO`, `local_enable=YES`, `write_enable=YES`
        *   `chroot_local_user=YES`, `allow_writeable_chroot=YES` (ou structure de répertoires spécifique)
        *   `local_root=/var/www/html` (ou le chemin vers les fichiers WordPress)
        *   `user_sub_token=$USER`, `local_root=/var/www/html`
        *   **Mode Passif (important !) :** `pasv_enable=YES`, `pasv_min_port=21100`, `pasv_max_port=21110`, `pasv_address=VOTRE_IP_OU_DOMAINE` (ou laissez Docker gérer si possible).
        *   **FTPS (recommandé) :** `ssl_enable=YES`, chemins vers certificats (`rsa_cert_file`, `rsa_private_key_file`).
    3.  **Service Docker Compose :**
        *   Ports : `21:21`, et la plage de ports passifs (ex: `21100-21110:21100-21110`).
        *   Volume : Monter `wordpress_files` sur `/var/www/html`.
        *   Variables d'environnement pour `FTP_USER`, `FTP_PASS`.
        *   Un script d'entrée pour créer l'utilisateur FTP avec le mot de passe et démarrer `vsftpd`.
*   **Vérification :** Utiliser un client FTP (FileZilla) pour se connecter en FTPS.

### ADMINER : Interface Web pour MariaDB
*   **Définition :** Adminer est un outil de gestion de base de données léger et complet, sous forme d'un unique fichier PHP.
*   **Configuration :**
    1.  **Service Docker Compose (ou intégré à Nginx/WordPress) :**
        *   Le plus simple est d'ajouter un service basé sur une image comme `adminer` (image officielle) ou de construire une image Nginx + PHP-FPM qui sert le fichier `adminer.php`.
        *   Si vous l'intégrez au service Nginx existant :
            *   Téléchargez `adminer.php` dans un sous-répertoire de `/var/www/html` (ex: `/var/www/html/adminer/adminer.php`) via le Dockerfile WordPress ou un volume.
            *   Ajoutez une `location` dans la configuration Nginx pour servir ce fichier PHP via le PHP-FPM de WordPress.
                ```nginx
                location /adminer {
                    alias /var/www/html/adminer/; # Assurez-vous que adminer.php est ici
                    index adminer.php;
                    location ~ ^/adminer/(.+\.php)$ {
                        try_files $uri =404;
                        fastcgi_pass wordpress:9000; # Utilise le PHP-FPM de WordPress
                        include fastcgi_params;
                        fastcgi_param SCRIPT_FILENAME $request_filename; # Ou $document_root$fastcgi_script_name
                    }
                }
                ```
*   **Vérification :** Accédez à `https://votredomaine.42.fr/adminer`. Connectez-vous à MariaDB avec `mariadb` comme serveur, et les identifiants de votre base de données.

### HUGO : Site Statique Personnalisé
*   **Définition :** Hugo est un générateur de sites statiques rapide.
*   **Configuration :**
    1.  **Structure du site Hugo :** Créez un répertoire pour votre site Hugo (`srcs/hugo-site/`) avec son `config.toml`, `content/`, `themes/`, etc.
    2.  **Dockerfile Hugo :** Base `klakegg/hugo` ou Alpine + Hugo. Copiez les sources du site, exécutez `hugo -D` pour générer les fichiers statiques dans `/public` ou un autre répertoire.
    3.  **Service Docker Compose `hugo` :** Ce service construit l'image Hugo. Il peut ne pas avoir besoin de ports s'il ne fait que générer les fichiers. Utilisez un volume pour partager le répertoire `public` généré.
    4.  **Modification Nginx :**
        *   Montez le volume contenant les fichiers `public` de Hugo dans le conteneur Nginx (ex: sur `/var/www/hugo_static`).
        *   Ajoutez une `location` dans Nginx pour servir ce site (ex: `location /hugo-site/ { alias /var/www/hugo_static/; index index.html; }`).
*   **Vérification :** Accédez à `https://votredomaine.42.fr/hugo-site/`.

### Portfolio : Page Web Statique Avancée
Ceci est une extension du bonus Hugo, impliquant plus de contenu et de personnalisation. Les étapes sont similaires à celles de Hugo, mais avec un contenu plus riche dans le répertoire `content/` de votre site Hugo.

## 8. Considérations de Sécurité
*   **Mots de Passe Robustes :** Utilisez des mots de passe forts et uniques pour MariaDB, WordPress, FTP. Stockez-les dans `.env` et ne commitez pas ce fichier.
*   **Minimiser les Privilèges :** Ne faites pas tourner les processus en tant que `root` dans les conteneurs si ce n'est pas absolument nécessaire. Utilisez des utilisateurs dédiés avec des permissions limitées.
*   **Mises à Jour Régulières :** Gardez vos images de base (Debian, Alpine, PHP, Nginx, MariaDB) et vos applications (WordPress, plugins) à jour pour corriger les failles de sécurité.
*   **HTTPS Partout :** Utilisez SSL/TLS pour toutes les communications externes.
*   **Pare-feu :** Configurez le pare-feu de votre machine hôte (ou VM) pour n'exposer que les ports nécessaires (ex: 443).
*   **Secrets Docker :** Pour une gestion plus avancée des secrets en production, explorez Docker Secrets (non requis pour Inception mais bon à savoir).
*   **Permissions des Fichiers :** Assurez-vous que les permissions des fichiers WordPress sont correctes (ex: `www-data` doit pouvoir écrire dans `wp-content/uploads`).

## 9. Dépannage et Logs
*   **`docker-compose logs <service_name>` :** Votre première source d'information. `docker-compose logs -f <service_name>` pour suivre en direct.
*   **`docker exec -it <container_name> bash` (ou `sh`) :** Entrez dans un conteneur pour inspecter son état, vérifier des fichiers, tester la connectivité réseau (`ping`, `curl`).
*   **`docker inspect <container_name>` :** Informations détaillées sur la configuration d'un conteneur, y compris ses IP, volumes, ports.
*   **`docker-compose config` :** Vérifiez la configuration interprétée de votre `docker-compose.yml`.
*   **Problèmes de réseau :**
    *   Assurez-vous que les services sont sur le même réseau Docker.
    *   Utilisez les noms de service (ex: `mariadb`, `wordpress`) pour la communication inter-conteneurs, pas `localhost` ou `127.0.0.1` (sauf si le service écoute spécifiquement sur cela à l'intérieur de son propre conteneur et que vous êtes dans ce conteneur).
*   **Problèmes de permissions :** Souvent liés aux volumes et à l'utilisateur qui exécute le processus dans le conteneur.
*   **Conflits de ports :** Assurez-vous que les ports mappés sur l'hôte ne sont pas déjà utilisés par d'autres applications.

## 10. Conclusion
Félicitations ! Si vous êtes arrivé jusqu'ici, vous avez acquis une solide compréhension de Docker, Docker Compose, et de l'orchestration de services web. Inception est un projet exigeant mais extrêmement formateur. Continuez à explorer et à expérimenter avec Docker pour approfondir vos connaissances.

N'hésitez pas à contribuer à ce guide ou à poser des questions si des points ne sont pas clairs.
*(Contact original de vbachele : Discord vbachele#7949)*
```

J'espère que cette version améliorée et plus détaillée vous sera utile ! Elle vise à être un guide plus complet tout en conservant l'esprit de votre document original.