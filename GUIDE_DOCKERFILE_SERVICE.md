# Comment bien réaliser et maîtriser : "Chaque service doit avoir un Dockerfile écrit par vous"

## 1. **Comprendre la règle**

- **Chaque service** (ex : nginx, wordpress, mariadb) doit avoir son propre `Dockerfile`.
- **Vous écrivez ce fichier vous-même**, pour chaque service.
- **Il est interdit d’utiliser simplement `image: mariadb:latest` dans `docker-compose.yml`** sans Dockerfile.
- **Objectif pédagogique** : apprendre à construire une image Docker personnalisée, à comprendre l’installation et la configuration d’un service.

---

## 2. **Exemple de réalisation pour un service (MariaDB)**

### **Étape 1 : Créez le dossier**

Dans votre projet, créez un dossier par service, par exemple :
```bash
mkdir -p srcs/requirements/mariadb
```

### **Étape 2 : Créez un Dockerfile vide**

Dans ce dossier, créez un fichier nommé `Dockerfile` :
```bash
touch srcs/requirements/mariadb/Dockerfile
```

### **Étape 3 : Écrivez le Dockerfile vous-même**

Voici un exemple pour MariaDB sur Alpine (avant-dernière version) :

```dockerfile
# Utiliser l'avant-dernière version stable d'Alpine
FROM alpine:3.19

# Installer MariaDB
RUN apk update && \
    apk add --no-cache mariadb mariadb-client

# Créer le dossier de la base de données
RUN mkdir -p /run/mysqld && chown -R mysql:mysql /run/mysqld

# Copier un script d'initialisation personnalisé (à créer dans votre dossier)
COPY ./tools/init_db.sh /init_db.sh
RUN chmod +x /init_db.sh

# Définir les variables d'environnement (non sensibles ici !)
ENV MYSQL_DATABASE=wordpress

# Commande de démarrage (PID 1)
CMD ["/init_db.sh"]
```

---

### **Étape 4 : Créez le fichier d'initialisation**

Créez un script par exemple `srcs/requirements/mariadb/tools/init_db.sh` qui démarre MariaDB et initialise la base.

---

### **Étape 5 : Déclarez le build dans `docker-compose.yml`**

```yaml
services:
  mariadb:
    build: ./requirements/mariadb
    image: mariadb
    # (autres configs)
```

---

## 3. **Répétez pour chaque service !**

- **NGINX** : Dockerfile dans `srcs/requirements/nginx/`
- **WordPress/PHP-FPM** : Dockerfile dans `srcs/requirements/wordpress/`
- **MariaDB** : Dockerfile dans `srcs/requirements/mariadb/`

Chaque service doit avoir sa **recette d’installation et de configuration dans son Dockerfile**.

---

## 4. **Bien maîtriser la démarche**

- **Comprenez chaque ligne** de vos Dockerfiles : pourquoi cette image de base, pourquoi chaque commande RUN/COPY/ENV/CMD.
- **Testez chaque build séparément** : lancez la construction de chaque image (`docker build .`) pour vérifier la compréhension et le fonctionnement.
- **Essayez d’ajouter une petite personnalisation** (par exemple, changer la config MariaDB, customiser la page d’accueil de NGINX, etc.).
- **Apprenez à déboguer** : si le build échoue, lisez bien le message d’erreur, corrigez et re-testez.

---

## 5. **Astuces pour progresser**

- Inspirez-vous de la documentation officielle de chaque service (MariaDB, NGINX, PHP, etc.).
- Lisez la [documentation Dockerfile](https://docs.docker.com/engine/reference/builder/) pour découvrir toutes les instructions possibles.
- Faites des tests avec des options différentes (par exemple, essayez `FROM debian:12` au lieu de Alpine).
- Posez-vous la question : "Si je devais expliquer ce Dockerfile à quelqu’un, saurais-je le faire ?"

---

## 6. **Résumé**

- **Un Dockerfile par service, écrit à la main**.
- **Pas d’images toutes faites** autres qu’Alpine/Debian de base.
- **Comprendre, tester, personnaliser**.
- **Savoir expliquer ce qu’on a fait.**

**En maîtrisant cette étape, vous saurez créer des images Docker personnalisées et comprendre la configuration de chaque service de votre projet.**