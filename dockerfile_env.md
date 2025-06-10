# Le Dockerfile (Suite) : `ENV` – Définir des Variables d'Environnement

L'instruction `ENV` permet de définir des variables d'environnement. Ces variables seront disponibles pour toutes les instructions suivantes dans le `Dockerfile` (pendant la phase de construction de l'image) ET pour l'application qui s'exécutera dans un conteneur démarré à partir de cette image.

## f) `ENV` : Spécifier des Variables d'Environnement

*   **Objectif :** Définir des paires clé-valeur qui seront disponibles comme variables d'environnement.
*   **Syntaxe :** Il y a deux formes :
    1.  `ENV <clé> <valeur>` (pour une seule variable)
        *   La `<valeur>` sera le reste de la ligne. Si la valeur contient des espaces, vous devez les échapper avec `\` ou mettre la valeur entre guillemets.
        *   *Exemple :* `ENV MY_NAME "John Doe"`
        *   *Exemple :* `ENV MY_VERSION 1.0`

    2.  `ENV <clé1>=<valeur1> <clé2>=<valeur2> ...` (pour plusieurs variables en une seule instruction)
        *   C'est souvent la forme préférée car elle ne crée qu'une seule couche pour plusieurs variables.
        *   *Exemple :* `ENV APP_VERSION="1.0.2" DB_HOST=mydatabase`

*   **Fonctionnement :**
    *   L'instruction `ENV` définit une variable d'environnement.
    *   Cette variable peut être utilisée par les instructions suivantes dans le `Dockerfile` en utilisant la syntaxe `$variable_name` ou `${variable_name}` (cette dernière est utile si la variable est adjacente à d'autres caractères, par exemple `${variable_name}_suffix`).
    *   La variable sera également définie dans l'environnement de tout conteneur lancé à partir de l'image résultante.
    *   Chaque instruction `ENV` (même si elle définit plusieurs variables avec la syntaxe `<clé>=<valeur> ...`) crée une nouvelle couche dans l'image. C'est pourquoi la deuxième forme est souvent privilégiée pour définir plusieurs variables afin de minimiser le nombre de couches.

*   **Persistance :** Les variables d'environnement définies avec `ENV` sont persistantes dans l'image. Elles peuvent être inspectées et peuvent être surchargées au moment de l'exécution d'un conteneur (avec l'option `-e` ou `--env` de `docker run`).

**Exemples Concrets :**

1.  **Définir une version d'application et l'utiliser :**
    ```dockerfile
    FROM alpine
    LABEL maintainer="c-andriam" # Nous verrons LABEL plus tard

    ENV APP_NAME="MaSuperApp"
    ENV APP_VERSION="1.0"

    # Utilisation dans une instruction RUN
    RUN echo "Construction de $APP_NAME version $APP_VERSION..."

    # Utilisation dans WORKDIR
    WORKDIR /opt/$APP_NAME

    # La variable sera aussi disponible quand le conteneur tournera
    # CMD ["echo", "Bienvenue dans $APP_NAME v${APP_VERSION}"] (nous verrons CMD plus tard)
    ```

2.  **Définir plusieurs variables en une seule instruction (meilleure pratique pour les couches) :**
    ```dockerfile
    FROM ubuntu:22.04

    ENV JAVA_HOME="/usr/lib/jvm/java-11-openjdk-amd64" \
        PATH="$PATH:/usr/lib/jvm/java-11-openjdk-amd64/bin" \
        APP_PORT="8080"

    # Vérifier que JAVA_HOME est bien défini
    RUN echo "JAVA_HOME est à : $JAVA_HOME"
    RUN echo "Le PATH inclut maintenant Java : $PATH"

    # Ces variables (JAVA_HOME, PATH, APP_PORT) seront disponibles
    # pour l'application Java qui tournera dans le conteneur.
    ```
    Notez l'utilisation de `\` pour continuer une longue instruction sur plusieurs lignes pour la lisibilité.

3.  **Utilisation pour des configurations :**
    ```dockerfile
    FROM python:3.9-slim
    WORKDIR /app

    # Configuration par défaut pour l'application
    ENV FLASK_APP="main.py" \
        FLASK_ENV="production" \
        PORT="5000"

    COPY . .
    RUN pip install flask

    # La commande CMD utilisera implicitement ces variables d'environnement
    # CMD ["flask", "run", "--host=0.0.0.0", "--port=$PORT"] (syntaxe illustratrice)
    ```
    L'application Flask pourra lire `FLASK_APP`, `FLASK_ENV`, et `PORT` depuis l'environnement.

*   **Variables d'environnement "bien connues" :**
    Certaines variables d'environnement ont une signification particulière pour les systèmes d'exploitation ou les logiciels, comme `PATH` (liste des répertoires où chercher les exécutables), `LANG` (paramètres de langue), `JAVA_HOME` (chemin d'installation de Java), `http_proxy`, `https_proxy`, etc. Vous pouvez les définir avec `ENV` pour configurer le comportement de l'environnement dans votre image.

*   **`ARG` vs `ENV` (un aperçu rapide) :**
    Il existe une autre instruction, `ARG`, qui définit des variables uniquement disponibles pendant la *construction* de l'image (elles ne sont pas persistées dans l'image finale ni disponibles pour le conteneur en exécution, sauf si elles sont aussi définies avec `ENV`). Nous verrons `ARG` plus en détail. `ENV` est pour les variables qui doivent être disponibles à la fois pendant la construction (pour les instructions `RUN`, etc.) et à l'exécution.

---

L'instruction `ENV` est cruciale pour paramétrer votre image et fournir des configurations à votre application de manière flexible. Elle permet de découpler la configuration de l'application du code lui-même.

Est-ce que le rôle et l'utilisation de `ENV` sont clairs ? Avez-vous des questions sur la portée des variables, la syntaxe, ou la différence potentielle avec d'autres types de variables ?

Après cela, nous pourrons aborder les instructions qui définissent ce qui se passe lorsque le conteneur démarre : `CMD` et `ENTRYPOINT`.