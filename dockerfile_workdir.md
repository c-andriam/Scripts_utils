# Le Dockerfile (Suite) : `WORKDIR` – Définir le Répertoire de Travail

Lorsque vous exécutez des commandes avec `RUN`, copiez des fichiers avec `COPY`/`ADD`, ou définissez la commande par défaut de votre conteneur (avec `CMD`/`ENTRYPOINT`, que nous verrons plus tard), ces opérations se font par rapport à un "répertoire courant" à l'intérieur de l'image/conteneur.

L'instruction `WORKDIR` vous permet de définir ce répertoire de travail pour toutes les instructions `RUN`, `CMD`, `ENTRYPOINT`, `COPY` et `ADD` qui la suivent dans le `Dockerfile`.

## e) `WORKDIR` : Spécifier le Répertoire de Travail Courant

*   **Objectif :** Définir le répertoire de travail qui sera utilisé comme point de départ pour les instructions suivantes dans le `Dockerfile`, ainsi que pour la commande par défaut du conteneur en cours d'exécution.
*   **Syntaxe :**
    `WORKDIR /chemin/vers/le/repertoire`

*   **Fonctionnement :**
    *   Si le répertoire spécifié dans `WORKDIR` n'existe pas, il sera **automatiquement créé** par Docker (ainsi que tous les répertoires parents nécessaires), comme si vous aviez fait un `mkdir -p`.
    *   `WORKDIR` peut être utilisé plusieurs fois dans un `Dockerfile`. Chaque `WORKDIR` change le répertoire de travail pour les instructions suivantes.
    *   Les chemins peuvent être absolus (commençant par `/`) ou relatifs. Si un chemin est relatif, il est interprété par rapport au `WORKDIR` précédent.

*   **Avantages :**
    *   **Clarté :** Rend votre `Dockerfile` plus lisible en évitant de répéter de longs chemins dans chaque instruction `COPY`, `RUN`, etc.
    *   **Maintenabilité :** Si vous devez changer le répertoire principal de votre application, vous ne le modifiez qu'à un seul endroit (dans l'instruction `WORKDIR`).
    *   **Évite les erreurs :** Réduit le risque de taper incorrectement des chemins longs et complexes.

**Exemples Concrets :**

1.  **Utilisation basique :**
    ```dockerfile
    FROM ubuntu:22.04

    # Définit /app comme répertoire de travail.
    # Si /app n'existe pas, il sera créé.
    WORKDIR /app

    # Les commandes suivantes sont exécutées DANS /app
    RUN touch file1.txt # Crée /app/file1.txt

    COPY mon_script.sh . # Copie mon_script.sh local vers /app/mon_script.sh
                         # Le "." signifie le répertoire de travail courant, donc /app

    RUN ./mon_script.sh  # Exécute /app/mon_script.sh

    # Si le conteneur démarre un shell interactif, il démarrera dans /app
    ```

2.  **`WORKDIR` multiples et chemins relatifs :**
    ```dockerfile
    FROM alpine

    WORKDIR /opt/app
    # Actuellement dans /opt/app

    RUN mkdir logs # Crée /opt/app/logs

    WORKDIR logs
    # Actuellement dans /opt/app/logs (chemin relatif 'logs' par rapport à /opt/app)
    RUN touch app.log # Crée /opt/app/logs/app.log

    WORKDIR /etc
    # Actuellement dans /etc (chemin absolu)

    COPY supervisord.conf . # Copie supervisord.conf local vers /etc/supervisord.conf
    ```

3.  **Importance pour `COPY` et `RUN` :**
    *   **Sans `WORKDIR` :**
        ```dockerfile
        FROM python:3.9-slim
        RUN mkdir -p /usr/src/app
        COPY requirements.txt /usr/src/app/requirements.txt
        COPY myapp/ /usr/src/app/myapp/
        RUN pip install -r /usr/src/app/requirements.txt
        # CMD python /usr/src/app/myapp/main.py (nous verrons CMD plus tard)
        ```
        Ici, vous répétez `/usr/src/app/` partout.

    *   **Avec `WORKDIR` :**
        ```dockerfile
        FROM python:3.9-slim
        WORKDIR /usr/src/app # Défini une seule fois

        COPY requirements.txt . # Copié dans /usr/src/app
        COPY myapp/ ./myapp/   # Copié dans /usr/src/app/myapp

        RUN pip install -r requirements.txt # Exécuté depuis /usr/src/app

        # CMD ["python", "myapp/main.py"] (exécuté depuis /usr/src/app)
        ```
        C'est beaucoup plus propre et facile à lire. Si vous décidez plus tard que votre application doit être dans `/srv/app` au lieu de `/usr/src/app`, vous ne changez que la ligne `WORKDIR`.

*   **`WORKDIR` et variables d'environnement :**
    Vous pouvez utiliser des variables d'environnement (que nous verrons avec `ENV`) dans `WORKDIR`.
    ```dockerfile
    ENV APP_DIR /opt/my_application
    WORKDIR $APP_DIR
    # Le répertoire de travail est maintenant /opt/my_application
    ```

---

L'instruction `WORKDIR` est une aide précieuse pour structurer votre `Dockerfile` et rendre vos chemins plus gérables. C'est une bonne pratique de l'utiliser pour définir le contexte de votre application à l'intérieur de l'image.

Est-ce que l'utilité et le fonctionnement de `WORKDIR` sont clairs ? Avez-vous des questions sur la manière dont il interagit avec d'autres instructions ou sur la création automatique de répertoires ?

Après cela, nous pourrons nous pencher sur la gestion des variables d'environnement avec `ENV`.