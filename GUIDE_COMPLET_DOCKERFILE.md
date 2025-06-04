# Guide Complet pour Comprendre et Maîtriser le Dockerfile

Un **Dockerfile** est un fichier texte qui décrit comment construire une image Docker personnalisée. L’image, une fois construite, peut être utilisée pour lancer des conteneurs avec l’application ou le service de votre choix, dans l’environnement exact que vous aurez défini.

---

## 1. **Qu’est-ce qu’un Dockerfile ?**

Un **Dockerfile** est une "recette" qui indique à Docker :
- Quelle image de base utiliser,
- Quelles commandes exécuter pour installer/configurer le service,
- Quels fichiers copier,
- Quelles variables d’environnement définir,
- Quelle commande démarrer au lancement du conteneur.

---

## 2. **Structure et Syntaxe de base**

Un Dockerfile est lu **ligne par ligne** de haut en bas.  
Voici un exemple minimal :

```dockerfile
# Utilise une image de base officielle
FROM debian:11

# Ajoute des métadonnées (optionnel)
LABEL maintainer="ton.email@exemple.com"

# Définit des variables d’environnement
ENV DEBIAN_FRONTEND=noninteractive

# Exécute des commandes dans l’image (installation de paquets, configuration, etc.)
RUN apt-get update && apt-get install -y nginx

# Copie des fichiers locaux dans l’image
COPY ./conf/nginx.conf /etc/nginx/nginx.conf

# Définit le port à exposer
EXPOSE 80

# Commande à exécuter au démarrage du conteneur
CMD ["nginx", "-g", "daemon off;"]
```

---

## 3. **Les instructions Dockerfile les plus courantes**

### - **FROM**
Spécifie l’image de base (obligatoire, doit toujours être la première instruction non-commentaire).
```dockerfile
FROM debian:11
```

### - **LABEL**
Ajoute des métadonnées à l’image.
```dockerfile
LABEL version="1.0"
LABEL description="Image NGINX custom"
```

### - **ENV**
Définit des variables d’environnement.
```dockerfile
ENV DEBIAN_FRONTEND=noninteractive
```

### - **RUN**
Exécute une commande dans l’image pendant la construction (souvent installation de paquets, configuration, etc.).
```dockerfile
RUN apt-get update && apt-get install -y nginx
```

### - **COPY**
Copie des fichiers ou dossiers locaux dans l’image.
```dockerfile
COPY ./conf/nginx.conf /etc/nginx/nginx.conf
```

### - **ADD**
Comme COPY mais peut aussi décompresser des archives ou récupérer des fichiers à distance (préférer COPY sauf besoin spécifique).
```dockerfile
ADD archive.tar.gz /app/
```

### - **EXPOSE**
Indique le port sur lequel l’application va écouter.
```dockerfile
EXPOSE 80
```

### - **WORKDIR**
Change le répertoire de travail pour les instructions suivantes.
```dockerfile
WORKDIR /var/www/html
```

### - **USER**
Spécifie quel utilisateur va exécuter les commandes suivantes.
```dockerfile
USER www-data
```

### - **CMD**
Commande lancée par défaut au démarrage du conteneur (une seule par Dockerfile, peut être écrasée par `docker run`).
```dockerfile
CMD ["nginx", "-g", "daemon off;"]
```

### - **ENTRYPOINT**
Commande de démarrage qui ne peut pas être écrasée facilement (voir la différence avec CMD).
```dockerfile
ENTRYPOINT ["mysqld"]
```

---

## 4. **Bonnes pratiques**

- **Une image = Un service** : chaque Dockerfile doit installer/configurer un seul service.
- **Toujours nettoyer les caches** après installation pour limiter la taille de l’image.
    ```dockerfile
    RUN apt-get update && apt-get install -y nginx && apt-get clean && rm -rf /var/lib/apt/lists/*
    ```
- **Pas de mot de passe/secrets dans le Dockerfile**. Utilisez les variables d’environnement ou des fichiers secrets.
- **Utilisez .dockerignore** pour ne pas inclure des fichiers inutiles (ex : .git, secrets, node_modules…).

---

## 5. **Cycle de vie d’un Dockerfile**

1. **Écriture** : créez et remplissez le Dockerfile.
2. **Construction** : créez l’image Docker avec :
    ```bash
    docker build -t monimage:latest .
    ```
3. **Test** : lancez un conteneur à partir de l’image :
    ```bash
    docker run --name monconteneur -p 8080:80 monimage:latest
    ```
4. **Amélioration** : modifiez, optimisez, ajoutez des commandes, puis recommencez à build et tester.

---

## 6. **Utilisation avancée**

- **Scripts d’initialisation** : Utilisez `COPY` pour ajouter vos scripts et lancez-les dans CMD ou ENTRYPOINT.
- **Multi-stage builds** : Pour des images plus légères (compilation dans une étape, copie du résultat dans une autre).
- **Variables dynamiques** : Utilisez `ARG` pour des variables de build que vous pouvez passer à `docker build`.

---

## 7. **Exemples de Dockerfile pour différents services**

### **NGINX**
```dockerfile
FROM debian:11
ENV DEBIAN_FRONTEND=noninteractive
RUN apt-get update && apt-get install -y nginx && apt-get clean && rm -rf /var/lib/apt/lists/*
COPY ./conf/nginx.conf /etc/nginx/nginx.conf
EXPOSE 443
CMD ["nginx", "-g", "daemon off;"]
```

### **MariaDB**
```dockerfile
FROM debian:11
ENV DEBIAN_FRONTEND=noninteractive
RUN apt-get update && apt-get install -y mariadb-server && apt-get clean && rm -rf /var/lib/apt/lists/*
COPY ./tools/init_db.sh /init_db.sh
RUN chmod +x /init_db.sh
EXPOSE 3306
CMD ["/init_db.sh"]
```

### **WordPress (PHP-FPM)**
```dockerfile
FROM debian:11
ENV DEBIAN_FRONTEND=noninteractive
RUN apt-get update && apt-get install -y php php-fpm php-mysql wget unzip && apt-get clean && rm -rf /var/lib/apt/lists/*
WORKDIR /var/www/html
RUN wget https://wordpress.org/latest.zip && unzip latest.zip && mv wordpress/* . && rm -rf wordpress latest.zip
EXPOSE 9000
CMD ["php-fpm7.4", "-F"]
```

---

## 8. **Différences entre CMD et ENTRYPOINT**

- **CMD** : Commande par défaut, peut être écrasée par `docker run ... <commande>`.
- **ENTRYPOINT** : Commande fixe, les arguments de `docker run ...` sont ajoutés à la suite.

---

## 9. **Exemple de .dockerignore**

À placer à la racine du build pour exclure des fichiers du contexte de build.

```
.git
secrets/
node_modules/
*.log
```

---

## 10. **Ressources pour aller plus loin**

- [Documentation officielle Dockerfile (fr)](https://docs.docker.com/engine/reference/builder/)
- [Play with Docker (bac à sable en ligne)](https://labs.play-with-docker.com/)
- [Dockerfile Best Practices (en)](https://docs.docker.com/develop/develop-images/dockerfile_best-practices/)

---

## 11. **Résumé visuel des instructions importantes**

| Instruction  | Usage principal                                 |
|--------------|-------------------------------------------------|
| FROM         | Image de base                                   |
| LABEL        | Métadonnées                                     |
| ENV          | Variable d’environnement                        |
| RUN          | Exécuter une commande pendant le build          |
| COPY/ADD     | Copier fichiers locaux dans l’image             |
| WORKDIR      | Changer le répertoire de travail                |
| EXPOSE       | Ouvrir un port                                  |
| CMD          | Commande de démarrage par défaut                 |
| ENTRYPOINT   | Commande de démarrage principale                 |
| USER         | Définir l’utilisateur pour les instructions      |

---

## 12. **Comment s’entraîner ?**

- Refais plusieurs Dockerfiles pour des services simples (nginx, apache, node, python…).
- Teste chaque image avec `docker build` puis `docker run`.
- Modifie, optimise, nettoie tes images.
- Essaye de lire et comprendre des Dockerfiles open source.

---

**Maîtriser le Dockerfile, c’est comprendre comment automatiser la création d’environnements reproductibles et portables pour tous tes projets !**
