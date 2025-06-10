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

# Le Dockerfile : `RUN` et l'Exécution de Plusieurs Commandes avec la Forme `exec`

Vous avez posé une question très juste : peut-on utiliser `RUN` une seule fois avec la forme `exec` pour exécuter plusieurs commandes, comme on le fait avec `&&` dans la forme `shell` ?

## Rappel des Deux Formes de `RUN`

1.  **Forme `shell` :**
    ```dockerfile
    RUN commande1 && commande2 && commande3
    ```
    Ici, les commandes sont passées à un shell (par exemple `/bin/sh -c "commande1 && commande2 && commande3"`). Le shell se charge d'interpréter les `&&` et d'exécuter les commandes séquentiellement. Si l'une échoue, les suivantes ne sont pas exécutées et la construction de l'image échoue. Cela crée **une seule couche** pour toutes ces commandes.

2.  **Forme `exec` :**
    ```dockerfile
    RUN ["executable", "param1", "param2"]
    ```
    Ici, Docker exécute directement l'`executable` spécifié avec les `param1`, `param2`, etc., comme arguments. **Il n'y a pas d'interpréteur de shell par défaut qui traiterait les `&&`, `;`, `||`, etc.**

## Peut-on Faire Plusieurs Commandes Directement avec la Forme `exec` ?

**Non, pas directement.** La syntaxe `RUN ["executable1", "arg1", "executable2", "arg2"]` n'est pas valide ou ne ferait pas ce que vous attendez. La forme `exec` attend un seul exécutable suivi de ses propres arguments.

Par exemple, ceci **ne fonctionnerait pas** comme attendu pour exécuter deux commandes distinctes :
```dockerfile
# CECI NE FONCTIONNE PAS POUR LANCER DEUX COMMANDES SÉPARÉES
RUN ["apt-get", "update", "&&", "apt-get", "install", "-y", "curl"]
# OU
# RUN ["apt-get", "update", "apt-get", "install", "-y", "curl"] # Interprété comme des arguments pour le premier apt-get
```
Dans le premier cas, `&&` serait probablement passé comme un argument littéral à `apt-get update`. Dans le second, `apt-get`, `install`, etc., seraient tous passés comme des arguments au premier `apt-get update`.

## La Solution : Utiliser la Forme `exec` pour Appeler un Shell

Si vous voulez absolument utiliser la forme `exec` (par exemple, pour éviter certains problèmes d'interprétation de variables par le shell ou pour une raison de style), mais que vous avez besoin d'exécuter plusieurs commandes en une seule couche, la solution est d'**utiliser la forme `exec` pour explicitement appeler un shell, et de passer votre chaîne de commandes à ce shell.**

**Syntaxe :**
```dockerfile
RUN ["/bin/sh", "-c", "commande1 && commande2 && commande3"]
```
Ou avec un autre shell si nécessaire, comme `/bin/bash` :
```dockerfile
RUN ["/bin/bash", "-c", "commande1 && commande2 && commande3"]
```

**Explication :**
*   `"/bin/sh"` (ou `"/bin/bash"`) est l'exécutable que vous appelez.
*   `"-c"` est un argument pour le shell qui lui dit d'exécuter la chaîne de caractères qui suit comme une commande.
*   `"commande1 && commande2 && commande3"` est la chaîne de commandes, exactement comme vous l'écririez dans la forme `shell` de `RUN`.

**Exemple Concret :**
Pour reprendre notre exemple d'installation de `curl` et `git` :

*   **Forme `shell` (la plus courante et souvent la plus simple pour cela) :**
    ```dockerfile
    RUN apt-get update && \
        apt-get install -y curl git && \
        rm -rf /var/lib/apt/lists/*
    ```

*   **Équivalent avec la forme `exec` appelant un shell :**
    ```dockerfile
    RUN ["/bin/sh", "-c", "apt-get update && apt-get install -y curl git && rm -rf /var/lib/apt/lists/*"]
    ```

Les deux versions ci-dessus produiront **une seule couche d'image** et auront le même résultat final.

## Pourquoi choisir l'une ou l'autre ?

*   **Forme `shell` (`RUN commande1 && commande2`) :**
    *   **Avantages :** Plus concise et naturelle pour des enchaînements de commandes shell simples. Permet d'utiliser facilement les fonctionnalités du shell comme les variables d'environnement (ex: `RUN echo $MA_VARIABLE`), les wildcards, les pipes (`|`), les redirections (`>`, `>>`), etc., sans avoir à les gérer explicitement.
    *   **Inconvénients :** Peut parfois avoir des comportements inattendus avec l'expansion de variables ou les caractères spéciaux si on ne fait pas attention.

*   **Forme `exec` (`RUN ["executable", "arg1", "arg2"]`) :**
    *   **Avantages :** Plus explicite, pas d'interpréteur de commandes shell intermédiaire (sauf si vous en appelez un explicitement). Évite les problèmes d'interprétation des variables d'environnement par le shell (les variables ne sont pas substituées). C'est la forme préférée si vous voulez un contrôle total sur la manière dont la commande est exécutée ou si votre commande n'a pas besoin de fonctionnalités de shell.
    *   **Inconvénients :** Plus verbeuse pour des enchaînements de commandes simples. Si vous avez besoin de fonctionnalités de shell (comme `&&`), vous devez explicitement appeler un shell (`RUN ["/bin/sh", "-c", "..."]`), ce qui peut sembler un peu redondant par rapport à la forme `shell` directe.

**En résumé pour votre question :**
Pour exécuter plusieurs commandes en une seule instruction `RUN` (et donc une seule couche) :
1.  La **forme `shell`** est le moyen le plus direct : `RUN cmd1 && cmd2`.
2.  Si vous tenez à utiliser la **forme `exec`**, vous devez le faire en appelant un shell : `RUN ["/bin/sh", "-c", "cmd1 && cmd2"]`.

L'objectif principal de chaîner les commandes dans une seule instruction `RUN` (que ce soit avec la forme `shell` ou la forme `exec` appelant un shell) est de **minimiser le nombre de couches dans votre image Docker**, ce qui est une bonne pratique pour optimiser la taille de l'image et la vitesse de construction.

---

L'instruction `RUN` est l'un des piliers de la construction d'images. Elle vous donne le pouvoir de sculpter l'environnement de votre future application.

Est-ce que le fonctionnement et l'utilité de `RUN` sont clairs pour vous ? Avez-vous des questions sur la syntaxe ou les exemples ?

Une fois que vous êtes à l'aise avec `RUN`, nous pourrons voir comment copier des fichiers de votre machine locale vers l'image en construction avec les instructions `COPY` et `ADD`.
