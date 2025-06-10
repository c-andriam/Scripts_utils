# Le Dockerfile (Suite) : `CMD` – Définir la Commande par Défaut du Conteneur

L'instruction `CMD` a pour but principal de fournir la **commande par défaut** qui sera exécutée lorsqu'un conteneur est démarré à partir de l'image, si l'utilisateur ne spécifie pas lui-même une commande lors du `docker run`.

## g) `CMD` : Spécifier la Commande par Défaut à l'Exécution

*   **Objectif :** Définir la commande qu'un conteneur exécutera par défaut au démarrage.
*   **Syntaxe :** Il existe trois formes, mais les deux premières sont les plus courantes et recommandées :
    1.  **Forme `exec` (préférée) :**
        `CMD ["executable", "param1", "param2"]`
        *   C'est la forme recommandée. La commande est exécutée directement, sans passer par un shell.
        *   *Exemple :* `CMD ["python", "app.py"]`
        *   *Exemple :* `CMD ["/usr/sbin/nginx", "-g", "daemon off;"]`

    2.  **Forme `shell` :**
        `CMD commande param1 param2`
        *   La commande est exécutée via `/bin/sh -c "commande param1 param2"`.
        *   Cette forme peut être utile si vous avez besoin de fonctionnalités de shell (comme la substitution de variables d'environnement directement dans la commande, bien que ce soit souvent mieux géré par l'application elle-même ou un script d'entrée).
        *   *Exemple :* `CMD echo "Bienvenue, l'heure est $(date)"` (Note : la substitution de `$(date)` se fera par le shell à l'intérieur du conteneur).

    3.  **Forme pour les paramètres par défaut de `ENTRYPOINT` (forme `exec`) :**
        `CMD ["param1_par_defaut", "param2_par_defaut"]`
        *   Cette forme est utilisée lorsque vous avez également une instruction `ENTRYPOINT` (que nous verrons ensuite). Dans ce cas, `CMD` fournit les arguments par défaut à l'`ENTRYPOINT`.
        *   *Exemple :* Si `ENTRYPOINT ["/usr/bin/git"]`, alors `CMD ["--help"]` ferait que `docker run <image>` exécute `/usr/bin/git --help`.

*   **Comportement Clé :**
    *   Il ne peut y avoir **qu'une seule instruction `CMD`** dans un `Dockerfile`. Si vous en mettez plusieurs, seule la dernière sera prise en compte.
    *   **Surchargable :** Si l'utilisateur spécifie une commande à la fin de `docker run <image_name> <autre_commande>`, cette `<autre_commande>` **remplacera complètement** la commande définie par `CMD` dans le `Dockerfile`.
        *   *Exemple :* Si `Dockerfile` a `CMD ["python", "app.py"]`
            *   `docker run mon_image` exécutera `python app.py`.
            *   `docker run mon_image /bin/bash` exécutera `/bin/bash` (et non `python app.py`).
            *   `docker run mon_image python utils.py --debug` exécutera `python utils.py --debug`.

**Exemples Concrets :**

1.  **Lancer une application Python (forme `exec`) :**
    ```dockerfile
    FROM python:3.9-slim
    WORKDIR /app
    COPY . .
    RUN pip install -r requirements.txt
    ENV PORT 5000

    # Commande par défaut : démarrer l'application Flask
    CMD ["flask", "run", "--host=0.0.0.0", "--port=$PORT"]
    ```
    *   Si on fait `docker run mon_image_flask`, cela lancera `flask run --host=0.0.0.0 --port=5000` (en supposant que `PORT` est bien à 5000).
    *   Si on fait `docker run mon_image_flask python --version`, cela affichera la version de Python et ne lancera pas Flask.

2.  **Lancer un serveur Nginx (forme `exec`) :**
    ```dockerfile
    FROM nginx:alpine
    # Nginx est généralement configuré pour tourner en tâche de fond (daemon).
    # Pour un conteneur, on veut qu'il tourne au premier plan.
    # L'image Nginx officielle a déjà un ENTRYPOINT, donc CMD fournit les arguments par défaut.
    # La CMD par défaut de l'image nginx:alpine est souvent ["nginx", "-g", "daemon off;"]
    # Mais si nous voulions la spécifier nous-mêmes (ou la modifier):
    CMD ["nginx", "-g", "daemon off;"]
    ```

3.  **Utiliser la forme `shell` (moins courante pour la commande principale) :**
    ```dockerfile
    FROM alpine
    ENV NOM="Mickey"
    # La variable d'environnement NOM sera interprétée par le shell du conteneur
    CMD echo "Bonjour $NOM ! Ce conteneur est prêt."
    ```
    *   `docker run mon_image_echo` affichera "Bonjour Mickey ! Ce conteneur est prêt."

4.  **`CMD` fournissant des arguments par défaut à `ENTRYPOINT` (aperçu) :**
    Imaginons un `Dockerfile` avec :
    ```dockerfile
    # ... autres instructions ...
    ENTRYPOINT ["/usr/local/bin/mon_outil_cli"]
    CMD ["--verbose", "process_par_defaut"]
    ```
    *   `docker run mon_image` exécuterait `/usr/local/bin/mon_outil_cli --verbose process_par_defaut`.
    *   `docker run mon_image --aide` exécuterait `/usr/local/bin/mon_outil_cli --aide` (la `CMD` est remplacée).

*   **Quand utiliser `CMD` ?**
    *   Principalement pour spécifier le programme principal que l'image est censée exécuter par défaut.
    *   Quand vous voulez que l'utilisateur de l'image puisse facilement remplacer cette commande par défaut en fournissant une autre commande à `docker run`.

---

`CMD` est donc notre moyen de dire "voici ce que ce conteneur fait par défaut quand on le lance". La possibilité de le surcharger facilement est une caractéristique clé.

Est-ce que le rôle, la syntaxe et le comportement de `CMD` sont clairs ? Avez-vous des questions sur la manière dont il est surchargé ou sur ses différentes formes ?

Ensuite, nous aborderons `ENTRYPOINT`, qui est similaire mais avec des nuances importantes, notamment sur la manière dont il est (ou n'est pas) surchargé.