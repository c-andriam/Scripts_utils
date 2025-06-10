# Le Dockerfile (Suite) : `ENTRYPOINT` – Configurer le Conteneur comme un Exécutable

L'instruction `ENTRYPOINT` permet de configurer un conteneur pour qu'il s'exécute comme un exécutable principal. Contrairement à `CMD`, la commande spécifiée par `ENTRYPOINT` n'est pas aussi facilement surchargée par les arguments de `docker run`. Au lieu de cela, les arguments passés à `docker run` sont généralement ajoutés en tant qu'arguments à l'`ENTRYPOINT`.

## h) `ENTRYPOINT` : Définir l'Exécutable Principal du Conteneur

*   **Objectif :** Spécifier le programme principal (l' "entrée") que le conteneur exécutera. L'idée est que l'image est construite pour exécuter *ce programme spécifique*, et les arguments de `docker run` (ou de `CMD`) servent de paramètres à ce programme.

*   **Syntaxe :** Il existe deux formes :
    1.  **Forme `exec` (préférée et la plus courante) :**
        `ENTRYPOINT ["executable", "param1_fixe", "param2_fixe"]`
        *   C'est la forme recommandée. L'exécutable est appelé directement.
        *   Les arguments de `docker run <image> arg_supp1 arg_supp2` seront ajoutés après `param2_fixe`.
        *   La `CMD` (si présente sous forme `exec` : `CMD ["arg_defaut1", "arg_defaut2"]`) fournira des arguments par défaut si aucun n'est donné à `docker run`.
        *   *Exemple :* `ENTRYPOINT ["/usr/bin/mon_app"]`

    2.  **Forme `shell` :**
        `ENTRYPOINT commande param1 param2`
        *   La commande est exécutée via `/bin/sh -c "commande param1 param2"`.
        *   **Important :** Sous cette forme, `CMD` est ignorée, et les arguments de `docker run` ne sont pas passés à l'entrypoint. Le shell consomme les signaux (comme SIGTERM pour arrêter le conteneur), ce qui peut empêcher l'exécutable principal de les recevoir correctement. Pour cette raison, la forme `shell` de `ENTRYPOINT` est **généralement déconseillée**.

*   **Comportement Clé :**
    *   Il ne peut y avoir **qu'une seule instruction `ENTRYPOINT`** dans un `Dockerfile`. Si vous en mettez plusieurs, seule la dernière sera prise en compte.
    *   **Surcharge de `ENTRYPOINT` :** Pour surcharger l'`ENTRYPOINT` lui-même, l'utilisateur doit utiliser l'option `--entrypoint` de `docker run`. Par exemple : `docker run --entrypoint /bin/bash <image_name> -c "ls /"`.
    *   **Interaction avec `CMD` :**
        *   Si `ENTRYPOINT` est sous forme `exec` (la plus courante) :
            *   `CMD` (si présente et sous forme `exec` : `CMD ["arg_defaut1", ...]`) fournit les arguments par défaut à l'`ENTRYPOINT`.
            *   Les arguments passés à `docker run <image> arg_cli1 ...` remplacent ceux de `CMD` et sont passés à l'`ENTRYPOINT`.
        *   Si `ENTRYPOINT` est sous forme `shell` :
            *   `CMD` est ignorée.
            *   Les arguments de `docker run` ne sont pas passés.

**Exemples Concrets :**

1.  **Utiliser l'image comme un outil CLI (forme `exec` pour `ENTRYPOINT` et `CMD`) :**
    Supposons que vous ayez un outil `statistiques` qui prend un nom de fichier en argument.
    ```dockerfile
    FROM alpine
    WORKDIR /app
    COPY statistiques_tool /usr/local/bin/statistiques_tool
    RUN chmod +x /usr/local/bin/statistiques_tool

    ENTRYPOINT ["/usr/local/bin/statistiques_tool"]
    CMD ["--help"] # Argument par défaut si rien n'est fourni à docker run
    ```
    *   `docker run mon_image_stats` exécutera `/usr/local/bin/statistiques_tool --help`.
    *   `docker run mon_image_stats data.csv` exécutera `/usr/local/bin/statistiques_tool data.csv`.
    *   `docker run mon_image_stats data.csv --verbose` exécutera `/usr/local/bin/statistiques_tool data.csv --verbose`.
    *   Pour lancer autre chose, il faut surcharger l'entrypoint : `docker run --entrypoint /bin/sh mon_image_stats`

2.  **Lancer une application avec des paramètres fixes et des paramètres par défaut :**
    ```dockerfile
    FROM openjdk:11-jre-slim
    WORKDIR /app
    COPY my-app.jar .

    # L'application Java est l'exécutable principal
    ENTRYPOINT ["java", "-jar", "my-app.jar"]
    # Arguments par défaut pour l'application Java
    CMD ["--config", "/etc/app/default.conf", "--mode", "production"]
    ```
    *   `docker run mon_app_java` exécutera `java -jar my-app.jar --config /etc/app/default.conf --mode production`.
    *   `docker run mon_app_java --mode staging` exécutera `java -jar my-app.jar --mode staging` (les arguments de `CMD` sont remplacés).

3.  **Forme `shell` pour `ENTRYPOINT` (généralement à éviter, mais pour illustration) :**
    ```dockerfile
    FROM alpine
    ENV NOM="Visiteur"
    # ATTENTION : CMD sera ignorée, et les signaux peuvent mal se comporter.
    ENTRYPOINT echo "Bienvenue à $NOM. L'application principale démarre..." && exec mon_app_shell
    # Si vous faites docker run mon_image_shell arg1 arg2, arg1 et arg2 ne sont pas passés.
    ```
    Dans ce cas, `mon_app_shell` devrait être un script qui gère lui-même les signaux correctement.

## `CMD` vs `ENTRYPOINT` : Lequel Choisir ?

| Caractéristique         | `CMD` (forme exec ou shell)                                     | `ENTRYPOINT` (forme exec)                                                              | `ENTRYPOINT` (forme shell)                                  |
| :---------------------- | :-------------------------------------------------------------- | :------------------------------------------------------------------------------------- | :---------------------------------------------------------- |
| **Objectif Principal**  | Fournir une commande *par défaut* et facilement surchargeable. | Définir l'image comme un *exécutable spécifique*.                                      | Similaire à la forme exec, mais avec les inconvénients du shell. |
| **Surcharge Facile**    | Oui (en ajoutant une commande à `docker run`)                   | Non (nécessite `docker run --entrypoint`)                                              | Non (nécessite `docker run --entrypoint`)                   |
| **Arguments de `docker run`** | Remplacent toute la `CMD`.                                    | Sont ajoutés *après* l'`ENTRYPOINT` (remplaçant les arguments de `CMD` s'il y en a). | Ne sont pas passés.                                         |
| **Utilisation avec l'autre** | Peut être utilisée seule.                                     | Souvent utilisée avec `CMD` pour fournir des arguments par défaut à l'`ENTRYPOINT`. | `CMD` est ignorée.                                          |
| **Forme recommandée**   | `CMD ["executable", "param1"]`                                | `ENTRYPOINT ["executable", "param1"]`                                                | Généralement déconseillée.                                  |

**Scénarios d'utilisation courants :**

1.  **Utiliser `CMD` seul :**
    *   Si vous voulez une image qui exécute une commande par défaut, mais que vous vous attendez à ce que les utilisateurs la surchargent fréquemment avec leurs propres commandes (par exemple, une image `ubuntu` avec `CMD ["/bin/bash"]`).
    *   Pour lancer une application spécifique, mais en gardant la flexibilité de lancer des commandes de débogage ou utilitaires facilement.
    ```dockerfile
    FROM python:3.9
    WORKDIR /app
    COPY . .
    CMD ["python", "main.py"]
    # docker run myimage -> python main.py
    # docker run myimage python test.py -> python test.py
    # docker run myimage bash -> lance un shell bash
    ```

2.  **Utiliser `ENTRYPOINT` (forme `exec`) avec `CMD` (forme `exec`) :**
    *   **Le cas le plus courant pour créer des images "exécutables".**
    *   L'`ENTRYPOINT` est l'exécutable principal, et `CMD` fournit des options/arguments par défaut.
    ```dockerfile
    FROM alpine
    ENTRYPOINT ["ping"]
    CMD ["localhost"]
    # docker run myping -> ping localhost
    # docker run myping google.com -> ping google.com
    # docker run myping -c 5 google.com -> ping -c 5 google.com
    # docker run --entrypoint /bin/sh myping -> lance un shell
    ```

3.  **Utiliser `ENTRYPOINT` seul (forme `exec`) :**
    *   Si votre exécutable principal ne prend pas d'arguments par défaut ou si vous voulez que les arguments soient toujours spécifiés par l'utilisateur.
    ```dockerfile
    ENTRYPOINT ["/usr/local/bin/mon_service_qui_tourne_en_continu"]
    # docker run mon_service -> /usr/local/bin/mon_service_qui_tourne_en_continu
    # docker run mon_service --debug -> /usr/local/bin/mon_service_qui_tourne_en_continu --debug
    ```

Il est crucial de bien choisir entre `CMD` et `ENTRYPOINT` (ou leur combinaison) pour que votre image se comporte comme prévu et soit facile à utiliser. La combinaison `ENTRYPOINT ["executable"]` et `CMD ["arg_par_defaut1", "arg_par_defaut2"]` est très puissante et flexible.

---

La distinction et l'interaction entre `CMD` et `ENTRYPOINT` sont des concepts clés pour définir le comportement de vos conteneurs. Est-ce que cette explication vous aide à mieux comprendre leurs rôles respectifs et comment ils fonctionnent ensemble ?

Avez-vous des questions spécifiques sur des cas d'usage ou sur la manière dont les arguments sont passés ?

Après cela, nous pourrons aborder d'autres instructions utiles comme `USER`, `ARG`, `EXPOSE`, et `VOLUME`.