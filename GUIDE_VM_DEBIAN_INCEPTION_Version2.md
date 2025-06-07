# Guide Détaillé du Projet Docker "Inception" pour Débutants

Bienvenue dans ce guide ! L'objectif du projet "Inception" est de vous faire découvrir Docker en construisant une petite infrastructure web. C'est un excellent moyen d'apprendre par la pratique. N'ayez pas peur, nous allons avancer pas à pas.

**Votre Login (pour les exemples) :** `c-andriam` (Pensez à le remplacer par le vôtre si différent)

## Table des Matières
1.  [Qu'est-ce que Docker ? (Les Bases)](#1-quest-ce-que-docker--les-bases)
2.  [Mise en Place de l'Environnement](#2-mise-en-place-de-lenvironnement)
    *   [Machine Virtuelle (VM)](#machine-virtuelle-vm)
    *   [Installation de Docker et Docker Compose](#installation-de-docker-et-docker-compose)
    *   [Structure des Dossiers du Projet](#structure-des-dossiers-du-projet)
3.  [Les Fichiers Clés du Projet](#3-les-fichiers-clés-du-projet)
    *   [Le `Makefile` : Votre Télécommande](#le-makefile--votre-télécommande)
    *   [Le Fichier `.env` : Vos Configurations Secrètes](#le-fichier-env--vos-configurations-secrètes)
    *   [Le Fichier `docker-compose.yml` : L'Orchestrateur](#le-fichier-docker-composeyml--lorchestrateur)
4.  [Construction Service par Service](#4-construction-service-par-service)
    *   [Service 1 : MariaDB (La Base de Données)](#service-1--mariadb-la-base-de-données)
    *   [Service 2 : WordPress (Le Moteur du Site)](#service-2--wordpress-le-moteur-du-site)
    *   [Service 3 : Nginx (Le Serveur Web)](#service-3--nginx-le-serveur-web)
5.  [Volumes et Réseaux Docker Expliqués](#5-volumes-et-réseaux-docker-expliqués)
6.  [Configurer Votre Domaine Local (`c-andriam.42.fr`)](#6-configurer-votre-domaine-local-c-andriam42fr)
7.  [Lancer et Tester Votre Projet](#7-lancer-et-tester-votre-projet)
8.  [Quelques Bonnes Pratiques et Sécurité](#8-quelques-bonnes-pratiques-et-sécurité)
9.  [Dépannage de Base](#9-dépannage-de-base)
10. [Pour Aller Plus Loin (Partie Bonus)](#10-pour-aller-plus-loin-partie-bonus)

---

## 1. Qu'est-ce que Docker ? (Les Bases)

Imaginez Docker comme une façon d'empaqueter une application avec tout ce dont elle a besoin pour fonctionner (code, bibliothèques, outils) dans une boîte standardisée appelée **conteneur**.

*   **Dockerfile :** C'est un fichier texte qui contient les instructions pour construire une "image" Docker. C'est comme la recette de cuisine de votre application.
*   **Image Docker :** C'est le résultat de la construction d'un Dockerfile. C'est un modèle léger, autonome et exécutable. Pensez-y comme un gâteau prêt à être cuit (ou plutôt, une application prête à être lancée). Vous ne modifiez pas une image une fois construite ; si vous avez besoin de changements, vous modifiez le Dockerfile et reconstruisez l'image.
*   **Conteneur Docker :** C'est une instance en cours d'exécution d'une image Docker. Si l'image est la recette, le conteneur est le gâteau que vous êtes en train de manger. Vous pouvez en lancer plusieurs à partir de la même image.
*   **Docker Compose :** C'est un outil pour définir et gérer des applications Docker multi-conteneurs. Votre projet a Nginx, WordPress, et MariaDB : ce sont 3 conteneurs. Docker Compose vous aide à les démarrer, les arrêter et les faire communiquer ensemble facilement, à partir d'un seul fichier de configuration (`docker-compose.yml`).

**Conteneurs vs. Machines Virtuelles (VMs) :**
Une VM inclut un système d'exploitation complet, ce qui la rend lourde. Un conteneur partage le noyau du système d'exploitation de la machine hôte et isole uniquement l'application, ce qui le rend beaucoup plus léger et rapide à démarrer. Pour ce projet, vous utiliserez Docker *dans* une VM.

---

## 2. Mise en Place de l'Environnement

### Machine Virtuelle (VM)
Le projet exige que tout soit fait dans une VM. Cela isole votre projet de votre système principal.
*   **Logiciel de VM :** VirtualBox (gratuit) ou VMware Workstation Player/Fusion sont de bons choix.
*   **Système d'Exploitation pour la VM :** Une distribution Linux est nécessaire. **Ubuntu Server LTS** (Long Term Support) ou **Debian** sont d'excellents choix et très courants. Prenez l'avant-dernière version stable si possible, sinon la dernière stable.

### Installation de Docker et Docker Compose
Une fois votre VM Linux installée et fonctionnelle :

1.  **Ouvrez un terminal dans votre VM.**
2.  **Installez Docker Engine :**
    Suivez les instructions officielles pour votre distribution. Pour Debian/Ubuntu, cela ressemble généralement à :
    ```bash
    # Désinstaller les anciennes versions (si présentes)
    sudo apt-get remove docker docker-engine docker.io containerd runc

    # Mettre à jour les paquets et installer les prérequis
    sudo apt-get update
    sudo apt-get install -y apt-transport-https ca-certificates curl gnupg lsb-release

    # Ajouter la clé GPG officielle de Docker
    curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /usr/share/keyrings/docker-archive-keyring.gpg # Pour Ubuntu
    # Pour Debian: curl -fsSL https://download.docker.com/linux/debian/gpg | sudo gpg --dearmor -o /usr/share/keyrings/docker-archive-keyring.gpg

    # Configurer le dépôt stable
    echo \
      "deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/ubuntu \
      $(lsb_release -cs) stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null # Pour Ubuntu
    # Pour Debian: echo "deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/debian $(lsb_release -cs) stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null

    # Installer Docker Engine
    sudo apt-get update
    sudo apt-get install -y docker-ce docker-ce-cli containerd.io
    ```
    *   Documentation officielle : [https://docs.docker.com/engine/install/](https://docs.docker.com/engine/install/)

3.  **Gérer Docker en tant qu'utilisateur non-root (Recommandé) :**
    ```bash
    sudo groupadd docker # Peut déjà exister
    sudo usermod -aG docker $USER
    newgrp docker # Appliquer les changements de groupe, ou déconnectez-vous/reconnectez-vous
    ```
    Testez avec `docker run hello-world`.

4.  **Installez Docker Compose :**
    Docker Compose V2 est maintenant un plugin Docker. Si vous avez installé `docker-ce` récemment, il est souvent inclus. Testez avec `docker compose version`.
    Si ce n'est pas le cas, ou pour installer/mettre à jour :
    ```bash
    # Instructions pour Linux depuis GitHub (vérifiez la dernière version sur leur page)
    sudo curl -SL https://github.com/docker/compose/releases/download/v2.27.0/docker-compose-linux-x86_64 -o /usr/local/bin/docker-compose # Remplacez v2.27.0 par la dernière version
    sudo chmod +x /usr/local/bin/docker-compose

    # Pour utiliser `docker compose` (avec un espace) au lieu de `docker-compose` (avec un tiret)
    # Créez un lien symbolique si docker-compose n'est pas déjà dans un chemin comme /usr/libexec/docker/cli-plugins
    # sudo ln -s /usr/local/bin/docker-compose /usr/libexec/docker/cli-plugins/docker-compose # Le chemin peut varier
    ```
    *   Documentation officielle : [https://docs.docker.com/compose/install/](https://docs.docker.com/compose/install/)

### Structure des Dossiers du Projet
Le sujet impose une structure spécifique. Créez-la à la racine de votre projet (par exemple, dans `~/inception`).

```bash
# Assurez-vous d'être dans le dossier de votre projet, par exemple /home/c-andriam/inception
# mkdir ~/inception && cd ~/inception # Si vous partez de zéro

mkdir -p srcs/requirements/mariadb/conf
mkdir -p srcs/requirements/mariadb/tools
mkdir -p srcs/requirements/nginx/conf
mkdir -p srcs/requirements/nginx/tools
mkdir -p srcs/requirements/wordpress/tools # WordPress n'a pas de sous-dossier conf dans l'exemple

mkdir secrets

# Créez les fichiers de base
touch Makefile
touch srcs/docker-compose.yml
touch srcs/.env
touch srcs/requirements/mariadb/Dockerfile
touch srcs/requirements/nginx/Dockerfile
touch srcs/requirements/wordpress/Dockerfile
# Fichiers .dockerignore (optionnel mais bonne pratique)
touch srcs/requirements/mariadb/.dockerignore
touch srcs/requirements/nginx/.dockerignore
touch srcs/requirements/wordpress/.dockerignore
```
Le dossier `data` pour les volumes (`/home/c-andriam/data/mariadb` et `/home/c-andriam/data/wordpress`) sera créé par Docker si vous utilisez un "bind mount" et qu'il n'existe pas, mais c'est une bonne pratique de le créer manuellement et de s'assurer des permissions.

```bash
mkdir -p /home/c-andriam/data/mariadb
mkdir -p /home/c-andriam/data/wordpress
```

---

## 3. Les Fichiers Clés du Projet

### Le `Makefile` : Votre Télécommande
Un `Makefile` vous permet d'automatiser des commandes courantes. C'est très pratique.
Placez ce fichier à la racine de votre projet.

```makefile name=Makefile
# Variable pour le fichier docker-compose
COMPOSE_FILE = srcs/docker-compose.yml

# Cible par défaut, exécute 'build' puis 'up'
all: build up

# Construit les images Docker, --no-cache force la reconstruction à partir de zéro
# Utile si vous modifiez un Dockerfile ou un fichier copié dedans
build:
	@echo "Construction des images Docker..."
	docker compose -f $(COMPOSE_FILE) build --no-cache

# Démarre les services en mode "detached" (en arrière-plan)
up:
	@echo "Démarrage des services..."
	docker compose -f $(COMPOSE_FILE) up -d

# Arrête les services
down:
	@echo "Arrêt des services..."
	docker compose -f $(COMPOSE_FILE) down

# Arrête les services ET supprime les volumes (ATTENTION: supprime les données !)
# Utile pour un nettoyage complet. --remove-orphans supprime les conteneurs pour les services non définis.
clean:
	@echo "Arrêt des services et suppression des volumes..."
	docker compose -f $(COMPOSE_FILE) down -v --remove-orphans

# Nettoyage complet: clean + suppression des images construites pour ce projet
fclean: clean
	@echo "Suppression des images Docker du projet..."
	docker compose -f $(COMPOSE_FILE) down --rmi all --remove-orphans
	# Alternative plus ciblée si vous avez nommé vos images :
	# docker rmi mariadb_image wordpress_image nginx_image || true

# Règle pour reconstruire et redémarrer proprement
re: fclean all

# Afficher les logs des conteneurs en direct
logs:
	docker compose -f $(COMPOSE_FILE) logs -f

# Afficher l'état des conteneurs
status:
	docker compose -f $(COMPOSE_FILE) ps

# Indique à make que ces cibles ne sont pas des fichiers
.PHONY: all build up down clean fclean re logs status
```
**Comment l'utiliser ?** Dans votre terminal, à la racine du projet :
*   `make build` : Construit les images.
*   `make up` : Démarre tout.
*   `make logs` : Affiche les journaux.
*   `make down` : Arrête tout.
*   `make fclean` : Nettoie tout (y compris les images).
*   `make re` : Nettoie tout et redémarre.

### Le Fichier `.env` : Vos Configurations Secrètes
Ce fichier, situé dans `srcs/.env`, contiendra vos variables d'environnement : mots de passe, noms d'utilisateur, nom de domaine, etc.
**TRÈS IMPORTANT :** Ce fichier ne doit JAMAIS être partagé publiquement (par exemple, sur Git). Ajoutez `.env` à votre fichier `.gitignore`.

```dotenv name=srcs/.env
# Remplacez c-andriam par votre login
DOMAIN_NAME=c-andriam.42.fr

# Configuration MariaDB/MySQL
MYSQL_DATABASE=wordpress_db
MYSQL_USER=wp_user
MYSQL_PASSWORD=ChangeMe_StrongPassword123! # Changez ceci par un mot de passe FORT
MYSQL_ROOT_PASSWORD=ChangeMe_EvenStrongerRootPassword456! # Changez ceci par un mot de passe ROOT FORT

# Configuration WordPress Admin (le sujet interdit "admin" comme nom d'utilisateur)
WP_ADMIN_USER=MyCoolAdminUser # Changez ceci
WP_ADMIN_PASSWORD=ChangeMe_AdminWordPressPassword789! # Changez ceci
WP_ADMIN_EMAIL=user@example.com # Changez ceci

# Clés de sécurité WordPress (générez les vôtres sur https://api.wordpress.org/secret-key/1.1/salt/)
# Exemple (NE PAS UTILISER CEUX-CI, GÉNÉREZ LES VÔTRES) :
AUTH_KEY='put your unique phrase here'
SECURE_AUTH_KEY='put your unique phrase here'
LOGGED_IN_KEY='put your unique phrase here'
NONCE_KEY='put your unique phrase here'
AUTH_SALT='put your unique phrase here'
SECURE_AUTH_SALT='put your unique phrase here'
LOGGED_IN_SALT='put your unique phrase here'
NONCE_SALT='put your unique phrase here'

# Autres variables si besoin
# ...
```
**Générez vos propres clés de sécurité WordPress !** C'est crucial pour la sécurité.

### Le Fichier `docker-compose.yml` : L'Orchestrateur
Ce fichier, dans `srcs/docker-compose.yml`, décrit comment vos services (MariaDB, WordPress, Nginx) fonctionnent ensemble.

```yaml name=srcs/docker-compose.yml
version: '3.8' # Version de la syntaxe Docker Compose

services:
  # Le service MariaDB sera défini ici
  # Le service WordPress sera défini ici
  # Le service Nginx sera défini ici

volumes:
  # Les volumes pour stocker les données persistantes seront définis ici
  # (données de la base de données, fichiers WordPress)

networks:
  # Le réseau pour que les conteneurs communiquent sera défini ici
```
Nous remplirons ce fichier au fur et à mesure que nous définissons chaque service.

---

## 4. Construction Service par Service

Pour chaque service, nous allons :
1.  Écrire un `Dockerfile` pour construire son image.
2.  Préparer les fichiers de configuration ou scripts nécessaires.
3.  Ajouter sa définition au fichier `docker-compose.yml`.

**Choix de l'image de base (Alpine ou Debian) :**
Le sujet demande l'avant-dernière version stable d'Alpine ou Debian.
*   **Alpine Linux :** Très léger (quelques Mo), ce qui rend vos images petites. Moins d'outils préinstallés, donc vous devrez peut-être en ajouter plus manuellement. Excellent pour la production.
*   **Debian :** Plus complet qu'Alpine, plus familier pour beaucoup. Les images sont plus grosses.
Pour ce projet, Alpine est un bon choix pour s'entraîner à minimiser les images. Vérifiez les versions actuelles sur Docker Hub (ex: `alpine:3.17` si `3.18` est la dernière stable et `3.19` en edge/testing - adaptez au moment où vous faites le projet).

**Interdiction du tag `latest` :**
Toujours spécifier une version exacte pour vos images de base (ex: `alpine:3.17`, `debian:bullseye-slim`). Le tag `latest` peut changer sans prévenir et casser votre build.

### Service 1 : MariaDB (La Base de Données)

MariaDB stockera toutes les données de votre site WordPress.

1.  **`srcs/requirements/mariadb/Dockerfile` :**

    ```dockerfile name=srcs/requirements/mariadb/Dockerfile
    # Choisissez l'avant-dernière version stable d'Alpine (ex: 3.17 si 3.18 est stable et 3.19 est edge)
    # Vérifiez sur Docker Hub pour la version correcte au moment où vous faites le projet.
    ARG ALPINE_VERSION=3.18 # Exemple, adaptez !
    FROM alpine:${ALPINE_VERSION}

    # Variables d'environnement (peuvent être surchargées par docker-compose)
    # Ces valeurs par défaut ne seront utilisées que si non fournies par docker-compose
    ENV MYSQL_DATABASE=wordpress_db
    ENV MYSQL_USER=wp_user
    ENV MYSQL_PASSWORD=default_password
    ENV MYSQL_ROOT_PASSWORD=default_root_password

    # Installer MariaDB et les outils nécessaires
    RUN apk update && \
        apk add --no-cache \
            mariadb \
            mariadb-client \
            pwgen # Pour générer des mots de passe si besoin (optionnel ici) \
            bash # Pour le script d'init si besoin de bashisms

    # Copier le fichier de configuration MariaDB personnalisé
    # Ce fichier permet à MariaDB d'écouter sur toutes les interfaces réseau dans le conteneur
    COPY ./conf/my.cnf /etc/mysql/my.cnf
    # Assurez-vous que le répertoire existe si my.cnf est dans un sous-dossier de /etc/mysql/
    # RUN mkdir -p /etc/mysql/conf.d/ # Si my.cnf est dans conf.d

    # Copier le script d'initialisation
    COPY ./tools/init_db.sh /usr/local/bin/init_db.sh
    RUN chmod +x /usr/local/bin/init_db.sh

    # Créer le répertoire pour les données de la base de données et définir les permissions
    # Le volume sera monté ici.
    RUN mkdir -p /var/lib/mysql && \
        chown -R mysql:mysql /var/lib/mysql

    # Exposer le port standard de MariaDB (ne le publie pas sur l'hôte, juste pour la communication entre conteneurs)
    EXPOSE 3306

    # Script d'entrée qui initialise la base si besoin, puis démarre MariaDB
    ENTRYPOINT ["/usr/local/bin/init_db.sh"]

    # Commande par défaut pour démarrer MariaDB en avant-plan
    # Cette commande sera appelée par le script init_db.sh après l'initialisation.
    CMD ["mariadbd", "--user=mysql", "--console", "--skip-name-resolve", "--skip-networking=0"]
    ```

2.  **`srcs/requirements/mariadb/conf/my.cnf` :**
    Ce fichier configure MariaDB pour écouter les connexions de l'extérieur du conteneur (mais toujours à l'intérieur du réseau Docker).

    ```ini name=srcs/requirements/mariadb/conf/my.cnf
    [mysqld]
    user = mysql
    port = 3306
    datadir = /var/lib/mysql
    socket = /run/mysqld/mysqld.sock
    bind-address = 0.0.0.0 # Important: écouter sur toutes les interfaces réseau du conteneur

    # Améliorations optionnelles (peuvent nécessiter plus de RAM)
    # key_buffer_size = 16M
    # max_allowed_packet = 128M
    # sort_buffer_size = 512K
    # net_buffer_length = 8K
    # read_buffer_size = 256K
    # read_rnd_buffer_size = 512K
    # myisam_sort_buffer_size = 8M

    [client]
    port = 3306
    socket = /run/mysqld/mysqld.sock
    default-character-set = utf8mb4

    [mariadb]
    default-character-set = utf8mb4
    ```
    Assurez-vous que le `socket` est correct. Pour Alpine, MariaDB peut utiliser `/run/mysqld/mysqld.sock`. Vérifiez l'emplacement après installation dans un conteneur test si besoin.

3.  **`srcs/requirements/mariadb/tools/init_db.sh` :**
    Ce script s'exécute au démarrage du conteneur MariaDB. Il initialise la base de données et crée l'utilisateur WordPress *uniquement si la base de données n'existe pas encore*.

    ```bash name=srcs/requirements/mariadb/tools/init_db.sh
    #!/bin/bash
    set -e # Arrête le script si une commande échoue

    # Répertoire où MariaDB stocke ses données
    DATADIR="/var/lib/mysql"

    # Vérifie si la base de données a déjà été initialisée
    if [ ! -d "$DATADIR/$MYSQL_DATABASE" ]; then
        echo "Base de données '$MYSQL_DATABASE' non trouvée. Initialisation..."

        # Initialiser la structure de base de MariaDB
        # mariadb-install-db crée les tables système (mysql, information_schema, etc.)
        mariadb-install-db --user=mysql --datadir="$DATADIR" --skip-test-db

        # Démarrer MariaDB temporairement en arrière-plan pour la configuration
        mariadbd --user=mysql --datadir="$DATADIR" --skip-networking --socket=/run/mysqld/mysqld.sock &
        MARIADB_PID=$!

        # Attendre que MariaDB démarre (boucle de vérification simple)
        echo "Attente du démarrage de MariaDB pour la configuration..."
        max_attempts=30
        attempt_num=0
        until mysqladmin ping --socket=/run/mysqld/mysqld.sock >/dev/null 2>&1 || [ $attempt_num -ge $max_attempts ]; do
            echo -n "."
            sleep 1
            attempt_num=$((attempt_num+1))
        done

        if ! mysqladmin ping --socket=/run/mysqld/mysqld.sock >/dev/null 2>&1; then
            echo "MariaDB n'a pas pu démarrer pour la configuration."
            kill $MARIADB_PID # Tenter de tuer le processus MariaDB
            wait $MARIADB_PID 2>/dev/null
            exit 1
        fi
        echo "MariaDB démarré pour la configuration."

        # Exécuter les commandes SQL pour créer la base, l'utilisateur et définir les mots de passe
        # Utilisation de "here-document" pour passer plusieurs commandes SQL
        mysql --user=root --socket=/run/mysqld/mysqld.sock <<-EOSQL
            ALTER USER 'root'@'localhost' IDENTIFIED BY '${MYSQL_ROOT_PASSWORD}';
            CREATE DATABASE IF NOT EXISTS \`${MYSQL_DATABASE}\` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
            CREATE USER IF NOT EXISTS '${MYSQL_USER}'@'%' IDENTIFIED BY '${MYSQL_PASSWORD}';
            GRANT ALL PRIVILEGES ON \`${MYSQL_DATABASE}\`.* TO '${MYSQL_USER}'@'%';
            FLUSH PRIVILEGES;
EOSQL
        # L'utilisateur est créé avec '@'%' pour autoriser les connexions depuis n'importe quel hôte
        # (dans le contexte de notre réseau Docker, cela signifie depuis le conteneur WordPress).

        echo "Base de données et utilisateur créés."

        # Arrêter le serveur MariaDB temporaire
        if ! mysqladmin -u root -p"${MYSQL_ROOT_PASSWORD}" --socket=/run/mysqld/mysqld.sock shutdown; then
            echo "Arrêt de MariaDB avec mysqladmin a échoué. Tentative avec kill."
            kill $MARIADB_PID
        fi
        wait $MARIADB_PID 2>/dev/null # Attendre que le processus soit terminé
        echo "Serveur MariaDB temporaire arrêté."
    else
        echo "Base de données '$MYSQL_DATABASE' déjà initialisée."
    fi

    echo "Démarrage du serveur MariaDB principal..."
    # Exécute la commande CMD du Dockerfile (mariadbd en avant-plan)
    exec "$@"
    ```
    **Note sur `MYSQL_USER@'%'` :** Le `%` signifie que l'utilisateur `wp_user` peut se connecter depuis n'importe quelle adresse IP. Dans notre réseau Docker privé, c'est sûr et permet au conteneur WordPress (qui aura sa propre IP dans ce réseau) de se connecter.

4.  **Ajouter MariaDB à `srcs/docker-compose.yml` :**

    ```yaml name=srcs/docker-compose.yml
    version: '3.8'

    services:
      mariadb:
        build:
          context: ./requirements/mariadb # Chemin vers le dossier contenant le Dockerfile de MariaDB
          dockerfile: Dockerfile
          args: # Vous pouvez passer des arguments de build au Dockerfile si besoin
            ALPINE_VERSION: 3.18 # Assurez-vous que cela correspond à ce que vous voulez
        image: mariadb_image_c-andriam # Nom de l'image construite (le sujet dit nom du service)
                                   # Pour respecter strictement: image: mariadb
        container_name: mariadb_c-andriam # Nom du conteneur en cours d'exécution
        restart: unless-stopped # Redémarre le conteneur sauf s'il est explicitement arrêté
        env_file:
          - .env # Charge les variables depuis srcs/.env
        environment: # Peut surcharger ou ajouter des variables de .env
          MYSQL_DATABASE: ${MYSQL_DATABASE} # Utilise les variables de .env
          MYSQL_USER: ${MYSQL_USER}
          MYSQL_PASSWORD: ${MYSQL_PASSWORD}
          MYSQL_ROOT_PASSWORD: ${MYSQL_ROOT_PASSWORD}
        volumes:
          # 'db_data' est un nom de volume Docker géré.
          # Il est mappé au répertoire /var/lib/mysql DANS le conteneur.
          # Les données de la base seront persistantes même si le conteneur est supprimé.
          - db_data:/var/lib/mysql
        networks:
          - inception_network # Connecte ce service au réseau 'inception_network'

    volumes:
      db_data: # Définition du volume nommé 'db_data'
        driver: local # Pilote de volume par défaut
        driver_opts:
          type: none # Pas de type de système de fichiers spécifique à forcer par Docker
          # 'device' est le chemin sur la machine HÔTE où les données seront stockées.
          device: /home/c-andriam/data/mariadb # REMPLACEZ c-andriam par votre login
          o: bind # Option de montage, 'bind' monte le répertoire hôte dans le volume

    networks:
      inception_network: # Définition du réseau personnalisé
        driver: bridge # Type de réseau par défaut, crée un réseau isolé pour vos conteneurs
    ```

### Service 2 : WordPress (Le Moteur du Site)

WordPress utilisera PHP-FPM pour exécuter le code PHP et se connectera à MariaDB.

1.  **`srcs/requirements/wordpress/Dockerfile` :**

    ```dockerfile name=srcs/requirements/wordpress/Dockerfile
    # Choisissez la même version d'Alpine que pour MariaDB pour la cohérence
    ARG ALPINE_VERSION=3.18 # Exemple, adaptez !
    FROM alpine:${ALPINE_VERSION}

    # Variables d'environnement pour la configuration de WordPress
    # Valeurs par défaut, seront surchargées par docker-compose et .env
    ENV WP_VERSION=6.4.3 # Spécifiez une version de WordPress (NE PAS UTILISER LATEST)
                         # Vérifiez la dernière version stable sur wordpress.org/download/releases/
                         # Le sujet demande l'avant-dernière stable. Si 6.5 est la dernière, 6.4.x est bien.
    ENV WP_SHA256_CHECKSUM=c7a5144c655ba3c170e4204189c89d73d087119af030107f7ba26804dbcb1085 # Checksum pour WP_VERSION 6.4.3
                                                                                       # Trouvez le checksum sur la page de release de WP
    ENV PHP_VERSION=php82 # Ou php81, php80 selon la compatibilité avec la version de WP
                          # Alpine package names might be php8, php81, php82 etc.

    # Installer PHP, PHP-FPM et les extensions nécessaires pour WordPress
    # La liste exacte peut varier selon la version de WP et les plugins que vous pourriez utiliser.
    RUN apk update && \
        apk add --no-cache \
            ${PHP_VERSION}-fpm \
            ${PHP_VERSION}-mysqli \
            ${PHP_VERSION}-gd \
            ${PHP_VERSION}-curl \
            ${PHP_VERSION}-zip \
            ${PHP_VERSION}-xml \
            ${PHP_VERSION}-mbstring \
            ${PHP_VERSION}-json \
            ${PHP_VERSION}-session \
            ${PHP_VERSION}-ctype \
            ${PHP_VERSION}-dom \
            ${PHP_VERSION}-phar \
            ${PHP_VERSION}-iconv \
            # Outils pour le téléchargement et la gestion
            curl \
            tar \
            bash # Pour le script de configuration

    # Configurer PHP-FPM pour écouter sur le port 9000 et ne pas tourner en démon
    # Le fichier de conf peut être /etc/php82/php-fpm.d/www.conf ou similaire
    RUN sed -i "s|listen = /run/php-fpm/www.sock|listen = 0.0.0.0:9000|g" /etc/php82/php-fpm.d/www.conf && \
        sed -i "s|;daemonize = yes|daemonize = no|g" /etc/php82/php-fpm.conf && \
        # Augmenter la taille max des uploads si besoin (exemple 64M)
        sed -i 's/upload_max_filesize = 2M/upload_max_filesize = 64M/g' /etc/php82/php.ini && \
        sed -i 's/post_max_size = 8M/post_max_size = 64M/g' /etc/php82/php.ini

    # Créer le répertoire pour WordPress
    WORKDIR /var/www/html

    # Télécharger, vérifier et extraire WordPress
    RUN curl -o wordpress.tar.gz -SL "https://wordpress.org/wordpress-${WP_VERSION}.tar.gz" && \
        echo "${WP_SHA256_CHECKSUM} *wordpress.tar.gz" | sha256sum -c - && \
        tar -xzf wordpress.tar.gz -C /var/www/html --strip-components=1 && \
        rm wordpress.tar.gz && \
        # Définir les permissions (un utilisateur www-data est souvent créé par les paquets php-fpm)
        # Si www-data n'existe pas, créez-le : RUN addgroup -g 82 www-data && adduser -u 82 -G www-data -s /sbin/nologin -D www-data
        chown -R nobody:nobody /var/www/html # Sur Alpine, php-fpm tourne souvent en tant que 'nobody' par défaut
                                           # Vérifiez l'utilisateur dans www.conf (user = ..., group = ...)

    # Copier le script de configuration de WordPress
    COPY ./tools/configure-wp.sh /usr/local/bin/configure-wp.sh
    RUN chmod +x /usr/local/bin/configure-wp.sh

    # Exposer le port 9000 où PHP-FPM écoute (pour Nginx)
    EXPOSE 9000

    # Script d'entrée qui configure WordPress, puis démarre PHP-FPM
    ENTRYPOINT ["/usr/local/bin/configure-wp.sh"]

    # Commande par défaut pour démarrer PHP-FPM en avant-plan
    # Cette commande sera appelée par le script configure-wp.sh
    CMD ["php-fpm82", "-F"] # Le -F est pour "Foreground"
    ```
    **Important :**
    *   Vérifiez la dernière version STABLE de WordPress et son avant-dernière. Utilisez une version spécifique (ex: `6.4.3`).
    *   Trouvez le **checksum SHA256** correspondant sur la page de release de WordPress pour vérifier l'intégrité du téléchargement.
    *   Adaptez les noms des paquets PHP (`php82-fpm`, etc.) à ce qui est disponible dans la version d'Alpine que vous utilisez. Vous pouvez tester avec `docker run -it alpine:3.18 sh` puis `apk search php`.
    *   L'utilisateur sous lequel php-fpm s'exécute (`nobody` sur Alpine par défaut, ou `www-data` sur Debian) doit avoir les droits d'écriture sur `/var/www/html` pour que WordPress puisse créer `wp-config.php` et gérer les uploads.

2.  **`srcs/requirements/wordpress/tools/configure-wp.sh` :**
    Ce script configure `wp-config.php` au premier démarrage.

    ```bash name=srcs/requirements/wordpress/tools/configure-wp.sh
    #!/bin/bash
    set -e

    WP_CONFIG_FILE="/var/www/html/wp-config.php"
    WP_CONFIG_SAMPLE="/var/www/html/wp-config-sample.php"

    # Attendre que MariaDB soit accessible (simple test de port)
    # Le nom 'mariadb' vient du nom du service dans docker-compose.yml
    echo "Attente de MariaDB sur mariadb:3306..."
    max_attempts=60 # Attendre jusqu'à 60 secondes
    attempt_num=0
    until nc -z mariadb 3306 >/dev/null 2>&1 || [ $attempt_num -ge $max_attempts ]; do
        echo -n "."
        sleep 1
        attempt_num=$((attempt_num+1))
    done

    if ! nc -z mariadb 3306 >/dev/null 2>&1; then
        echo "MariaDB n'est pas accessible après $max_attempts secondes."
        exit 1
    fi
    echo "MariaDB est accessible."


    if [ ! -f "$WP_CONFIG_FILE" ]; then
        echo "Configuration de wp-config.php..."

        # Copier le fichier sample s'il n'existe pas déjà (normalement il est là)
        if [ ! -f "$WP_CONFIG_SAMPLE" ]; then
            echo "Erreur: wp-config-sample.php non trouvé !"
            exit 1
        fi

        # Remplacer les placeholders dans wp-config-sample.php et créer wp-config.php
        # Utilisation de variables d'environnement passées par docker-compose (venant de .env)
        sed -e "s/database_name_here/$MYSQL_DATABASE/g" \
            -e "s/username_here/$MYSQL_USER/g" \
            -e "s/password_here/$MYSQL_PASSWORD/g" \
            -e "s/localhost/$DB_HOST/g" \
            "$WP_CONFIG_SAMPLE" > "$WP_CONFIG_FILE"

        # Ajouter les clés de sécurité WordPress depuis les variables d'environnement
        # (Définies dans .env et passées par docker-compose)
        echo "Ajout des clés de sécurité WordPress..."
        {
            echo "define('AUTH_KEY',         '${AUTH_KEY}');"
            echo "define('SECURE_AUTH_KEY',  '${SECURE_AUTH_KEY}');"
            echo "define('LOGGED_IN_KEY',    '${LOGGED_IN_KEY}');"
            echo "define('NONCE_KEY',        '${NONCE_KEY}');"
            echo "define('AUTH_SALT',        '${AUTH_SALT}');"
            echo "define('SECURE_AUTH_SALT', '${SECURE_AUTH_SALT}');"
            echo "define('LOGGED_IN_SALT',   '${LOGGED_IN_SALT}');"
            echo "define('NONCE_SALT',       '${NONCE_SALT}');"
        } >> "$WP_CONFIG_FILE"

        # Définir les permissions pour que WordPress puisse écrire (thèmes, plugins, uploads)
        # L'utilisateur doit correspondre à celui de php-fpm (nobody sur Alpine, www-data sur Debian)
        chown -R nobody:nobody /var/www/html
        find /var/www/html -type d -exec chmod 755 {} \;
        find /var/www/html -type f -exec chmod 644 {} \;
        # Permissions plus spécifiques pour wp-config.php (moins permissif)
        chmod 600 "$WP_CONFIG_FILE" # Ou 640 si le groupe a besoin de lire

        echo "wp-config.php configuré."
    else
        echo "wp-config.php déjà configuré."
    fi

    echo "Démarrage de PHP-FPM..."
    # Exécute la commande CMD du Dockerfile (php-fpm en avant-plan)
    exec "$@"
    ```

3.  **Ajouter WordPress à `srcs/docker-compose.yml` :**

    ```yaml name=srcs/docker-compose.yml
    # ... (version, services.mariadb) ...

      wordpress:
        build:
          context: ./requirements/wordpress
          dockerfile: Dockerfile
          args:
            ALPINE_VERSION: 3.18 # Doit correspondre à celui utilisé dans le Dockerfile de WP
            WP_VERSION: 6.4.3    # Doit correspondre
            PHP_VERSION: php82   # Doit correspondre
        image: wordpress_image_c-andriam # Ou: image: wordpress
        container_name: wordpress_c-andriam
        restart: unless-stopped
        env_file:
          - .env # Charge toutes les variables de .env
        environment:
          # Ces variables sont utilisées par configure-wp.sh
          DB_HOST: mariadb # Important: 'mariadb' est le nom du service MariaDB
          MYSQL_DATABASE: ${MYSQL_DATABASE}
          MYSQL_USER: ${MYSQL_USER}
          MYSQL_PASSWORD: ${MYSQL_PASSWORD}
          # Les clés AUTH_KEY, SECURE_AUTH_KEY, etc. sont aussi chargées depuis .env
        volumes:
          # 'wp_files' est un volume Docker géré pour les fichiers de WordPress.
          - wp_files:/var/www/html
        depends_on: # S'assure que MariaDB démarre avant WordPress
          mariadb: # Cela ne garantit pas que MariaDB soit "prêt", juste que le conteneur est démarré.
            condition: service_started # Ou condition: service_healthy si vous implémentez un healthcheck pour MariaDB
        networks:
          - inception_network

    # ... (volumes.db_data) ...
    volumes:
      db_data:
        driver: local
        driver_opts:
          type: none
          device: /home/c-andriam/data/mariadb # REMPLACEZ c-andriam
          o: bind
      wp_files: # Volume pour les fichiers WordPress
        driver: local
        driver_opts:
          type: none
          device: /home/c-andriam/data/wordpress # REMPLACEZ c-andriam
          o: bind

    # ... (networks.inception_network) ...
    ```

### Service 3 : Nginx (Le Serveur Web)

Nginx sera le point d'entrée de votre site. Il recevra les requêtes HTTPS et les transmettra à WordPress (PHP-FPM).

1.  **`srcs/requirements/nginx/Dockerfile` :**

    ```dockerfile name=srcs/requirements/nginx/Dockerfile
    ARG ALPINE_VERSION=3.18 # Exemple, adaptez !
    FROM alpine:${ALPINE_VERSION}

    # Variable pour le nom de domaine (sera passée par docker-compose depuis .env)
    ARG DOMAIN_NAME=c-andriam.42.fr

    # Installer Nginx et OpenSSL (pour générer le certificat)
    RUN apk update && \
        apk add --no-cache \
            nginx \
            openssl \
            bash # Si votre script de démarrage utilise bash

    # Créer les répertoires pour les certificats SSL
    RUN mkdir -p /etc/nginx/ssl/private /etc/nginx/ssl/certs

    # Générer un certificat SSL auto-signé
    # Le CN (Common Name) doit correspondre à votre nom de domaine
    RUN openssl req -x509 -nodes -days 365 -newkey rsa:2048 \
        -keyout /etc/nginx/ssl/private/nginx-selfsigned.key \
        -out /etc/nginx/ssl/certs/nginx-selfsigned.crt \
        -subj "/C=FR/ST=IDF/L=Paris/O=42/OU=${DOMAIN_NAME}/CN=${DOMAIN_NAME}"

    # Optionnel mais recommandé : Générer des paramètres Diffie-Hellman plus forts
    # Cela peut prendre du temps lors de la construction de l'image.
    # RUN openssl dhparam -out /etc/nginx/ssl/dhparam.pem 2048

    # Copier le fichier de configuration Nginx
    # Il va remplacer ou s'ajouter à la configuration par défaut de Nginx.
    COPY ./conf/nginx.conf /etc/nginx/nginx.conf
    # Si vous préférez mettre votre conf dans conf.d :
    # COPY ./conf/default.conf /etc/nginx/conf.d/default.conf
    # Et assurez-vous que nginx.conf inclut conf.d/*.conf

    # Le répertoire root de Nginx pointera vers le volume des fichiers WordPress
    # Ce volume sera partagé avec le conteneur WordPress.
    # WORKDIR /var/www/html # Pas nécessaire ici car Nginx servira les fichiers depuis le volume monté

    # Exposer le port 443 (HTTPS)
    EXPOSE 443

    # Commande pour démarrer Nginx en avant-plan
    # Le '; daemon off;' empêche Nginx de se mettre en arrière-plan,
    # ce qui est nécessaire pour que Docker puisse gérer le processus.
    CMD ["nginx", "-g", "daemon off;"]
    ```

2.  **`srcs/requirements/nginx/conf/nginx.conf` :**
    C'est la configuration principale de Nginx.

    ```nginx name=srcs/requirements/nginx/conf/nginx.conf
    # Utilisateur sous lequel Nginx s'exécute. Sur Alpine, 'nginx' est courant.
    user nginx;
    # Nombre de processus worker, 'auto' essaie de détecter le nombre de cœurs CPU.
    worker_processes auto;

    # Fichier où Nginx stocke son ID de processus principal.
    pid /run/nginx/nginx.pid;

    # Fichier de log d'erreur global.
    error_log /var/log/nginx/error.log warn;

    events {
        # Nombre maximum de connexions simultanées qu'un worker peut gérer.
        worker_connections 1024;
    }

    http {
        include /etc/nginx/mime.types; # Types MIME pour les fichiers
        default_type application/octet-stream; # Type par défaut si non reconnu

        # Format des logs d'accès
        log_format main '$remote_addr - $remote_user [$time_local] "$request" '
                          '$status $body_bytes_sent "$http_referer" '
                          '"$http_user_agent" "$http_x_forwarded_for"';
        access_log /var/log/nginx/access.log main;

        sendfile on; # Permet d'envoyer des fichiers plus efficacement
        # tcp_nopush on; # Optimisation pour envoyer les en-têtes et le début du fichier en même temps
        keepalive_timeout 65; # Durée pendant laquelle une connexion persistante reste ouverte
        # gzip on; # Activer la compression Gzip (économise de la bande passante)
        # server_tokens off; # Ne pas afficher la version de Nginx dans les en-têtes (sécurité)

        server {
            listen 443 ssl http2; # Écouter sur le port 443 pour HTTPS et HTTP/2
            listen [::]:443 ssl http2; # Idem pour IPv6

            # IMPORTANT: Remplacez par votre nom de domaine (issu de .env)
            # Vous ne pouvez pas utiliser directement ${DOMAIN_NAME} ici,
            # il est fixé lors de la construction du certificat.
            # Si vous avez besoin de dynamisme, il faudrait un script d'entrée
            # qui génère ce fichier de conf au démarrage du conteneur.
            # Pour ce projet, le certificat est généré avec le DOMAIN_NAME du build.
            server_name c-andriam.42.fr; # Mettez VOTRE domaine ici

            # Chemins vers vos certificats SSL (générés dans le Dockerfile)
            ssl_certificate /etc/nginx/ssl/certs/nginx-selfsigned.crt;
            ssl_certificate_key /etc/nginx/ssl/private/nginx-selfsigned.key;
            # ssl_dhparam /etc/nginx/ssl/dhparam.pem; # Si vous avez généré dhparam.pem

            # Protocoles SSL/TLS autorisés (TLSv1.2 et TLSv1.3 uniquement comme demandé)
            ssl_protocols TLSv1.2 TLSv1.3;
            # Suite de chiffrements recommandées (consultez les guides de Mozilla SSL Config Generator)
            ssl_prefer_server_ciphers on;
            ssl_ciphers ECDHE-ECDSA-AES128-GCM-SHA256:ECDHE-RSA-AES128-GCM-SHA256:ECDHE-ECDSA-AES256-GCM-SHA384:ECDHE-RSA-AES256-GCM-SHA384:ECDHE-ECDSA-CHACHA20-POLY1305:ECDHE-RSA-CHACHA20-POLY1305:DHE-RSA-AES128-GCM-SHA256:DHE-RSA-AES256-GCM-SHA384;

            # Répertoire racine où se trouvent les fichiers de WordPress
            # Ce chemin doit correspondre à celui où le volume wp_files est monté.
            root /var/www/html;
            index index.php index.html index.htm;

            location / {
                # Essaye de servir le fichier demandé, puis un répertoire, puis passe à index.php
                try_files $uri $uri/ /index.php?$args;
            }

            # Transmettre les scripts PHP à PHP-FPM qui écoute sur le port 9000
            # du conteneur nommé 'wordpress' (nom du service dans docker-compose).
            location ~ \.php$ {
                try_files $uri =404; # Si le fichier .php n'existe pas, retourne 404
                fastcgi_split_path_info ^(.+\.php)(/.+)$;
                # 'wordpress' est le nom du service WordPress dans docker-compose.yml
                fastcgi_pass wordpress:9000;
                fastcgi_index index.php;
                # Nécessaire pour que WordPress connaisse le nom du script à exécuter
                fastcgi_param SCRIPT_FILENAME $document_root$fastcgi_script_name;
                include fastcgi_params; # Inclut les paramètres FastCGI standard
                # fastcgi_param PATH_INFO $fastcgi_path_info; # Peut être nécessaire pour certains setups
            }

            # Interdire l'accès aux fichiers .htaccess (sécurité)
            location ~ /\.ht {
                deny all;
            }

            # Interdire l'accès à xmlrpc.php (souvent cible d'attaques)
            # Sauf si vous en avez spécifiquement besoin.
            location = /xmlrpc.php {
                deny all;
            }
        }
    }
    ```
    **Note :** Le `server_name` dans `nginx.conf` doit correspondre exactement au `CN` utilisé pour générer le certificat SSL et au domaine que vous utiliserez dans votre navigateur.

3.  **Ajouter Nginx à `srcs/docker-compose.yml` :**

    ```yaml name=srcs/docker-compose.yml
    # ... (version, services.mariadb, services.wordpress) ...

      nginx:
        build:
          context: ./requirements/nginx
          dockerfile: Dockerfile
          args:
            ALPINE_VERSION: 3.18 # Assurez-vous que cela correspond
            DOMAIN_NAME: ${DOMAIN_NAME} # Passe la variable DOMAIN_NAME de .env au Dockerfile
        image: nginx_image_c-andriam # Ou: image: nginx
        container_name: nginx_c-andriam
        restart: unless-stopped
        ports:
          # Mappe le port 443 de votre machine HÔTE (VM) au port 443 du conteneur Nginx.
          # C'est ce qui rend votre site accessible depuis l'extérieur du conteneur.
          - "443:443"
        volumes:
          # Partage les fichiers WordPress avec Nginx pour qu'il puisse servir
          # les fichiers statiques (images, CSS, JS) directement.
          # Le contenu dynamique (.php) sera passé à PHP-FPM.
          - wp_files:/var/www/html:ro # ':ro' signifie read-only, Nginx n'a pas besoin d'écrire ici.
        depends_on:
          - wordpress # S'assure que WordPress démarre avant Nginx
        networks:
          - inception_network

    # ... (volumes, networks) ...
    ```
    **Fichier `docker-compose.yml` complet (jusqu'ici) :**
    ```yaml name=srcs/docker-compose.yml
    version: '3.8'

    services:
      mariadb:
        build:
          context: ./requirements/mariadb
          dockerfile: Dockerfile
          args:
            ALPINE_VERSION: 3.18
        image: mariadb # Nom de l'image = nom du service
        container_name: mariadb_c-andriam
        restart: unless-stopped
        env_file:
          - .env
        environment:
          MYSQL_DATABASE: ${MYSQL_DATABASE}
          MYSQL_USER: ${MYSQL_USER}
          MYSQL_PASSWORD: ${MYSQL_PASSWORD}
          MYSQL_ROOT_PASSWORD: ${MYSQL_ROOT_PASSWORD}
        volumes:
          - db_data:/var/lib/mysql
        networks:
          - inception_network

      wordpress:
        build:
          context: ./requirements/wordpress
          dockerfile: Dockerfile
          args:
            ALPINE_VERSION: 3.18
            WP_VERSION: 6.4.3 # Adaptez
            PHP_VERSION: php82 # Adaptez
            WP_SHA256_CHECKSUM: c7a5144c655ba3c170e4204189c89d73d087119af030107f7ba26804dbcb1085 # Adaptez
        image: wordpress # Nom de l'image = nom du service
        container_name: wordpress_c-andriam
        restart: unless-stopped
        env_file:
          - .env
        environment:
          DB_HOST: mariadb
          MYSQL_DATABASE: ${MYSQL_DATABASE}
          MYSQL_USER: ${MYSQL_USER}
          MYSQL_PASSWORD: ${MYSQL_PASSWORD}
          # Les clés AUTH_KEY, etc., sont chargées depuis .env
        volumes:
          - wp_files:/var/www/html
        depends_on:
          mariadb:
            condition: service_started
        networks:
          - inception_network

      nginx:
        build:
          context: ./requirements/nginx
          dockerfile: Dockerfile
          args:
            ALPINE_VERSION: 3.18
            DOMAIN_NAME: ${DOMAIN_NAME}
        image: nginx # Nom de l'image = nom du service
        container_name: nginx_c-andriam
        restart: unless-stopped
        ports:
          - "443:443"
        volumes:
          - wp_files:/var/www/html:ro # read-only pour Nginx
        depends_on:
          - wordpress
        networks:
          - inception_network

    volumes:
      db_data:
        driver: local
        driver_opts:
          type: none
          device: /home/c-andriam/data/mariadb # REMPLACEZ c-andriam
          o: bind
      wp_files:
        driver: local
        driver_opts:
          type: none
          device: /home/c-andriam/data/wordpress # REMPLACEZ c-andriam
          o: bind

    networks:
      inception_network:
        driver: bridge
        name: inception_network_c-andriam # Donner un nom explicite au réseau
    ```

---

## 5. Volumes et Réseaux Docker Expliqués

*   **Volumes :**
    *   `db_data:/var/lib/mysql` : Les fichiers de la base de données MariaDB sont stockés dans `/var/lib/mysql` *à l'intérieur* du conteneur MariaDB. Ce volume Docker nommé `db_data` lie ce répertoire interne à `/home/c-andriam/data/mariadb` sur votre machine hôte (la VM). Si vous supprimez le conteneur MariaDB, les données persistent sur votre hôte.
    *   `wp_files:/var/www/html` : De même, les fichiers de WordPress (code source, thèmes, plugins, uploads) sont dans `/var/www/html` dans les conteneurs WordPress et Nginx. Ce volume `wp_files` les lie à `/home/c-andriam/data/wordpress` sur votre hôte.
    *   **Pourquoi `driver_opts` ?** Le sujet spécifie que les volumes doivent être dans `/home/login/data`. L'utilisation de `driver: local` avec `driver_opts` et `o: bind` est une façon de créer un "bind mount" via une définition de volume nommé, ce qui est une approche valide.

*   **Réseaux :**
    *   `inception_network` : Nous avons créé un réseau "bridge" personnalisé.
        *   **Isolation :** Seuls les conteneurs attachés à ce réseau peuvent communiquer entre eux.
        *   **Résolution de noms :** Docker fournit une résolution DNS interne sur ce réseau. Le conteneur WordPress peut joindre MariaDB en utilisant le nom de service `mariadb` (parce que c'est le nom du service dans `docker-compose.yml`). Nginx peut joindre WordPress/PHP-FPM via le nom de service `wordpress`. C'est beaucoup mieux que de coder des adresses IP en dur.
    *   **Interdiction de `network_mode: host` ou `links` :** Le sujet l'interdit, et c'est une bonne pratique. Les réseaux personnalisés sont la méthode moderne et plus flexible.

---

## 6. Configurer Votre Domaine Local (`c-andriam.42.fr`)

Pour que votre navigateur sache que `c-andriam.42.fr` doit pointer vers votre VM (où Nginx tourne), vous devez modifier le fichier `hosts` de la machine sur laquelle vous utilisez votre navigateur.

1.  **Trouvez l'adresse IP de votre VM :**
    Dans le terminal de votre VM, tapez :
    ```bash
    ip a
    # ou
    hostname -I
    ```
    Cherchez une adresse IP qui ressemble à `192.168.x.x` ou `10.x.x.x` (cela dépend de la configuration réseau de votre logiciel de VM).

2.  **Modifiez le fichier `hosts` :**
    *   **Si votre navigateur est sur votre machine hôte (Windows, macOS, Linux de bureau) :**
        *   Windows : `C:\Windows\System32\drivers\etc\hosts` (ouvrez Notepad en tant qu'administrateur).
        *   macOS/Linux : `/etc/hosts` (utilisez `sudo nano /etc/hosts`).
    *   **Si votre navigateur est DANS la VM (si vous avez installé un environnement de bureau sur votre VM Linux) :**
        *   Modifiez `/etc/hosts` dans la VM avec `sudo nano /etc/hosts`.

    Ajoutez une ligne à la fin du fichier :
    ```
    <IP_DE_VOTRE_VM>   c-andriam.42.fr
    ```
    Remplacez `<IP_DE_VOTRE_VM>` par l'IP trouvée et `c-andriam.42.fr` par votre domaine.
    Exemple : `192.168.1.105   c-andriam.42.fr`

---

## 7. Lancer et Tester Votre Projet

1.  **Construire les images :**
    À la racine de votre projet (où se trouve le `Makefile`), ouvrez un terminal et tapez :
    ```bash
    make build
    ```
    Cela peut prendre un certain temps la première fois, car Docker télécharge les images de base et exécute les instructions de vos Dockerfiles.

2.  **Démarrer les services :**
    ```bash
    make up
    ```
    Les conteneurs vont démarrer en arrière-plan.

3.  **Vérifier les logs (surtout s'il y a des problèmes) :**
    ```bash
    make logs
    # Ou pour un conteneur spécifique :
    # docker logs mariadb_c-andriam
    # docker logs wordpress_c-andriam
    # docker logs nginx_c-andriam
    ```
    Regardez s'il y a des messages d'erreur.

4.  **Vérifier l'état des conteneurs :**
    ```bash
    make status
    # ou
    docker ps -a
    ```
    Vous devriez voir vos trois conteneurs (`mariadb_c-andriam`, `wordpress_c-andriam`, `nginx_c-andriam`) avec un statut "Up" ou "Running". Nginx devrait avoir `0.0.0.0:443->443/tcp` dans la colonne PORTS.

5.  **Accéder à votre site :**
    Ouvrez votre navigateur web et allez à l'adresse : `https://c-andriam.42.fr` (utilisez VOTRE domaine).
    *   **Avertissement de sécurité :** Votre navigateur affichera probablement un avertissement ("Votre connexion n'est pas privée", "Risque de sécurité potentiel", etc.). C'est normal car vous utilisez un certificat SSL auto-signé que le navigateur ne reconnaît pas comme émis par une autorité de confiance. Cliquez sur "Avancé" ou "Continuer vers le site" (les options varient selon le navigateur).

6.  **Installation de WordPress :**
    Si tout va bien, vous devriez voir la page d'installation de WordPress.
    *   Choisissez votre langue.
    *   Sur la page "Bienvenue", WordPress vous demandera les informations de base de données. Vous les avez déjà configurées dans `wp-config.php` via le script `configure-wp.sh`. Normalement, si `wp-config.php` est correct, WordPress devrait passer cette étape, ou vous dire que le fichier existe déjà.
    *   Si WordPress demande les infos de base de données manuellement :
        *   Nom de la base de données : `wordpress_db` (ou ce que vous avez mis dans `.env` pour `MYSQL_DATABASE`)
        *   Nom d’utilisateur : `wp_user` (ou `MYSQL_USER`)
        *   Mot de passe : Le mot de passe que vous avez mis pour `MYSQL_PASSWORD` dans `.env`
        *   Hôte de la base de données : `mariadb` (c'est le nom du service MariaDB)
        *   Préfixe de table : `wp_` (laissez par défaut)
    *   Ensuite, configurez les informations du site :
        *   Titre du site : Ce que vous voulez.
        *   Nom d’utilisateur (Admin) : **N'UTILISEZ PAS "admin" ou des variantes.** Choisissez un nom unique (celui de `WP_ADMIN_USER` dans `.env`).
        *   Mot de passe (Admin) : Un mot de passe fort (celui de `WP_ADMIN_PASSWORD` dans `.env`).
        *   Votre adresse de messagerie : (celle de `WP_ADMIN_EMAIL` dans `.env`).
        *   Visibilité pour les moteurs de recherche : Cochez si vous ne voulez pas que les moteurs l'indexent (pour un projet de test, c'est bien).
    *   Cliquez sur "Installer WordPress".

7.  **Connectez-vous et créez un deuxième utilisateur :**
    Une fois installé, connectez-vous à l'interface d'administration de WordPress (`https://c-andriam.42.fr/wp-admin`).
    Allez dans "Utilisateurs" > "Ajouter" et créez un deuxième utilisateur avec un rôle différent (par exemple, "Auteur" ou "Éditeur").

---

## 8. Quelques Bonnes Pratiques et Sécurité

*   **Variables d'environnement et `.env` :** C'est bien pour ce projet. Pour une sécurité accrue en production, on utiliserait des "Docker Secrets" ou des systèmes de gestion de secrets externes. Le sujet vous encourage à utiliser Docker Secrets, ce qui est une excellente pratique.
    *   **Utiliser Docker Secrets (avancé mais recommandé par le sujet) :**
        1.  Créez vos fichiers de secrets dans le dossier `secrets` à la racine (ex: `secrets/db_password.txt` contenant juste le mot de passe).
        2.  Dans `docker-compose.yml`, déclarez les secrets :
            ```yaml
            secrets:
              db_user_password:
                file: ./../secrets/db_password.txt # Chemin relatif au docker-compose.yml
            ```
        3.  Dans la définition du service (ex: MariaDB), attachez le secret et utilisez la variable `_FILE` :
            ```yaml
            services:
              mariadb:
                secrets:
                  - db_user_password
                environment:
                  MYSQL_PASSWORD_FILE: /run/secrets/db_user_password # MariaDB lit le contenu de ce fichier
            ```
            WordPress supporte aussi `WORDPRESS_DB_PASSWORD_FILE`. Votre script `init_db.sh` ou `configure-wp.sh` devrait alors lire le contenu de `/run/secrets/<nom_du_secret>`.
*   **Mots de passe forts :** Utilisez des mots de passe longs, uniques et complexes.
*   **Pas de `latest` :** Toujours spécifier des versions d'images.
*   **`.dockerignore` :** Créez un fichier `.dockerignore` dans chaque dossier de service (`srcs/requirements/mariadb/`, etc.) pour exclure des fichiers inutiles du contexte de build Docker (ex: `.git`, `*.md`, `secrets/`). Cela accélère les builds et réduit la taille des images.
    Exemple de `.dockerignore` :
    ```
    .git
    .gitignore
    README.md
    secrets/
    *.txt
    ```
*   **Principe du moindre privilège :** Ne donnez que les permissions nécessaires. Par exemple, le volume `wp_files` est monté en `ro` (read-only) pour Nginx.
*   **PID 1 et Daemons :** Le processus principal de votre conteneur (celui lancé par `CMD` ou `ENTRYPOINT`) doit être le service lui-même, tournant en avant-plan (ex: `nginx -g 'daemon off;'`, `php-fpm -F`, `mariadbd --console`). Docker gère ainsi correctement le cycle de vie du conteneur.

---

## 9. Dépannage de Base

*   **`make logs` ou `docker logs <nom_du_conteneur>` :** Votre meilleur ami. Lisez attentivement les messages d'erreur.
*   **`make status` ou `docker ps -a` :** Vérifiez si les conteneurs tournent, s'ils redémarrent en boucle ("Restarting"), ou s'ils sont sortis ("Exited").
*   **`docker exec -it <nom_du_conteneur> sh` (ou `bash`) :** Permet d'ouvrir un shell à l'intérieur d'un conteneur en cours d'exécution pour explorer les fichiers, tester la connectivité (`ping mariadb`, `nc -vz mariadb 3306`). Utilisez-le pour le débogage, pas pour modifier la configuration en direct (ces changements seraient perdus).
*   **Problèmes de réseau :**
    *   Assurez-vous que les conteneurs sont sur le même réseau `inception_network`.
    *   Vérifiez que les noms de service dans les configurations (ex: `DB_HOST: mariadb`, `fastcgi_pass wordpress:9000`) correspondent aux noms des services dans `docker-compose.yml`.
*   **Problèmes de permissions :** Souvent liés aux volumes. Assurez-vous que l'utilisateur dans le conteneur (ex: `mysql`, `nobody`, `nginx`) a les droits de lecture/écriture nécessaires sur les volumes montés.
*   **Reconstruire si vous changez un `Dockerfile` ou un fichier copié :** `make build` (grâce à `--no-cache`) ou `docker compose build --no-cache <service_name>`.

---

## 10. Pour Aller Plus Loin (Partie Bonus)

Une fois la partie obligatoire parfaitement fonctionnelle, vous pouvez explorer les bonus :
*   **Redis Cache pour WordPress :** Améliore les performances.
*   **Serveur FTP :** Pour accéder aux fichiers WordPress via FTP (attention à la sécurité).
*   **Site statique :** Un petit site distinct (pas en PHP).
*   **Adminer :** Une interface web légère pour gérer MariaDB.
*   **Autre service utile :** À vous de choisir et de justifier.

Chaque bonus nécessitera son propre `Dockerfile` et une entrée dans `docker-compose.yml`.

---

Bon courage pour votre projet "Inception" ! C'est un excellent apprentissage. Prenez votre temps, lisez la documentation, et n'hésitez pas à expérimenter.
