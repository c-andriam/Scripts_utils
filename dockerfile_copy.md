# Le Dockerfile (Suite) : `COPY` – Copier des Fichiers et Répertoires Locaux

Lorsque vous construisez une image Docker, vous avez souvent besoin d'inclure des fichiers de votre projet dans cette image. Il peut s'agir du code source de votre application, de fichiers de configuration, de scripts, d'assets (images, CSS, JS pour un site web), etc.

L'instruction `COPY` est utilisée pour cela. Elle copie des fichiers ou des répertoires depuis le **contexte de build** (généralement le répertoire où se trouve votre `Dockerfile` et les fichiers de votre projet) vers le système de fichiers de l'image en cours de construction.

## c) `COPY` : Copier des Fichiers/Dossiers depuis le Contexte de Build

*   **Objectif :** Transférer des fichiers et des répertoires de votre machine (où vous exécutez la commande `docker build`) vers l'intérieur de l'image Docker.
*   **Syntaxe :**
    `COPY [--chown=<user>:<group>] <source>... <destination>`
    `COPY [--chown=<user>:<group>] ["<source>",... "<destination>"]` (cette forme est requise pour les chemins contenant des espaces)

    *   `<source>` : Le chemin d'un fichier ou d'un répertoire **sur votre machine locale**, relatif au contexte de build.
        *   Vous pouvez utiliser des wildcards (caractères génériques) comme `*` (remplace n'importe quelle séquence de caractères sauf `/`) et `?` (remplace un seul caractère sauf `/`). Par exemple, `COPY *.txt /app/` copierait tous les fichiers `.txt`.
        *   Si la source est un répertoire, tout son contenu (fichiers et sous-répertoires) est copié.
    *   `<destination>` : Le chemin **à l'intérieur de l'image Docker** où les fichiers/répertoires doivent être copiés.
        *   Si la destination n'existe pas, Docker la crée (ainsi que les répertoires parents nécessaires).
        *   Si la destination se termine par un `/`, elle est considérée comme un répertoire, et la source y sera copiée.
        *   Si la destination est un fichier existant, il sera écrasé.
    *   `--chown=<user>:<group>` (optionnel) : Permet de spécifier le propriétaire (utilisateur et groupe) des fichiers copiés à l'intérieur de l'image. C'est utile pour des raisons de sécurité et de permissions. `<user>` et `<group>` peuvent être des noms ou des IDs.

*   **Fonctionnement :**
    1.  Docker localise les fichiers/répertoires source dans le contexte de build.
    2.  Il les copie dans une nouvelle couche de l'image, à l'emplacement de destination spécifié.
    3.  Chaque instruction `COPY` crée une nouvelle couche dans l'image.

*   **Contexte de Build :**
    Lorsque vous exécutez la commande `docker build <chemin_vers_contexte>`, Docker envoie d'abord l'ensemble du répertoire `<chemin_vers_contexte>` (et ses sous-répertoires) au démon Docker. C'est ce qu'on appelle le "contexte de build". L'instruction `COPY` ne peut accéder qu'aux fichiers présents dans ce contexte.
    *   Il est donc important de ne pas avoir de fichiers inutiles ou sensibles (comme des `.git` ou des secrets) à la racine de votre contexte de build, ou d'utiliser un fichier `.dockerignore` pour les exclure (nous verrons `.dockerignore` plus tard).

**Exemples Concrets :**

1.  **Copier un seul fichier :**
    Supposons que vous ayez un fichier `mon_app.py` à côté de votre `Dockerfile`.
    ```dockerfile
    FROM python:3.9-slim

    # Copier le fichier mon_app.py de votre machine locale
    # dans le répertoire /usr/src/app/ à l'intérieur de l'image.
    # Si /usr/src/app/ n'existe pas, il sera créé.
    COPY mon_app.py /usr/src/app/mon_app.py
    ```
    Ou, si `/usr/src/app/` doit être un répertoire :
    ```dockerfile
    FROM python:3.9-slim

    RUN mkdir -p /usr/src/app
    COPY mon_app.py /usr/src/app/
    # mon_app.py sera copié en tant que /usr/src/app/mon_app.py
    ```

2.  **Copier tout le contenu d'un répertoire :**
    Supposons que vous ayez un répertoire `src/` à côté de votre `Dockerfile` contenant le code de votre application.
    ```dockerfile
    FROM node:18-alpine

    # Créer un répertoire de travail dans l'image
    RUN mkdir -p /home/node/app
    WORKDIR /home/node/app # Nous verrons WORKDIR plus tard

    # Copier tout le contenu du répertoire 'src' de votre machine locale
    # dans le répertoire courant de l'image (/home/node/app ici, grâce à WORKDIR)
    COPY src/ .
    # Si src/ contient main.js et utils/helper.js, vous aurez :
    # /home/node/app/main.js
    # /home/node/app/utils/helper.js

    # Alternative sans WORKDIR pour être explicite :
    # COPY src/ /home/node/app/
    ```
    Le `.` à la fin de `COPY src/ .` signifie "copier dans le répertoire de travail courant" (défini par `WORKDIR` ou la racine `/` si `WORKDIR` n'est pas utilisé).

3.  **Copier des fichiers spécifiques avec un wildcard :**
    ```dockerfile
    FROM ubuntu:22.04

    RUN mkdir /config
    # Copier tous les fichiers se terminant par .conf depuis le répertoire 'configs' local
    # vers le répertoire /config de l'image.
    COPY configs/*.conf /config/
    ```

4.  **Copier avec changement de propriétaire :**
    Supposons que vous ayez créé un utilisateur `appuser` dans votre image.
    ```dockerfile
    FROM alpine
    RUN addgroup -S appgroup && adduser -S appuser -G appgroup

    WORKDIR /app
    # Copier le fichier app.jar et le rendre appartenant à appuser:appgroup
    COPY --chown=appuser:appgroup target/app.jar .
    ```

*   **`COPY` vs `ADD` (un aperçu rapide) :**
    Il existe une autre instruction, `ADD`, qui ressemble beaucoup à `COPY`. `ADD` a quelques fonctionnalités supplémentaires (comme la décompression automatique des archives `.tar.gz` et la possibilité de copier depuis des URLs), mais ces fonctionnalités peuvent rendre son comportement moins prévisible.
    **La recommandation générale est de préférer `COPY`** pour sa simplicité et sa transparence lorsque vous copiez simplement des fichiers locaux. Nous parlerons de `ADD` plus en détail ensuite.

---

L'instruction `COPY` est essentielle pour amener le code de votre application et ses fichiers de configuration à l'intérieur de l'image que vous construisez.

Est-ce que l'utilisation de `COPY`, sa syntaxe et ses exemples sont clairs pour vous ? Avez-vous des questions sur le contexte de build ou la manière dont les chemins sont interprétés ?

Une fois que `COPY` est bien compris, nous pourrons aborder `ADD` et discuter des cas où l'une est préférable à l'autre.