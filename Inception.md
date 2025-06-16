# Guide Complet du Projet Inception - Docker et Conteneurisation

## Table des matières

1. [Introduction à Docker et à la conteneurisation](#1-introduction-à-docker-et-à-la-conteneurisation)
2. [Installation et configuration de Docker](#2-installation-et-configuration-de-docker)
3. [Concepts fondamentaux de Docker](#3-concepts-fondamentaux-de-docker)
4. [Création de Dockerfiles personnalisés](#4-création-de-dockerfiles-personnalisés)
5. [Introduction à Docker Compose](#5-introduction-à-docker-compose)
6. [Structure du projet Inception](#6-structure-du-projet-inception)
7. [Configuration des services](#7-configuration-des-services)
8. [Configuration des réseaux Docker](#8-configuration-des-réseaux-docker)
9. [Gestion des volumes et persistance des données](#9-gestion-des-volumes-et-persistance-des-données)
10. [Variables d'environnement et secrets](#10-variables-denvironnement-et-secrets)
11. [Déploiement et gestion du projet](#11-déploiement-et-gestion-du-projet)
12. [Bonnes pratiques et optimisations](#12-bonnes-pratiques-et-optimisations)
13. [Dépannage et problèmes courants](#13-dépannage-et-problèmes-courants)

## 1. Introduction à Docker et à la conteneurisation

### Qu'est-ce que Docker ?

Docker est une plateforme qui permet de développer, déployer et exécuter des applications dans des conteneurs. Ces conteneurs permettent d'emballer une application avec toutes ses dépendances dans une unité standardisée pour le développement logiciel.

### Conteneurisation vs Virtualisation

| Conteneurisation | Virtualisation traditionnelle |
|-----------------|-------------------------------|
| Partage le noyau (kernel) du système hôte | Exécute un système d'exploitation complet |
| Léger et démarre en quelques secondes | Plus lourd et peut prendre plusieurs minutes à démarrer |
| Utilise moins de ressources | Nécessite plus de ressources |
| Isolation au niveau du processus | Isolation complète au niveau matériel |

### Avantages de la conteneurisation

- **Portabilité** : Fonctionne partout où Docker est installé
- **Légèreté** : Utilise moins de ressources que les machines virtuelles
- **Cohérence** : Élimine les problèmes de "ça marche sur ma machine"
- **Isolation** : Chaque application fonctionne dans son propre environnement
- **Scalabilité** : Facilite le déploiement à grande échelle
- **Déploiement rapide** : Les conteneurs démarrent en quelques secondes

## 2. Installation et configuration de Docker

### Installation sur Linux (Debian/Ubuntu)

```bash
# Mettre à jour les paquets
sudo apt-get update

# Installer les dépendances
sudo apt-get install -y apt-transport-https ca-certificates curl software-properties-common

# Ajouter la clé GPG officielle de Docker
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo apt-key add -

# Ajouter le dépôt Docker
sudo add-apt-repository "deb [arch=amd64] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable"

# Mettre à jour les paquets
sudo apt-get update

# Installer Docker CE
sudo apt-get install -y docker-ce

# Installer Docker Compose
sudo curl -L "https://github.com/docker/compose/releases/download/v2.23.0/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
sudo chmod +x /usr/local/bin/docker-compose
```

### Vérification de l'installation

```bash
# Vérifier la version de Docker
docker --version

# Vérifier la version de Docker Compose
docker-compose --version

# Exécuter un conteneur de test
docker run hello-world
```

### Configuration post-installation

Pour utiliser Docker sans sudo (optionnel mais recommandé) :

```bash
# Créer le groupe docker s'il n'existe pas
sudo groupadd docker

# Ajouter votre utilisateur au groupe docker
sudo usermod -aG docker $USER

# Appliquer les changements de groupe
newgrp docker
```

## 3. Concepts fondamentaux de Docker

### Images Docker

Une image Docker est un modèle en lecture seule qui contient un ensemble d'instructions pour créer un conteneur Docker. Les images sont stockées dans un registre comme Docker Hub.

```bash
# Lister les images locales
docker images

# Télécharger une image
docker pull nginx:latest

# Chercher une image sur Docker Hub
docker search mariadb
```

### Conteneurs Docker

Un conteneur est une instance en cours d'exécution d'une image Docker. Il s'agit d'un environnement isolé qui exécute une application.

```bash
# Lister les conteneurs en cours d'exécution
docker ps

# Lister tous les conteneurs (y compris ceux arrêtés)
docker ps -a

# Créer et démarrer un conteneur
docker run --name mon-nginx -p 8080:80 -d nginx

# Arrêter un conteneur
docker stop mon-nginx

# Démarrer un conteneur arrêté
docker start mon-nginx

# Supprimer un conteneur
docker rm mon-nginx
```

### Cycle de vie d'un conteneur

![Cycle de vie d'un conteneur Docker](https://i.imgur.com/WU2A9Py.png)

1. **Création** : `docker create` ou `docker run`
2. **Démarrage** : `docker start` ou inclus dans `docker run`
3. **Exécution** : Le conteneur est en cours d'exécution
4. **Pause/Reprise** : `docker pause` / `docker unpause`
5. **Arrêt** : `docker stop` (SIGTERM puis SIGKILL après un délai) ou `docker kill` (SIGKILL immédiat)
6. **Suppression** : `docker rm`

### Volumes Docker

Les volumes permettent de persister les données générées et utilisées par les conteneurs.

```bash
# Créer un volume
docker volume create mon-volume

# Lister les volumes
docker volume ls

# Utiliser un volume avec un conteneur
docker run -d --name mon-conteneur -v mon-volume:/data nginx

# Monter un répertoire de l'hôte dans un conteneur
docker run -d --name mon-conteneur -v /chemin/sur/hote:/data nginx
```

### Réseaux Docker

Les réseaux Docker permettent la communication entre conteneurs et avec le monde extérieur.

```bash
# Lister les réseaux
docker network ls

# Créer un réseau
docker network create mon-reseau

# Connecter un conteneur à un réseau
docker network connect mon-reseau mon-conteneur

# Créer un conteneur dans un réseau spécifique
docker run --name mon-conteneur --network mon-reseau -d nginx
```

## 4. Création de Dockerfiles personnalisés

Un Dockerfile est un script contenant une série d'instructions pour créer une image Docker personnalisée.

### Anatomie d'un Dockerfile

```dockerfile
# Image de base
FROM debian:bullseye-slim

# Métadonnées
LABEL maintainer="votre-email@example.com"
LABEL version="1.0"

# Variables d'environnement
ENV APP_HOME /app

# Répertoire de travail
WORKDIR $APP_HOME

# Copie des fichiers
COPY . .

# Installation des dépendances
RUN apt-get update && \
    apt-get install -y nginx && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# Exposition des ports
EXPOSE 80

# Volume pour les données persistantes
VOLUME ["/data"]

# Commande par défaut au démarrage du conteneur
CMD ["nginx", "-g", "daemon off;"]

# Point d'entrée
ENTRYPOINT ["/entrypoint.sh"]
```

### Instructions courantes dans un Dockerfile

| Instruction | Description |
|-------------|-------------|
| `FROM` | Spécifie l'image de base |
| `LABEL` | Ajoute des métadonnées à l'image |
| `ENV` | Définit des variables d'environnement |
| `WORKDIR` | Définit le répertoire de travail |
| `COPY` | Copie des fichiers du système hôte vers l'image |
| `ADD` | Similaire à COPY mais avec des fonctionnalités supplémentaires (extraction d'archives, URLs) |
| `RUN` | Exécute des commandes pendant la construction de l'image |
| `EXPOSE` | Informe Docker que le conteneur écoute sur des ports spécifiés |
| `VOLUME` | Crée un point de montage pour les données persistantes |
| `CMD` | Commande par défaut à exécuter au démarrage du conteneur |
| `ENTRYPOINT` | Configure le conteneur pour qu'il s'exécute comme un exécutable |

### Construire une image à partir d'un Dockerfile

```bash
# Construire une image (depuis le répertoire contenant le Dockerfile)
docker build -t mon-image:v1 .

# Construire une image avec un Dockerfile spécifique
docker build -t mon-image:v1 -f MonDockerfile .

# Construire sans utiliser le cache
docker build --no-cache -t mon-image:v1 .
```

### Exemple simple : Dockerfile pour NGINX personnalisé

```dockerfile
FROM nginx:alpine

COPY ./conf/nginx.conf /etc/nginx/nginx.conf
COPY ./html /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
```

## 5. Introduction à Docker Compose

Docker Compose est un outil qui permet de définir et gérer des applications multi-conteneurs. Il utilise un fichier YAML pour configurer les services, réseaux et volumes.

### Fichier docker-compose.yml

```yaml
version: '3.8'

services:
  nginx:
    build: ./nginx
    ports:
      - "80:80"
    volumes:
      - ./nginx/html:/usr/share/nginx/html
    depends_on:
      - wordpress
    restart: unless-stopped
    networks:
      - inception-network

  wordpress:
    build: ./wordpress
    volumes:
      - wordpress_data:/var/www/html
    depends_on:
      - mariadb
    environment:
      - WORDPRESS_DB_HOST=mariadb
      - WORDPRESS_DB_NAME=wordpress
      - WORDPRESS_DB_USER=wp_user
      - WORDPRESS_DB_PASSWORD=wp_password
    restart: unless-stopped
    networks:
      - inception-network

  mariadb:
    build: ./mariadb
    volumes:
      - mariadb_data:/var/lib/mysql
    environment:
      - MYSQL_ROOT_PASSWORD=root_password
      - MYSQL_DATABASE=wordpress
      - MYSQL_USER=wp_user
      - MYSQL_PASSWORD=wp_password
    restart: unless-stopped
    networks:
      - inception-network

networks:
  inception-network:
    driver: bridge

volumes:
  wordpress_data:
  mariadb_data:
```

### Commandes Docker Compose

```bash
# Démarrer les services (en arrière-plan)
docker-compose up -d

# Arrêter les services
docker-compose down

# Arrêter les services et supprimer les volumes
docker-compose down -v

# Voir les logs
docker-compose logs

# Logs en continu
docker-compose logs -f

# Logs d'un service spécifique
docker-compose logs -f nginx

# Reconstruire les images
docker-compose build

# Redémarrer un service
docker-compose restart nginx

# Exécuter une commande dans un service
docker-compose exec mariadb mysql -u root -p
```

## 6. Structure du projet Inception

Le projet Inception consiste généralement à mettre en place une infrastructure avec plusieurs services conteneurisés, communiquant entre eux à travers un réseau Docker.

### Structure de répertoires typique

```
inception/
├── .env                      # Variables d'environnement globales
├── docker-compose.yml        # Configuration des services
├── Makefile                  # Commandes pour faciliter la gestion
├── srcs/                     # Répertoire source
│   ├── requirements/         # Services et leurs configurations
│   │   ├── nginx/            # Service NGINX
│   │   │   ├── Dockerfile    # Instructions de build pour NGINX
│   │   │   ├── conf/         # Fichiers de configuration
│   │   │   └── tools/        # Scripts et outils
│   │   ├── mariadb/          # Service MariaDB
│   │   │   ├── Dockerfile    # Instructions de build pour MariaDB
│   │   │   ├── conf/         # Fichiers de configuration
│   │   │   └── tools/        # Scripts et outils
│   │   └── wordpress/        # Service WordPress
│   │       ├── Dockerfile    # Instructions de build pour WordPress
│   │       ├── conf/         # Fichiers de configuration
│   │       └── tools/        # Scripts et outils
│   └── .env                  # Variables d'environnement pour les services
└── README.md                 # Documentation du projet
```

### Exemple de Makefile

```makefile
# Variables
DOCKER_COMPOSE = docker-compose -f srcs/docker-compose.yml

# Commandes
all: up

up:
	$(DOCKER_COMPOSE) up -d

down:
	$(DOCKER_COMPOSE) down

clean: down
	$(DOCKER_COMPOSE) down -v

fclean: clean
	docker system prune -a --volumes

re: fclean all

logs:
	$(DOCKER_COMPOSE) logs -f

ps:
	$(DOCKER_COMPOSE) ps

.PHONY: all up down clean fclean re logs ps
```

## 7. Configuration des services

### NGINX

NGINX agit comme un serveur web et un proxy inverse pour rediriger les requêtes vers les services appropriés.

#### Dockerfile pour NGINX

```dockerfile
FROM alpine:3.16

RUN apk update && \
    apk add --no-cache nginx openssl && \
    mkdir -p /run/nginx

# Configuration SSL
RUN mkdir -p /etc/nginx/ssl && \
    openssl req -x509 -nodes -days 365 -newkey rsa:2048 \
    -keyout /etc/nginx/ssl/nginx.key \
    -out /etc/nginx/ssl/nginx.crt \
    -subj "/C=FR/ST=IDF/L=Paris/O=42/OU=42/CN=login.42.fr"

# Copier les fichiers de configuration
COPY conf/nginx.conf /etc/nginx/nginx.conf

# Exposer le port 443 (HTTPS)
EXPOSE 443

# Démarrer NGINX en mode foreground
CMD ["nginx", "-g", "daemon off;"]
```

#### Configuration NGINX (nginx.conf)

```nginx
user nginx;
worker_processes auto;
error_log /var/log/nginx/error.log warn;
pid /var/run/nginx.pid;

events {
    worker_connections 1024;
}

http {
    include /etc/nginx/mime.types;
    default_type application/octet-stream;

    log_format main '$remote_addr - $remote_user [$time_local] "$request" '
                    '$status $body_bytes_sent "$http_referer" '
                    '"$http_user_agent" "$http_x_forwarded_for"';

    access_log /var/log/nginx/access.log main;
    sendfile on;
    keepalive_timeout 65;

    server {
        listen 443 ssl;
        server_name login.42.fr;

        ssl_certificate /etc/nginx/ssl/nginx.crt;
        ssl_certificate_key /etc/nginx/ssl/nginx.key;
        ssl_protocols TLSv1.2 TLSv1.3;

        root /var/www/html;
        index index.php index.html;

        location / {
            try_files $uri $uri/ /index.php?$args;
        }

        location ~ \.php$ {
            fastcgi_split_path_info ^(.+\.php)(/.+)$;
            fastcgi_pass wordpress:9000;
            fastcgi_index index.php;
            include fastcgi_params;
            fastcgi_param SCRIPT_FILENAME $document_root$fastcgi_script_name;
            fastcgi_param PATH_INFO $fastcgi_path_info;
        }
    }
}
```

### MariaDB

MariaDB est un système de gestion de base de données relationnelle dérivé de MySQL.

#### Dockerfile pour MariaDB

```dockerfile
FROM alpine:3.16

RUN apk update && \
    apk add --no-cache mariadb mariadb-client && \
    mkdir -p /run/mysqld && \
    chown -R mysql:mysql /run/mysqld && \
    mkdir -p /var/lib/mysql

# Copier les fichiers de configuration
COPY conf/my.cnf /etc/mysql/my.cnf
COPY tools/init-db.sh /docker-entrypoint-initdb.d/

# Exposer le port de MariaDB
EXPOSE 3306

# Définir les permissions
RUN chmod +x /docker-entrypoint-initdb.d/init-db.sh

# Démarrer MariaDB
CMD ["sh", "-c", "/docker-entrypoint-initdb.d/init-db.sh && mysqld --user=mysql --console"]
```

#### Script d'initialisation (init-db.sh)

```bash
#!/bin/sh

# Vérifier si la base de données est déjà initialisée
if [ -d "/var/lib/mysql/mysql" ]; then
    echo "Database already initialized"
else
    # Initialiser la base de données
    mysql_install_db --user=mysql --datadir=/var/lib/mysql

    # Démarrer MariaDB temporairement
    mysqld --user=mysql --bootstrap << EOF
USE mysql;
FLUSH PRIVILEGES;

# Créer l'utilisateur root
ALTER USER 'root'@'localhost' IDENTIFIED BY '${MYSQL_ROOT_PASSWORD}';
GRANT ALL PRIVILEGES ON *.* TO 'root'@'localhost' WITH GRANT OPTION;
FLUSH PRIVILEGES;

# Créer la base de données et l'utilisateur WordPress
CREATE DATABASE IF NOT EXISTS ${MYSQL_DATABASE};
CREATE USER IF NOT EXISTS '${MYSQL_USER}'@'%' IDENTIFIED BY '${MYSQL_PASSWORD}';
GRANT ALL PRIVILEGES ON ${MYSQL_DATABASE}.* TO '${MYSQL_USER}'@'%';
FLUSH PRIVILEGES;
EOF

    echo "Database initialized"
fi

# Configurer MariaDB pour accepter les connexions externes
sed -i "s/skip-networking/#skip-networking/g" /etc/mysql/my.cnf

echo "MariaDB setup completed"
```

#### Configuration MariaDB (my.cnf)

```ini
[mysqld]
user = mysql
datadir = /var/lib/mysql
port = 3306
bind-address = 0.0.0.0
skip-networking = false

[mysql]
user = root
password = ${MYSQL_ROOT_PASSWORD}
```

### WordPress

WordPress est un système de gestion de contenu (CMS) populaire.

#### Dockerfile pour WordPress

```dockerfile
FROM alpine:3.16

RUN apk update && \
    apk add --no-cache php8 php8-fpm php8-mysqli php8-json php8-curl \
    php8-dom php8-exif php8-fileinfo php8-mbstring php8-openssl php8-xml \
    php8-zip php8-gd curl

# Installer WordPress CLI
RUN curl -O https://raw.githubusercontent.com/wp-cli/builds/gh-pages/phar/wp-cli.phar && \
    chmod +x wp-cli.phar && \
    mv wp-cli.phar /usr/local/bin/wp

# Créer le répertoire pour WordPress
RUN mkdir -p /var/www/html

# Copier les fichiers de configuration
COPY conf/www.conf /etc/php8/php-fpm.d/www.conf
COPY tools/setup-wordpress.sh /usr/local/bin/
RUN chmod +x /usr/local/bin/setup-wordpress.sh

# Exposer le port de PHP-FPM
EXPOSE 9000

# Démarrer WordPress
CMD ["sh", "-c", "/usr/local/bin/setup-wordpress.sh && php-fpm8 -F"]
```

#### Script d'initialisation (setup-wordpress.sh)

```bash
#!/bin/sh

# Vérifier si WordPress est déjà installé
if [ -f "/var/www/html/wp-config.php" ]; then
    echo "WordPress already installed"
else
    # Télécharger WordPress
    wp core download --path=/var/www/html --allow-root

    # Attendre que MariaDB soit prêt
    until mysqladmin ping -h${WORDPRESS_DB_HOST} -u${WORDPRESS_DB_USER} -p${WORDPRESS_DB_PASSWORD} --silent; do
        echo "Waiting for MariaDB..."
        sleep 2
    done

    # Créer le fichier de configuration WordPress
    wp config create \
        --path=/var/www/html \
        --dbname=${WORDPRESS_DB_NAME} \
        --dbuser=${WORDPRESS_DB_USER} \
        --dbpass=${WORDPRESS_DB_PASSWORD} \
        --dbhost=${WORDPRESS_DB_HOST} \
        --allow-root

    # Installer WordPress
    wp core install \
        --path=/var/www/html \
        --url=https://login.42.fr \
        --title="Inception" \
        --admin_user=${WORDPRESS_ADMIN_USER} \
        --admin_password=${WORDPRESS_ADMIN_PASSWORD} \
        --admin_email=${WORDPRESS_ADMIN_EMAIL} \
        --allow-root

    # Créer un utilisateur supplémentaire
    wp user create \
        ${WORDPRESS_USER} \
        ${WORDPRESS_EMAIL} \
        --role=author \
        --user_pass=${WORDPRESS_PASSWORD} \
        --path=/var/www/html \
        --allow-root

    echo "WordPress installed successfully"
fi

# Définir les permissions
chown -R nobody:nobody /var/www/html

echo "WordPress setup completed"
```

#### Configuration PHP-FPM (www.conf)

```ini
[www]
user = nobody
group = nobody
listen = 9000
pm = dynamic
pm.max_children = 5
pm.start_servers = 2
pm.min_spare_servers = 1
pm.max_spare_servers = 3
```

## 8. Configuration des réseaux Docker

### Types de réseaux Docker

| Type | Description |
|------|-------------|
| `bridge` | Réseau par défaut pour les conteneurs sur un même hôte |
| `host` | Supprime l'isolation réseau entre le conteneur et l'hôte |
| `none` | Aucune connectivité externe pour les conteneurs |
| `overlay` | Réseau distribué entre plusieurs hôtes Docker |
| `macvlan` | Assigne une adresse MAC à un conteneur |
| `ipvlan` | Similaire à macvlan mais sans adresse MAC unique |

### Configuration réseau dans docker-compose.yml

```yaml
networks:
  inception-network:
    driver: bridge
    ipam:
      config:
        - subnet: 172.20.0.0/16
```

### Bonnes pratiques pour les réseaux

1. **Isoler les services** : Utiliser des réseaux dédiés pour isoler les groupes de services
2. **Nommer les réseaux** : Utiliser des noms significatifs pour les réseaux
3. **Limiter l'exposition des ports** : N'exposer que les ports nécessaires
4. **Communication entre conteneurs** : Utiliser les noms de service pour la communication interne

```yaml
services:
  wordpress:
    # ...
    networks:
      - frontend
      - backend
  
  nginx:
    # ...
    networks:
      - frontend
  
  mariadb:
    # ...
    networks:
      - backend

networks:
  frontend:
    driver: bridge
  backend:
    driver: bridge
```

## 9. Gestion des volumes et persistance des données

### Types de volumes Docker

| Type | Description | Syntaxe |
|------|-------------|---------|
| **Volumes nommés** | Gérés par Docker, stockés dans `/var/lib/docker/volumes` | `volume_name:/container/path` |
| **Bind mounts** | Montages du système de fichiers hôte | `/host/path:/container/path` |
| **tmpfs mounts** | Stockés en mémoire (non persistants) | `tmpfs:/container/path` |

### Configuration des volumes dans docker-compose.yml

```yaml
services:
  wordpress:
    # ...
    volumes:
      - wordpress_data:/var/www/html
  
  mariadb:
    # ...
    volumes:
      - mariadb_data:/var/lib/mysql

volumes:
  wordpress_data:
    driver: local
    driver_opts:
      type: none
      device: /home/login/data/wordpress
      o: bind
  mariadb_data:
    driver: local
    driver_opts:
      type: none
      device: /home/login/data/mariadb
      o: bind
```

### Bonnes pratiques pour les volumes

1. **Utiliser des volumes nommés** : Ils sont plus faciles à gérer et à sauvegarder
2. **Monter en lecture seule quand possible** : `ro` pour réduire les risques
3. **Séparer les données d'application et de configuration** : Volumes distincts
4. **Définir clairement les points de montage** : Structure cohérente

```yaml
volumes:
  wordpress_data:
    name: inception_wordpress_data
  mariadb_data:
    name: inception_mariadb_data
```

## 10. Variables d'environnement et secrets

### Utilisation de variables d'environnement

#### Fichier .env

```env
# MariaDB
MYSQL_ROOT_PASSWORD=rootpassword123
MYSQL_DATABASE=wordpress
MYSQL_USER=wp_user
MYSQL_PASSWORD=wp_password123

# WordPress
WORDPRESS_DB_HOST=mariadb
WORDPRESS_DB_NAME=wordpress
WORDPRESS_DB_USER=wp_user
WORDPRESS_DB_PASSWORD=wp_password123
WORDPRESS_ADMIN_USER=admin
WORDPRESS_ADMIN_PASSWORD=admin_password123
WORDPRESS_ADMIN_EMAIL=admin@example.com
WORDPRESS_USER=user
WORDPRESS_PASSWORD=user_password123
WORDPRESS_EMAIL=user@example.com

# Domain
DOMAIN_NAME=login.42.fr
```

#### Référence dans docker-compose.yml

```yaml
services:
  wordpress:
    # ...
    environment:
      - WORDPRESS_DB_HOST=${WORDPRESS_DB_HOST}
      - WORDPRESS_DB_NAME=${WORDPRESS_DB_NAME}
      - WORDPRESS_DB_USER=${WORDPRESS_DB_USER}
      - WORDPRESS_DB_PASSWORD=${WORDPRESS_DB_PASSWORD}
      - WORDPRESS_ADMIN_USER=${WORDPRESS_ADMIN_USER}
      - WORDPRESS_ADMIN_PASSWORD=${WORDPRESS_ADMIN_PASSWORD}
      - WORDPRESS_ADMIN_EMAIL=${WORDPRESS_ADMIN_EMAIL}
      - WORDPRESS_USER=${WORDPRESS_USER}
      - WORDPRESS_PASSWORD=${WORDPRESS_PASSWORD}
      - WORDPRESS_EMAIL=${WORDPRESS_EMAIL}
```

### Gestion des secrets

Pour les projets qui nécessitent une sécurité accrue, Docker Swarm propose une gestion des secrets :

```bash
# Créer un secret
echo "secret_password" | docker secret create db_password -

# Utiliser le secret dans docker-compose.yml (mode Swarm)
services:
  mariadb:
    # ...
    secrets:
      - db_password
    environment:
      - MYSQL_ROOT_PASSWORD_FILE=/run/secrets/db_password

secrets:
  db_password:
    external: true
```

## 11. Déploiement et gestion du projet

### Déploiement initial

```bash
# Cloner le projet
git clone https://github.com/votre-nom/inception.git
cd inception

# Créer les répertoires pour les volumes
mkdir -p /home/login/data/wordpress /home/login/data/mariadb

# Démarrer les services
make
```

### Gestion quotidienne

```bash
# Voir l'état des services
docker-compose ps

# Voir les logs
docker-compose logs -f

# Redémarrer un service spécifique
docker-compose restart nginx

# Arrêter tous les services
docker-compose down

# Redémarrer tous les services
docker-compose up -d
```

### Sauvegarde et restauration

```bash
# Sauvegarde de la base de données
docker-compose exec mariadb sh -c 'mysqldump -u root -p"$MYSQL_ROOT_PASSWORD" wordpress' > backup.sql

# Restauration de la base de données
cat backup.sql | docker-compose exec -T mariadb sh -c 'mysql -u root -p"$MYSQL_ROOT_PASSWORD" wordpress'
```

## 12. Bonnes pratiques et optimisations

### Optimisation des images Docker

1. **Utiliser des images de base légères** : Alpine plutôt qu'Ubuntu
2. **Réduire le nombre de couches** : Combiner les commandes RUN
3. **Nettoyer après l'installation** : Supprimer les caches et fichiers temporaires
4. **Multi-stage builds** : Séparer la compilation et l'exécution

```dockerfile
# Exemple d'optimisation
RUN apk update && \
    apk add --no-cache package1 package2 && \
    rm -rf /var/cache/apk/*
```

### Sécurité

1. **Ne jamais exécuter en tant que root** : Utiliser des utilisateurs non privilégiés
2. **Scanner les images** : Utiliser des outils comme Docker Scan ou Trivy
3. **Limiter les capacités** : Restreindre les capacités du conteneur
4. **Mettre à jour régulièrement** : Garder les images à jour

```yaml
services:
  app:
    # ...
    user: nobody
    cap_drop:
      - ALL
    cap_add:
      - NET_BIND_SERVICE
```

### Performance

1. **Limiter les ressources** : Définir des limites de CPU et mémoire
2. **Optimiser les volumes** : Utiliser des bind mounts pour les données fréquemment accédées
3. **Healthchecks** : Implémenter des vérifications de santé pour les services

```yaml
services:
  wordpress:
    # ...
    deploy:
      resources:
        limits:
          cpus: '0.5'
          memory: 512M
    healthcheck:
      test: ["CMD", "curl", "-f", "http://localhost"]
      interval: 30s
      timeout: 10s
      retries: 3
```

## 13. Dépannage et problèmes courants

### Problèmes de réseau

| Problème | Solution |
|----------|----------|
| **Les conteneurs ne peuvent pas communiquer** | Vérifier qu'ils sont dans le même réseau Docker |
| **Ports déjà utilisés** | Changer le mapping des ports ou arrêter les services en conflit |
| **Impossible d'accéder au service** | Vérifier les règles de pare-feu et la configuration nginx |

### Problèmes de volumes

| Problème | Solution |
|----------|----------|
| **Permissions insuffisantes** | Vérifier les propriétaires et les permissions des répertoires |
| **Volumes manquants** | Vérifier si les volumes sont correctement créés et montés |
| **Données corrompues** | Restaurer à partir d'une sauvegarde |

### Problèmes de démarrage

| Problème | Solution |
|----------|----------|
| **Service qui redémarre en boucle** | Vérifier les logs avec `docker-compose logs service_name` |
| **Erreurs de configuration** | Vérifier les fichiers de configuration et variables d'environnement |
| **Dépendances non satisfaites** | Vérifier l'ordre de démarrage avec `depends_on` |

### Commandes de débogage utiles

```bash
# Inspecter un conteneur
docker inspect container_id

# Exécuter une commande dans un conteneur en cours d'exécution
docker-compose exec service_name command

# Voir les logs d'un conteneur
docker logs -f container_id

# Voir l'utilisation des ressources
docker stats
```

---

## Conclusion

Ce guide vous a fourni une introduction complète à Docker et à la mise en place du projet Inception. Vous avez appris :

- Les concepts fondamentaux de Docker et de la conteneurisation
- Comment créer et gérer des Dockerfiles personnalisés
- L'utilisation de Docker Compose pour orchestrer des applications multi-conteneurs
- La configuration des services essentiels (NGINX, MariaDB, WordPress)
- La gestion des réseaux, volumes et variables d'environnement
- Les bonnes pratiques et techniques de dépannage

Continuez à explorer et à expérimenter pour approfondir vos connaissances en conteneurisation !

## Ressources supplémentaires

- [Documentation officielle de Docker](https://docs.docker.com/)
- [Documentation de Docker Compose](https://docs.docker.com/compose/)
- [Hub Docker](https://hub.docker.com/)
- [Bonnes pratiques pour les Dockerfiles](https://docs.docker.com/develop/develop-images/dockerfile_best-practices/)
- [Guide de sécurité Docker](https://docs.docker.com/engine/security/)
