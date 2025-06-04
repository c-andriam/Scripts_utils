# 🐳 Guide Docker Mastery - Approfondissement Complet et Simplifié

## 📋 Plan d'Approfondissement
1. [Les Fondamentaux - Analogies Simples](#1-les-fondamentaux---analogies-simples)
2. [Images Docker - Comprendre en Profondeur](#2-images-docker---comprendre-en-profondeur)
3. [Conteneurs Docker - Maîtrise Complète](#3-conteneurs-docker---maîtrise-complète)
4. [Volumes Docker - Persistance de Données](#4-volumes-docker---persistance-de-données)
5. [Réseaux Docker - Communication Inter-Conteneurs](#5-réseaux-docker---communication-inter-conteneurs)
6. [Dockerfile - Création d'Images Personnalisées](#6-dockerfile---création-dimages-personnalisées)
7. [Docker Compose - Orchestration Multi-Conteneurs](#7-docker-compose---orchestration-multi-conteneurs)
8. [Sécurité Docker - Bonnes Pratiques](#8-sécurité-docker---bonnes-pratiques)
9. [Performance et Optimisation](#9-performance-et-optimisation)
10. [Debugging et Troubleshooting](#10-debugging-et-troubleshooting)
11. [Exercices Pratiques Progressifs](#11-exercices-pratiques-progressifs)

---

## 1. **Les Fondamentaux - Analogies Simples**

### **🏠 Analogie de l'Immeuble**
Imagine Docker comme un **immeuble moderne** :

- **L'immeuble** = Ton serveur/machine
- **Les appartements** = Les conteneurs Docker
- **Le plan d'architecte** = Les images Docker
- **Le syndic** = Docker Engine
- **Les parties communes** = Volumes partagés
- **Le réseau électrique/internet** = Réseaux Docker

### **📦 Analogie de l'Expédition**
- **Image Docker** = **Modèle/Template** d'un colis standard Amazon
- **Conteneur** = **Colis réel** expédié avec ton produit dedans
- **Dockerfile** = **Instructions de conditionnement** du colis
- **Docker Hub** = **Entrepôt** où sont stockés tous les modèles de colis

### **🔧 En Pratique**
```bash
# Tu "télécharges" un modèle de colis (image)
docker pull nginx:latest

# Tu "remplis" le colis et l'expédies (conteneur)
docker run -d --name mon-serveur nginx:latest

# Tu vérifies que ton "colis" est bien arrivé
docker ps
```

---

## 2. **Images Docker - Comprendre en Profondeur**

### **🧠 Concept Mental**
Une image Docker est un **système de fichiers en couches** (layers) qui contient :
- Le système d'exploitation de base
- Les applications installées
- Les fichiers de configuration
- Les métadonnées

### **🍰 Analogie des Couches (Layers)**
Imagine une image comme un **gâteau à étages** :
```
┌─────────────────────┐ ← Couche 4: Ton app
├─────────────────────┤ ← Couche 3: Configuration
├─────────────────────┤ ← Couche 2: NGINX installé  
├─────────────────────┤ ← Couche 1: Paquets de base
└─────────────────────┘ ← Couche 0: Debian 11
```

### **💡 Pourquoi des Couches ?**
- **Réutilisation** : Si 10 images utilisent Debian 11, cette couche n'est stockée qu'une fois
- **Efficacité** : Seules les couches modifiées sont re-téléchargées
- **Rapidité** : Construction plus rapide grâce au cache

### **🔍 Explorer une Image**
```bash
# Voir l'historique des couches
docker history nginx:latest

# Inspecter tous les détails d'une image
docker inspect nginx:latest

# Voir les couches d'une image
docker image inspect nginx:latest | grep -A 20 "RootFS"
```

### **📊 Exercice Pratique - Analyser une Image**
```bash
# 1. Télécharge l'image
docker pull debian:11

# 2. Regarde sa taille
docker images debian:11

# 3. Regarde son historique
docker history debian:11

# 4. Compare avec Alpine
docker pull alpine:latest
docker images alpine:latest
docker history alpine:latest

# Question : Pourquoi Alpine est plus petite ?
# Réponse : Alpine utilise musl libc au lieu de glibc, et est optimisée pour la taille
```

---

## 3. **Conteneurs Docker - Maîtrise Complète**

### **🚀 Cycle de Vie d'un Conteneur**
```
Created → Running → Paused → Stopped → Deleted
   ↑         ↓         ↓         ↓         ↓
docker   docker    docker    docker    docker
create    run      pause     stop      rm
```

### **🧪 États d'un Conteneur**
```bash
# État: Created (créé mais pas démarré)
docker create --name test nginx
docker ps -a  # Status: Created

# État: Running (en cours d'exécution)
docker start test
docker ps     # Status: Up

# État: Paused (suspendu)
docker pause test
docker ps     # Status: Up (Paused)

# État: Stopped (arrêté)
docker unpause test
docker stop test
docker ps -a  # Status: Exited

# État: Deleted (supprimé)
docker rm test
```

### **🔧 Modes de Lancement**

#### **Mode Détaché (-d)**
```bash
# Lance en arrière-plan
docker run -d --name web nginx
# Tu récupères immédiatement la main sur le terminal
```

#### **Mode Interactif (-it)**
```bash
# Lance avec un terminal interactif
docker run -it --name shell debian:11 bash
# Tu es "à l'intérieur" du conteneur
```

#### **Mode Temporaire (--rm)**
```bash
# Le conteneur se supprime automatiquement à l'arrêt
docker run --rm -it debian:11 bash
exit  # Le conteneur disparaît automatiquement
```

### **📡 Redirection de Ports**
```bash
# Syntaxe: -p [IP:]PORT_HOTE:PORT_CONTENEUR
docker run -d -p 8080:80 nginx                    # Port 8080 → 80
docker run -d -p 127.0.0.1:8080:80 nginx         # Seulement localhost
docker run -d -p 8080:80 -p 8443:443 nginx       # Plusieurs ports
```

### **💻 Exercice - Maîtriser les Conteneurs**
```bash
# 1. Lance un serveur web temporaire
docker run --rm -d -p 8080:80 --name temp-web nginx

# 2. Vérifie qu'il fonctionne
curl http://localhost:8080

# 3. Regarde ses logs en temps réel
docker logs -f temp-web

# 4. Exécute une commande à l'intérieur
docker exec -it temp-web bash
ls /usr/share/nginx/html/
exit

# 5. Arrête le conteneur (il se supprime automatiquement)
docker stop temp-web

# 6. Vérifie qu'il a disparu
docker ps -a | grep temp-web  # Aucun résultat
```

---

## 4. **Volumes Docker - Persistance de Données**

### **💾 Problème Sans Volumes**
```bash
# Lance un conteneur MariaDB
docker run -d --name test-db \
  -e MYSQL_ROOT_PASSWORD=password123 \
  mariadb:10.6

# Crée une base de données
docker exec -it test-db mysql -p
CREATE DATABASE ma_base;
exit

# Supprime le conteneur
docker rm -f test-db

# Relance un nouveau conteneur
docker run -d --name test-db2 \
  -e MYSQL_ROOT_PASSWORD=password123 \
  mariadb:10.6

# Ta base de données a disparu ! 😱
```

### **💡 Solution : Les Volumes**

#### **Types de Volumes Expliqués**

1. **Volume Nommé** (Recommandé)
```bash
# Docker gère complètement le stockage
docker volume create ma-donnee
docker run -d -v ma-donnee:/data --name app ubuntu
```

2. **Bind Mount** (Dossier local)
```bash
# Tu pointes vers un dossier de ton disque
docker run -d -v /home/c-andriam/data:/data --name app ubuntu
```

3. **Volume Temporaire** (tmpfs)
```bash
# Stockage en mémoire (rapide mais temporaire)
docker run -d --tmpfs /cache:rw,size=100m --name app ubuntu
```

### **🗂️ Analogie des Volumes**
- **Volume Nommé** = **Coffre-fort à la banque** (sécurisé, géré par Docker)
- **Bind Mount** = **Classeur dans ton bureau** (tu contrôles, tu vois directement)
- **tmpfs** = **Post-it** (rapide mais temporaire)

### **📋 Gestion des Volumes**
```bash
# Créer un volume
docker volume create mon-volume

# Lister les volumes
docker volume ls

# Inspecter un volume (voir où il est stocké)
docker volume inspect mon-volume

# Utiliser un volume
docker run -d -v mon-volume:/var/lib/mysql \
  -e MYSQL_ROOT_PASSWORD=password123 \
  --name persistent-db mariadb:10.6

# Supprimer un volume (attention : données perdues !)
docker volume rm mon-volume
```

### **🏋️ Exercice - Persistance des Données**
```bash
# 1. Crée un volume pour la base de données
docker volume create wordpress-db

# 2. Lance MariaDB avec ce volume
docker run -d \
  --name wp-database \
  -v wordpress-db:/var/lib/mysql \
  -e MYSQL_ROOT_PASSWORD=rootpass123 \
  -e MYSQL_DATABASE=wordpress \
  -e MYSQL_USER=wpuser \
  -e MYSQL_PASSWORD=wppass123 \
  mariadb:10.6

# 3. Attends que la DB soit prête (30 secondes)
sleep 30

# 4. Vérifie que la base existe
docker exec -it wp-database mysql -u wpuser -pwppass123 -e "SHOW DATABASES;"

# 5. Arrête et supprime le conteneur
docker rm -f wp-database

# 6. Relance un nouveau conteneur avec le MÊME volume
docker run -d \
  --name wp-database-new \
  -v wordpress-db:/var/lib/mysql \
  -e MYSQL_ROOT_PASSWORD=rootpass123 \
  mariadb:10.6

# 7. Vérifie que tes données sont toujours là ! 🎉
sleep 20
docker exec -it wp-database-new mysql -u wpuser -pwppass123 -e "SHOW DATABASES;"

# Nettoyage
docker rm -f wp-database-new
docker volume rm wordpress-db
```

---

## 5. **Réseaux Docker - Communication Inter-Conteneurs**

### **🌐 Types de Réseaux Docker**

#### **Bridge (Par défaut)**
```bash
# Réseau par défaut pour les conteneurs
docker network ls  # Tu verras 'bridge'
```

#### **Host**
```bash
# Le conteneur utilise directement le réseau de l'hôte
docker run --network host nginx
# NGINX sera accessible directement sur le port 80 de ta machine
```

#### **None**
```bash
# Aucun réseau (conteneur isolé)
docker run --network none alpine
```

### **🏠 Analogie des Réseaux**
- **Bridge** = **WiFi d'appartement** (conteneurs communiquent entre eux, sortent par la box)
- **Host** = **Ethernet direct** (conteneur branché directement sur ton réseau)
- **None** = **Mode avion** (aucune communication)

### **🔗 Communication Entre Conteneurs**

#### **Problème : Communication par IP**
```bash
# Lance 2 conteneurs
docker run -d --name web nginx
docker run -d --name app alpine sleep 3600

# Trouve l'IP du serveur web
docker inspect web | grep IPAddress
# IP : 172.17.0.2 (par exemple)

# Test depuis l'app
docker exec app wget -qO- http://172.17.0.2
# ❌ Problème : L'IP peut changer !
```

#### **Solution : Réseau Personnalisé**
```bash
# Crée un réseau personnalisé
docker network create mon-reseau

# Lance les conteneurs sur ce réseau
docker run -d --network mon-reseau --name web nginx
docker run -d --network mon-reseau --name app alpine sleep 3600

# Maintenant ils communiquent par nom !
docker exec app wget -qO- http://web
# ✅ Ça marche ! Le nom "web" est résolu automatiquement
```

### **📡 Exercice - Stack LAMP Complète**
```bash
# 1. Crée un réseau pour l'application
docker network create lamp-network

# 2. Lance la base de données
docker run -d \
  --network lamp-network \
  --name database \
  -e MYSQL_ROOT_PASSWORD=rootpass \
  -e MYSQL_DATABASE=webapp \
  -e MYSQL_USER=webuser \
  -e MYSQL_PASSWORD=webpass \
  mariadb:10.6

# 3. Lance PHP (qui se connectera à 'database:3306')
docker run -d \
  --network lamp-network \
  --name backend \
  -e DATABASE_HOST=database \
  -e DATABASE_NAME=webapp \
  -e DATABASE_USER=webuser \
  -e DATABASE_PASSWORD=webpass \
  php:8.1-apache

# 4. Lance un reverse proxy NGINX
docker run -d \
  --network lamp-network \
  --name frontend \
  -p 8080:80 \
  nginx:alpine

# 5. Teste la communication
docker exec backend ping -c 3 database    # Backend → Database
docker exec frontend ping -c 3 backend    # Frontend → Backend

# 6. Inspecte le réseau
docker network inspect lamp-network

# Nettoyage
docker rm -f database backend frontend
docker network rm lamp-network
```

---

## 6. **Dockerfile - Création d'Images Personnalisées**

### **🏗️ Architecture d'un Dockerfile**

#### **Ordre Optimal des Instructions**
```dockerfile
# 1. Base (change rarement)
FROM debian:11

# 2. Métadonnées (change rarement)
LABEL maintainer="c-andriam@student.42.fr"
LABEL version="1.0"

# 3. Variables d'environnement (change rarement)
ENV DEBIAN_FRONTEND=noninteractive
ENV APP_ENV=production

# 4. Installation de paquets (change parfois)
RUN apt-get update && apt-get install -y \
    nginx \
    php-fpm \
    && rm -rf /var/lib/apt/lists/*

# 5. Configuration (change parfois)
COPY nginx.conf /etc/nginx/nginx.conf
COPY php.ini /etc/php/8.1/fpm/php.ini

# 6. Code applicatif (change souvent)
COPY ./app /var/www/html

# 7. Point d'entrée (change rarement)
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
```

### **🎯 Instructions Avancées**

#### **HEALTHCHECK - Surveillance de Santé**
```dockerfile
# Vérifie que le service répond
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
  CMD curl -f http://localhost/ || exit 1

# Alternative avec wget
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
  CMD wget --no-verbose --tries=1 --spider http://localhost/ || exit 1
```

#### **ARG - Variables de Build**
```dockerfile
# Défini une variable pour le build
ARG PHP_VERSION=8.1
ARG APP_ENV=production

# Utilise la variable
FROM php:${PHP_VERSION}-fpm
ENV APP_ENV=${APP_ENV}

# Build avec variables personnalisées :
# docker build --build-arg PHP_VERSION=8.2 --build-arg APP_ENV=development .
```

#### **ONBUILD - Instructions Différées**
```dockerfile
# Instructions exécutées seulement quand cette image est utilisée comme base
FROM php:8.1-fpm
ONBUILD COPY . /var/www/html
ONBUILD RUN composer install --no-dev

# Quand quelqu'un fait "FROM mon-image-php", les ONBUILD s'exécutent
```

### **🚀 Multi-Stage Builds (Optimisation)**
```dockerfile
# Stage 1: Build (image lourde avec outils de développement)
FROM node:18 AS builder
WORKDIR /app
COPY package*.json ./
RUN npm ci --only=production
COPY . .
RUN npm run build

# Stage 2: Production (image légère)
FROM nginx:alpine AS production
COPY --from=builder /app/dist /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]

# Résultat : Image finale ne contient que NGINX + les fichiers buildés
# (pas Node.js ni les sources)
```

### **🔥 Exercice - Dockerfile WordPress Optimisé**
```dockerfile
# Crée ce Dockerfile dans un dossier 'wordpress-custom'
FROM debian:11

# Évite les questions interactives
ENV DEBIAN_FRONTEND=noninteractive

# Met à jour et installe PHP-FPM + extensions WordPress
RUN apt-get update && apt-get install -y \
    php8.2-fpm \
    php8.2-mysql \
    php8.2-curl \
    php8.2-gd \
    php8.2-xml \
    php8.2-zip \
    wget \
    unzip \
    && rm -rf /var/lib/apt/lists/*

# Télécharge WordPress
WORKDIR /var/www/html
RUN wget https://wordpress.org/latest.tar.gz \
    && tar xzf latest.tar.gz --strip-components=1 \
    && rm latest.tar.gz \
    && chown -R www-data:www-data /var/www/html

# Configuration PHP-FPM
COPY php-fpm.conf /etc/php/8.2/fpm/pool.d/www.conf

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
  CMD php-fpm8.2 -t || exit 1

# Expose PHP-FPM
EXPOSE 9000

# Lance PHP-FPM en premier plan
CMD ["php-fpm8.2", "--nodaemonize"]
```

```bash
# Build et test
cd wordpress-custom
docker build -t wordpress-custom:latest .
docker run -d -p 9000:9000 --name wp-test wordpress-custom:latest
docker logs wp-test
docker rm -f wp-test
```

---

## 7. **Docker Compose - Orchestration Multi-Conteneurs**

### **🎼 Analogie de l'Orchestre**
- **docker-compose.yml** = **Partition musicale** (définit qui joue quoi et quand)
- **services** = **Musiciens** (chaque conteneur a son rôle)
- **networks** = **Acoustique de la salle** (comment ils s'entendent)
- **volumes** = **Instruments** (outils persistants)
- **docker-compose up** = **Chef d'orchestre** (démarre tout en harmonie)

### **📝 Structure Complète d'un docker-compose.yml**
```yaml
version: '3.8'  # Version du format Compose

# Définit les services (= conteneurs)
services:
  # Service 1: Base de données
  database:
    image: mariadb:10.6                    # Image à utiliser
    container_name: wp-database             # Nom du conteneur
    restart: unless-stopped                 # Politique de redémarrage
    environment:                           # Variables d'environnement
      MYSQL_ROOT_PASSWORD: ${DB_ROOT_PASS}
      MYSQL_DATABASE: ${DB_NAME}
      MYSQL_USER: ${DB_USER}
      MYSQL_PASSWORD: ${DB_PASS}
    volumes:                               # Montage de volumes
      - db_data:/var/lib/mysql
      - ./config/mysql.cnf:/etc/mysql/conf.d/custom.cnf
    networks:                              # Réseaux à rejoindre
      - app-network
    ports:                                 # Ports à exposer (optionnel)
      - "3306:3306"
    healthcheck:                           # Vérification de santé
      test: ["CMD", "mysqladmin", "ping", "-h", "localhost"]
      interval: 30s
      timeout: 10s
      retries: 3
      start_period: 30s

  # Service 2: Application WordPress
  wordpress:
    build:                                 # Build depuis Dockerfile
      context: ./wordpress
      dockerfile: Dockerfile
      args:                                # Arguments de build
        PHP_VERSION: 8.2
    container_name: wp-app
    restart: unless-stopped
    depends_on:                            # Démarre après database
      - database
    environment:
      WORDPRESS_DB_HOST: database:3306
      WORDPRESS_DB_NAME: ${DB_NAME}
      WORDPRESS_DB_USER: ${DB_USER}
      WORDPRESS_DB_PASSWORD: ${DB_PASS}
    volumes:
      - wp_data:/var/www/html
      - ./config/php.ini:/usr/local/etc/php/php.ini
    networks:
      - app-network

  # Service 3: Serveur web NGINX
  nginx:
    build: ./nginx
    container_name: wp-nginx
    restart: unless-stopped
    depends_on:
      - wordpress
    ports:
      - "80:80"
      - "443:443"
    volumes:
      - wp_data:/var/www/html:ro            # :ro = lecture seule
      - ./config/nginx.conf:/etc/nginx/nginx.conf
      - ./ssl:/etc/nginx/ssl
    networks:
      - app-network

# Définit les volumes nommés
volumes:
  db_data:                                 # Volume géré par Docker
    driver: local
  wp_data:                                 # Volume géré par Docker
    driver: local

# Définit les réseaux
networks:
  app-network:                             # Réseau personnalisé
    driver: bridge
    ipam:                                  # Configuration IP
      config:
        - subnet: 172.20.0.0/16
```

### **🌍 Variables d'Environnement (.env)**
```bash
# Crée un fichier .env
DB_ROOT_PASS=super_secret_root_password
DB_NAME=wordpress
DB_USER=wp_user
DB_PASS=wp_password_123
```

### **⚡ Commandes Docker Compose Avancées**
```bash
# Lance tous les services
docker-compose up                          # Premier plan
docker-compose up -d                       # Arrière-plan

# Lance des services spécifiques
docker-compose up database wordpress       # Seulement DB + WP

# Build et lance
docker-compose up --build                  # Rebuild les images

# Scale des services
docker-compose up --scale wordpress=3      # 3 instances de WordPress

# Arrête tout
docker-compose down                        # Arrête et supprime conteneurs
docker-compose down -v                     # + supprime volumes
docker-compose down --rmi all              # + supprime images

# Gestion individuelle
docker-compose start database              # Démarre un service
docker-compose stop nginx                  # Arrête un service
docker-compose restart wordpress           # Redémarre un service

# Logs
docker-compose logs                        # Tous les logs
docker-compose logs -f wordpress           # Logs WordPress en temps réel
docker-compose logs --tail=50 database     # 50 dernières lignes

# Exécution de commandes
docker-compose exec wordpress bash         # Terminal dans WordPress
docker-compose exec database mysql -p      # MySQL dans la base

# État des services
docker-compose ps                          # État des services
docker-compose top                         # Processus en cours
```

### **🏆 Exercice - Stack Complète WordPress**
```bash
# 1. Crée la structure
mkdir wordpress-stack
cd wordpress-stack
mkdir -p {nginx,wordpress,config,ssl}
```

```yaml
# 2. Crée docker-compose.yml
version: '3.8'

services:
  database:
    image: mariadb:10.6
    container_name: wp-db
    restart: unless-stopped
    environment:
      MYSQL_ROOT_PASSWORD: rootpass123
      MYSQL_DATABASE: wordpress
      MYSQL_USER: wpuser
      MYSQL_PASSWORD: wppass123
    volumes:
      - db_data:/var/lib/mysql
    networks:
      - wp-network

  wordpress:
    image: wordpress:6.0-php8.1-fpm
    container_name: wp-app
    restart: unless-stopped
    depends_on:
      - database
    environment:
      WORDPRESS_DB_HOST: database:3306
      WORDPRESS_DB_NAME: wordpress
      WORDPRESS_DB_USER: wpuser
      WORDPRESS_DB_PASSWORD: wppass123
    volumes:
      - wp_data:/var/www/html
    networks:
      - wp-network

  nginx:
    image: nginx:alpine
    container_name: wp-nginx
    restart: unless-stopped
    depends_on:
      - wordpress
    ports:
      - "8080:80"
    volumes:
      - wp_data:/var/www/html:ro
      - ./config/nginx.conf:/etc/nginx/nginx.conf
    networks:
      - wp-network

volumes:
  db_data:
  wp_data:

networks:
  wp-network:
    driver: bridge
```

```nginx
# 3. Crée config/nginx.conf
events {
    worker_connections 1024;
}

http {
    upstream php {
        server wordpress:9000;
    }

    server {
        listen 80;
        root /var/www/html;
        index index.php;

        location / {
            try_files $uri $uri/ /index.php?$args;
        }

        location ~ \.php$ {
            fastcgi_pass php;
            fastcgi_index index.php;
            fastcgi_param SCRIPT_FILENAME $document_root$fastcgi_script_name;
            include fastcgi_params;
        }
    }
}
```

```bash
# 4. Lance la stack
docker-compose up -d

# 5. Vérifie que tout fonctionne
docker-compose ps
curl http://localhost:8080

# 6. Accède à WordPress
# Ouvre http://localhost:8080 dans ton navigateur

# 7. Regarde les logs
docker-compose logs -f

# 8. Nettoyage
docker-compose down -v
```

---

## 8. **Sécurité Docker - Bonnes Pratiques**

### **🔒 Principe Fondamental**
**"Least Privilege"** = Donner le minimum de droits nécessaires

### **👤 Gestion des Utilisateurs**

#### **❌ Problème : Root par Défaut**
```dockerfile
FROM debian:11
RUN apt-get update && apt-get install -y nginx
# Par défaut, tout s'exécute en root = DANGEREUX
```

#### **✅ Solution : Utilisateur Non-Root**
```dockerfile
FROM debian:11

# Crée un utilisateur dédié
RUN groupadd -r appgroup && useradd -r -g appgroup appuser

# Installe les paquets (en root)
RUN apt-get update && apt-get install -y nginx

# Change vers l'utilisateur non-root
USER appuser

# Toutes les commandes suivantes s'exécutent avec 'appuser'
CMD ["nginx", "-g", "daemon off;"]
```

### **🚫 Conteneurs Privilégiés**
```bash
# ❌ DANGEREUX : Accès complet au système hôte
docker run --privileged alpine

# ✅ SÉCURISÉ : Accès limité
docker run --user 1001:1001 alpine

# ✅ Lecture seule
docker run --read-only alpine

# ✅ Limiter les capacités
docker run --cap-drop=ALL --cap-add=NET_BIND_SERVICE nginx
```

### **🔐 Gestion des Secrets**

#### **❌ Mauvaises Pratiques**
```dockerfile
# NE JAMAIS FAIRE ÇA !
ENV MYSQL_ROOT_PASSWORD=super_secret_123
ENV API_KEY=sk-1234567890abcdef
```

#### **✅ Bonnes Pratiques**
```yaml
# docker-compose.yml avec secrets
version: '3.8'
services:
  database:
    image: mariadb:10.6
    environment:
      MYSQL_ROOT_PASSWORD_FILE: /run/secrets/db_root_password
    secrets:
      - db_root_password

secrets:
  db_root_password:
    file: ./secrets/db_root_password.txt
```

```bash
# Crée le fichier secret
mkdir secrets
echo "super_secret_password_123" > secrets/db_root_password.txt
chmod 600 secrets/db_root_password.txt

# Ajoute à .gitignore
echo "secrets/" >> .gitignore
```

### **🛡️ Hardening des Images**

#### **Scan de Sécurité**
```bash
# Scanne une image pour les vulnérabilités
docker scan nginx:latest

# Alternative avec Trivy
docker run --rm -v /var/run/docker.sock:/var/run/docker.sock \
  aquasec/trivy image nginx:latest
```

#### **Images Minimales**
```dockerfile
# ✅ Utilise des images minimales
FROM alpine:latest          # Au lieu de ubuntu
FROM node:alpine            # Au lieu de node:latest
FROM scratch                # Image complètement vide
```

### **🔥 Exercice - Sécurisation d'une App**
```dockerfile
# Dockerfile sécurisé pour une app Node.js
FROM node:18-alpine

# Crée un utilisateur non-root
RUN addgroup -g 1001 -S nodejs && \
    adduser -S -D -H -u 1001 -s /sbin/nologin -G nodejs nodejs

# Crée le dossier de l'app et change le propriétaire
WORKDIR /app
RUN chown nodejs:nodejs /app

# Copie et installe les dépendances (en root pour npm install)
COPY package*.json ./
RUN npm ci --only=production && \
    npm cache clean --force

# Copie le code source
COPY --chown=nodejs:nodejs . .

# Bascule vers l'utilisateur non-root
USER nodejs

# Expose le port (>1024 car non-root)
EXPOSE 3000

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
  CMD wget --no-verbose --tries=1 --spider http://localhost:3000/health || exit 1

# Lance l'application
CMD ["node", "server.js"]
```

---

## 9. **Performance et Optimisation**

### **📊 Optimisation des Images**

#### **🏗️ Build Context Optimization**
```bash
# .dockerignore (comme .gitignore pour Docker)
node_modules/
npm-debug.log
.git/
.gitignore
README.md
.env
.nyc_output
coverage/
.nyc_output
```

#### **📦 Réduction de la Taille**
```dockerfile
# ❌ Mauvais : Plusieurs couches
FROM debian:11
RUN apt-get update
RUN apt-get install -y nginx
RUN apt-get clean
RUN rm -rf /var/lib/apt/lists/*

# ✅ Bon : Une seule couche optimisée
FROM debian:11
RUN apt-get update && \
    apt-get install -y nginx && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/* && \
    rm -rf /tmp/* && \
    rm -rf /var/tmp/*
```

#### **🎯 Multi-Stage pour Production**
```dockerfile
# Stage 1: Build (image complète avec outils)
FROM node:18 AS builder
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build
RUN npm prune --production

# Stage 2: Production (image minimale)
FROM node:18-alpine AS production
WORKDIR /app
COPY --from=builder /app/node_modules ./node_modules
COPY --from=builder /app/dist ./dist
COPY --from=builder /app/package.json ./
USER 1001
EXPOSE 3000
CMD ["node", "dist/server.js"]
```

### **⚡ Optimisation Runtime**

#### **🎛️ Limitation des Ressources**
```bash
# Limite la mémoire
docker run -m 512m nginx

# Limite le CPU
docker run --cpus="1.5" nginx

# Limite les I/O
docker run --device-read-bps /dev/sda:1mb nginx
```

```yaml
# docker-compose.yml avec limites
version: '3.8'
services:
  web:
    image: nginx
    deploy:
      resources:
        limits:
          cpus: '0.5'
          memory: 512M
        reservations:
          cpus: '0.25'
          memory: 256M
```

#### **📈 Monitoring des Performances**
```bash
# Statistiques temps réel
docker stats

# Statistiques d'un conteneur spécifique
docker stats nginx-container

# Informations détaillées sur l'utilisation des ressources
docker system df
docker system df -v

# Processus dans un conteneur
docker exec nginx-container ps aux
```

### **🔍 Exercice - Optimisation d'Image**
```bash
# 1. Crée une app simple
mkdir optimize-test
cd optimize-test

# Dockerfile non-optimisé
cat > Dockerfile.bad << 'EOF'
FROM ubuntu:20.04
RUN apt-get update
RUN apt-get install -y python3
RUN apt-get install -y python3-pip
RUN apt-get install -y curl
RUN apt-get install -y wget
COPY app.py /app/app.py
WORKDIR /app
CMD ["python3", "app.py"]
EOF

# App Python simple
cat > app.py << 'EOF'
print("Hello from optimized Docker!")
import time
while True:
    time.sleep(30)
EOF

# Build l'image non-optimisée
docker build -f Dockerfile.bad -t app:bad .
docker images app:bad  # Note la taille

# Dockerfile optimisé
cat > Dockerfile.good << 'EOF'
FROM python:3.9-alpine
COPY app.py /app/app.py
WORKDIR /app
RUN adduser -D -s /bin/sh appuser
USER appuser
CMD ["python", "app.py"]
EOF

# Build l'image optimisée
docker build -f Dockerfile.good -t app:good .
docker images app:good  # Compare la taille !

# Nettoyage
docker rmi app:bad app:good
```

---

## 10. **Debugging et Troubleshooting**

### **🔍 Diagnostics de Base**

#### **📋 Informations Système**
```bash
# État général de Docker
docker info
docker version

# Utilisation de l'espace disque
docker system df
docker system df -v

# Processus Docker
docker system events      # Événements en temps réel
```

#### **🩺 Diagnostic d'un Conteneur**
```bash
# Informations complètes
docker inspect container_name

# Logs détaillés
docker logs container_name
docker logs -f container_name          # Temps réel
docker logs --tail 50 container_name   # 50 dernières lignes
docker logs --since 2023-06-04T10:00:00 container_name

# Processus dans le conteneur
docker exec container_name ps aux
docker top container_name

# Statistiques de performance
docker stats container_name
```

### **🚨 Problèmes Courants et Solutions**

#### **Problème 1 : Conteneur qui s'arrête immédiatement**
```bash
# Diagnostic
docker ps -a  # Voir le statut "Exited"
docker logs container_name

# Causes fréquentes :
# - CMD qui se termine (pas de processus en premier plan)
# - Erreur dans le script de démarrage
# - Permissions incorrectes

# Solution : Lancer en mode interactif pour déboguer
docker run -it --entrypoint /bin/bash image_name
```

#### **Problème 2 : Permission Denied**
```bash
# Diagnostic
docker exec container_name ls -la /path/to/file

# Solutions possibles :
# 1. Fixer les permissions dans le Dockerfile
RUN chmod +x /app/script.sh

# 2. Changer le propriétaire
RUN chown -R appuser:appgroup /app

# 3. Utiliser le bon utilisateur
USER appuser
```

#### **Problème 3 : Réseau - Conteneurs ne communiquent pas**
```bash
# Diagnostic réseau
docker network ls
docker network inspect network_name

# Test de connectivité
docker exec container1 ping container2
docker exec container1 nslookup container2

# Solution : Vérifier qu'ils sont sur le même réseau
docker run --network my-network container1
docker run --network my-network container2
```

#### **Problème 4 : Volume non monté**
```bash
# Diagnostic volumes
docker volume ls
docker volume inspect volume_name
docker exec container_name ls -la /mount/point

# Vérifier le montage
docker inspect container_name | grep -A 10 "Mounts"
```

### **🛠️ Outils de Debug Avancés**

#### **Mode Debug avec Shell**
```bash
# Entre dans un conteneur en cours
docker exec -it container_name /bin/bash

# Lance un conteneur en mode debug
docker run -it --entrypoint /bin/bash image_name

# Lance avec un shell différent
docker run -it --entrypoint /bin/sh alpine_image
```

#### **Outils de Monitoring**
```bash
# htop dans un conteneur
docker exec -it container_name htop

# netstat pour les connexions réseau
docker exec container_name netstat -tulpn

# Taille des dossiers
docker exec container_name du -sh /*
```

### **📱 Exercice - Debug d'une App Cassée**
```bash
# 1. Lance une app "cassée"
docker run -d --name broken-app \
  -e DATABASE_URL=postgresql://wrong:url@nowhere:5432/db \
  --restart unless-stopped \
  nginx

# 2. Diagnostic étape par étape
echo "=== État du conteneur ==="
docker ps -a | grep broken-app

echo "=== Logs du conteneur ==="
docker logs broken-app

echo "=== Inspection détaillée ==="
docker inspect broken-app | grep -A 5 -B 5 "ExitCode"

echo "=== Test interactif ==="
docker exec -it broken-app /bin/bash
# Dans le conteneur :
# - ps aux
# - ls -la /etc/nginx/
# - nginx -t
# - exit

# 3. Fix les problèmes identifiés
docker rm -f broken-app

# Lance une version corrigée
docker run -d --name fixed-app nginx
docker logs fixed-app
```

---

## 11. **Exercices Pratiques Progressifs**

### **🎯 Niveau 1 : Bases**

#### **Exercice 1.1 : Premier Conteneur**
```bash
# Objectif : Maîtriser les commandes de base
# 1. Lance un conteneur Ubuntu interactif
docker run -it ubuntu:20.04 bash

# Dans le conteneur :
apt update && apt install -y curl
curl http://httpbin.org/get
exit

# 2. Lance la même chose en mode détaché avec un nom
docker run -d --name test-ubuntu ubuntu:20.04 sleep 3600

# 3. Exécute des commandes dans le conteneur détaché
docker exec test-ubuntu apt update
docker exec -it test-ubuntu bash

# 4. Nettoyage
docker rm -f test-ubuntu
```

#### **Exercice 1.2 : Serveur Web Simple**
```bash
# Objectif : Redirection de ports et volumes
# 1. Crée une page HTML
mkdir web-content
echo "<h1>Mon Premier Site Docker!</h1>" > web-content/index.html

# 2. Lance NGINX avec volume
docker run -d \
  --name mon-site \
  -p 8080:80 \
  -v $(pwd)/web-content:/usr/share/nginx/html \
  nginx:alpine

# 3. Teste le site
curl http://localhost:8080

# 4. Modifie la page et vérifie le changement
echo "<h1>Page Modifiée!</h1>" > web-content/index.html
curl http://localhost:8080

# 5. Nettoyage
docker rm -f mon-site
rm -rf web-content
```

### **🚀 Niveau 2 : Intermédiaire**

#### **Exercice 2.1 : Dockerfile Personnalisé**
```bash
# Objectif : Créer sa première image personnalisée
mkdir custom-nginx
cd custom-nginx
```

```dockerfile
# Dockerfile
FROM nginx:alpine

# Copie une configuration personnalisée
COPY nginx.conf /etc/nginx/nginx.conf

# Copie du contenu web
COPY html/ /usr/share/nginx/html/

# Ajoute une page de santé
RUN echo '{"status":"ok","service":"custom-nginx"}' > /usr/share/nginx/html/health.json

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
  CMD wget --quiet --tries=1 --spider http://localhost/health.json || exit 1

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
```

```nginx
# nginx.conf
events {
    worker_connections 1024;
}

http {
    server {
        listen 80;
        location / {
            root /usr/share/nginx/html;
            index index.html;
        }
        
        location /health {
            alias /usr/share/nginx/html/health.json;
            add_header Content-Type application/json;
        }
    }
}
```

```bash
# Crée le contenu HTML
mkdir html
echo "<h1>Custom NGINX Container</h1><p>Built with Dockerfile!</p>" > html/index.html

# Build et test
docker build -t custom-nginx:v1.0 .
docker run -d --name test-custom -p 8080:80 custom-nginx:v1.0

# Test
curl http://localhost:8080
curl http://localhost:8080/health

# Vérifie le health check
docker ps  # Regarde la colonne STATUS

# Nettoyage
docker rm -f test-custom
cd ..
rm -rf custom-nginx
```

#### **Exercice 2.2 : Communication Multi-Conteneurs**
```bash
# Objectif : Faire communiquer 2 conteneurs
# 1. Crée un réseau
docker network create app-net

# 2. Lance une base de données
docker run -d \
  --network app-net \
  --name database \
  -e POSTGRES_PASSWORD=password123 \
  -e POSTGRES_DB=testdb \
  postgres:13

# 3. Attends que la DB soit prête
sleep 10

# 4. Lance une app qui se connecte à la DB
docker run -d \
  --network app-net \
  --name webapp \
  -p 8080:80 \
  -e DATABASE_URL=postgresql://postgres:password123@database:5432/testdb \
  nginx:alpine

# 5. Test de communication
docker exec webapp ping -c 3 database

# 6. Regarde les détails du réseau
docker network inspect app-net

# 7. Nettoyage
docker rm -f database webapp
docker network rm app-net
```

### **🏆 Niveau 3 : Avancé**

#### **Exercice 3.1 : Stack Complète avec Docker Compose**
```bash
# Objectif : Stack WordPress complète
mkdir wordpress-stack
cd wordpress-stack
```

```yaml
# docker-compose.yml
version: '3.8'

services:
  database:
    image: mariadb:10.6
    container_name: wp-database
    restart: unless-stopped
    environment:
      MYSQL_ROOT_PASSWORD: ${DB_ROOT_PASSWORD}
      MYSQL_DATABASE: ${DB_NAME}
      MYSQL_USER: ${DB_USER}
      MYSQL_PASSWORD: ${DB_PASSWORD}
    volumes:
      - db_data:/var/lib/mysql
    networks:
      - wordpress-net
    healthcheck:
      test: ["CMD", "mysqladmin", "ping", "-h", "localhost"]
      interval: 30s
      timeout: 10s
      retries: 3
      start_period: 30s

  wordpress:
    depends_on:
      - database
    image: wordpress:6.0-php8.1-fpm
    container_name: wp-app
    restart: unless-stopped
    environment:
      WORDPRESS_DB_HOST: database:3306
      WORDPRESS_DB_NAME: ${DB_NAME}
      WORDPRESS_DB_USER: ${DB_USER}
      WORDPRESS_DB_PASSWORD: ${DB_PASSWORD}
    volumes:
      - wp_data:/var/www/html
    networks:
      - wordpress-net

  nginx:
    depends_on:
      - wordpress
    build: ./nginx
    container_name: wp-nginx
    restart: unless-stopped
    ports:
      - "8080:80"
    volumes:
      - wp_data:/var/www/html:ro
    networks:
      - wordpress-net

volumes:
  db_data:
  wp_data:

networks:
  wordpress-net:
    driver: bridge
```

```bash
# .env
DB_ROOT_PASSWORD=rootpassword123
DB_NAME=wordpress
DB_USER=wpuser
DB_PASSWORD=wppassword123
```

```bash
# Crée le Dockerfile NGINX
mkdir nginx
```

```dockerfile
# nginx/Dockerfile
FROM nginx:alpine

# Copie la configuration personnalisée
COPY nginx.conf /etc/nginx/nginx.conf

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
  CMD wget --quiet --tries=1 --spider http://localhost/ || exit 1

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
```

```nginx
# nginx/nginx.conf
events {
    worker_connections 1024;
}

http {
    include /etc/nginx/mime.types;
    default_type application/octet-stream;

    upstream php {
        server wordpress:9000;
    }

    server {
        listen 80;
        root /var/www/html;
        index index.php index.html;

        location / {
            try_files $uri $uri/ /index.php?$args;
        }

        location ~ \.php$ {
            fastcgi_pass php;
            fastcgi_index index.php;
            fastcgi_param SCRIPT_FILENAME $document_root$fastcgi_script_name;
            include fastcgi_params;
        }

        location ~ /\.ht {
            deny all;
        }
    }
}
```

```bash
# Test de la stack complète
docker-compose up -d

# Vérifie que tout est lancé
docker-compose ps

# Regarde les logs
docker-compose logs -f

# Test du site
curl -I http://localhost:8080

# Accède à WordPress dans le navigateur
# http://localhost:8080

# Nettoyage
docker-compose down -v
cd ..
rm -rf wordpress-stack
```

---

## 12. **Checklist Finale de Maîtrise Docker**

### **🎯 Concepts Fondamentaux**
- [ ] Je comprends la différence entre image et conteneur
- [ ] Je sais expliquer l'architecture Docker (Engine, Registry, etc.)
- [ ] Je maîtrise le cycle de vie d'un conteneur
- [ ] Je comprends le système de couches des images

### **💻 Commandes de Base**
- [ ] docker run avec toutes ses options importantes
- [ ] docker build pour créer des images
- [ ] docker ps, docker logs, docker exec
- [ ] docker volume et docker network
- [ ] docker-compose up/down/logs/exec

### **🏗️ Dockerfile**
- [ ] Je peux écrire un Dockerfile optimisé
- [ ] Je connais toutes les instructions importantes
- [ ] Je sais utiliser les multi-stage builds
- [ ] J'applique les bonnes pratiques de sécurité

### **🎼 Docker Compose**
- [ ] Je peux créer un docker-compose.yml complet
- [ ] Je sais gérer les dépendances entre services
- [ ] Je maîtrise les volumes et réseaux avec Compose
- [ ] Je sais utiliser les variables d'environnement

### **🔒 Sécurité**
- [ ] Je n'utilise jamais root dans les conteneurs
- [ ] Je ne mets jamais de secrets dans les images
- [ ] Je scanne mes images pour les vulnérabilités
- [ ] J'applique le principe du moindre privilège

### **⚡ Performance**
- [ ] Je sais optimiser la taille de mes images
- [ ] Je limite les ressources des conteneurs
- [ ] J'utilise des images de base minimales
- [ ] Je surveille les performances avec docker stats

### **🔍 Debugging**
- [ ] Je sais diagnostiquer un conteneur qui ne démarre pas
- [ ] Je peux résoudre les problèmes de réseau
- [ ] Je maîtrise les logs et l'inspection
- [ ] Je sais utiliser les outils de monitoring

---

## 13. **Ressources pour Continuer l'Apprentissage**

### **📚 Documentation Officielle**
- [Docker Documentation](https://docs.docker.com/)
- [Docker Compose Documentation](https://docs.docker.com/compose/)
- [Dockerfile Best Practices](https://docs.docker.com/develop/develop-images/dockerfile_best-practices/)

### **🎓 Formations et Tutoriels**
- [Play with Docker](https://labs.play-with-docker.com/) - Lab gratuit en ligne
- [Docker Curriculum](https://docker-curriculum.com/) - Tutorial complet
- [Katacoda Docker Scenarios](https://katacoda.com/courses/docker) - Exercices interactifs

### **🛠️ Outils Utiles**
- [Dive](https://github.com/wagoodman/dive) - Analyser les couches d'images
- [Docker Slim](https://github.com/docker-slim/docker-slim) - Optimiser les images
- [Trivy](https://github.com/aquasecurity/trivy) - Scanner de sécurité

### **📖 Livres Recommandés**
- "Docker Deep Dive" par Nigel Poulton
- "Docker in Action" par Jeff Nickoloff
- "The Docker Book" par James Turnbull

---

**🎉 Félicitations ! Tu as maintenant toutes les clés pour maîtriser Docker complètement. Le secret maintenant : PRATIQUER, PRATIQUER, PRATIQUER !**

**Pour ton projet Inception, tu es maintenant armé pour créer une infrastructure Docker propre, sécurisée et performante. Bon courage c-andriam ! 🚀**