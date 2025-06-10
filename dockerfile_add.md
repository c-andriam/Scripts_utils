# Le Dockerfile (Suite) : `ADD` – Une Alternative à `COPY` avec des Fonctionnalités Supplémentaires

L'instruction `ADD` est la grande sœur de `COPY`. Elle peut faire tout ce que `COPY` fait, mais elle a deux capacités additionnelles :

1.  **Copier depuis une URL :** `ADD` peut télécharger un fichier depuis une URL et le copier dans la destination de l'image.
2.  **Décompression automatique d'archives :** Si la `<source>` est une archive locale reconnue (comme `.tar`, `.tar.gz`, `.tar.bz2`, `.tar.xz`, `.tgz`, `.tbz`, `.txz`, `.zip`), `ADD` va automatiquement la décompresser dans le répertoire `<destination>`.

## d) `ADD` : Copier des Fichiers/Dossiers avec des Extras

*   **Objectif :** Similaire à `COPY`, mais avec la capacité de télécharger depuis des URLs et de décompresser des archives locales.
*   **Syntaxe :**
    `ADD [--chown=<user>:<group>] [--checksum=<checksum>] <source>... <destination>`
    `ADD [--chown=<user>:<group>] ["<source>",... "<destination>"]` (pour les chemins avec des espaces)

    *   `<source>` : Peut être un chemin local (fichier, répertoire, wildcard) comme pour `COPY`, OU une **URL**.
    *   `<destination>` : Le chemin à l'intérieur de l'image Docker, comme pour `COPY`.
    *   `--chown=<user>:<group>` : Fonctionne comme pour `COPY`.
    *   `--checksum=<checksum>` (optionnel, pour les sources URL) : Permet de spécifier un checksum SHA256 du fichier distant. Si le checksum du fichier téléchargé ne correspond pas, la construction échoue. C'est une mesure de sécurité et d'intégrité.

*   **Fonctionnement :**
    *   **Pour les sources locales :**
        *   Si la source est un fichier ou un répertoire simple, `ADD` se comporte **exactement comme `COPY`**.
        *   Si la source est une **archive locale reconnue** (par exemple, `myapp.tar.gz`), `ADD` va **extraire le contenu de l'archive** dans le répertoire de destination. Le répertoire de destination lui-même ne sera pas créé à partir du nom de l'archive, mais son contenu y sera placé.
    *   **Pour les sources URL :**
        *   `ADD` télécharge le fichier depuis l'URL et le copie dans la destination.
        *   **Important :** Les archives téléchargées depuis des URLs ne sont **PAS** décompressées automatiquement.
        *   Les permissions du fichier téléchargé sont généralement 0600 (lecture/écriture pour le propriétaire seulement), sauf si `--checksum` est utilisé et que le serveur distant fournit des informations de permissions.

*   **Chaque instruction `ADD` crée une nouvelle couche dans l'image.**

**Exemples Concrets :**

1.  **Copier un fichier local (se comporte comme `COPY`) :**
    ```dockerfile
    FROM alpine
    WORKDIR /app
    # Si app.jar est un fichier local, ADD se comporte comme COPY
    ADD app.jar .
    ```

2.  **Extraire une archive locale :**
    Supposons que vous ayez `myfiles.tar.gz` localement, contenant `file1.txt` et `dir1/file2.txt`.
    ```dockerfile
    FROM alpine
    WORKDIR /data
    # myfiles.tar.gz sera décompressé dans /data
    # Vous aurez /data/file1.txt et /data/dir1/file2.txt
    ADD myfiles.tar.gz .
    ```
    Si vous aviez utilisé `COPY myfiles.tar.gz .`, vous auriez eu le fichier `myfiles.tar.gz` lui-même dans `/data`, non décompressé.

3.  **Télécharger un fichier depuis une URL :**
    ```dockerfile
    FROM alpine
    # Télécharger le binaire de 'dumb-init' depuis GitHub et le placer dans /usr/local/bin/dumb-init
    ADD https://github.com/Yelp/dumb-init/releases/download/v1.2.5/dumb-init_1.2.5_x86_64 /usr/local/bin/dumb-init
    # Rendre le fichier exécutable (ADD ne le fait pas par défaut pour les URL)
    RUN chmod +x /usr/local/bin/dumb-init
    ```

4.  **Télécharger un fichier depuis une URL avec vérification du checksum :**
    ```dockerfile
    FROM alpine
    ADD --checksum=sha256:abcdef0123456789... https://example.com/somefile.txt /app/somefile.txt
    ```
    Si le checksum du fichier téléchargé ne correspond pas à `abcdef0123456789...`, la construction échouera.

## `COPY` vs `ADD` : Quand Utiliser Quoi ?

C'est une question fréquente. La **recommandation générale de Docker et de la communauté est de préférer `COPY`** dans la plupart des cas.

*   **Utilisez `COPY` si :**
    *   Vous copiez simplement des fichiers ou des répertoires **locaux** vers votre image. `COPY` est plus explicite et son comportement est plus simple et prévisible (il copie tel quel, sans magie).

*   **Utilisez `ADD` si (et seulement si) :**
    *   Vous avez spécifiquement besoin de la fonctionnalité de **décompression automatique des archives locales** (par exemple, copier un `.tar.gz` et l'extraire en une seule étape).
    *   Vous avez besoin de **télécharger des fichiers depuis des URLs** directement dans votre image.
        *   Cependant, même pour les URLs, il est souvent considéré comme une meilleure pratique d'utiliser `RUN` avec des outils comme `curl` ou `wget`, puis de décompresser ou de traiter le fichier si nécessaire. Cela vous donne plus de contrôle sur le processus (gestion des erreurs, nettoyage, etc.) et ne crée qu'une seule couche pour le téléchargement et le traitement.
        *   *Exemple alternatif pour télécharger et décompresser :*
            ```dockerfile
            FROM alpine
            RUN apk add --no-cache curl tar && \
                curl -L https://example.com/archive.tar.gz -o /tmp/archive.tar.gz && \
                tar -xzf /tmp/archive.tar.gz -C /usr/local/somedir && \
                rm /tmp/archive.tar.gz && \
                apk del curl tar
            ```
            Cet exemple est plus verbeux mais offre plus de contrôle et nettoie les outils après usage.

**Pourquoi préférer `COPY` pour les fichiers locaux ?**
Parce que `ADD` a un comportement "magique" (la décompression automatique) qui peut surprendre si vous ne vous y attendez pas. Si vous voulez copier une archive `.tar.gz` *en tant que fichier archive* (sans la décompresser), `COPY` le fera, tandis que `ADD` l'extraira. `COPY` rend vos intentions plus claires.

**En résumé :**
*   Pour les fichiers locaux : **`COPY` est votre ami.**
*   Pour décompresser une archive locale en la copiant : `ADD` peut être un raccourci pratique.
*   Pour les URLs : `ADD` fonctionne, mais `RUN curl/wget` est souvent plus flexible et transparent.

---

La distinction entre `COPY` et `ADD` et savoir quand utiliser l'une ou l'autre est une bonne pratique pour écrire des `Dockerfile` clairs et maintenables.

Est-ce que cette explication sur `ADD` et la comparaison avec `COPY` vous semble claire ? Avez-vous des scénarios en tête où vous vous demanderiez lequel utiliser ?

Après cela, nous pourrons aborder des instructions comme `WORKDIR` pour définir le répertoire de travail, `ENV` pour les variables d'environnement, et `CMD` ou `ENTRYPOINT` pour définir ce qui s'exécute quand un conteneur démarre.