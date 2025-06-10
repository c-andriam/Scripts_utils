# Docker : Images vs Conteneurs – Le Plan et la Construction

Si nous reprenons notre analogie du château de LEGO :

*   Une **Image Docker** est comme le **plan de montage détaillé de votre château de LEGO** ET la **boîte d'origine contenant toutes les briques nécessaires non assemblées**.
*   Un **Conteneur Docker** est le **château de LEGO réellement construit et fonctionnel**, basé sur ce plan et utilisant ces briques.

Explorons cela plus en détail.

## 1. L'Image Docker : Le Plan Directeur et les Matériaux

Une **Image Docker** est un **modèle (template) en lecture seule** qui contient un ensemble d'instructions pour créer un conteneur. Elle inclut tout ce dont une application a besoin pour fonctionner :
*   Le **code de l'application** (par exemple, votre site web WordPress, votre application Python).
*   Les **dépendances** (librairies, outils, paquets logiciels dont votre code a besoin – par exemple, PHP pour WordPress, une version spécifique de Python).
*   Les **variables d'environnement** (des configurations spécifiques).
*   Des **informations sur la façon de lancer l'application** (par exemple, quelle commande exécuter au démarrage).

**Caractéristiques Clés d'une Image Docker :**

*   **Statique et Immuable :** Une fois qu'une image est créée, elle ne change plus. Si vous voulez faire une modification, vous devez créer une nouvelle version de l'image (comme une nouvelle édition d'un plan de construction).
*   **Superposable (Layers) :** Les images sont construites en couches (layers). Chaque instruction dans un `Dockerfile` (nous verrons le `Dockerfile` plus tard, c'est le fichier qui décrit comment construire une image) crée une nouvelle couche. Cela rend les images efficaces à stocker et à partager, car les couches communes peuvent être réutilisées.
    *   *Analogie :* Imaginez que votre plan de montage de LEGO est fait de plusieurs feuilles transparentes superposées. La première feuille pourrait être "les fondations", la suivante "les murs du rez-de-chaussée", puis "les murs du premier étage", etc. Si vous construisez un autre château qui a les mêmes fondations, vous pouvez réutiliser cette première feuille transparente.
*   **Portable :** Les images peuvent être stockées dans des registres (comme Docker Hub, qui est une sorte de bibliothèque géante d'images Docker) et partagées facilement. Vous pouvez télécharger une image depuis Docker Hub et l'utiliser pour créer un conteneur sur n'importe quelle machine où Docker est installé.

**Où trouve-t-on les Images ?**
*   **Docker Hub :** C'est le registre public par défaut pour les images Docker. Vous y trouverez des images officielles pour des milliers de logiciels (comme NGINX, WordPress, MariaDB, Python, Node.js, etc.).
*   **Registres Privés :** Les entreprises peuvent avoir leurs propres registres privés pour stocker leurs images internes.
*   **Construites Localement :** Vous pouvez créer vos propres images à partir d'un `Dockerfile`.

## 2. Le Conteneur Docker : L'Instance Vivante

Un **Conteneur Docker** est une **instance en cours d'exécution d'une image Docker**. C'est l'entité vivante, l'application qui tourne réellement.

*   Si l'image est le plan et les briques, le conteneur est le château de LEGO assemblé et exposé.
*   Vous pouvez créer **plusieurs conteneurs à partir de la même image**, tout comme vous pouvez construire plusieurs châteaux identiques à partir du même plan et de plusieurs boîtes de briques identiques. Chaque château (conteneur) sera indépendant des autres, même s'ils proviennent du même plan (image).

**Caractéristiques Clés d'un Conteneur Docker :**

*   **Exécutable :** C'est un processus (ou un groupe de processus) qui s'exécute sur votre machine hôte, mais de manière isolée.
*   **Isolé :** Chaque conteneur a son propre système de fichiers, son propre réseau (virtuel), et ses propres processus. Ce qui se passe à l'intérieur d'un conteneur n'affecte généralement pas les autres conteneurs ou la machine hôte (sauf si vous le configurez explicitement pour cela, par exemple avec les volumes ou le mapping de ports).
*   **Éphémère (par défaut) :** Si vous supprimez un conteneur, toutes les modifications que vous avez apportées à l'intérieur de ce conteneur (par exemple, si vous avez créé des fichiers directement dans le système de fichiers du conteneur sans utiliser de volumes) sont perdues, sauf si vous avez configuré des **volumes** pour persister les données. C'est comme si vous démontiez votre château de LEGO ; les briques retournent à la boîte, mais les arrangements spécifiques que vous aviez faits sont perdus.
*   **Possède un état :** Contrairement à une image (statique), un conteneur a un état (il peut être en cours d'exécution, arrêté, en pause).

**Comment ça marche ensemble ?**

1.  Vous **obtenez une image** (soit en la téléchargeant depuis Docker Hub, soit en la construisant vous-même avec un `Dockerfile`).
2.  Vous utilisez la commande `docker run <nom_de_l_image>` (ou des commandes similaires).
3.  Docker prend cette image, crée une couche accessible en écriture par-dessus les couches de l'image (pour que le conteneur puisse modifier des fichiers sans altérer l'image d'origine), et démarre l'application définie dans l'image. C'est votre **conteneur**.

## Résumé de la Différence : Image vs Conteneur

| Caractéristique      | Image Docker                                       | Conteneur Docker                                          |
| :------------------- | :------------------------------------------------- | :-------------------------------------------------------- |
| **État**             | Statique, en lecture seule, immuable               | Dynamique, en cours d'exécution (ou arrêté, etc.)      |
| **Analogie LEGO**    | Plan de montage + Boîte de briques non assemblées  | Château de LEGO construit et fonctionnel                  |
| **Nature**           | Modèle, Blueprint, Template                        | Instance, Réalisation du modèle, Processus                |
| **Création**         | À partir d'un `Dockerfile` ou téléchargée         | À partir d'une image (avec `docker run`)                  |
| **Cycle de vie**     | Construite une fois, peut avoir plusieurs versions | Créé, démarré, arrêté, supprimé                           |
| **Persistance**      | Persistante (stockée sur disque)                   | Modifications internes éphémères (sauf si volumes utilisés) |
| **Nombre**           | Une image peut servir de base à plusieurs conteneurs | Plusieurs conteneurs peuvent tourner depuis la même image |

---

Cette distinction entre image et conteneur est absolument cruciale pour comprendre Docker. Prenez le temps de bien l'assimiler.

Est-ce que cette explication vous aide à mieux distinguer les deux ? Avez-vous des questions sur cette partie avant que nous passions à la suite (par exemple, comment on construit une image avec un `Dockerfile`) ?

# Docker : Images vs Conteneurs – Le Moule et le Gâteau (Reformulation)

Imaginez que vous voulez faire des gâteaux.

## 1. L'Image Docker : La Recette et le Moule (et les ingrédients secs pré-mélangés)

Une **Image Docker** est comme :
*   La **recette détaillée** de votre gâteau : elle liste tous les ingrédients, les quantités, les étapes de préparation, la température du four, etc.
*   Le **moule** spécifique que vous allez utiliser pour donner sa forme au gâteau.
*   Parfois même, un sachet contenant **tous les ingrédients secs déjà pré-mélangés** selon la recette (comme un "mélange à gâteau" du commerce).

**Ce qu'est une Image Docker :**

*   **Un plan détaillé et complet :** Elle contient tout ce qu'il faut pour *potentiellement* créer et faire fonctionner votre application. Cela inclut le code de l'application, toutes les "dépendances" (comme la farine, le sucre, les œufs pour le gâteau – ce sont les librairies, les outils logiciels), et les instructions pour "cuire" (lancer) l'application.
*   **C'est figé, ça ne change pas :** Votre recette écrite sur papier ne change pas d'elle-même. Le moule reste le même. Le sachet d'ingrédients secs reste le même tant que vous ne l'ouvrez pas. Une image Docker, une fois créée, est **en lecture seule**. Si vous voulez changer la recette, vous écrivez une nouvelle recette (vous créez une nouvelle image).
*   **C'est un modèle, un "blueprint" :** Elle sert de base pour créer quelque chose de concret. Elle ne "tourne" pas, elle ne "fait" rien par elle-même, tout comme une recette sur une feuille de papier ne produit pas un gâteau toute seule.
*   **Elle est stockée :** Vous pouvez garder votre recette et votre moule précieusement. Les images Docker sont stockées sur votre ordinateur ou dans des "bibliothèques d'images" en ligne (comme Docker Hub).

**En résumé pour l'Image :** C'est le **"quoi faire"** et le **"avec quoi le faire"**, mais pas encore l'action de le faire. C'est le *potentiel*.

## 2. Le Conteneur Docker : Le Gâteau Cuit et Prêt à Manger

Un **Conteneur Docker** est comme :
*   Le **gâteau que vous avez réellement préparé et cuit** en suivant la recette et en utilisant le moule et les ingrédients.

**Ce qu'est un Conteneur Docker :**

*   **L'application en action :** C'est votre application qui fonctionne réellement, qui exécute des tâches. C'est le gâteau que vous pouvez voir, toucher, et même "manger" (interagir avec).
*   **C'est vivant et dynamique :** Le gâteau existe, il a une forme, il peut être chaud ou froid. Un conteneur est un processus qui s'exécute sur votre ordinateur. Il a un état (démarré, arrêté).
*   **Créé à partir d'une Image :** Vous ne pouvez pas avoir de gâteau sans recette ni moule. Vous ne pouvez pas avoir de conteneur sans image. Le conteneur est une **instance** d'une image.
*   **Isolé :** Si vous faites tomber un gâteau, cela n'affecte pas les autres gâteaux que vous pourriez faire avec la même recette et le même moule. Les conteneurs sont isolés les uns des autres et du système hôte. Ce qui se passe à l'intérieur d'un conteneur reste à l'intérieur (sauf configuration spécifique).
*   **Peut être multiple :** Avec la même recette et le même moule, vous pouvez faire plusieurs gâteaux identiques. De la même manière, à partir d'**une seule image, vous pouvez créer et exécuter plusieurs conteneurs identiques ou légèrement modifiés.** Chaque gâteau (conteneur) est distinct, même s'ils partagent la même origine (recette/image).
*   **Éphémère (par défaut) :** Si vous mangez le gâteau, il disparaît. Si vous jetez le gâteau, il disparaît. De même, si vous supprimez un conteneur, toutes les modifications que vous auriez pu faire *directement à l'intérieur* de ce gâteau (par exemple, ajouter un glaçage non prévu dans la recette initiale, après cuisson) sont perdues. Pour que des données persistent (comme si vous mettiez le gâteau dans une boîte pour le conserver), Docker utilise un mécanisme appelé **volumes**.

**En résumé pour le Conteneur :** C'est l'**"action de faire"** et le **"résultat concret"**. C'est l'application qui *tourne*.

## Comparaison Finale : Image vs Conteneur (avec l'analogie du gâteau)

| Caractéristique        | Image Docker (Recette + Moule)                     | Conteneur Docker (Gâteau Cuit)                       |
| :--------------------- | :------------------------------------------------- | :--------------------------------------------------- |
| **État**               | Statique, figée, en lecture seule                  | Dynamique, en cours d'exécution (ou arrêté)          |
| **Rôle**               | Modèle, plan, "comment faire"                      | Instance du modèle, "ce qui est fait et fonctionne" |
| **Base de création**   | `Dockerfile` (la recette écrite) ou téléchargement | Une Image Docker                                     |
| **Interaction**        | On la construit, la stocke, la partage             | On le démarre, l'arrête, le supprime, on interagit avec lui |
| **Nombre**             | Une image peut servir à faire plusieurs gâteaux    | Plusieurs gâteaux peuvent être faits depuis une image |
| **Persistance des modifs** | L'image ne change pas. Pour modifier, nouvelle image. | Les modifs internes au conteneur sont perdues à sa suppression (sauf si "boîte de conservation" / volume). |

---
