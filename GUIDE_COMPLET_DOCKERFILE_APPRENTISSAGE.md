# Guide Complet pour Apprendre et Maîtriser le Dockerfile

## 1. **Qu'est-ce qu'un Dockerfile ?**

Un **Dockerfile** est un fichier texte qui contient une série d'instructions pour créer automatiquement une image Docker. C'est comme une "recette de cuisine" qui dit à Docker :
- Quelle base utiliser
- Quoi installer
- Comment configurer
- Quoi faire au démarrage

**Analogie simple :** Si une image Docker est une "photo" d'un système prêt à l'emploi, le Dockerfile est la "recette" pour créer cette photo.

---

## 2. **Structure de base d'un Dockerfile**

```dockerfile
# Commentaire : Utilise une image de base
FROM debian:11

# Commentaire : Met à jour et installe des paquets
RUN apt-get update && apt-get install -y nginx

# Commentaire : Copie un fichier local dans l'image
COPY index.html /var/www/html/

# Commentaire : Expose le port 80
EXPOSE 80

# Commentaire : Commande à lancer au démarrage
CMD ["nginx", "-g", "daemon off;"]
```

---

## 3. **Les Instructions Principales (À connaître absolument)**

### **FROM** - L'image de base (OBLIGATOIRE)
```dockerfile
FROM debian:11
# Choisis l'image de base sur laquelle construire
# Pour Inception : toujours Debian ou Alpine (avant-dernière version)
```

### **RUN** - Exécuter des commandes
```dockerfile
RUN apt-get update && apt-get install -y nginx
# Exécute des commandes PENDANT la construction de l'image
# Chaque RUN crée une nouvelle "couche" dans l'image
```

### **COPY** - Copier des fichiers
```dockerfile
COPY ./config/nginx.conf /etc/nginx/nginx.conf
# Copie des fichiers de ton ordinateur vers l'image
# Chemin source (local) -> Chemin destination (dans l'image)
```

### **EXPOSE** - Déclarer un port
```dockerfile
EXPOSE 80
# Indique que le conteneur écoutera sur ce port
# Ne publie PAS automatiquement le port (c'est juste une documentation)
```

### **CMD** - Commande par défaut
```dockerfile
CMD ["nginx", "-g", "daemon off;"]
# Commande lancée quand le conteneur démarre
# Format recommandé : ["executable", "param1", "param2"]
```

---

## 4. **Instructions Avancées (Pour aller plus loin)**

### **ENV** - Variables d'environnement
```dockerfile
ENV DEBIAN_FRONTEND=noninteractive
ENV MYSQL_DATABASE=wordpress
# Définit des variables d'environnement utilisables dans l'image
```

### **WORKDIR** - Répertoire de travail
```dockerfile
WORKDIR /var/www/html
# Change le répertoire courant pour les instructions suivantes
# Équivalent à "cd /var/www/html"
```

### **USER** - Changer d'utilisateur
```dockerfile
USER www-data
# Les commandes suivantes s'exécutent avec cet utilisateur
# Bonne pratique de sécurité : ne pas rester en root
```

### **ENTRYPOINT** - Point d'entrée fixe
```dockerfile
ENTRYPOINT ["docker-entrypoint.sh"]
CMD ["mysqld"]
# ENTRYPOINT : commande qui ne peut pas être écrasée
# CMD : arguments par défaut pour ENTRYPOINT
```

---

## 5. **Exemple Pratique Étape par Étape : Créer une image NGINX**

### **Étape 1 : Créer les fichiers**
```bash
mkdir mon-nginx
cd mon-nginx
touch Dockerfile
echo "<h1>Hello from Docker!</h1>" > index.html
```

### **Étape 2 : Écrire le Dockerfile**
```dockerfile
# Utilise Debian 11 comme base
FROM debian:11

# Évite les questions interactives
ENV DEBIAN_FRONTEND=noninteractive

# Met à jour et installe NGINX
RUN apt-get update && \
    apt-get install -y nginx && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# Copie notre page HTML personnalisée
COPY index.html /var/www/html/

# Expose le port 80
EXPOSE 80

# Lance NGINX en premier plan (obligatoire pour Docker)
CMD ["nginx", "-g", "daemon off;"]
```

### **Étape 3 : Construire l'image**
```bash
docker build -t mon-nginx:latest .
# -t : donne un nom/tag à l'image
# . : utilise le répertoire courant comme contexte
```

### **Étape 4 : Lancer un conteneur**
```bash
docker run -d -p 8080:80 --name test-nginx mon-nginx:latest
# -d : en arrière-plan
# -p 8080:80 : redirige le port 8080 de l'hôte vers le port 80 du conteneur
# --name : donne un nom au conteneur
```

### **Étape 5 : Tester**
```bash
curl http://localhost:8080
# Ou ouvre ton navigateur sur http://localhost:8080
```

---

## 6. **Bonnes Pratiques (TRÈS IMPORTANT)**

### **Optimiser les couches**
```dockerfile
# ❌ Mauvais : crée plusieurs couches
RUN apt-get update
RUN apt-get install -y nginx
RUN apt-get clean

# ✅ Bon : une seule couche
RUN apt-get update && \
    apt-get install -y nginx && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*
```

### **Nettoyer après installation**
```dockerfile
RUN apt-get update && \
    apt-get install -y package && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*
# Supprime les caches pour réduire la taille de l'image
```

### **Sécurité : pas de mots de passe**
```dockerfile
# ❌ Interdit
ENV MYSQL_ROOT_PASSWORD=monpassword

# ✅ Utilise des variables externes
ENV MYSQL_ROOT_PASSWORD_FILE=/run/secrets/db_password
```

---

## 7. **Commandes Docker pour travailler avec les Dockerfiles**

### **Construire une image**
```bash
docker build -t nom-image:tag .
docker build -t nginx-custom:v1.0 .
```

### **Lister les images**
```bash
docker images
```

### **Supprimer une image**
```bash
docker rmi nom-image:tag
```

### **Voir l'historique d'une image**
```bash
docker history nom-image:tag
```

### **Inspecter une image**
```bash
docker inspect nom-image:tag
```

---

## 8. **Debugging : Erreurs Courantes**

### **Erreur : "No such file or directory"**
```dockerfile
# Problème : fichier inexistant
COPY config.txt /app/

# Solution : vérifier que le fichier existe localement
```

### **Erreur : "Permission denied"**
```dockerfile
# Problème : permissions manquantes
COPY script.sh /app/
CMD ["/app/script.sh"]

# Solution : ajouter les permissions
COPY script.sh /app/
RUN chmod +x /app/script.sh
CMD ["/app/script.sh"]
```

### **Erreur : Le conteneur s'arrête immédiatement**
```dockerfile
# Problème : CMD n'est pas en premier plan
CMD ["nginx"]

# Solution : forcer le premier plan
CMD ["nginx", "-g", "daemon off;"]
```

---

## 9. **Exercices Pratiques pour Maîtriser**

### **Exercice 1 : Image PHP simple**
Crée un Dockerfile qui :
- Part de `debian:11`
- Installe PHP
- Copie un fichier `info.php` contenant `<?php phpinfo(); ?>`
- Lance PHP en serveur web sur le port 8000

### **Exercice 2 : Image MariaDB basique**
Crée un Dockerfile qui :
- Part de `debian:11`
- Installe MariaDB
- Configure une base de données de test
- Expose le port 3306

---

## 10. **Structure pour le Projet Inception**

```
srcs/
├── requirements/
│   ├── nginx/
│   │   ├── Dockerfile
│   │   └── conf/
│   ├── wordpress/
│   │   ├── Dockerfile
│   │   └── tools/
│   └── mariadb/
│       ├── Dockerfile
│       └── tools/
└── docker-compose.yml
```

---

## 11. **Ressources pour Continuer à Apprendre**

- **Documentation officielle :** https://docs.docker.com/engine/reference/builder/
- **Exemples GitHub :** Cherche "dockerfile examples" sur GitHub
- **Practice Labs :** https://labs.play-with-docker.com/

---

## 12. **Checklist de Maîtrise**

Tu maîtrises le Dockerfile quand tu peux :
- [ ] Expliquer ce que fait chaque instruction (FROM, RUN, COPY, CMD...)
- [ ] Écrire un Dockerfile fonctionnel pour un service simple
- [ ] Construire une image et lancer un conteneur
- [ ] Déboguer les erreurs de construction
- [ ] Optimiser la taille et les performances d'une image
- [ ] Appliquer les bonnes pratiques de sécurité

---

**Maintenant, passe à la pratique ! Commence par l'exercice NGINX ci-dessus, puis adapte pour tes autres services du projet Inception.**