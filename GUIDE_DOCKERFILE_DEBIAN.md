# Guide : Écrire un Dockerfile personnalisé sous Debian

## 1. Crée le dossier du service

Exemple pour NGINX :
```bash
mkdir -p srcs/requirements/nginx
cd srcs/requirements/nginx
```

---

## 2. Crée le fichier `Dockerfile`

```bash
touch Dockerfile
```

---

## 3. Structure de base d’un Dockerfile Debian

Voici un modèle de base, avec explications ligne par ligne :

```dockerfile
# 1. Choisir la base : toujours l'avant-dernière version officielle de Debian
FROM debian:12

# 2. Empêcher les questions interactives pendant l'installation
ENV DEBIAN_FRONTEND=noninteractive

# 3. Mettre à jour les paquets et installer ceux nécessaires (ici pour NGINX + OpenSSL)
RUN apt-get update && \
    apt-get upgrade -y && \
    apt-get install -y nginx openssl && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# 4. Copier tes fichiers de configuration personnalisés (facultatif, à adapter)
COPY ./conf/nginx.conf /etc/nginx/nginx.conf

# 5. Exposer le port utilisé (ici 443 pour HTTPS)
EXPOSE 443

# 6. Définir la commande de démarrage propre (le "PID 1" du container)
CMD ["nginx", "-g", "daemon off;"]
```

---

## 4. Explications détaillées

- **FROM debian:12**  
  Utilise une image officielle Debian (ici l’avant-dernière version stable, à vérifier sur [Docker Hub Debian](https://hub.docker.com/_/debian)).

- **ENV DEBIAN_FRONTEND=noninteractive**  
  Empêche les prompts interactifs qui bloqueraient le build Docker.

- **RUN apt-get update && ...**  
  - Met à jour la liste des paquets.
  - Installe les logiciels nécessaires à ton service (remplace `nginx openssl` par ce qu'il te faut pour MariaDB, PHP, etc.).
  - Nettoie les fichiers temporaires pour que l’image reste légère.

- **COPY ./conf/nginx.conf ...**  
  Copie une configuration personnalisée (à adapter selon le service).

- **EXPOSE 443**  
  Indique à Docker que ce container écoutera sur ce port (à adapter selon le service).

- **CMD ...**  
  Commande qui sera lancée par défaut quand le container démarre.  
  Pour NGINX et la plupart des services, il faut forcer le processus principal à tourner au premier plan (`daemon off;`).

---

## 5. Adapter selon le service

- **Pour MariaDB** : installe `mariadb-server` au lieu de `nginx`.
- **Pour WordPress** : installe `php`, `php-fpm`, et les extensions requises, puis télécharge WordPress.
- **Pour PHP-FPM** : installe `php-fpm` et configure-le comme il faut.

---

## 6. Exemple pour MariaDB

```dockerfile
FROM debian:12
ENV DEBIAN_FRONTEND=noninteractive
RUN apt-get update && \
    apt-get install -y mariadb-server && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*
EXPOSE 3306
CMD ["mysqld_safe"]
```
> N’oublie pas de gérer l’initialisation et la configuration avec des scripts adaptés (voir l’étape suivante pour chaque service dans le guide Inception).

---

## 7. Bonnes pratiques

- **Aucun mot de passe ou secret dans le Dockerfile.**
- **Garde tes Dockerfiles clairs et commentés.**
- **Teste chaque build avec :**
  ```bash
  docker build -t test-nginx .
  ```
  puis lance-le pour vérifier qu’il fonctionne :
  ```bash
  docker run -it --rm -p 443:443 test-nginx
  ```

---

## 8. Aller plus loin

- Pour des configurations complexes, ajoute d’autres fichiers (scripts d’init, conf personnalisées…) et copie-les dans l’image avec `COPY`.
- Utilise les variables d’environnement dans `docker-compose.yml` pour passer les mots de passe et paramètres dynamiquement.

---

**En maîtrisant ces étapes, tu sauras écrire des Dockerfiles Debian propres, adaptés à chaque service de ton projet.**