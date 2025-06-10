# Le Dockerfile (Suite) : `RUN` – Exécuter des Commandes

## b) `RUN` : Exécuter des Commandes pendant la Construction de l'Image

L'instruction `RUN` vous permet d'exécuter n'importe quelle commande comme si vous étiez dans un terminal à l'intérieur de l'image en cours de construction. Chaque instruction `RUN` crée une nouvelle couche dans l'image.

*   **Objectif :** Installer des paquets logiciels, créer des répertoires, compiler du code, modifier des permissions de fichiers, etc. Tout ce que vous feriez manuellement pour configurer un environnement.

*   **Syntaxe :** Il existe deux formes :
    1.  **Forme shell (shell form) :**
        `RUN <commande>`
        La commande est exécutée dans un shell, qui par défaut est `/bin/sh -c` sous Linux ou `cmd /S /C` sous Windows.
        *   *Analogie :* C'est comme si vous tapiez directement la commande dans un terminal.
        *   *Exemple :* `RUN apt-get update && apt-get install -y curl`

    2.  **Forme exec (exec form) :**
        `RUN ["executable", "param1", "param2"]`
        La commande est exécutée directement, sans passer par un shell. C'est la forme recommandée, surtout si votre commande est complexe ou contient des espaces qui pourraient être mal interprétés par le shell.
        *   *Analogie :* C'est comme si vous appeliez directement un programme avec ses arguments.
        *   *Exemple :* `RUN ["/bin/bash", "-c", "echo hello"]` ou `RUN ["apt-get", "install", "-y", "curl"]`

*   **Fonctionnement :**
    1.  Docker démarre un conteneur temporaire à partir de la couche précédente de l'image.
    2.  Il exécute la commande spécifiée dans `RUN` à l'intérieur de ce conteneur temporaire.
    3.  Si la commande réussit (code de sortie 0), les modifications apportées au système de fichiers du conteneur temporaire sont "commitées" (enregistrées) comme une nouvelle couche de l'image.
    4.  Le conteneur temporaire est ensuite supprimé.
    5.  Si la commande échoue, la construction de l'image s'arrête avec une erreur.

*   **Bonnes Pratiques avec `RUN` :**
    *   **Chaîner les commandes :** Pour réduire le nombre de couches (et donc la taille de l'image finale), il est souvent préférable de chaîner plusieurs commandes logiquement liées avec `&&` dans une seule instruction `RUN`.
        *   *Mauvais (crée 2 couches) :*
            ```dockerfile
            RUN apt-get update
            RUN apt-get install -y curl
            ```
        *   *Bon (crée 1 couche) :*
            ```dockerfile
            RUN apt-get update && apt-get install -y curl
            ```
    *   **Nettoyer après installation :** Après avoir installé des paquets, supprimez les fichiers de cache et les fichiers temporaires qui ne sont plus nécessaires pour réduire la taille de l'image.
        *   *Exemple (pour les systèmes basés sur Debian/Ubuntu) :*
            ```dockerfile
            RUN apt-get update && \
                apt-get install -y --no-install-recommends software-properties-common curl && \
                apt-get clean && \
                rm -rf /var/lib/apt/lists/*
            ```
            (Le `\` à la fin d'une ligne permet de continuer la commande sur la ligne suivante pour une meilleure lisibilité).
            Le `--no-install-recommends` évite d'installer des paquets recommandés qui ne sont pas toujours nécessaires.
    *   **Être spécifique :** Installez uniquement les paquets dont vous avez réellement besoin.

**Exemples Concrets :**

1.  **Installer `curl` et `git` sur une image Debian :**
    ```dockerfile
    FROM debian:bullseye-slim

    # Mettre à jour la liste des paquets et installer curl et git en une seule couche
    # Puis nettoyer le cache d'apt pour réduire la taille de l'image
    RUN apt-get update && \
        apt-get install -y curl git && \
        rm -rf /var/lib/apt/lists/*
    ```
    *Explication :*
    1.  `apt-get update` : Met à jour la liste des paquets disponibles depuis les dépôts configurés dans l'image Debian.
    2.  `apt-get install -y curl git` : Installe les paquets `curl` et `git`. L'option `-y` répond automatiquement "oui" à toutes les confirmations.
    3.  `rm -rf /var/lib/apt/lists/*` : Supprime les listes de paquets téléchargées par `apt-get update`, qui ne sont plus nécessaires une fois les paquets installés et qui prennent de la place.

2.  **Créer un répertoire :**
    ```dockerfile
    FROM alpine:latest # Alpine Linux est une distribution très légère

    # Créer un répertoire /app
    RUN mkdir /app
    # Créer un autre répertoire /data avec des sous-répertoires
    RUN mkdir -p /data/config /data/logs
    ```
    *Explication :*
    1.  `mkdir /app` : Crée un répertoire nommé `app` à la racine du système de fichiers de l'image.
    2.  `mkdir -p /data/config /data/logs` : Crée le répertoire `data` et, à l'intérieur, les sous-répertoires `config` et `logs`. L'option `-p` (parents) crée les répertoires parents s'ils n'existent pas, évitant ainsi une erreur si `/data` n'existait pas encore.

3.  **Utiliser la forme `exec` (moins courant pour des commandes shell simples mais bon à savoir) :**
    ```dockerfile
    FROM ubuntu:22.04

    RUN ["apt-get", "update"]
    RUN ["apt-get", "install", "-y", "nginx"]
    RUN ["rm", "-rf", "/var/lib/apt/lists/*"]
    ```
    Ici, chaque `apt-get` et `rm` est appelé directement comme un exécutable avec ses arguments sous forme de liste JSON.

---

L'instruction `RUN` est l'un des piliers de la construction d'images. Elle vous donne le pouvoir de sculpter l'environnement de votre future application.

Est-ce que le fonctionnement et l'utilité de `RUN` sont clairs pour vous ? Avez-vous des questions sur la syntaxe ou les exemples ?

Une fois que vous êtes à l'aise avec `RUN`, nous pourrons voir comment copier des fichiers de votre machine locale vers l'image en construction avec les instructions `COPY` et `ADD`.