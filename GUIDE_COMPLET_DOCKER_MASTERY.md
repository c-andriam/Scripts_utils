# Guide Complet pour Maîtriser Docker de A à Z

## 🐳 Table des Matières
1. [Qu'est-ce que Docker ?](#1-quest-ce-que-docker)
2. [Architecture et Concepts Fondamentaux](#2-architecture-et-concepts-fondamentaux)
3. [Installation et Configuration](#3-installation-et-configuration)
4. [Les Images Docker](#4-les-images-docker)
5. [Les Conteneurs Docker](#5-les-conteneurs-docker)
6. [Les Volumes Docker](#6-les-volumes-docker)
7. [Les Réseaux Docker](#7-les-réseaux-docker)
8. [Docker Compose](#8-docker-compose)
9. [Dockerfile (Rappel et Approfondissement)](#9-dockerfile)
10. [Commandes Docker Essentielles](#10-commandes-docker-essentielles)
11. [Bonnes Pratiques et Sécurité](#11-bonnes-pratiques-et-sécurité)
12. [Debugging et Troubleshooting](#12-debugging-et-troubleshooting)
13. [Docker pour le Projet Inception](#13-docker-pour-le-projet-inception)

---

## 1. **Qu'est-ce que Docker ?**

### **Définition Simple**
Docker est une plateforme qui permet de **containeriser** des applications. Un conteneur est comme une "boîte" légère qui contient tout ce dont ton application a besoin pour fonctionner : code, runtime, libraries, variables d'environnement, etc.

### **Analogie**
- **Machine Virtuelle** = Maison complète (avec fondations, murs, toit, etc.)
- **Conteneur Docker** = Appartement dans un immeuble (partage l'infrastructure, mais isolé)

### **Avantages**
- ✅ **Portabilité** : "Ça marche sur ma machine" → "Ça marche partout"
- ✅ **Légèreté** : Plus rapide qu'une VM
- ✅ **Isolation** : Les apps ne se gênent pas entre elles
- ✅ **Reproductibilité** : Même environnement en dev/test/prod

---

## 2. **Architecture et Concepts Fondamentaux**

### **Les 4 Piliers de Docker**

#### **2.1 Docker Engine**
Le "moteur" qui fait tourner Docker sur ton système.
```bash
docker version  # Voir la version du moteur
```

#### **2.2 Images Docker**
Des "modèles" ou "templates" pour créer des conteneurs.
```bash
docker images  # Lister les images disponibles
```

#### **2.3 Conteneurs Docker**
Des instances "vivantes" créées à partir d'images.
```bash
docker ps  # Voir les conteneurs en cours
```

#### **2.4 Docker Registry (Docker Hub)**
Un "magasin" d'images Docker (comme GitHub pour le code).
```bash
docker pull nginx  # Télécharger une image depuis Docker Hub
```

### **Schéma Mental**
```
Docker Registry (Docker Hub)
    ↓ (docker pull)
Images (sur ton disque)
    ↓ (docker run)
Conteneurs (en mémoire)
```

---

## 3. **Installation et Configuration**

### **Sur Debian/Ubuntu**
```bash
# Mise à jour
sudo apt-get update

# Installation des dépendances
sudo apt-get install ca-certificates curl gnupg lsb-release

# Ajout de la clé GPG officielle de Docker
sudo mkdir -p /etc/apt/keyrings
curl -fsSL https://download.docker.com/linux/debian/gpg | sudo gpg --dearmor -o /etc/apt/keyrings/docker.gpg

# Ajout du repository Docker
echo "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/debian $(lsb_release -cs) stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null

# Installation de Docker
sudo apt-get update
sudo apt-get install docker-ce docker-ce-cli containerd.io docker-compose-plugin

# Ajouter ton utilisateur au groupe docker (évite sudo)
sudo usermod -aG docker $USER
newgrp docker

# Test d'installation
docker run hello-world
```

---

## 4. **Les Images Docker**

### **Qu'est-ce qu'une Image ?**
Une image est un **template en lecture seule** utilisé pour créer des conteneurs.

### **Commandes Essentielles**
```bash
# Lister les images
docker images
docker image ls

# Télécharger une image
docker pull debian:11
docker pull nginx:latest

# Supprimer une image
docker rmi debian:11
docker image rm nginx:latest

# Voir l'historique d'une image
docker history debian:11

# Inspecter une image
docker inspect debian:11

# Chercher des images sur Docker Hub
docker search nginx
```

### **Tags et Versions**
```bash
docker pull nginx:1.21        # Version spécifique
docker pull nginx:latest      # Dernière version
docker pull nginx:alpine      # Variante Alpine (plus légère)
```

---

## 5. **Les Conteneurs Docker**

### **Lifecycle d'un Conteneur**
```
Created → Running → Paused → Stopped → Deleted
```

### **Commandes de Base**
```bash
# Créer et lancer un conteneur
docker run [OPTIONS] IMAGE [COMMAND]
docker run -d nginx                    # En arrière-plan
docker run -it debian:11 bash         # Interactif avec terminal
docker run -p 8080:80 nginx           # Redirection de port

# Lister les conteneurs
docker ps                              # Conteneurs en cours
docker ps -a                           # Tous les conteneurs

# Démarrer/Arrêter un conteneur
docker start container_name
docker stop container_name
docker restart container_name

# Supprimer un conteneur
docker rm container_name
docker rm -f container_name            # Force (même si en cours)

# Exécuter une commande dans un conteneur en cours
docker exec -it container_name bash

# Voir les logs d'un conteneur
docker logs container_name
docker logs -f container_name          # En temps réel
```

### **Options Importantes de `docker run`**
```bash
-d, --detach          # En arrière-plan
-it                   # Interactif avec terminal
-p, --publish         # Redirection de port (host:container)
-v, --volume          # Montage de volume
--name                # Nom du conteneur
--rm                  # Supprime automatiquement à l'arrêt
-e, --env             # Variable d'environnement
--network             # Spécifier un réseau
```

### **Exemples Pratiques**
```bash
# Lancer NGINX avec redirection de port
docker run -d -p 8080:80 --name mon-nginx nginx

# Lancer un conteneur temporaire pour tester
docker run --rm -it debian:11 bash

# Lancer avec des variables d'environnement
docker run -e MYSQL_ROOT_PASSWORD=motdepasse -d mariadb:10.6
```

---

## 6. **Les Volumes Docker**

### **Pourquoi les Volumes ?**
Les conteneurs sont **éphémères** : quand ils s'arrêtent, leurs données disparaissent. Les volumes permettent de **persister les données**.

### **Types de Volumes**

#### **6.1 Volumes Nommés (Recommandé)**
```bash
# Créer un volume
docker volume create mon-volume

# Lister les volumes
docker volume ls

# Utiliser un volume
docker run -v mon-volume:/var/lib/mysql -d mariadb:10.6

# Inspecter un volume
docker volume inspect mon-volume

# Supprimer un volume
docker volume rm mon-volume
```

#### **6.2 Bind Mounts (Dossier Local)**
```bash
# Monter un dossier local dans le conteneur
docker run -v /home/user/data:/app/data -d nginx

# Avec chemin relatif
docker run -v $(pwd)/config:/etc/nginx/conf.d -d nginx
```

#### **6.3 Volumes Temporaires (tmpfs)**
```bash
# Volume en mémoire (données perdues à l'arrêt)
docker run --tmpfs /tmp:rw,noexec,nosuid,size=100m -d nginx
```

---

## 7. **Les Réseaux Docker**

### **Types de Réseaux**

#### **7.1 Bridge (Par défaut)**
Les conteneurs communiquent entre eux sur le même réseau.

#### **7.2 Host**
Le conteneur utilise directement le réseau de l'hôte.

#### **7.3 None**
Aucun réseau (conteneur isolé).

### **Commandes Réseau**
```bash
# Lister les réseaux
docker network ls

# Créer un réseau
docker network create mon-reseau

# Lancer un conteneur sur un réseau spécifique
docker run --network mon-reseau -d nginx

# Connecter un conteneur existant à un réseau
docker network connect mon-reseau container_name

# Inspecter un réseau
docker network inspect mon-reseau

# Supprimer un réseau
docker network rm mon-reseau
```

### **Communication entre Conteneurs**
```bash
# Créer un réseau
docker network create app-network

# Lancer une base de données
docker run -d --network app-network --name db mariadb:10.6

# Lancer une app qui se connecte à la DB par son nom
docker run -d --network app-network --name app mon-app
# Dans l'app, la DB est accessible via : db:3306
```

---

## 8. **Docker Compose**

### **Qu'est-ce que Docker Compose ?**
Un outil pour définir et gérer des applications **multi-conteneurs** avec un fichier YAML.

### **Structure de base d'un `docker-compose.yml`**
```yaml
version: '3.8'

services:
  web:
    build: .
    ports:
      - "8080:80"
    volumes:
      - .:/app
    environment:
      - NODE_ENV=development
    depends_on:
      - db

  db:
    image: mariadb:10.6
    environment:
      MYSQL_ROOT_PASSWORD: motdepasse
    volumes:
      - db_data:/var/lib/mysql

volumes:
  db_data:

networks:
  default:
    driver: bridge
```

### **Commandes Docker Compose**
```bash
# Lancer tous les services
docker-compose up
docker-compose up -d                   # En arrière-plan

# Arrêter tous les services
docker-compose down

# Reconstruire les images
docker-compose build

# Voir les logs
docker-compose logs
docker-compose logs web               # Logs d'un service spécifique

# Voir les services en cours
docker-compose ps

# Exécuter une commande dans un service
docker-compose exec web bash

# Redémarrer un service
docker-compose restart web
```

---

## 9. **Dockerfile (Rappel et Approfondissement)**

### **Instructions Avancées**

#### **ARG - Variables de Build**
```dockerfile
ARG VERSION=11
FROM debian:${VERSION}

ARG USER_ID=1000
RUN useradd -u ${USER_ID} appuser
```

#### **HEALTHCHECK - Vérification de Santé**
```dockerfile
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost/ || exit 1
```

#### **ONBUILD - Instructions Différées**
```dockerfile
ONBUILD COPY . /app
ONBUILD RUN npm install
```

### **Multi-Stage Builds (Optimisation)**
```dockerfile
# Stage 1: Build
FROM node:16 AS builder
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build

# Stage 2: Production
FROM nginx:alpine
COPY --from=builder /app/dist /usr/share/nginx/html
```

---

## 10. **Commandes Docker Essentielles**

### **Gestion des Images**
```bash
docker images                          # Lister
docker pull image:tag                  # Télécharger
docker build -t name:tag .             # Construire
docker rmi image:tag                   # Supprimer
docker tag source:tag target:tag       # Renommer/Tag
```

### **Gestion des Conteneurs**
```bash
docker ps                              # Lister (actifs)
docker ps -a                           # Lister (tous)
docker run [options] image             # Créer et lancer
docker start container                 # Démarrer
docker stop container                  # Arrêter
docker restart container               # Redémarrer
docker rm container                    # Supprimer
docker exec -it container bash         # Exécuter commande
docker logs container                  # Voir logs
```

### **Gestion des Volumes**
```bash
docker volume ls                       # Lister
docker volume create name             # Créer
docker volume inspect name            # Inspecter
docker volume rm name                 # Supprimer
```

### **Gestion des Réseaux**
```bash
docker network ls                      # Lister
docker network create name            # Créer
docker network inspect name           # Inspecter
docker network rm name                # Supprimer
```

### **Nettoyage**
```bash
docker system prune                   # Nettoyer tout l'inutilisé
docker image prune                    # Nettoyer images inutilisées
docker container prune                # Nettoyer conteneurs arrêtés
docker volume prune                   # Nettoyer volumes inutilisés
docker network prune                  # Nettoyer réseaux inutilisés
```

---

## 11. **Bonnes Pratiques et Sécurité**

### **Dockerfile**
- ✅ Utiliser des images de base officielles
- ✅ Minimiser le nombre de couches (RUN)
- ✅ Nettoyer les caches après installation
- ✅ Ne pas installer d'outils inutiles
- ✅ Utiliser .dockerignore
- ❌ Ne jamais mettre de secrets dans l'image

### **Sécurité**
```dockerfile
# Créer un utilisateur non-root
RUN addgroup -g 1001 appgroup && \
    adduser -D -s /bin/sh -u 1001 -G appgroup appuser
USER appuser

# Limiter les privilèges
docker run --user 1001:1001 myapp
docker run --read-only myapp
```

### **Performance**
- Utiliser des images légères (Alpine)
- Optimiser l'ordre des instructions Dockerfile
- Utiliser des multi-stage builds
- Monitorer l'utilisation des ressources

---

## 12. **Debugging et Troubleshooting**

### **Conteneur qui ne démarre pas**
```bash
# Voir les logs
docker logs container_name

# Lancer en mode interactif
docker run -it image bash

# Voir les processus
docker exec container_name ps aux
```

### **Problèmes de Réseau**
```bash
# Inspecter le réseau
docker network inspect bridge

# Tester la connectivité
docker exec container_name ping autre_container
```

### **Problèmes de Performance**
```bash
# Voir l'utilisation des ressources
docker stats

# Voir les processus Docker
docker system df
```

---

## 13. **Docker pour le Projet Inception**

### **Structure Recommandée**
```
inception/
├── Makefile
├── srcs/
│   ├── requirements/
│   │   ├── nginx/
│   │   │   ├── Dockerfile
│   │   │   ├── conf/
│   │   │   └── tools/
│   │   ├── wordpress/
│   │   │   ├── Dockerfile
│   │   │   ├── conf/
│   │   │   └── tools/
│   │   └── mariadb/
│   │       ├── Dockerfile
│   │       ├── conf/
│   │       └── tools/
│   ├── docker-compose.yml
│   └── .env
└── secrets/
```

### **Exemple docker-compose.yml pour Inception**
```yaml
version: '3.8'

services:
  mariadb:
    build: 
      context: requirements/mariadb
      dockerfile: Dockerfile
    image: mariadb
    restart: unless-stopped
    volumes:
      - mariadb_data:/var/lib/mysql
    networks:
      - inception
    environment:
      MYSQL_ROOT_PASSWORD_FILE: /run/secrets/db_root_password
      MYSQL_DATABASE: ${DB_NAME}
      MYSQL_USER: ${DB_USER}
      MYSQL_PASSWORD_FILE: /run/secrets/db_password
    secrets:
      - db_root_password
      - db_password

  nginx:
    build:
      context: requirements/nginx
      dockerfile: Dockerfile
    image: nginx
    restart: unless-stopped
    ports:
      - "443:443"
    volumes:
      - wordpress_data:/var/www/html
    networks:
      - inception
    depends_on:
      - wordpress

  wordpress:
    build:
      context: requirements/wordpress
      dockerfile: Dockerfile
    image: wordpress
    restart: unless-stopped
    volumes:
      - wordpress_data:/var/www/html
    networks:
      - inception
    depends_on:
      - mariadb
    environment:
      WORDPRESS_DB_HOST: mariadb:3306
      WORDPRESS_DB_NAME: ${DB_NAME}
      WORDPRESS_DB_USER: ${DB_USER}
      WORDPRESS_DB_PASSWORD_FILE: /run/secrets/db_password

volumes:
  mariadb_data:
    driver: local
    driver_opts:
      type: none
      o: bind
      device: /home/${USER}/data/mariadb
  wordpress_data:
    driver: local
    driver_opts:
      type: none
      o: bind
      device: /home/${USER}/data/wordpress

networks:
  inception:
    driver: bridge

secrets:
  db_root_password:
    file: ../secrets/db_root_password.txt
  db_password:
    file: ../secrets/db_password.txt
```

---

## 14. **Checklist de Maîtrise Docker**

Tu maîtrises Docker quand tu peux :
- [ ] Expliquer la différence entre image et conteneur
- [ ] Écrire un Dockerfile optimisé
- [ ] Créer et gérer des volumes pour la persistance
- [ ] Configurer des réseaux entre conteneurs
- [ ] Utiliser Docker Compose pour des apps multi-conteneurs
- [ ] Déboguer des problèmes Docker
- [ ] Appliquer les bonnes pratiques de sécurité
- [ ] Optimiser les performances et la taille des images

---

## 15. **Ressources pour Continuer**

- **Documentation officielle :** https://docs.docker.com/
- **Docker Hub :** https://hub.docker.com/
- **Practice Labs :** https://labs.play-with-docker.com/
- **Awesome Docker :** https://github.com/veggiemonk/awesome-docker

---

**🎯 Maintenant tu as toutes les clés pour maîtriser Docker ! Passe à la pratique avec ton projet Inception, et n'hésite pas si tu as des questions spécifiques !**