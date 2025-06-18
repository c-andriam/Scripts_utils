# Docker Deep Dive

**Pourquoi devrais-je lire ce livre ou m'intéresser à Docker ?**

Docker est incontournable, il est inutile de l'ignorer. Les développeurs l'ont adopté en masse, et les équipes d'exploitation (IT Ops) se doivent d'être à la hauteur ! Nous avons tout intérêt à savoir comment créer et maintenir des applications Docker de qualité professionnelle dans nos environnements de production critiques. Ce livre vous y aidera.

**Docker, n'est-ce pas uniquement pour les développeurs ?**

Si vous pensez que Docker est seulement destiné aux développeurs, préparez-vous à voir votre monde complètement chamboulé !

Toutes les applications « dockerisées » que les développeurs produisent en série ont besoin d'un endroit pour s'exécuter et de quelqu'un pour les gérer. Si vous pensez que les développeurs vont s'en charger, vous rêvez. Les équipes d'exploitation (Ops) devront construire et faire tourner des infrastructures Docker performantes et hautement disponibles. Si votre travail est axé sur l'exploitation et que vous n'avez pas de compétences sur Docker, vous allez au-devant de grandes difficultés. Mais pas de panique, ce livre est là pour vous former !

**Dois-je acheter le livre si j'ai déjà suivi vos formations vidéo ?**

Si vous aimez mes formations vidéo, vous aimerez probablement ce livre. Si vous n'aimez pas mes formations vidéo, vous n'aimerez probablement pas ce livre.

**Comment le livre est-il structuré**

J'ai divisé le livre en deux sections :
*   La vue d'ensemble
*   La partie technique

La partie sur la vue d'ensemble aborde des sujets tels que : qui est Docker, Inc., qu'est-ce que le projet Docker (Moby), qu'est-ce que l'OCI (Open Container Initiative), pourquoi les conteneurs existent-ils... Ce n'est pas la partie la plus passionnante du livre, mais c'est le genre de connaissances nécessaires pour avoir une compréhension solide de Docker et des conteneurs. C'est une section assez courte, et sa lecture est recommandée.

La partie technique, c'est le cœur du livre ! C'est là que vous trouverez tout ce dont vous avez besoin pour commencer à travailler avec Docker. Elle entre dans le détail des images, des conteneurs et du sujet de plus en plus crucial de l'orchestration. Vous y trouverez la théorie pour comprendre comment tout s'articule, ainsi que des commandes et des exemples pour vous montrer comment tout fonctionne en pratique.

Chaque chapitre de la partie technique est divisé en trois sections :
*   Le résumé (TL;DR)
*   L'analyse approfondie
*   Les commandes

Le **résumé (TL;DR)** vous donnera deux ou trois paragraphes que vous pourrez utiliser pour expliquer le sujet à la machine à café.

L'**analyse approfondie** est la partie où nous expliquons le fonctionnement de chaque élément et passons en revue les exemples.

La section des **commandes** répertorie toutes les commandes pertinentes dans une liste facile à lire, avec de brefs rappels de leur fonction.

**Les versions du livre**

Docker évolue à une vitesse fulgurante ! Par conséquent, la valeur d'un livre comme celui-ci est inversement proportionnelle à son ancienneté. Autrement dit, plus ce livre est ancien, moins il a de valeur. C'est pourquoi je le maintiens à jour !

Si cela vous semble étrange... bienvenue dans la nouvelle normalité ! Nous ne vivons plus à une époque où un livre vieux de 5 ans a encore de la valeur (croyez-moi, ça me fait mal de le dire).

Rassurez-vous cependant, votre investissement dans ce livre est garanti !

Si vous achetez la version papier, vous obtenez la version Kindle pour une somme dérisoire ! Les versions Kindle et Leanpub sont toujours maintenues à jour. C'est la meilleure solution que nous puissions proposer actuellement !

Voici la liste des versions :

**Version 4.** Il s'agit de la quatrième version du livre, publiée le 3 octobre 2017. Cette version ajoute un nouveau chapitre intitulé « Conteneuriser une application ». Elle ajoute également du contenu sur les images multi-architectures et les identifiants cryptographiques (crypto ID's) au chapitre sur les Images, ainsi que des informations complémentaires au chapitre « La vue d'ensemble ».

**Version 3.** Ajout du chapitre « Le moteur Docker ».

**Version 2.** Ajout du chapitre « La sécurité dans Docker ».

# PREMIÈRE PARTIE : LA VUE D'ENSEMBLE

## 1 : Les conteneurs : une vue d'ensemble

Les conteneurs sont devenus incontournables.

Dans ce chapitre, nous aborderons des questions telles que : pourquoi les conteneurs existent-ils, à quoi nous servent-ils et où pouvons-nous les utiliser ?

### Le mauvais vieux temps

Les applications font tourner les entreprises. Si les applications tombent en panne, les entreprises en pâtissent et peuvent parfois disparaître. Ces affirmations sont de plus en plus vraies chaque jour !

La plupart des applications s'exécutent sur des serveurs. Et par le passé, nous ne pouvions exécuter qu'une seule application par serveur. Le monde des systèmes ouverts comme Windows et Linux ne disposait tout simplement pas des technologies nécessaires pour exécuter plusieurs applications de manière sûre et sécurisée sur un même serveur.

L'histoire se déroulait donc généralement comme suit... Chaque fois que l'entreprise avait besoin d'une nouvelle application, le service informatique devait acheter un nouveau serveur. Et la plupart du temps, personne ne connaissait les exigences de performance de la nouvelle application ! Cela signifiait que le service informatique devait faire des suppositions au moment de choisir le modèle et la taille des serveurs à acheter.

En conséquence, le service informatique faisait la seule chose possible : il achetait de gros serveurs rapides et très résilients. Après tout, la dernière chose que quiconque souhaitait – y compris l'entreprise – était d'avoir des serveurs sous-dimensionnés. Des serveurs sous-dimensionnés pourraient être incapables d'exécuter des transactions, ce qui pourrait entraîner des pertes de clients et de revenus. Le service informatique achetait donc généralement des serveurs plus gros que nécessaire. Il en résultait un nombre considérable de serveurs fonctionnant à seulement 5-10 % de leur capacité potentielle. Un gaspillage tragique de capital et de ressources pour l'entreprise !

### L'arrivée de VMware !

Au milieu de tout cela, VMware, Inc. a offert un cadeau au monde : la machine virtuelle (VM). Et presque du jour au lendemain, le monde est devenu bien meilleur ! Nous disposions enfin d'une technologie qui nous permettait d'exécuter de manière sûre et sécurisée plusieurs applications métier sur un seul serveur.

Cela a changé la donne ! Le service informatique n'avait plus besoin de se procurer un tout nouveau serveur surdimensionné chaque fois que l'entreprise demandait une nouvelle application. Le plus souvent, il pouvait exécuter de nouvelles applications sur des serveurs existants qui disposaient d'une capacité inutilisée.

Tout à coup, nous pouvions tirer une valeur considérable des actifs existants de l'entreprise, tels que les serveurs, ce qui assurait une bien meilleure rentabilité pour l'entreprise.

### Les inconvénients des VM

Mais... il y a toujours un mais ! Aussi géniales que soient les VM, elles ne sont pas parfaites !

Le fait que chaque VM nécessite son propre système d'exploitation (OS) dédié est un défaut majeur. Chaque OS consomme du CPU, de la RAM et du stockage qui pourraient autrement être utilisés pour alimenter davantage d'applications. Chaque OS doit être mis à jour et surveillé. Et dans certains cas, chaque OS nécessite une licence. Tout cela représente un gaspillage de dépenses d'exploitation (OpEx) et de capital (CapEx).

Le modèle des VM présente également d'autres défis. Les VM sont lentes à démarrer et leur portabilité n'est pas idéale : la migration et le déplacement des charges de travail des VM entre différents hyperviseurs et plateformes cloud sont plus compliqués qu'ils ne devraient l'être.

### L'avènement des conteneurs !

Depuis longtemps, les grands acteurs du web comme Google utilisent les technologies de conteneurs pour pallier les lacunes du modèle des VM.

Dans le modèle des conteneurs, le conteneur est à peu près analogue à la VM. La différence majeure, cependant, est que chaque conteneur ne nécessite pas un système d'exploitation complet. En fait, tous les conteneurs sur un même hôte partagent un seul et même OS. Cela libère d'énormes quantités de ressources système telles que le CPU, la RAM et le stockage. Cela réduit également les coûts de licence potentiels et diminue la charge de travail liée à l'application des correctifs de l'OS et à d'autres tâches de maintenance. Il en résulte des économies sur les fronts des dépenses de capital (CapEx) et d'exploitation (OpEx).

Les conteneurs sont également rapides à démarrer et ultra-portables. Déplacer des charges de travail conteneurisées de votre ordinateur portable vers le cloud, puis vers des VM ou des serveurs physiques (*bare metal*) dans votre datacenter est un jeu d'enfant.

### Les conteneurs Linux

Les conteneurs modernes ont vu le jour dans le monde Linux et sont le fruit d'un travail immense accompli par une grande variété de personnes sur une longue période. À titre d'exemple, Google Inc. a contribué à de nombreuses technologies liées aux conteneurs dans le noyau Linux. Sans ces contributions, et d'autres, nous n'aurions pas les conteneurs modernes que nous connaissons aujourd'hui.

Parmi les technologies majeures qui ont permis la croissance massive des conteneurs ces dernières années, on trouve les *namespaces* du noyau, les *control groups*, et bien sûr, Docker. Pour souligner à nouveau ce qui a été dit précédemment : l'écosystème moderne des conteneurs est profondément redevable aux nombreuses personnes et organisations qui ont posé les fondations solides sur lesquelles nous bâtissons actuellement !

Malgré tout cela, les conteneurs restaient complexes et hors de portée de la plupart des organisations. Ce n'est qu'à l'arrivée de Docker que les conteneurs ont été véritablement démocratisés et rendus accessibles au plus grand nombre.

> \* *Il existe de nombreuses technologies de virtualisation au niveau du système d'exploitation, similaires aux conteneurs, qui sont antérieures à Docker et aux conteneurs modernes. Certaines remontent même au System/360 sur Mainframe. Les BSD Jails et les Solaris Zones sont d'autres exemples bien connus de technologies de conteneurs de type Unix. Cependant, dans cette section, nous limitons notre discussion et nos commentaires aux conteneurs modernes popularisés par Docker.*

### L'arrivée de Docker !

Nous parlerons de Docker un peu plus en détail dans le prochain chapitre. Mais pour l'instant, il suffit de dire que Docker a été la touche de magie qui a rendu les conteneurs Linux utilisables par le commun des mortels. Autrement dit, Docker, Inc. a rendu les conteneurs simples !

### Les conteneurs Windows

Au cours des dernières années, Microsoft Corp. a travaillé d'arrache-pied pour porter les technologies Docker et de conteneurs sur la plateforme Windows.

Au moment de la rédaction, les conteneurs Windows sont disponibles sur les plateformes Windows 10 et Windows Server 2016. Pour y parvenir, Microsoft a travaillé en étroite collaboration avec Docker, Inc.

Les technologies de base de Windows nécessaires à la mise en œuvre des conteneurs sont collectivement appelées *Windows Containers*. L'outillage utilisateur pour travailler avec ces *Windows Containers* est Docker. Cela rend l'expérience Docker sur Windows presque identique à celle de Docker sur Linux. Ainsi, les développeurs et les administrateurs système familiers avec l'outillage Docker de la plateforme Linux se sentiront à l'aise avec les conteneurs Windows.

Cette version du livre inclut des exemples pour Linux et Windows pour la quasi-totalité des cas cités.

### Conteneurs Windows vs conteneurs Linux

Il est essentiel de comprendre qu'un conteneur en cours d'exécution utilise le noyau de la machine hôte sur laquelle il tourne. Cela signifie qu'un conteneur conçu pour s'exécuter sur un hôte avec un noyau Windows ne fonctionnera pas sur un hôte Linux. À un niveau général, on peut donc considérer que les conteneurs Windows nécessitent un hôte Windows, et les conteneurs Linux un hôte Linux. Cependant, ce n'est pas aussi simple…

Au moment de la rédaction de cette version du livre, il est possible d'exécuter des conteneurs Linux sur des machines Windows. Par exemple, Docker for Windows (une offre de Docker, Inc. conçue pour Windows 10) peut basculer entre les modes conteneurs Windows et conteneurs Linux. C'est un domaine qui évolue rapidement, et il est conseillé de consulter la documentation de Docker pour les dernières informations.

### Et les conteneurs sur Mac ?

Il n'existe actuellement pas de « conteneurs Mac ».

Cependant, vous pouvez exécuter des conteneurs Linux sur votre Mac en utilisant le produit Docker for Mac. Celui-ci fonctionne en exécutant de manière transparente vos conteneurs à l'intérieur d'une VM Linux légère qui tourne sur votre Mac. C'est extrêmement populaire auprès des développeurs, qui peuvent ainsi facilement développer et tester leurs conteneurs Linux sur leur Mac.

### Résumé du chapitre

Nous vivions dans un monde où chaque nouvelle application de l'entreprise nécessitait l'achat d'un serveur flambant neuf. Puis VMware est arrivé et a permis aux services informatiques de mieux valoriser les actifs informatiques, nouveaux comme existants. Mais aussi performants que soient VMware et le modèle des VM, ils ne sont pas parfaits. Après le succès de VMware et des hyperviseurs, une technologie de virtualisation plus récente, plus efficace et plus légère a fait son apparition : les conteneurs. Cependant, les conteneurs étaient initialement difficiles à mettre en œuvre et se trouvaient uniquement dans les datacenters des géants du web qui employaient des ingénieurs spécialisés dans le noyau Linux. C'est alors que Docker Inc. est entré en scène et que les technologies de virtualisation par conteneurs sont soudainement devenues accessibles au plus grand nombre.

En parlant de Docker... découvrons maintenant qui, quoi et pourquoi Docker !

## 2 : Docker

Aucun livre ou conversation sur les conteneurs n'est complet sans aborder le sujet de Docker. Mais lorsque quelqu'un dit « Docker », il peut faire référence à au moins trois choses différentes :

1.  Docker, Inc., l'entreprise
2.  Docker, la technologie de runtime et d'orchestration de conteneurs
3.  Docker, le projet open source (aujourd'hui appelé Moby)

Si vous voulez vous faire une place dans le monde des conteneurs, vous devrez en savoir un peu sur chacun des trois.

### Docker - Le résumé (TL;DR)

Docker est un logiciel qui fonctionne sur Linux et Windows. Il crée, gère et orchestre des conteneurs. Le logiciel est développé de manière ouverte dans le cadre du projet open source Moby sur GitHub. Docker, Inc. est une entreprise basée à San Francisco et est le principal mainteneur du projet open source. Docker, Inc. propose également des versions commerciales de Docker avec des contrats de support, etc.

Voilà pour la version rapide. Nous allons maintenant explorer chaque point plus en détail. Nous parlerons également un peu de l'écosystème des conteneurs et nous mentionnerons l'Open Container Initiative (OCI).

### Docker, Inc.

Docker, Inc. est la start-up technologique basée à San Francisco, fondée par le développeur et entrepreneur franco-américain Solomon Hykes.
![Figure 2.1 Logo de Docker, Inc.](https://github.com/user-attachments/assets/aead3e0d-80b1-4908-bb20-f0b5c481d522)<p align="center">Figure 2.1 Logo de Docker, Inc.</p>

Il est intéressant de noter que Docker, Inc. a commencé son existence en tant que fournisseur de plateforme en tant que service (PaaS) appelé dotCloud. En coulisses, la plateforme dotCloud s'appuyait sur des conteneurs Linux. Pour les aider à créer et à gérer ces conteneurs, ils ont développé un outil interne qu'ils ont surnommé « Docker ». Et c'est ainsi que Docker est né !

En 2013, l'activité PaaS de dotCloud était en difficulté et l'entreprise avait besoin d'un nouveau souffle. Pour y remédier, ils ont engagé Ben Golub comme nouveau PDG, ont rebaptisé l'entreprise « Docker, Inc. », se sont débarrassés de la plateforme PaaS dotCloud et ont entamé une nouvelle aventure avec pour mission d'apporter Docker et les conteneurs au monde entier.

Aujourd'hui, Docker, Inc. est largement reconnue comme une entreprise technologique innovante avec une valorisation boursière estimée à environ 1 milliard de dollars. Au moment de la rédaction, elle a levé plus de 180 millions de dollars à travers 7 tours de financement auprès de certains des plus grands noms du capital-risque de la Silicon Valley. La quasi-totalité de ce financement a été levée après que l'entreprise a pivoté pour devenir Docker, Inc.

Depuis qu'elle est devenue Docker, Inc., l'entreprise a réalisé plusieurs petites acquisitions, pour des montants non divulgués, afin de développer son portefeuille de produits et de services.

Au moment de la rédaction, Docker, Inc. compte entre 200 et 300 employés et organise une conférence annuelle appelée Dockercon. L'objectif de Dockercon est de rassembler l'écosystème croissant des conteneurs et de promouvoir l'adoption de Docker et des technologies de conteneurs.

Tout au long de ce livre, nous utiliserons le terme « Docker, Inc. » pour faire référence à l'entreprise Docker. Toutes les autres utilisations du terme « Docker » feront référence à la technologie ou au projet open source.

> **Note** : Le mot « Docker » vient d'un terme familier britannique signifiant *dock worker* - quelqu'un qui charge et décharge les navires.

### Le moteur de runtime et d'orchestration Docker

Lorsque la plupart des technologues parlent de Docker, ils font référence au **Moteur Docker** (Docker Engine).

Le Moteur Docker est le logiciel d'infrastructure de base qui exécute et orchestre les conteneurs. Si vous êtes un administrateur VMware, vous pouvez le considérer comme étant similaire à ESXi. De la même manière que ESXi est la technologie d'hyperviseur principale qui exécute les machines virtuelles, le Moteur Docker est le runtime de conteneur principal qui exécute les conteneurs.

Tous les autres produits de Docker, Inc. et des tiers se connectent au Moteur Docker et s'articulent autour de lui. La Figure 2.2 montre le Moteur Docker au centre. Tous les autres produits du diagramme s'appuient sur le Moteur et tirent parti de ses capacités fondamentales.![Figure 2.2](https://github.com/user-attachments/assets/a693a224-4b9f-41b3-ab29-e6787517b3ce)<p align="center">Figure 2.2</p>
Le Moteur Docker peut être téléchargé depuis le site web de Docker ou compilé à partir des sources sur GitHub. Il est disponible sur Linux et Windows, avec des offres open source et d'autres bénéficiant d'un support commercial.

Au moment de la rédaction, il existe deux éditions principales :

*   **Édition Entreprise (EE)**
*   **Édition Communautaire (CE)**

L'Édition Entreprise et l'Édition Communautaire ont toutes deux un canal de publication stable (*stable release channel*) avec des sorties trimestrielles. Chaque Édition Communautaire est supportée pendant 4 mois et chaque Édition Entreprise est supportée pendant 12 mois.

L'Édition Communautaire dispose d'un canal de publication mensuel supplémentaire, le canal *edge*.

À partir du premier trimestre 2017, les numéros de version de Docker suivent le schéma de versionnage `AA.MM-xx`, similaire à celui d'Ubuntu et d'autres projets. Par exemple, la première version de l'Édition Communautaire de juin 2017 est la `17.06.0-ce`.

> **Note :** Avant le premier trimestre 2017, les numéros de version de Docker suivaient le schéma de versionnage majeur.mineur (*major.minor*). La dernière version avant ce nouveau schéma était Docker 1.13.x.

### Le projet open source Docker (Moby)

Le terme « Docker » est également utilisé pour désigner le projet open source Docker. Il s'agit de l'ensemble des outils qui sont combinés pour créer des éléments tels que le démon (daemon) et le client Docker que vous pouvez télécharger et installer depuis docker.com. Cependant, le projet a été officiellement renommé **projet Moby** lors de la DockerCon 2017 à Austin, au Texas. Dans le cadre de ce changement de nom, le dépôt GitHub a été déplacé de `docker/docker` à `moby/moby` et le projet a reçu son propre logo.![Figure 2.3](https://github.com/user-attachments/assets/ae9cc2ea-319c-4e03-8c14-73de51c594bd)<p align="center">Figure 2.3</p>
L'objectif du projet Moby est de décomposer Docker en composants plus modulaires, et de le faire de manière ouverte. Il est hébergé sur GitHub et vous pouvez consulter la liste des sous-projets et outils actuels inclus dans le dépôt Moby à l'adresse https://github.com/moby. Le projet principal du Moteur Docker se trouve actuellement à l'adresse https://github.com/moby/moby, mais des parties sont continuellement extraites et modularisées.

En tant que projet open source, le code source est publiquement disponible et vous êtes libre de le télécharger, d'y contribuer, de le modifier et de l'utiliser, à condition de respecter les termes de la licence Apache 2.0.

Si vous prenez le temps de regarder l'historique des *commits* du projet, vous y verrez le gratin de la technologie d'infrastructure, notamment RedHat, Microsoft, IBM, Cisco et HPE. Vous y verrez également les noms de personnes non associées à de grandes entreprises.

La plupart du projet et de ses outils sont écrits en Golang – le langage de programmation système relativement nouveau de Google, également connu sous le nom de Go. Si vous programmez en Go, vous êtes en excellente position pour contribuer au projet !

Un effet secondaire appréciable du fait que Moby/Docker soit un projet open source est que la majeure partie de son développement et de sa conception se fait de manière ouverte. Cela met fin à de nombreuses anciennes pratiques où le code était propriétaire et gardé secret. Cela signifie également que les cycles de publication sont publiés et travaillés au grand jour. Fini les cycles de sortie incertains, tenus secrets puis pré-annoncés des mois à l'avance avec une pompe et une cérémonie ridicules. Le projet Moby/Docker ne fonctionne pas comme ça. La plupart des choses se font ouvertement, à la vue de tous et avec la possibilité pour tous de contribuer.

Le projet Moby, et plus largement le mouvement Docker, est immense et prend de l'ampleur. Il compte des milliers de *pull requests* sur GitHub, des dizaines de milliers de projets « dockerisés », sans parler des milliards de téléchargements d'images depuis Docker Hub. Le projet prend littéralement l'industrie d'assaut !

Ne vous y trompez pas, Docker est massivement utilisé !

### L'écosystème des conteneurs

L'une des philosophies fondamentales de Docker, Inc. est souvent résumée par l'expression « Piles incluses mais remplaçables » (*Batteries included but removable*).

C'est une façon de dire que vous pouvez remplacer de nombreux composants natifs de Docker par des composants de tiers. Un bon exemple est la pile réseau. Le produit de base Docker est livré avec une mise en réseau intégrée. Mais la pile réseau est *pluggable*, ce qui signifie que vous pouvez retirer la mise en réseau native de Docker et la remplacer par une autre solution d'un fournisseur tiers. De nombreuses personnes le font.

Au début, il était courant que les plugins tiers soient meilleurs que les offres natives fournies avec Docker. Cependant, cela a posé des défis de modèle économique pour Docker, Inc. Après tout, Docker, Inc. doit devenir rentable à un moment donné pour être une entreprise viable à long terme. En conséquence, les « piles » qui sont incluses deviennent de meilleures en meilleures. Cela crée des tensions et augmente le niveau de concurrence au sein de l'écosystème.

Pour faire court, les piles natives de Docker sont toujours remplaçables, il y a juste de moins en moins de raisons de vouloir les remplacer.

Malgré cela, l'écosystème des conteneurs est florissant, avec un équilibre sain entre coopération et concurrence. Vous entendrez souvent des gens utiliser des termes comme « coopétition » (un équilibre entre coopération et compétition) et « ami-ennemi » (*frenemy*) en parlant de l'écosystème des conteneurs. C'est une excellente chose ! Une saine concurrence est la mère de l'innovation !

### L'Open Container Initiative (OCI)

Aucune discussion sur Docker et l'écosystème des conteneurs n'est complète sans mentionner l'Open Containers Initiative (OCI).![Figure 2.4](https://github.com/user-attachments/assets/0f39734b-25c1-4e02-9029-765f3caaa3b7)<p align="center">Figure 2.4</p>
L'OCI est un conseil de gouvernance relativement nouveau, responsable de la normalisation des composants les plus fondamentaux de l'infrastructure des conteneurs, tels que le format d'image et le runtime de conteneur (ne vous inquiétez pas si ces termes sont nouveaux pour vous, nous les aborderons dans le livre).

Il est également vrai qu'aucune discussion sur l'OCI n'est complète sans mentionner un peu d'histoire. Et comme pour tout récit historique, la version que vous entendez dépend de qui la raconte. Voici donc la version de l'histoire selon Nigel :-D

Dès le premier jour, l'utilisation de Docker a connu une croissance folle. De plus en plus de gens l'ont utilisé, de plus en plus de manières, pour de plus en plus de choses. Il était donc inévitable que quelqu'un finisse par être frustré. C'est normal et sain.

Le résumé (TL;DR) de cette histoire, selon Nigel, est qu'une société nommée CoreOS n'appréciait pas la façon dont Docker faisait certaines choses. Ils ont donc agi ! Ils ont créé un nouveau standard ouvert appelé `appc` qui définissait des éléments comme le format d'image et le runtime de conteneur. Ils ont également créé une implémentation de cette spécification appelée `rkt` (prononcé « rocket »).

Cela a mis l'écosystème des conteneurs dans une position délicate avec deux standards concurrents.

Pour en revenir à l'histoire, cela menaçait de fracturer l'écosystème et de placer les utilisateurs et les clients face à un dilemme. Bien que la concurrence soit généralement une bonne chose, des standards concurrents ne le sont généralement pas. Ils sèment la confusion et ralentissent l'adoption par les utilisateurs. Ce n'est bon pour personne.

Dans cet esprit, tout le monde a fait de son mieux pour agir en adultes et s'est réuni pour former l'OCI - un conseil léger et agile pour gouverner les standards des conteneurs.

Au moment de la rédaction, l'OCI a publié deux spécifications (standards) :
*   La **spécification d'image** (*image-spec*)
*   La **spécification de runtime** (*runtime-spec*)

Une analogie souvent utilisée pour désigner ces deux standards est celle des voies ferrées. Ces deux standards reviennent à s'accorder sur des tailles et des propriétés standards pour les voies ferrées. Cela laisse tout le monde libre de construire de meilleurs trains, de meilleurs wagons, de meilleurs systèmes de signalisation, de meilleures gares... tout en ayant l'assurance qu'ils fonctionneront sur des voies normalisées. Personne ne veut de deux standards concurrents pour la taille des voies ferrées !

Il est juste de dire que les deux spécifications de l'OCI ont eu un impact majeur sur l'architecture et la conception du produit Docker de base. Depuis Docker 1.11, l'architecture du Moteur Docker est conforme à la spécification de runtime de l'OCI.

Jusqu'à présent, l'OCI a accompli de bonnes choses et a contribué à unifier l'écosystème. Cependant, les standards ralentissent toujours l'innovation ! Surtout avec les nouvelles technologies qui évoluent presque à la vitesse de la lumière. Cela a donné lieu à des ~~débats houleux~~ discussions passionnées au sein de la communauté des conteneurs. De l'avis de votre auteur, c'est une bonne chose ! L'industrie des conteneurs est en train de changer le monde et il est normal que les personnes à l'avant-garde soient passionnées et aient des opinions bien arrêtées. Attendez-vous à d'autres discussions passionnées sur les standards et l'innovation !

L'OCI est organisée sous les auspices de la Fondation Linux, et Docker, Inc. ainsi que CoreOS, Inc. en sont des contributeurs majeurs.

### Résumé du chapitre

Dans ce chapitre, nous en avons appris un peu plus sur Docker, Inc. C'est une start-up technologique de San Francisco qui a pour ambition de changer notre façon de concevoir les logiciels. On peut dire qu'ils ont été les pionniers et les instigateurs de la révolution des conteneurs. Mais un vaste écosystème de partenaires et de concurrents existe désormais.

Le projet Docker est open source et se trouve dans le dépôt `moby/moby` sur GitHub.

L'Open Container Initiative (OCI) a joué un rôle déterminant dans la normalisation du format de runtime de conteneur et du format d'image de conteneur.

## 3 : Installer Docker

Il existe une multitude de manières et d'endroits pour installer Docker. Il y a Windows, Mac, et bien sûr Linux. Mais il y a aussi le cloud, les installations sur site (*on-premises*), ou sur votre ordinateur portable. Sans parler des installations manuelles, par script, ou via des assistants. Il existe littéralement une pléthore de façons et de lieux pour installer Docker !

Mais ne vous laissez pas effrayer ! Elles sont toutes faciles.

Dans ce chapitre, nous couvrirons certaines des installations les plus importantes :

*   **Installations sur ordinateur de bureau**
    *   Docker pour Windows
    *   Docker pour Mac
*   **Installations sur serveur**
    *   Linux
    *   Windows Server 2016

### Docker pour Windows (DfW)

La première chose à noter est que Docker pour Windows est un produit packagé par Docker, Inc. Cela signifie qu'il dispose d'un installateur élégant qui met en place un environnement Docker à moteur unique sur un ordinateur de bureau ou portable Windows 10 64 bits.

La deuxième chose à noter est qu'il s'agit d'une application de l'Édition Communautaire (CE). Cela signifie qu'elle n'est pas destinée aux charges de travail de production.

La troisième chose à noter est qu'elle peut souffrir d'un certain retard au niveau des fonctionnalités. C'est parce que Docker, Inc. adopte une approche privilégiant la stabilité avant les fonctionnalités pour ce produit.

Ces trois points aboutissent à une installation rapide et facile, mais qui n'est pas destinée aux charges de travail de production.

Trêve de bavardage. Voyons comment installer Docker pour Windows.

Tout d'abord, les prérequis. Docker pour Windows nécessite :

*   Windows 10 Pro | Entreprise | Éducation (mise à jour de novembre 1511, build 10586 ou ultérieure)
*   Doit être en version 64 bits
*   Les fonctionnalités Hyper-V et Conteneurs doivent être activées dans Windows
*   Le support de la virtualisation matérielle doit être activé dans le BIOS de votre système

Nous supposerons que le support de la virtualisation matérielle est déjà activé dans le BIOS de votre système. Si ce n'est pas le cas, vous devez suivre attentivement la procédure correspondant à votre machine.

La première chose à faire sous Windows 10 est de s'assurer que les fonctionnalités Hyper-V et Conteneurs sont installées et activées.

1.  Faites un clic droit sur le bouton Démarrer de Windows et choisissez **Applications et fonctionnalités**.
2.  Cliquez sur le lien **Programmes et fonctionnalités**.
3.  Cliquez sur **Activer ou désactiver des fonctionnalités Windows**.
4.  Cochez les cases **Hyper-V** et **Conteneurs**, puis cliquez sur **OK**.

Cela installera et activera les fonctionnalités Hyper-V et Conteneurs. Votre système pourrait nécessiter un redémarrage.![Figure 3.1](https://github.com/user-attachments/assets/c2dd8541-de30-4872-acdb-8141aad5eb8d)<p align="center">Figure 3.1</p>
La fonctionnalité **Conteneurs** n'est disponible que si vous exécutez la mise à jour anniversaire de Windows 10 de l'été 2016 (build 14393) ou une version ultérieure.

Une fois que vous avez installé les fonctionnalités Hyper-V et Conteneurs et redémarré votre machine, il est temps d'installer Docker pour Windows.

1.  Rendez-vous sur [www.docker.com](https://www.docker.com) et cliquez sur **Get Docker** en haut de la page. Un menu déroulant s'ouvrira.
2.  Choisissez **Windows** dans la section *Desktop*.
3.  Cliquez sur **Download from Docker Store**.
4.  Cliquez sur l'un des liens de téléchargement **Get Docker**.

Plusieurs versions *stable* et *edge* sont disponibles. La version *edge* contient des fonctionnalités plus récentes mais peut être moins stable.

Un paquet d'installation nommé `InstallDocker.msi` sera téléchargé dans votre répertoire de téléchargements par défaut.

1.  Localisez et lancez le paquet `InstallDocker.msi` que vous venez de télécharger.

Suivez les étapes de l'assistant d'installation et fournissez les informations d'identification d'un administrateur local pour terminer l'installation. Docker démarrera automatiquement en tant que service système et une icône de baleine Moby Dock apparaîtra dans la zone de notification de Windows.

Félicitations ! Vous avez installé Docker pour Windows.

Maintenant que Docker pour Windows est installé, vous pouvez ouvrir une invite de commandes ou une fenêtre PowerShell et exécuter quelques commandes Docker. Essayez les commandes suivantes :

```shell
C:\> docker version
Client:
 Version: 17.05.0-ce
 API version: 1.29
 Go version: go1.7.5
 Git commit: 89658be
 Built: Thu May 4 21:43:09 2017
 OS/Arch: windows/amd64

Server:
 Version: 17.05.0-ce
 API version: 1.29 (minimum version 1.12)
 Go version: go1.7.5
 Git commit: 89658be
 Built: Thu May 4 21:43:09 2017
 OS/Arch: linux/amd64
 Experimental: false
```

Remarquez que la ligne `OS/Arch:` pour le composant *Server* affiche `linux/amd64` dans la sortie ci-dessus. C'est parce que l'installation par défaut installe actuellement le démon Docker à l'intérieur d'une machine virtuelle Hyper-V Linux légère. Dans ce scénario par défaut, vous ne pourrez exécuter que des conteneurs Linux sur votre installation de Docker pour Windows.

Si vous souhaitez exécuter des conteneurs Windows natifs, vous pouvez faire un clic droit sur l'icône de la baleine Docker dans la zone de notification de Windows et sélectionner l'option **Switch to Windows containers...** (Basculer vers les conteneurs Windows...). Vous pouvez obtenir le même résultat depuis la ligne de commande avec la commande suivante (située dans le répertoire `\Program Files\Docker\Docker`) :

```shell
C:\Program Files\Docker\Docker> dockercli -SwitchDaemon
```

Vous recevrez l'alerte suivante si vous n'avez pas activé la fonctionnalité Conteneurs de Windows.![Figure 3.2](https://github.com/user-attachments/assets/6af2c2e6-138d-4d1e-af66-7776104bb33d)<p align="center">Figure 3.2</p>
Si la fonctionnalité Conteneurs de Windows est déjà activée, le basculement ne prendra que quelques secondes. Une fois le changement effectué, la sortie de la commande `docker version` ressemblera à ceci.

```shell
C:\> docker version
Client:
 Version: 17.05.0-ce
 API version: 1.29
 Go version: go1.7.5
 Git commit: 89658be
 Built: Thu May 4 21:43:09 2017
 OS/Arch: windows/amd64

Server:
 Version: 17.05.0-ce
 API version: 1.29 (minimum version 1.12)
 Go version: go1.7.5
 Git commit: 89658be
 Built: Thu May 4 21:43:09 2017
 OS/Arch: windows/amd64
 Experimental: true
```

Remarquez que la version du *Server* affiche maintenant `windows/amd64`. Cela signifie que le démon s'exécute désormais nativement sur le noyau Windows et ne pourra donc exécuter que des conteneurs Windows.

Notez également que le système ci-dessus exécute maintenant la version expérimentale de Docker (`Experimental: true`). Comme mentionné précédemment, Docker pour Windows dispose d'un canal *stable* et d'un canal *edge*. L'exemple ci-dessus provient d'une machine Windows 10 utilisant le canal *edge*. La fonctionnalité de conteneurs Windows du canal *edge* est actuellement une fonctionnalité expérimentale.

Vous pouvez vérifier quel canal vous utilisez avec la commande `dockercli -Version`. La commande `dockercli` se trouve dans `C:\Program Files\Docker\Docker`.

```shell
C:\> dockercli -Version
Docker for Windows
Version: 17.05.0-ce-win11 (12053)
Channel: edge
Sha1: ffbc5f5871f44611dfb2bbf49e8312332531c112
OS Name: Windows 10 Pro
Windows Edition: Professional
Windows Build Number: 15063
```

Comme montré ci-dessous, les autres commandes Docker habituelles fonctionnent normalement.

```shell
C:\>docker info
Containers: 1
 Running: 0
 Paused: 0
 Stopped: 1
Images: 1
Server Version: 17.05.0-ce
Storage Driver: windowsfilter
 Windows:
Logging Driver: json-file
Plugins:
 Volume: local
 Network: l2bridge l2tunnel nat null overlay transparent
<SNIP>
Experimental: true
Insecure Registries:
 127.0.0.0/8
Live Restore Enabled: false
```

Docker pour Windows inclut le Moteur Docker (client et démon), Docker Compose, Docker Machine et la ligne de commande Docker Notary. Utilisez les commandes suivantes pour vérifier que chacun a été installé avec succès et pour connaître les versions que vous possédez :

```shell
C:\> docker --version
Docker version 1.12.1, build 23cf638, experimental

C:\> docker-compose --version
docker-compose version 1.13.0, build 1719ceb8

C:\> docker-machine --version
docker-machine version 0.11.0, build 5b27455

C:\> notary version
notary
  Version:    0.4.3
  Git commit: 9211198
```

### Docker pour Mac (DfM)

Docker pour Mac est également un produit packagé par Docker, Inc. Alors détendez-vous, vous n'avez pas besoin d'être un ingénieur spécialisé dans le noyau, et nous ne sommes pas sur le point de vous guider à travers une manipulation complexe pour installer Docker sur votre Mac. Nous allons vous accompagner dans le processus d'installation de Docker pour Mac sur votre ordinateur de bureau ou portable Mac, et c'est ridiculement facile.

#### Qu'est-ce que Docker pour Mac ?

Tout d'abord, Docker pour Mac est un produit packagé par Docker, Inc. qui est basé sur l'Édition Communautaire de Docker. Cela signifie que c'est un moyen facile d'installer une version à moteur unique de Docker sur votre Mac. Cela signifie également qu'il n'est pas destiné à un usage en production. Si vous avez entendu parler de *boot2docker*, alors Docker pour Mac est ce que vous avez toujours souhaité que *boot2docker* soit : il est fluide, simple et stable.

Il est également important de noter que Docker pour Mac ne vous donnera pas le Moteur Docker fonctionnant nativement sur le noyau Darwin de Mac OS. En coulisses, il exécute le démon Docker à l'intérieur d'une machine virtuelle Linux légère. Il expose ensuite de manière transparente le démon et l'API à votre environnement Mac. Mais il fait tout cela d'une manière qui cache la magie et le mystère qui lient le tout. Tout ce que vous devez savoir, c'est que vous pouvez ouvrir un terminal sur votre Mac et utiliser les commandes Docker habituelles pour interroger l'API Docker.

Bien que cela fonctionne de manière transparente sur votre Mac, il s'agit évidemment de Docker sur Linux sous le capot, donc cela ne fonctionnera qu'avec des conteneurs Docker basés sur Linux. C'est une bonne chose, car c'est là que se déroule la majeure partie de l'action des conteneurs.

La Figure 3.3 montre une représentation de haut niveau de l'architecture de Docker pour Mac.![Figure 3.3](https://github.com/user-attachments/assets/46a24e80-4c5f-4d9f-a980-90f923c576bd)<p align="center">Figure 3.3</p>
> **Note :** Pour le lecteur curieux, Docker pour Mac s'appuie sur **HyperKit** pour implémenter un hyperviseur extrêmement léger. HyperKit est basé sur l'hyperviseur xhyve. Docker pour Mac tire également parti des fonctionnalités de **DataKit** et exécute une distribution Linux hautement optimisée appelée **Moby**, qui est basée sur **Alpine Linux**.

Mettons en place Docker pour Mac.

1.  Pointez votre navigateur sur [www.docker.com](https://www.docker.com) et cliquez sur **Get Docker** en haut de la page. Un menu déroulant s'ouvrira.
2.  Choisissez **Mac** dans la section *Desktop*.
3.  Cliquez sur **Download from Docker Store**.
4.  Cliquez sur l'un des liens de téléchargement **Get Docker**.

Plusieurs versions *stable* et *edge* sont disponibles. La version *edge* contient des fonctionnalités plus récentes mais peut être moins stable.

Un paquet d'installation `Docker.dmg` sera téléchargé.

1.  Lancez le fichier `Docker.dmg` que vous avez téléchargé à l'étape précédente. Il vous sera demandé de glisser-déposer l'image de la baleine Moby Dock dans le dossier **Applications**.
2.  Ouvrez votre dossier **Applications** (il peut s'ouvrir automatiquement) et double-cliquez sur l'icône de l'application Docker pour la démarrer. Il se peut qu'on vous demande de confirmer l'action car l'application a été téléchargée depuis Internet.
3.  Entrez votre mot de passe pour que l'installateur puisse créer des composants, tels que la mise en !
réseau, qui nécessitent des privilèges élevés.
4.  Le démon Docker va maintenant démarrer.

Une icône de baleine animée apparaîtra dans la barre d'état en haut de votre écran pendant le démarrage du démon. Une fois que Docker a démarré avec succès, la baleine cessera d'être animée. Vous pouvez cliquer sur l'icône de la baleine et effectuer des actions de base telles que redémarrer le démon, rechercher des mises à jour et ouvrir l'interface utilisateur.

Maintenant que Docker pour Mac est installé, vous pouvez ouvrir une fenêtre de terminal et exécuter des commandes Docker habituelles. Essayez les commandes ci-dessous.

```shell
$ docker version
Client:
 Version: 17.05.0-ce
 API version: 1.29
 Go version: go1.7.5
 Git commit: 89658be
 Built: Thu May 4 21:43:09 2017
 OS/Arch: darwin/amd64

Server:
 Version: 17.05.0-ce
 API version: 1.29 (minimum version 1.12)
 Go version: go1.7.5
 Git commit: 89658be
 Built: Thu May 4 21:43:09 2017
 OS/Arch: linux/amd64
 Experimental: true
```

Remarquez dans la sortie ci-dessus que la ligne `OS/Arch:` pour le composant *Server* affiche `linux/amd64`. C'est parce que la partie serveur du Moteur Docker (alias le « démon ») s'exécute à l'intérieur de la VM Linux que nous avons mentionnée précédemment. Le composant *Client* est une application Mac native et s'exécute directement sur le noyau Darwin de Mac OS (`OS/Arch: darwin/amd64`).

Notez également que le système exécute la version expérimentale (`Experimental: true`) de Docker. C'est parce que le système utilise la version *edge* qui est livrée avec les fonctionnalités expérimentales activées.

Exécutez quelques autres commandes Docker.

```shell
$ docker info
Containers: 0
 Running: 0
 Paused: 0
 Stopped: 0
Images: 0
Server Version: 17.05.0-ce
<Snip>
Registry: https://index.docker.io/v1/
Experimental: true
Insecure Registries:
 127.0.0.0/8
Live Restore Enabled: false
```

Docker pour Mac installe le Moteur Docker (client et démon), Docker Compose, Docker Machine et la ligne de commande Notary. Les commandes suivantes vous montrent comment vérifier que tous ces composants se sont installés avec succès et découvrir quelles versions vous avez.

```shell
$ docker --version
Docker version 17.05.0-ce, build 89658be

$ docker-compose --version
docker-compose version 1.13.0, build 1719ceb

$ docker-machine --version
docker-machine version 0.11.0, build 5b27455

$ notary version
notary
  Version:    0.4.3
  Git commit: 9211198
```

### Installer Docker sur Linux

Installer Docker sur Linux est le type d'installation le plus courant et c'est étonnamment facile. La difficulté la plus fréquente réside dans les légères variations entre les distributions Linux, comme Ubuntu par rapport à CentOS. L'exemple que nous utiliserons dans cette section est basé sur Ubuntu Linux, mais devrait fonctionner sur les forks en amont et en aval. Il devrait également fonctionner sur CentOS et ses forks. Peu importe que votre machine Linux soit un serveur physique dans votre propre centre de données, à l'autre bout de la planète dans un cloud public, ou une VM sur votre ordinateur portable. Les seules exigences sont que la machine exécute Linux et ait accès à `https://get.docker.com`.

La première chose à décider avant d'installer Docker sur Linux est l'édition à installer. Il existe actuellement deux éditions :

*   **Édition Communautaire (CE)**
*   **Édition Entreprise (EE)**

Docker CE est gratuit et c'est la version que nous allons démontrer ici. Docker EE est identique à CE mais est fourni avec un support commercial et un accès à d'autres produits Docker tels que Docker Trusted Registry et Universal Control Plane.

Dans les exemples ci-dessous, nous utiliserons la commande `wget` pour appeler un script shell qui installe Docker CE. Pour plus d'informations sur d'autres manières d'installer Docker sur Linux, rendez-vous sur [https://www.docker.com](https://www.docker.com) et cliquez sur **Get Docker**.

> **Note :** Vous devez vous assurer que votre système est à jour avec les derniers paquets et correctifs de sécurité avant de continuer.

1.  Ouvrez un nouveau shell sur votre machine Linux.
2.  Utilisez `wget` pour récupérer et exécuter le script d'installation de Docker depuis `https://get.docker.com` et le transmettre à votre shell via un pipe.

```shell
$ wget -qO- https://get.docker.com/ | sh
modprobe: FATAL: Module aufs not found /lib/modules/4.4.0-36-generic
+ sh -c 'sleep 3; yum -y -q install docker-engine'
<Snip>
If you would like to use Docker as a non-root user, you should
now consider adding your user to the "docker" group with
something like:

    sudo usermod -aG docker your-user

Remember that you will have to log out and back in...
```

1.  Il est recommandé de n'utiliser que des utilisateurs non-root lorsque vous travaillez avec Docker. Pour ce faire, vous devez ajouter vos utilisateurs non-root au groupe Unix local `docker` sur votre machine Linux. Les commandes ci-dessous montrent comment ajouter l'utilisateur `npoulton` au groupe `docker` et vérifier que l'opération a réussi. Vous devrez utiliser un compte utilisateur valide sur votre propre système.

```shell
$ sudo usermod -aG docker npoulton
$
$ cat /etc/group | grep docker
docker:x:999:npoulton
```

Si vous êtes déjà connecté en tant que l'utilisateur que vous venez d'ajouter au groupe `docker`, vous devrez vous déconnecter et vous reconnecter pour que l'appartenance au groupe prenne effet.

Félicitations ! Docker est maintenant installé sur votre machine Linux. Exécutez les commandes suivantes pour vérifier votre installation.

```shell
$ docker --version
Docker version 17.05.0-ce, build 89658be
$
$ docker info
Containers: 0
 Running: 0
 Paused: 0
 Stopped: 0
Images: 0
<Snip>
Kernel Version: 4.4.0-1013-aws
Operating System: Ubuntu 16.04.2 LTS
OSType: linux
Architecture: x86_64
CPUs: 1
Total Memory: 990.7MiB
Name: ip-172-31-45-57
ID: L3GX:LLFI:YABL:WVUS:DHLL:2ZQU:44E3:V6BB:LWUY:WIGX:Z6RJ:JBVL
Docker Root Dir: /var/lib/docker
Debug Mode (client): false
Debug Mode (server): false
Registry: https://index.docker.io/v1/
Experimental: false
Insecure Registries:
 127.0.0.0/8
Live Restore Enabled: false
```

Si le processus décrit ci-dessus ne fonctionne pas pour votre distribution Linux, vous pouvez vous rendre sur le site de la documentation Docker et cliquer sur le lien correspondant à votre distribution. Cela vous mènera aux instructions d'installation officielles de Docker qui sont généralement tenues à jour. Attention cependant, les instructions sur le site de Docker ont tendance à utiliser le gestionnaire de paquets et nécessitent beaucoup plus d'étapes que la procédure que nous avons utilisée ci-dessus. En fait, si vous ouvrez un navigateur web à l'adresse `https://get.docker.com`, vous verrez que c'est un script shell qui fait tout le travail difficile de l'installation pour vous.

> **Avertissement :** Si vous installez Docker à partir d'une source autre que les dépôts officiels de Docker, vous pourriez vous retrouver avec une version dérivée (*fork*) de Docker. C'est parce que certains fournisseurs et distributions choisissent de dériver le projet Docker et de développer leurs propres versions légèrement personnalisées. Vous devez être conscient de ce genre de choses si vous installez à partir de dépôts personnalisés, car vous pourriez vous retrouver involontairement dans une situation où vous exécutez un fork qui a divergé du projet Docker officiel. Ce n'est pas un problème si c'est ce que vous avez l'intention de faire. Si ce n'est pas votre intention, cela peut conduire à des situations où les modifications et les correctifs apportés par votre fournisseur ne remontent pas en amont dans le projet Docker officiel. Dans ces situations, vous ne pourrez pas obtenir de support commercial pour votre installation de la part de Docker, Inc. ou de ses partenaires de service autorisés.

### Installer Docker sur Windows Server 2016

Dans cette section, nous examinerons l'une des manières d'installer Docker sur Windows Server 2016. Nous suivrons les étapes générales suivantes :

1.  Installer la fonctionnalité Conteneurs de Windows
2.  Installer Docker
3.  Vérifier l'installation

Avant de continuer, vous devez vous assurer que votre système est à jour avec les dernières versions de paquets et mises à jour de sécurité. Vous pouvez le faire rapidement avec la commande `sconfig` et en choisissant l'option 6 pour installer les mises à jour. Cela peut nécessiter un redémarrage du système.

Nous allons démontrer une installation sur une version de Windows Server 2016 qui n'a pas déjà la fonctionnalité Conteneurs ou une ancienne version de Docker d'installée.

Assurez-vous que la fonctionnalité **Conteneurs** est installée et activée.

1.  Faites un clic droit sur le bouton Démarrer de Windows et sélectionnez **Programmes et fonctionnalités**. Cela ouvrira l'application **Gestionnaire de serveur**.
2.  Sélectionnez le **Tableau de bord** et cliquez sur **Ajouter des rôles et fonctionnalités**.
3.  Cliquez à travers l'assistant jusqu'à la page **Fonctionnalités**.
4.  Assurez-vous que la fonctionnalité **Conteneurs** est cochée et terminez l'assistant. Cela peut nécessiter un redémarrage du système.

Maintenant que la fonctionnalité Conteneurs de Windows est installée, vous pouvez installer Docker. Nous utiliserons PowerShell pour cela.

1.  Ouvrez un nouveau terminal PowerShell en tant qu'administrateur.
2.  Utilisez la commande suivante pour installer le fournisseur de gestion de paquets Docker-Microsoft.

```powershell
> Install-Module -Name DockerMsftProvider -Repository PSGallery -Force
```

Acceptez l'installation du fournisseur NuGet si vous y êtes invité.

3.  Installez Docker.

```powershell
> Install-Package -Name docker -ProviderName DockerMsftProvider
```

Sélectionnez `A` pour confirmer l'installation du paquet et supprimer toute autre invite.

Une fois l'installation terminée, vous obtiendrez un résumé comme indiqué ci-dessous.

```
Name           Version      Source         Summary
----           -------      ------         -------
Docker         17.03.1-ee   DockerDefault  Contains Docker EE...
```

4.  Redémarrez votre système.

Docker est maintenant installé et vous pouvez commencer à déployer des conteneurs. Les deux commandes suivantes sont de bons moyens de vérifier que l'installation a réussi.

```powershell
> docker version
Client:
 Version:      17.03.1-ee-3
 API version:  1.27
 Go version:   go1.7.5
 Git commit:   3fcee33
 Built:        Thu Mar 30 19:31:22 2017
 OS/Arch:      windows/amd64

Server:
 Version:      17.03.1-ee-3
 API version:  1.27 (minimum version 1.24)
 Go version:   go1.7.5
 Git commit:   3fcee33
 Built:        Thu Mar 30 19:31:22 2017
 OS/Arch:      windows/amd64
 Experimental: false

> docker info
Containers: 0
 Running: 0
 Paused: 0
 Stopped: 0
Images: 0
Server Version: 17.03.1-ee-3
Storage Driver: windowsfilter
 Windows:
Logging Driver: json-file
Plugins:
 Volume: local
 Network: l2bridge l2tunnel nat null overlay transparent
<SNIP>
Insecure Registries:
 127.0.0.0/8
Live Restore Enabled: false
```

Docker est maintenant installé et vous êtes prêt à commencer à utiliser des conteneurs Windows.

### Résumé du chapitre

Dans ce chapitre, vous avez vu comment installer Docker sur Windows 10, Mac OS X, Linux et Windows Server 2016. Maintenant que vous savez comment installer Docker, vous êtes prêt à commencer à travailler avec les images et les conteneurs.

## 4 : Vue d'ensemble

L'idée de ce chapitre est de vous donner une vue d'ensemble rapide de ce qu'est Docker avant de plonger plus en profondeur dans les chapitres suivants.

Nous diviserons ce chapitre en deux parties :

*   **La perspective Ops (Opérations)**
*   **La perspective Dev (Développement)**

La section **Perspective Ops** téléchargera une image, démarrera un nouveau conteneur, se connectera à ce nouveau conteneur, y exécutera une commande, puis le détruira.

La section **Perspective Dev** récupérera du code d'application depuis GitHub, inspectera un Dockerfile, conteneurisera l'application, puis l'exécutera en tant que conteneur.

Ces deux sections vous donneront une bonne idée de ce qu'est Docker et de la manière dont certains des principaux composants s'articulent. Il est recommandé de lire les deux sections pour obtenir les perspectives dev et ops !

Ne vous inquiétez pas si certaines des choses que nous faisons ici sont totalement nouvelles pour vous. Nous n'essayons pas de faire de vous un expert à la fin de ce chapitre. Il s'agit de vous donner une idée générale des choses - vous préparer pour que, lorsque nous entrerons dans les détails dans les chapitres suivants, vous ayez une idée de la façon dont les pièces s'assemblent.

Tout ce dont vous avez besoin pour suivre les exercices de ce chapitre est un seul hôte Docker avec une connexion Internet. Votre hôte Docker peut être Linux ou Windows, et peu importe qu'il s'agisse d'une VM sur votre ordinateur portable, d'une instance dans le cloud public ou d'un serveur physique (*bare metal*) dans votre centre de données. Tout ce dont il a besoin, c'est d'exécuter Docker avec une connexion à Internet. Nous montrerons des exemples utilisant Linux et Windows !

### La perspective Ops

Lorsque vous installez Docker, vous obtenez deux composants majeurs :

*   le **client** Docker
*   le **démon** Docker (parfois appelé « serveur » ou « moteur »)

Le démon implémente l'API distante de Docker.

Dans une installation Linux par défaut, le client communique avec le démon via un socket IPC/Unix local à l'adresse `/var/run/docker.sock`. Sous Windows, cela se fait via un *named pipe* à l'adresse `npipe:////./pipe/docker_engine`. Vous pouvez tester que le client et le démon fonctionnent et peuvent communiquer entre eux avec la commande `docker version`.

```shell
$ docker version
Client:
 Version:      17.05.0-ce
 API version:  1.29
 Go version:   go1.7.5
 Git commit:   89658be
 Built:        Thu May 4 22:10:54 2017
 OS/Arch:      linux/amd64

Server:
 Version:      17.05.0-ce
 API version:  1.29 (minimum version 1.12)
 Go version:   go1.7.5
 Git commit:   89658be
 Built:        Thu May 4 22:10:54 2017
 OS/Arch:      linux/amd64
 Experimental: false
```

Si vous obtenez une réponse des composants *Client* et *Server*, vous devriez être prêt. Si vous utilisez Linux et obtenez une réponse d'erreur du composant *Server*, essayez à nouveau la commande en la préfixant avec `sudo` : `sudo docker version`. Si cela fonctionne avec `sudo`, vous devrez ajouter votre compte utilisateur au groupe local `docker`, ou préfixer le reste des commandes de ce chapitre avec `sudo`.

#### Images

Une bonne façon de se représenter une image Docker est de la voir comme un objet qui contient un système de fichiers d'un système d'exploitation et une application. Si vous travaillez dans les opérations, c'est comme un modèle de machine virtuelle. Un modèle de machine virtuelle est essentiellement une machine virtuelle arrêtée. Dans le monde de Docker, une image est en fait un conteneur arrêté. Si vous êtes un développeur, vous pouvez penser à une image comme à une **classe**.

Exécutez la commande `docker image ls` sur votre hôte Docker.

```shell
$ docker image ls
REPOSITORY          TAG                 IMAGE ID            CREATED             SIZE
```

Si vous travaillez à partir d'un hôte Docker fraîchement installé, il n'aura aucune image et la sortie ressemblera à celle ci-dessus.

Le processus d'obtention d'images sur votre hôte Docker s'appelle « pull ». Si vous suivez avec Linux, faites un *pull* de `ubuntu:latest`. Si vous suivez avec Windows, faites un *pull* de `microsoft/powershell:nanoserver`.

```shell
$ docker image pull ubuntu:latest
latest: Pulling from library/ubuntu
b6f892c0043b: Pull complete
55010f332b04: Pull complete
2955fb827c94: Pull complete
3deef3fcbd30: Pull complete
cf9722e506aa: Pull complete
Digest: sha256:382452f82a8b....463c62a9848133ecb1aa8
Status: Downloaded newer image for ubuntu:latest
```

Exécutez à nouveau la commande `docker image ls` pour voir l'image que vous venez de télécharger.

```shell
$ docker images
REPOSITORY          TAG                 IMAGE ID            CREATED             SIZE
ubuntu              latest              bd3d4369aebc        11 days ago         126.6 MB
```

Nous entrerons dans les détails de l'endroit où l'image est stockée et de ce qu'elle contient dans les chapitres suivants. Pour l'instant, il suffit de comprendre qu'une image contient une portion suffisante d'un système d'exploitation (OS), ainsi que tout le code et les dépendances nécessaires pour exécuter l'application pour laquelle elle est conçue. L'image `ubuntu` que nous avons téléchargée contient une version allégée du système de fichiers Ubuntu Linux, y compris quelques-uns des utilitaires Ubuntu courants. L'image `microsoft/powershell` téléchargée dans l'exemple Windows contient un OS Windows Nano Server avec PowerShell.

Si vous téléchargez un conteneur d'application tel que `nginx` ou `microsoft/iis`, vous obtiendrez une image qui contient un certain OS ainsi que le code pour exécuter soit nginx, soit IIS.

Il convient également de noter que chaque image reçoit son propre ID unique. Lorsque vous travaillez avec les images, vous pouvez vous y référer en utilisant soit les ID, soit les noms.

#### Conteneurs

Maintenant que nous avons une image téléchargée localement sur notre hôte Docker, nous pouvons utiliser la commande `docker container run` pour lancer un conteneur à partir de celle-ci.

**Pour Linux :**

```shell
$ docker container run -it ubuntu:latest /bin/bash
root@6dc20d508db0:/#
```

**Pour Windows :**

```shell
> docker container run -it microsoft/powershell:nanoserver PowerShell.exe
Windows PowerShell
Copyright (C) 2016 Microsoft Corporation. All rights reserved.
PS C:\>
```

Regardez attentivement la sortie des commandes ci-dessus. Vous devriez remarquer que l'invite de commande a changé dans chaque cas. C'est parce que votre shell est maintenant attaché au shell du nouveau conteneur - vous êtes littéralement à l'intérieur du nouveau conteneur !

Examinons cette commande `docker container run`. `docker container run` indique au démon Docker de démarrer un nouveau conteneur. Les options `-it` indiquent au démon de rendre le conteneur interactif et d'attacher notre terminal actuel au shell du conteneur (nous serons plus spécifiques à ce sujet dans le chapitre sur les conteneurs). Ensuite, la commande indique à Docker que nous voulons que le conteneur soit basé sur l'image `ubuntu:latest` (ou l'image `microsoft/powershell:nanoserver` si vous suivez avec Windows). Enfin, nous indiquons à Docker quel processus nous voulons exécuter à l'intérieur du conteneur. Pour l'exemple Linux, nous exécutons un shell Bash, pour le conteneur Windows, nous exécutons PowerShell.

Exécutez une commande `ps` depuis l'intérieur du conteneur pour lister tous les processus en cours.

**Exemple Linux :**

```shell
root@6dc20d508db0:/# ps -elf
F S UID   PID  PPID  C PRI  NI ADDR SZ WCHAN  STIME TTY          TIME CMD
4 S root      1     0  0  80   0 -  4560 wait   13:38 ?        00:00:00 /bin/bash
0 R root      9     1  0  80   0 -  8606 -      13:38 ?        00:00:00 ps -elf
```

**Exemple Windows :**

```powershell
PS C:\> ps
Handles  NPM(K)    PM(K)      WS(K)     CPU(s)     Id  SI ProcessName
-------  ------    -----      -----     ------     --  -- -----------
      0       5      964       1292       0.00   4716   4 CExecSvc
      0       5      592        956       0.00   4524   4 csrss
      0       0        0          4       0.00      0   0 Idle
      0      18     3984       8624       0.13    700   4 lsass
      0      52    26624      19400       1.64   2100   4 powershell
      0      38    28324      49616       1.69   4464   4 powershell
      0       8     1488       3032       0.06   2488   4 services
      0       2      288        504       0.00   4508   0 smss
      0       8     1600       3004       0.03    908   4 svchost
      0      12     1492       3504       0.06   4572   4 svchost
      0      15    20284      23428       5.64   4628   4 svchost
      0      15     3704       7536       0.09   4688   4 svchost
      0      28     5708       6588       0.45   4712   4 svchost
      0      10     2028       4736       0.03   4840   4 svchost
      0      11     5364       4824       0.08   4928   4 svchost
      0       0      128        136      37.02      4   0 System
      0       7      920       1832       0.02   3752   4 wininit
      0       8     5472      11124       0.77   5568   4 WmiPrvSE
```

À l'intérieur du conteneur Linux, seuls deux processus sont en cours d'exécution :
*   **PID 1**. C'est le processus `/bin/bash` que nous avons demandé au conteneur d'exécuter avec la commande `docker container run`.
*   **PID 9**. C'est la commande/processus `ps -elf` que nous avons exécutée pour lister les processus en cours.

La présence du processus `ps -elf` dans la sortie Linux ci-dessus pourrait être un peu trompeuse car c'est un processus de courte durée qui meurt dès que la commande `ps` se termine. Cela signifie que le seul processus de longue durée à l'intérieur du conteneur est le processus `/bin/bash`.

Le conteneur Windows a beaucoup plus de processus internes en cours. C'est un artefact de la façon dont le système d'exploitation Windows fonctionne. Bien que le conteneur Windows ait beaucoup plus de processus que le conteneur Linux, il en a toujours beaucoup moins qu'un serveur Windows classique.

Appuyez sur `Ctrl+P` `Ctrl+Q` pour quitter le conteneur sans le terminer. Cela vous ramènera au shell de votre hôte Docker. Vous pouvez le vérifier en regardant votre invite de commande.

Maintenant que vous êtes de retour à l'invite de commande de votre hôte Docker, exécutez à nouveau la commande `ps`.

**Exemple Linux :**

```shell
$ ps -elf
F S UID   PID  PPID  C PRI  NI ADDR SZ WCHAN  TIME     CMD
4 S root      1     0  0  80   0 -  9407 -      00:00:03 /sbin/init
1 S root      2     0  0  80   0 -     0 -      00:00:00 [kthreadd]
1 S root      3     2  0  80   0 -     0 -      00:00:00 [ksoftirqd/0]
1 S root      5     2 -20  80   0 -     0 -      00:00:00 [kworker/0:0H]
1 S root      7     2  0  80   0 -     0 -      00:00:00 [rcu_sched]
<Snip>
0 R ubuntu 22783 22475  0  80   0 -  9021 -      00:00:00 ps -elf
```

**Exemple Windows :**

```powershell
> ps
Handles  NPM(K)    PM(K)      WS(K)     CPU(s)     Id  SI ProcessName
-------  ------    -----      -----     ------     --  -- -----------
    220      11     7396       7872       0.33   1732   0 amazon-ssm-agen
     84       5      908       2096       0.00   2428   3 CExecSvc
     87       5      936       1336       0.00   4716   4 CExecSvc
    203      13     3600      13132       2.53   3192   2 conhost
    210      13     3768      22948       0.08   5260   2 conhost
    257      11     1808        992       0.64    524   0 csrss
    116       8     1348        580       0.08    592   1 csrss
     85       5      532       1136       0.23   2440   3 csrss
    242      11     1848        952       0.42   2708   2 csrss
     95       5      592        980       0.00   4524   4 csrss
    137       9     7784       6776       0.05   5080   2 docker
    401      17    22744      14016      28.59   1748   0 dockerd
    307      18    13344       1628       0.17    936   1 dwm
<SNIP>
   1888       0      128        136      37.17      4   0 System
    272      15     3372       2452       0.23   3340   2 TabTip
     72       7     1184          8       0.00   3400   2 TabTip32
    244      16     2676       3148       0.06   1880   2 taskhostw
    142       7     6172       6680       0.78   4952   3 WmiPrvSE
    148       8     5620      11028       0.77   5568   4 WmiPrvSE
```

Remarquez combien de processus supplémentaires s'exécutent sur votre hôte Docker par rapport aux conteneurs que nous avons lancés.

Dans une étape précédente, vous avez appuyé sur `Ctrl+P` `Ctrl+Q` pour quitter le conteneur. Faire cela depuis l'intérieur d'un conteneur vous en fera sortir sans le tuer. Vous pouvez voir tous les conteneurs en cours d'exécution sur votre système en utilisant la commande `docker container ls`.

```shell
$ docker container ls
CONTAINER ID   IMAGE           COMMAND       CREATED         STATUS         NAMES
e2b69eeb55cb   ubuntu:latest   "/bin/bash"   7 minutes ago   Up 7 minutes   vigilant_borg
```

La sortie ci-dessus montre un seul conteneur en cours d'exécution. C'est le conteneur que vous avez créé plus tôt. La présence de votre conteneur dans cette sortie prouve qu'il est toujours en cours d'exécution. Vous pouvez également voir qu'il a été créé il y a 7 minutes et qu'il est en cours d'exécution depuis 7 minutes.

#### S'attacher aux conteneurs en cours d'exécution

Vous pouvez attacher votre shell aux conteneurs en cours d'exécution avec la commande `docker container exec`. Comme le conteneur des étapes précédentes est toujours en cours d'exécution, reconnectons-nous à lui.

**Exemple Linux :**
Cet exemple fait référence à un conteneur nommé « vigilant_borg ». Le nom de votre conteneur sera différent, alors n'oubliez pas de remplacer « vigilant_borg » par le nom ou l'ID du conteneur en cours d'exécution sur votre hôte Docker.

```shell
$ docker container exec -it vigilant_borg bash
root@e2b69eeb55cb:/#
```

**Exemple Windows :**
Cet exemple fait référence à un conteneur nommé « pensive_hamilton ». Le nom de votre conteneur sera différent, alors n'oubliez pas de remplacer « pensive_hamilton » par le nom ou l'ID du conteneur en cours d'exécution sur votre hôte Docker.

```powershell
> docker container exec -it pensive_hamilton PowerShell.exe
Windows PowerShell
Copyright (C) 2016 Microsoft Corporation. All rights reserved.
PS C:\>
```

Remarquez que votre invite de commande a de nouveau changé. Vous êtes de retour à l'intérieur du conteneur.

Le format de la commande `docker container exec` est : `docker container exec -options <nom-conteneur ou id-conteneur> <commande>`. Dans notre exemple, nous avons utilisé les options `-it` pour attacher notre shell au shell du conteneur. Nous avons référencé le conteneur par son nom et lui avons dit d'exécuter le shell `bash` (PowerShell dans l'exemple Windows). Nous aurions facilement pu référencer le conteneur par son ID.

Quittez à nouveau le conteneur en appuyant sur `Ctrl+P` `Ctrl+Q`.

Votre invite de commande devrait être revenue à celle de votre hôte Docker.

Exécutez à nouveau la commande `docker container ls` pour vérifier que votre conteneur est toujours en cours d'exécution.

```shell
$ docker container ls
CONTAINER ID   IMAGE           COMMAND       CREATED         STATUS         NAMES
e2b69eeb55cb   ubuntu:latest   "/bin/bash"   9 minutes ago   Up 9 minutes   vigilant_borg
```

Arrêtez et supprimez le conteneur en utilisant les commandes `docker container stop` et `docker container rm`. N'oubliez pas de remplacer par les noms/ID de vos propres conteneurs.

```shell
$ docker container stop vigilant_borg
vigilant_borg
$
$ docker container rm vigilant_borg
vigilant_borg
```

Vérifiez que le conteneur a été supprimé avec succès en exécutant une autre commande `docker container ls`.

```shell
$ docker container ls
CONTAINER ID   IMAGE           COMMAND         CREATED         STATUS          PORTS           NAMES
```

### La perspective Dev

Les conteneurs, c'est avant tout les applications !

Dans cette section, nous allons cloner une application depuis un dépôt Git, inspecter son Dockerfile, la conteneuriser et l'exécuter en tant que conteneur.

L'application Linux peut être trouvée ici : https://github.com/nigelpoulton/psweb.git
L'application Windows peut être trouvée ici : https://github.com/nigelpoulton/dotnet-docker-samples.git

Le reste de cette section vous guidera à travers l'exemple Linux. Cependant, les deux exemples conteneurisent des applications web simples, donc le processus est le même. Là où il y a des différences dans l'exemple Windows, nous les soulignerons pour vous aider à suivre.

Exécutez toutes les commandes suivantes depuis un terminal sur votre hôte Docker.

Clonez le dépôt localement. Cela téléchargera le code de l'application sur votre hôte Docker local, prêt à être conteneurisé.

Assurez-vous de remplacer le dépôt ci-dessous par le dépôt Windows si vous suivez l'exemple Windows.

```shell
$ git clone https://github.com/nigelpoulton/psweb.git
Cloning into 'psweb'...
remote: Counting objects: 15, done.
remote: Compressing objects: 100% (11/11), done.
remote: Total 15 (delta 2), reused 15 (delta 2), pack-reused 0
Unpacking objects: 100% (15/15), done.
Checking connectivity... done.
```

Changez de répertoire pour vous placer dans le répertoire du dépôt cloné et listez son contenu.

```shell
$ cd psweb
$ ls -l
total 28
-rw-rw-r-- 1 ubuntu ubuntu  341 Sep 29 12:15 app.js
-rw-rw-r-- 1 ubuntu ubuntu  216 Sep 29 12:15 circle.yml
-rw-rw-r-- 1 ubuntu ubuntu  338 Sep 29 12:15 Dockerfile
-rw-rw-r-- 1 ubuntu ubuntu  421 Sep 29 12:15 package.json
-rw-rw-r-- 1 ubuntu ubuntu  370 Sep 29 12:15 README.md
drwxrwxr-x 2 ubuntu ubuntu 4096 Sep 29 12:15 test
drwxrwxr-x 2 ubuntu ubuntu 4096 Sep 29 12:15 views
```

Pour l'exemple Windows, vous devriez vous déplacer dans le répertoire `dotnet-docker-samples\aspnetapp`.

L'exemple Linux est une simple application web Node.js. L'exemple Windows est une simple application web ASP.NET Core.

Les deux dépôts Git contiennent un fichier nommé `Dockerfile`. Un Dockerfile est un document en texte brut décrivant comment construire une image Docker.

Listez le contenu du Dockerfile.

```shell
$ cat Dockerfile
FROM alpine
LABEL maintainer="nigelpoulton@hotmail.com"
RUN apk add --update nodejs nodejs-npm
COPY . /src
WORKDIR /src
RUN npm install
EXPOSE 8080
ENTRYPOINT ["node", "./app.js"]
```

Le contenu du Dockerfile dans l'exemple Windows est différent. Cependant, ce n'est pas important à ce stade. Nous couvrirons les Dockerfiles plus en détail plus tard dans le livre. Pour l'instant, il suffit de comprendre que chaque ligne représente une instruction utilisée pour construire une image.

À ce stade, nous avons récupéré du code d'application depuis un dépôt Git distant. Nous avons également un Dockerfile contenant des instructions qui décrivent comment créer une nouvelle image Docker avec l'application à l'intérieur.

Utilisez la commande `docker image build` pour créer une nouvelle image en utilisant les instructions contenues dans le Dockerfile. Cet exemple crée une nouvelle image Docker appelée `test:latest`.

Assurez-vous d'exécuter cette commande depuis le répertoire contenant le code de l'application et le Dockerfile.

```shell
$ docker image build -t test:latest .
Sending build context to Docker daemon  74.75kB
Step 1/8 : FROM alpine
latest: Pulling from library/alpine
88286f41530e: Pull complete
Digest: sha256:f006ecbb824...0c103f4820a417d
Status: Downloaded newer image for alpine:latest
 ---> 76da55c8019d
<Snip>
Successfully built f154cb3ddbd4
Successfully tagged test:latest
```

> **Note :** La construction peut prendre beaucoup de temps pour l'exemple Windows. Cela est dû à la taille et à la complexité des couches en cours de téléchargement.

Vérifiez que la nouvelle image `test:latest` existe sur votre hôte.

```shell
$ docker image ls
REPOSITORY   TAG      IMAGE ID         CREATED          SIZE
test         latest   f154cb3ddbd4     1 minute ago     55.6MB
...
```

Vous avez maintenant une image fraîchement construite avec l'application à l'intérieur.

Exécutez un conteneur à partir de l'image et testez l'application.

**Exemple Linux :**

```shell
$ docker container run -d \
--name web1 \
-p 8080:8080 \
test:latest
```

Ouvrez un navigateur web et accédez au nom DNS ou à l'adresse IP de l'hôte sur lequel vous exécutez le conteneur, en le pointant sur le port 8080. Vous verrez la page web suivante.![Figure 4.1](https://github.com/user-attachments/assets/45205c1d-4052-4753-9873-1130ef83f4e0)<p align="center">Figure 4.1</p>
**Exemple Windows :**

```powershell
> docker container run -d \
--name web1 \
-p 8000:80 \
test:latest
```

Ouvrez un navigateur web et accédez au nom DNS ou à l'adresse IP de l'hôte sur lequel vous exécutez le conteneur, en le pointant sur le port 8000. Vous verrez la page web suivante.![Figure 4.2](https://github.com/user-attachments/assets/a6da662b-99bf-4502-8a91-50fd339de421)<p align="center">Figure 4.2</p>
Bien joué. Vous avez pris une application et l'avez conteneurisée (construit une image Docker à partir de celle-ci).

### Résumé du chapitre

Dans ce chapitre, vous avez effectué les tâches suivantes liées aux opérations : téléchargé une image Docker, lancé un conteneur à partir de l'image, exécuté une commande à l'intérieur du conteneur (`ps`), puis arrêté et supprimé le conteneur. Vous avez également conteneurisé une application simple en récupérant du code source depuis GitHub et en le construisant en une image à l'aide des instructions d'un Dockerfile.

Cette vue d'ensemble devrait vous aider pour les chapitres à venir où nous approfondirons les images et les conteneurs.

## PARTIE 2 : LES ASPECTS TECHNIQUES

### 5 : Le Moteur Docker

Dans ce chapitre, nous allons jeter un coup d'œil rapide sous le capot du Moteur Docker (*Docker Engine*).

Vous pouvez utiliser Docker sans comprendre quoi que ce soit de ce que nous allons aborder dans ce chapitre. N'hésitez donc pas à le sauter. Cependant, pour vraiment maîtriser un sujet, il faut comprendre ce qui se passe en coulisses.

Ce sera un chapitre théorique, sans exercices pratiques.

Comme ce chapitre fait partie de la section technique du livre, nous allons employer une approche à trois niveaux, en divisant le chapitre en trois sections :

*   **Le TL;DR :** Deux ou trois paragraphes rapides que vous pouvez lire en faisant la queue pour un café.
*   **L'analyse approfondie :** La partie vraiment longue où nous entrons dans les détails.
*   **Les commandes :** Un récapitulatif rapide des commandes que nous avons apprises.

Allons découvrir le Moteur Docker !

#### Moteur Docker - Le TL;DR

Le Moteur Docker est le logiciel principal qui exécute et gère les conteneurs. Nous l'appelons souvent simplement **Docker**, ou la **plateforme Docker**. Si vous connaissez un peu VMware, il peut être utile de le considérer comme l'équivalent d'ESXi dans le monde VMware.

Le Moteur Docker a une conception modulaire avec de nombreux composants interchangeables. Dans la mesure du possible, ceux-ci sont basés sur des standards ouverts définis par l'Open Container Initiative (OCI).

À bien des égards, le Moteur Docker est comme un moteur de voiture - tous deux sont modulaires et créés en connectant de nombreuses petites pièces spécialisées :
- Un moteur de voiture est composé de nombreuses pièces spécialisées qui fonctionnent ensemble pour faire avancer une voiture - collecteurs d'admission, corps de papillon, cylindres, bougies d'allumage, collecteurs d'échappement, etc.
- Le Moteur Docker est composé de nombreux outils spécialisés qui fonctionnent ensemble pour créer et exécuter des conteneurs - images, API, pilote d'exécution, runtime, shims, etc.

Au moment de la rédaction, les principaux composants qui constituent le Moteur Docker sont : le **client Docker**, le **démon Docker**, **containerd** et **runc**. Ensemble, ils créent et exécutent des conteneurs.

La Figure 5.1 montre une vue de haut niveau.![Figure 5.1](https://github.com/user-attachments/assets/7210e8b1-1cc1-4c34-9be6-6ac98d02a4eb)<p align="center">Figure 5.1</p>
### Moteur Docker - L'analyse approfondie

Lorsque Docker a été lancé pour la première fois, le Moteur Docker comportait deux composants majeurs :

*   Le **démon Docker** (ci-après dénommé simplement « le démon »)
*   **LXC**

Le démon Docker était un binaire monolithique. Il contenait tout le code pour le client Docker, l'API Docker, le runtime de conteneur, la construction d'images, et bien plus encore.

Le composant LXC fournissait au démon un accès aux briques de base fondamentales des conteneurs, telles que les espaces de noms du noyau (*kernel namespaces*) et les groupes de contrôle (*cgroups*).

L'interaction entre le démon, LXC et le système d'exploitation est illustrée à la Figure 5.2.![Figure 5.2](https://github.com/user-attachments/assets/acebbdba-fc84-4f34-a7ec-6c5801a66cd9)<p align="center">Figure 5.2</p>
#### Se débarrasser de LXC

La dépendance à LXC a été un problème dès le début.

Premièrement, LXC est spécifique à Linux. C'était un problème pour un projet qui aspirait à être multi-plateforme.

Deuxièmement, dépendre d'un outil externe pour quelque chose d'aussi central au projet représentait un risque énorme qui pouvait entraver le développement.

En conséquence, Docker, Inc. a développé son propre outil appelé **libcontainer** en remplacement de LXC. L'objectif de `libcontainer` était d'être un outil agnostique de la plateforme qui fournirait à Docker un accès aux briques de construction fondamentales des conteneurs qui existent à l'intérieur du système d'exploitation.

`libcontainer` a remplacé LXC comme pilote d'exécution par défaut dans Docker 0.9.

#### Se débarrasser du démon Docker monolithique

Avec le temps, la nature monolithique du démon Docker est devenue de plus en plus problématique :

1.  Il est difficile d'innover dessus.
2.  Il est devenu plus lent.
3.  Ce n'était pas ce que l'écosystème (ou Docker, Inc.) voulait.

Docker, Inc. était conscient de ces défis et a entrepris un effort considérable pour décomposer le démon monolithique et le modulariser. Le but de ce travail est d'extraire autant de fonctionnalités que possible du démon et de les réimplémenter dans des outils plus petits et spécialisés. Ces outils spécialisés peuvent être interchangés, et également être facilement utilisés par des tiers pour construire d'autres outils. Ce plan suit la philosophie éprouvée d'Unix qui consiste à construire de petits outils spécialisés qui peuvent être assemblés pour former des outils plus grands.

Ce travail de décomposition et de refactorisation du Moteur Docker est un processus continu. Cependant, il a déjà vu tout le code d'exécution des conteneurs et du runtime de conteneur être entièrement retiré du démon et refactorisé en petits outils spécialisés.

La Figure 5.3 montre une vue de haut niveau de l'architecture du Moteur Docker avec de brèves descriptions.![Figure 5.3](https://github.com/user-attachments/assets/a27d49ac-e60d-4163-84d7-96925a7895f1)<p align="center">Figure 5.3</p>
#### L'influence de l'Open Container Initiative (OCI)

Pendant que Docker, Inc. décomposait le démon et refactorisait le code, l'OCI était en train de définir deux standards liés aux conteneurs :

1.  La spécification d'Image (*Image spec*)
2.  La spécification de Runtime de Conteneur (*Container runtime spec*)

Les deux spécifications ont été publiées en version 1.0 en juillet 2017.

Docker, Inc. a été fortement impliqué dans la création de ces spécifications et y a contribué beaucoup de code.

À partir de Docker 1.11 (début 2016), le Moteur Docker implémente les spécifications OCI aussi fidèlement que possible. Par exemple, le démon Docker ne contient plus aucun code de runtime de conteneur - tout le code de runtime de conteneur est implémenté dans une couche distincte conforme à l'OCI. Par défaut, Docker utilise un outil appelé **runc** pour cela. `runc` est l'implémentation de référence de la spécification de runtime de conteneur de l'OCI, et l'un des objectifs du projet `runc` est de le maintenir en parfaite adéquation avec la spécification OCI.

En plus de cela, le composant **containerd** du Moteur Docker s'assure que les images Docker sont présentées à `runc` sous forme de *bundles* OCI valides.

> **Note :** Le Moteur Docker a implémenté des parties des spécifications OCI avant que celles-ci ne soient officiellement publiées en version 1.0.

#### runc

Comme mentionné précédemment, `runc` est l'implémentation de référence de la spécification de runtime de conteneur de l'OCI. Docker, Inc. a été fortement impliqué dans la définition de la spécification et le développement de `runc`.

`runc` est petit. C'est en fait un outil en ligne de commande (CLI) léger qui encapsule `libcontainer`. Il n'a qu'un seul but dans la vie : créer des conteneurs. Et il est sacrément doué pour ça. Et rapide !

Nous nous référons souvent à `runc` comme un *runtime de conteneur*.

Vous pouvez voir les informations de version de `runc` à l'adresse :
[https://github.com/opencontainers/runc/releases](https://github.com/opencontainers/runc/releases)

#### containerd

Afin d'utiliser `runc`, le Moteur Docker avait besoin de quelque chose pour servir de pont entre le démon et `runc`. C'est là que **containerd** entre en scène.

`containerd` implémente la logique d'exécution qui a été extraite du démon Docker. Cette logique a évidemment été refactorisée et optimisée lorsqu'elle a été réécrite sous le nom de `containerd`.

Il est utile de considérer `containerd` comme un superviseur de conteneurs - le composant responsable des opérations du cycle de vie des conteneurs telles que : démarrer et arrêter les conteneurs, les mettre en pause et les reprendre, et les détruire.

Comme `runc`, `containerd` est petit, léger et conçu pour une seule tâche dans la vie - `containerd` ne s'intéresse qu'aux opérations du cycle de vie des conteneurs.

`containerd` a été développé par Docker, Inc. et donné à la Cloud Native Computing Foundation (CNCF).

Vous pouvez voir les informations de version de `containerd` à l'adresse :
[https://github.com/containerd/containerd/releases](https://github.com/containerd/containerd/releases)

#### Démarrer un nouveau conteneur (exemple)

Maintenant que nous avons une vue d'ensemble, et un peu d'histoire, parcourons le processus de création d'un nouveau conteneur.

La manière la plus courante de démarrer des conteneurs est d'utiliser le CLI Docker. La commande `docker container run` suivante démarrera un nouveau conteneur simple basé sur l'image `alpine:latest`.

```shell
$ docker container run --name ctr1 -it alpine:latest sh
```

Lorsque vous tapez des commandes comme celle-ci dans le CLI Docker, le client Docker les convertit en la charge utile API appropriée et les envoie (POST) au bon point de terminaison de l'API.

L'API est implémentée dans le démon. C'est la même API REST riche, versionnée, qui est devenue une marque de fabrique de Docker et qui est acceptée dans l'industrie comme l'API de conteneur de facto.

Une fois que le démon reçoit la commande de créer un nouveau conteneur, il fait un appel à `containerd`. Rappelez-vous que le démon ne contient plus aucun code pour créer des conteneurs !

Le démon communique avec `containerd` via une API de type CRUD sur gRPC.

Malgré son nom, `containerd` ne peut pas réellement créer de conteneurs. Il utilise `runc` pour le faire. Il convertit l'image Docker requise en un *bundle* OCI et dit à `runc` de l'utiliser pour créer un nouveau conteneur.

`runc` s'interface avec le noyau du système d'exploitation pour rassembler toutes les constructions nécessaires à la création d'un conteneur (sous Linux, cela inclut les *namespaces* et les *cgroups*). Le processus du conteneur est démarré en tant que processus enfant de `runc`, et dès qu'il est démarré, `runc` se termine.

Voilà ! Le conteneur est maintenant démarré.

La Figure 5.4 résume le processus.![Figure 5.4](https://github.com/user-attachments/assets/7cabf02a-afc0-4ab8-8de5-2a54729d8c31)<p align="center">Figure 5.4</p>
#### Un énorme avantage de ce modèle

Le fait d'avoir toute la logique et le code pour démarrer et gérer les conteneurs retirés du démon signifie que l'ensemble du runtime de conteneur est découplé du démon Docker. Nous appelons parfois cela les « conteneurs sans démon » (*daemonless containers*), et cela permet d'effectuer la maintenance et les mises à jour sur le démon Docker sans impacter les conteneurs en cours d'exécution !

Dans l'ancien modèle, où toute la logique du runtime de conteneur était implémentée dans le démon, démarrer et arrêter le démon tuait tous les conteneurs en cours d'exécution sur l'hôte. C'était un énorme problème dans les environnements de production - surtout si l'on considère la fréquence à laquelle de nouvelles versions de Docker sont publiées ! Chaque mise à jour du démon tuait tous les conteneurs sur cet hôte - pas bon ! Heureusement, ce n'est plus un problème.

#### À quoi sert ce *shim* ?

Certains des diagrammes du chapitre ont montré un composant *shim*.

Le *shim* est une partie intégrante de l'implémentation des conteneurs sans démon (ce que nous venons de mentionner sur le découplage des conteneurs en cours d'exécution du démon pour des choses comme la mise à jour du démon sans tuer les conteneurs).

Nous avons mentionné plus tôt que `containerd` utilise `runc` pour créer de nouveaux conteneurs. En fait, il *forke* une nouvelle instance de `runc` pour chaque conteneur qu'il crée. Cependant, une fois que chaque conteneur est créé, son processus `runc` parent se termine. Cela signifie que nous pouvons exécuter des centaines de conteneurs sans avoir à exécuter des centaines d'instances de `runc`.

Une fois que le processus `runc` parent d'un conteneur se termine, le processus `containerd-shim` associé devient le processus parent du conteneur. Parmi les responsabilités que le *shim* assume en tant que parent d'un conteneur, on trouve :

*   Garder les flux STDIN et STDOUT ouverts afin que lorsque le démon est redémarré, le conteneur ne se termine pas à cause de la fermeture des *pipes*, etc.
*   Rapporter le statut de sortie du conteneur au démon.

#### Comment c'est implémenté sous Linux

Sur un système Linux, les composants que nous avons discutés sont implémentés comme des binaires séparés comme suit :
- `dockerd` (le démon Docker)
- `docker-containerd` (`containerd`)
- `docker-containerd-shim` (*shim*)
- `docker-runc` (`runc`)

Vous pouvez voir tous ces éléments sur un système Linux en exécutant une commande `ps` sur l'hôte Docker. Évidemment, certains d'entre eux ne seront présents que lorsque le système a des conteneurs en cours d'exécution.

#### Alors, à quoi sert le démon ?

Avec tout le code d'exécution et de runtime extrait du démon, vous pourriez vous poser la question : « que reste-t-il dans le démon ? ».

Évidemment, la réponse à cette question changera avec le temps, à mesure que de plus en plus de fonctionnalités seront extraites et modularisées. Cependant, au moment de la rédaction, certaines des fonctionnalités majeures qui existent encore dans le démon incluent : la gestion des images, la construction d'images, l'API REST, l'authentification, la sécurité, le réseau de base et l'orchestration.

### Résumé du chapitre

Le Moteur Docker a une conception modulaire et est fortement basé sur les standards ouverts de l'OCI.

Le démon Docker implémente l'API Docker, qui est actuellement une API HTTP riche, versionnée, qui s'est développée parallèlement au reste du projet Docker. Cette API Docker est acceptée comme l'API de conteneur standard de l'industrie.

L'exécution des conteneurs est gérée par `containerd`. `containerd` a été écrit par Docker, Inc. et contribué à la CNCF. Vous pouvez le considérer comme un superviseur de conteneurs qui gère les opérations du cycle de vie des conteneurs. Il est petit et léger et peut être utilisé par d'autres projets et outils tiers.

`containerd` doit communiquer avec un runtime de conteneur conforme à l'OCI pour créer réellement des conteneurs. Par défaut, Docker utilise `runc` comme runtime de conteneur par défaut. `runc` est l'implémentation de facto de la spécification de runtime de conteneur de l'OCI et s'attend à démarrer des conteneurs à partir de *bundles* conformes à l'OCI. `containerd` communique avec `runc` et s'assure que les images Docker sont présentées à `runc` comme des *bundles* conformes à l'OCI.

`runc` peut être utilisé comme un outil autonome pour créer des conteneurs. Il peut également être utilisé par d'autres projets et outils tiers.

Il y a encore beaucoup de fonctionnalités implémentées au sein du démon Docker. D'autres pourraient être extraites avec le temps. Les fonctionnalités actuellement encore à l'intérieur du démon Docker incluent, mais ne sont pas limitées à : l'API, la gestion des images, l'authentification, les fonctionnalités de sécurité, le réseau de base.

Le travail de modularisation du Moteur Docker est en cours.

---

### 6 : Les Images

Dans ce chapitre, nous allons nous plonger dans les images Docker. L'objectif est de vous donner une solide compréhension de ce que sont les images Docker et comment effectuer les opérations de base. Dans un chapitre ultérieur, nous verrons comment construire de nouvelles images avec nos propres applications à l'intérieur (conteneuriser une application).

Nous diviserons ce chapitre en trois parties habituelles :

*   Le TL;DR
*   L'analyse approfondie
*   Les commandes

Allons découvrir les images !

#### Images Docker - Le TL;DR

Si vous êtes un ancien administrateur de VM, vous pouvez considérer les images Docker comme des modèles de VM. Un modèle de VM est comme une VM arrêtée - une image Docker est comme un conteneur arrêté. Si vous êtes un développeur, vous pouvez les considérer comme similaires à des **classes**.

Vous commencez par télécharger (*pull*) des images depuis un registre d'images. Le registre le plus populaire est **Docker Hub**, mais d'autres existent. L'opération de *pull* télécharge l'image sur votre hôte Docker local où vous pouvez l'utiliser pour démarrer un ou plusieurs conteneurs Docker.

Les images sont composées de multiples **couches** (*layers*) qui sont empilées les unes sur les autres et représentées comme un seul objet. À l'intérieur de l'image se trouve un système d'exploitation (OS) allégé et tous les fichiers et dépendances nécessaires pour exécuter une application. Parce que les conteneurs sont destinés à être rapides et légers, les images ont tendance à être petites.

Félicitations ! Vous avez maintenant une petite idée de ce qu'est une image Docker :-D Il est maintenant temps de vous épater !

#### Images Docker - L'analyse approfondie

Nous avons déjà mentionné à plusieurs reprises que les images sont comme des conteneurs arrêtés (ou des classes si vous êtes un développeur). En fait, vous pouvez arrêter un conteneur et créer une nouvelle image à partir de celui-ci. Dans cette optique, les images sont considérées comme des constructions de **temps de construction** (*build-time*) alors que les conteneurs sont des constructions de **temps d'exécution** (*run-time*).![Figure 6.1](https://github.com/user-attachments/assets/6c1e7790-f348-4bc4-b3f5-a73b72e40e53)<p align="center">Figure 6.1</p>
#### Images et conteneurs

La Figure 6.1 montre une vue de haut niveau de la relation entre les images et les conteneurs. Nous utilisons les commandes `docker container run` et `docker service create` pour démarrer un ou plusieurs conteneurs à partir d'une seule image. Cependant, une fois que vous avez démarré un conteneur à partir d'une image, les deux constructions deviennent dépendantes l'une de l'autre et vous ne pouvez pas supprimer l'image tant que le dernier conteneur l'utilisant n'a pas été arrêté et détruit. Tenter de supprimer une image sans arrêter et détruire tous les conteneurs qui l'utilisent entraînera l'erreur suivante :

```shell
$ docker image rm <nom-image>
Error response from daemon: conflict: unable to remove repository reference \
"<nom-image>" (must force) - container <id-conteneur> is using its referenc\
ed image <id-image>
```
*(Réponse d'erreur du démon : conflit : impossible de supprimer la référence du dépôt "<nom-image>" (doit forcer) - le conteneur <id-conteneur> utilise son image référencée <id-image>)*

#### Les images sont généralement petites

Le but même d'un conteneur est d'exécuter une application ou un service. Cela signifie que l'image à partir de laquelle un conteneur est créé doit contenir tous les fichiers du système d'exploitation et de l'application requis pour exécuter l'application/le service. Cependant, les conteneurs sont conçus pour être rapides et légers. Cela signifie que les images à partir desquelles ils sont construits sont généralement petites et dépouillées de toutes les parties non essentielles.

Par exemple, les images Docker ne sont pas livrées avec 6 shells différents au choix - elles sont généralement fournies avec un seul shell minimaliste, voire aucun shell du tout. Elles ne contiennent pas non plus de noyau - tous les conteneurs fonctionnant sur un hôte Docker partagent l'accès au noyau de l'hôte. Pour ces raisons, nous disons parfois que les images contiennent *juste assez de système d'exploitation* (généralement uniquement des fichiers et des objets de système de fichiers liés à l'OS).

> **Note :** Les conteneurs Hyper-V s'exécutent à l'intérieur d'une VM légère dédiée et tirent parti du noyau du système d'exploitation exécuté à l'intérieur de la VM.

L'image Docker officielle d'Alpine Linux fait environ 4 Mo et est un exemple extrême de la petite taille que peuvent avoir les images Docker. Ce n'est pas une faute de frappe ! Elle fait vraiment environ 4 mégaoctets ! Cependant, un exemple plus typique pourrait être quelque chose comme l'image Docker officielle d'Ubuntu qui fait actuellement environ 120 Mo. Celles-ci sont clairement dépouillées de la plupart des parties non essentielles !

Les images basées sur Windows ont tendance à être plus grandes que les images basées sur Linux en raison du fonctionnement du système d'exploitation Windows. Par exemple, la dernière image Microsoft .NET (`microsoft/dotnet:latest`) pèse plus de 2 Go une fois téléchargée et décompressée. L'image Windows Server 2016 Nano Server dépasse légèrement 1 Go une fois téléchargée et décompressée.

#### Télécharger des images (*Pulling*)

Un hôte Docker fraîchement installé n'a aucune image dans son dépôt local.

Le dépôt d'images local sur un hôte Docker basé sur Linux est généralement situé à `/var/lib/docker/<storage-driver>`. Sur les hôtes Docker basés sur Windows, c'est `C:\ProgramData\docker\windowsfilter`.

Vous pouvez vérifier si votre hôte Docker a des images dans son dépôt local avec la commande suivante.

```shell
$ docker image ls
REPOSITORY          TAG                 IMAGE ID            CREATED             SIZE
```

Le processus d'obtention d'images sur un hôte Docker s'appelle le *pulling* (téléchargement). Donc, si vous voulez la dernière image Ubuntu sur votre hôte Docker, vous devrez la *pull*. Utilisez les commandes ci-dessous pour télécharger quelques images, puis vérifiez leurs tailles.

> Si vous suivez sur Linux et n'avez pas ajouté votre compte utilisateur au groupe Unix local `docker`, vous devrez peut-être ajouter `sudo` au début de toutes les commandes suivantes.

**Exemple Linux :**

```shell
$ docker image pull ubuntu:latest
latest: Pulling from library/ubuntu
b6f892c0043b: Pull complete
55010f332b04: Pull complete
2955fb827c94: Pull complete
3deef3fcbd30: Pull complete
cf9722e506aa: Pull complete
Digest: sha256:38245....44463c62a9848133ecb1aa8
Status: Downloaded newer image for ubuntu:latest
$
$ docker image pull alpine:latest
latest: Pulling from library/alpine
cfc728c1c558: Pull complete
Digest: sha256:c0537...497c0a7726c88e2bb7584dc96
Status: Downloaded newer image for alpine:latest
$
$ docker image ls
REPOSITORY          TAG                 IMAGE ID            CREATED             SIZE
ubuntu              latest              ebcd9d4fca80        3 days ago          118MB
alpine              latest              02674b9cb179        8 days ago          3.99MB
```

**Exemple Windows :**

```powershell
> docker image pull microsoft/powershell:nanoserver
nanoserver: Pulling from microsoft/powershell
bce2fbc256ea: Pull complete
...
Digest: sha256:090fe875...fdd9a8779592ea50c9d4524842
Status: Downloaded newer image for microsoft/powershell:nanoserver
>
> docker image pull microsoft/dotnet:latest
latest: Pulling from microsoft/dotnet
bce2fbc256ea: Already exists
...
Digest: sha256:530343cd483dc3e1...6f0378e24310bd67d2a
Status: Downloaded newer image for microsoft/dotnet:latest
>
> docker image ls
REPOSITORY              TAG                 IMAGE ID            CREATED             SIZE
microsoft/dotnet        latest              831..686d           7 hours ago         1.65GB
microsoft/powershell    nanoserver          d06..5427           8 days ago          1.21GB
```

Comme vous pouvez le voir, les images que vous venez de télécharger sont maintenant présentes dans le dépôt local de votre hôte Docker.

#### Nommage des images

Dans chaque commande, nous avons dû spécifier quelle image télécharger. Prenons donc une minute pour examiner le nommage des images. Pour ce faire, nous avons besoin d'un peu de contexte sur la façon dont nous stockons les images.

#### Registres d'images (*Image registries*)

Les images Docker sont stockées dans des **registres d'images**. Le registre le plus courant est **Docker Hub** ([https://hub.docker.com](https://hub.docker.com)). D'autres registres existent, y compris des registres tiers et des registres sécurisés sur site (*on-premises*). Cependant, le client Docker a une opinion bien arrêtée et utilise Docker Hub par défaut. Nous utiliserons Docker Hub pour le reste du livre.

Les registres d'images contiennent plusieurs **dépôts d'images** (*image repositories*). À leur tour, les dépôts d'images peuvent contenir plusieurs images. Cela peut être un peu déroutant, donc la Figure 6.2 montre une image d'un registre d'images contenant 3 dépôts, et chaque dépôt contient une ou plusieurs images.![Figure 6.2](https://github.com/user-attachments/assets/d13c5ad5-501b-41e1-9710-6dbf100c9d8b)<p align="center">Figure 6.2</p>

#### Dépôts officiels et non officiels

Docker Hub a également le concept de dépôts **officiels** et **non officiels**.

Comme leur nom l'indique, les dépôts officiels contiennent des images qui ont été validées par Docker, Inc. Cela signifie qu'elles devraient contenir du code à jour et de haute qualité, qui est sécurisé, bien documenté et conforme aux meilleures pratiques (s'il vous plaît, puis-je avoir une récompense pour avoir utilisé cinq traits d'union dans une seule phrase).

Les dépôts non officiels peuvent être comme le Far West - vous ne devez pas vous attendre à ce qu'ils soient sûrs, bien documentés ou construits selon les meilleures pratiques. Cela ne veut pas dire que tout ce qui se trouve dans les dépôts non officiels est mauvais ! Il y a des choses brillantes dans les dépôts non officiels. Il faut juste être très prudent avant de faire confiance au code qui en provient. Pour être honnête, vous devriez toujours être prudent lorsque vous obtenez des logiciels sur Internet - même des images provenant de dépôts officiels !

La plupart des systèmes d'exploitation et applications populaires ont leurs propres dépôts officiels sur Docker Hub. Ils sont faciles à repérer car ils se trouvent au plus haut niveau de l'espace de noms de Docker Hub. La liste ci-dessous contient quelques-uns des dépôts officiels et montre leurs URL qui existent au plus haut niveau de l'espace de noms de Docker Hub :

*   **nginx** - https://hub.docker.com/_/nginx/
*   **busybox** - https://hub.docker.com/_/busybox/
*   **redis** - https://hub.docker.com/_/redis/
*   **mongo** - https://hub.docker.com/_/mongo/

D'un autre côté, mes propres images personnelles vivent dans le Far West des dépôts non officiels et ne devraient pas être considérées comme fiables ! Voici quelques exemples d'images dans mes dépôts :

*   **nigelpoulton/tu-demo**
    https://hub.docker.com/r/nigelpoulton/tu-demo/
*   **nigelpoulton/pluralsight-docker-ci**
    https://hub.docker.com/r/nigelpoulton/pluralsight-docker-ci/

Non seulement les images de mes dépôts ne sont pas validées, pas tenues à jour, pas sécurisées et pas bien documentées... vous devriez aussi remarquer qu'elles ne se trouvent pas au plus haut niveau de l'espace de noms de Docker Hub. Mes dépôts vivent tous dans un espace de noms de second niveau appelé `nigelpoulton`.

Vous remarquerez probablement que les images Microsoft que nous avons utilisées n'existent pas au plus haut niveau de l'espace de noms de Docker Hub. Au moment de la rédaction, elles existent sous l'espace de noms de second niveau `microsoft`.

Après tout cela, nous pouvons enfin regarder comment nous adressons les images sur la ligne de commande Docker.

#### Nommage et *tagging* des images

Adresser des images provenant de dépôts officiels est aussi simple que de donner le nom du dépôt et le *tag* (étiquette) séparés par deux-points (`:`). Le format pour `docker image pull` lorsque l'on travaille avec une image d'un dépôt officiel est :
`docker image pull <dépôt>:<tag>`

Dans les exemples Linux précédents, nous avons téléchargé une image Alpine et une image Ubuntu avec les deux commandes suivantes :
`docker image pull alpine:latest` et `docker image pull ubuntu:latest`

Ces deux commandes téléchargent les images étiquetées comme « latest » depuis les dépôts « alpine » et « ubuntu ».

Les exemples suivants montrent comment télécharger diverses images différentes depuis des dépôts officiels :

```shell
$ docker image pull mongo:3.3.11
//Ceci téléchargera l'image avec le tag `3.3.11`
//depuis le dépôt officiel `mongo`.

$ docker image pull redis:latest
//Ceci téléchargera l'image avec le tag `latest`
//depuis le dépôt officiel `redis`.

$ docker image pull alpine
//Ceci téléchargera l'image avec le tag `latest`
//depuis le dépôt officiel `alpine`.
```

Quelques points à noter sur les commandes ci-dessus.

Premièrement, si vous ne spécifiez pas de *tag* d'image après le nom du dépôt, Docker supposera que vous faites référence à l'image avec le *tag* `latest`.

Deuxièmement, le *tag* `latest` n'a aucun pouvoir magique ! Ce n'est pas parce qu'une image est étiquetée `latest` que c'est garanti d'être l'image la plus récente dans un dépôt ! Par exemple, l'image la plus récente dans le dépôt `alpine` est généralement étiquetée `edge`. Morale de l'histoire - soyez prudent lorsque vous utilisez le *tag* `latest` !

Télécharger des images depuis un dépôt non officiel est essentiellement la même chose - il vous suffit de préfixer le nom du dépôt avec un nom d'utilisateur ou d'organisation Docker Hub. L'exemple ci-dessous montre comment télécharger l'image `v2` depuis le dépôt `tu-demo` appartenant à une personne peu recommandable dont le nom de compte Docker Hub est `nigelpoulton`.

```shell
$ docker image pull nigelpoulton/tu-demo:v2
//Ceci téléchargera l'image avec le tag `v2`
//depuis le dépôt `tu-demo` dans l'espace de noms
//de mon compte Docker Hub personnel.
```

Dans nos exemples Windows précédents, nous avons téléchargé une image PowerShell et une image .NET avec les deux commandes suivantes :

```powershell
> docker image pull microsoft/powershell:nanoserver
> docker image pull microsoft/dotnet:latest
```

La première commande télécharge l'image avec le *tag* `nanoserver` depuis le dépôt `microsoft/powershell`. La seconde commande télécharge l'image avec le *tag* `latest` depuis le dépôt `microsoft/dotnet`.

Si vous voulez télécharger des images depuis des registres tiers (autres que Docker Hub), vous devez préfixer le nom du dépôt avec le nom DNS du registre. Par exemple, si l'image de l'exemple ci-dessus se trouvait dans le Google Container Registry (GCR), vous devriez ajouter `gcr.io` avant le nom du dépôt comme suit - `docker pull gcr.io/nigelpoulton/tu-demo:v2` (un tel dépôt et une telle image n'existent pas).

Vous pourriez avoir besoin d'un compte sur les registres tiers et d'être connecté pour pouvoir en télécharger des images.

#### Images avec plusieurs *tags*

Un dernier mot sur les *tags* d'images... Une seule image peut avoir autant de *tags* que vous le souhaitez. C'est parce que les *tags* sont des valeurs alphanumériques arbitraires qui sont stockées comme des métadonnées à côté de l'image. Regardons un exemple.

Téléchargez toutes les images d'un dépôt en ajoutant l'option `-a` à la commande `docker image pull`. Ensuite, exécutez `docker image ls` pour regarder les images téléchargées. Si vous suivez avec Windows, vous pouvez télécharger depuis le dépôt `microsoft/nanoserver` au lieu de `nigelpoulton/tu-demo`.

> **Note :** Si le dépôt depuis lequel vous téléchargez contient des images pour plusieurs architectures et plateformes, comme Linux et Windows, la commande est susceptible d'échouer.

```shell
$ docker image pull -a nigelpoulton/tu-demo
latest: Pulling from nigelpoulton/tu-demo
237d5fcd25cf: Pull complete
a3ed95caeb02: Pull complete
<Snip>
Digest: sha256:42e34e546cee61adb1...3a0c5b53f324a9e1c1aae451e9
v1: Pulling from nigelpoulton/tu-demo
237d5fcd25cf: Already exists
a3ed95caeb02: Already exists
<Snip>
Digest: sha256:9ccc0c67e5c5eaae4b...624c1d5c80f2c9623cbcc9b59a
v2: Pulling from nigelpoulton/tu-demo
237d5fcd25cf: Already exists
a3ed95caeb02: Already exists
<Snip>
Digest: sha256:d3c0d8c9d5719d31b7...9fef58a7e038cf0ef2ba5eb74c
Status: Downloaded newer image for nigelpoulton/tu-demo
$
$ docker image ls
REPOSITORY              TAG                 IMAGE ID            CREATED             SIZE
nigelpoulton/tu-demo    v2                  6ac2...ad           12 months ago       211.6 MB
nigelpoulton/tu-demo    latest              9b91...29           12 months ago       211.6 MB
nigelpoulton/tu-demo    v1                  9b91...29           12 months ago       211.6 MB
```

Quelques points sur ce qui vient de se passer dans l'exemple cité ci-dessus :

Premièrement. La commande a téléchargé trois images depuis le dépôt : `latest`, `v1` et `v2`.

Deuxièmement. Regardez attentivement la colonne `IMAGE ID` dans la sortie de la commande `docker image ls`. Vous verrez qu'il n'y a que deux ID d'image uniques. C'est parce que seulement deux images ont été réellement téléchargées. Ceci, à son tour, est dû au fait que deux des *tags* font référence à la même image. Autrement dit... l'une des images a deux *tags*. Si vous regardez attentivement, vous verrez que les *tags* `v1` et `latest` ont le même `IMAGE ID`. Cela signifie qu'il s'agit de deux *tags* pour la même image.

C'est aussi un exemple parfait de l'avertissement que nous avons émis plus tôt à propos du *tag* `latest`. Dans cet exemple, le *tag* `latest` fait référence à la même image que le *tag* `v1`. Cela signifie qu'il pointe vers la plus ancienne des deux images - pas la plus récente ! `latest` est un *tag* arbitraire et il n'est pas garanti qu'il pointe vers l'image la plus récente d'un dépôt !

#### Images et couches (*layers*)

Une image Docker n'est qu'un ensemble de **couches** en lecture seule, faiblement connectées. Ceci est illustré à la Figure 6.3.![Figure 6.3](https://github.com/user-attachments/assets/7bfdace1-4d21-4613-9ba9-05f44c0097a8)<p align="center">Figure 6.3</p>
Docker se charge d'empiler ces couches et de les représenter comme un seul objet unifié.

Il existe plusieurs façons de voir et d'inspecter les couches qui composent une image, et nous en avons déjà vu une. Jetons un second coup d'œil à la sortie de la commande `docker image pull ubuntu:latest` de tout à l'heure :

```shell
$ docker image pull ubuntu:latest
latest: Pulling from library/ubuntu
952132ac251a: Pull complete
82659f8f1b76: Pull complete
c19118ca682d: Pull complete
8296858250fe: Pull complete
24e0251a0e2c: Pull complete
Digest: sha256:f4691c96e6bbaa99d...28ae95a60369c506dd6e6f6ab
Status: Downloaded newer image for ubuntu:latest
```

Chaque ligne de la sortie ci-dessus qui se termine par « Pull complete » représente une couche de l'image qui a été téléchargée. Comme nous pouvons le voir, cette image a 5 couches.

La Figure 6.4 ci-dessous montre cela sous forme d'image.![Figure 6.4](https://github.com/user-attachments/assets/6d8dd1f6-ceba-4250-a866-a79acebfee7d)<p align="center">Figure 6.4</p>
Une autre façon de voir les couches d'une image est d'inspecter l'image avec la commande `docker image inspect`. L'exemple ci-dessous inspecte la même image `ubuntu:latest`.

```shell
$ docker image inspect ubuntu:latest
[
    {
        "Id": "sha256:bd3d4369ae.......fa2645f5699037d7d8c6b415a10",
        "RepoTags": [
            "ubuntu:latest"
        ],
<Snip>
        "RootFS": {
            "Type": "layers",
            "Layers": [
                "sha256:c8a75145fc...894129005e461a43875a094b93412",
                "sha256:c6f2b330b6...7214ed6aac305dd03f70b95cdc610",
                "sha256:055757a193...3a9565d78962c7f368d5ac5984998",
                "sha256:4837348061...12695f548406ea77feb5074e195e3",
                "sha256:0cad5e07ba...4bae4cfc66b376265e16c32a0aae9"
            ]
        }
    }
]
```

La sortie tronquée montre à nouveau 5 couches. Seulement cette fois, elles sont affichées en utilisant leurs hachages SHA256. Cependant, les deux commandes montrent que l'image a 5 couches.

> **Note :** La commande `docker history` montre l'historique de construction d'une image et n'est pas une liste stricte des couches de l'image. Par exemple, certaines instructions du Dockerfile utilisées pour construire une image ne créent pas de couches. Celles-ci incluent ; « MAINTAINER », « ENV », « EXPOSE » et « ENTRYPOINT ». Au lieu de créer de nouvelles couches, elles ajoutent des métadonnées à l'image.

Toutes les images Docker commencent par une couche de base, et à mesure que des modifications sont apportées et que du nouveau contenu est ajouté, de nouvelles couches sont ajoutées par-dessus.

À titre d'exemple très simplifié, vous pourriez créer une nouvelle image basée sur Ubuntu Linux 16.04. Ce serait la première couche de votre image. Si vous ajoutez plus tard le paquet Python, celui-ci serait ajouté comme une deuxième couche par-dessus la couche de base. Si vous ajoutez ensuite un correctif de sécurité, celui-ci serait ajouté comme une troisième couche au sommet. Votre image aurait maintenant trois couches comme le montre la Figure 6.5 (rappelez-vous qu'il s'agit d'un exemple très simplifié à des fins de démonstration).![Figure 6.5](https://github.com/user-attachments/assets/dc8d6a35-51dc-4e1e-aef9-700fb4e5543c)<p align="center">Figure 6.5</p>
Il est important de comprendre qu'à mesure que des couches supplémentaires sont ajoutées, l'image devient la combinaison de toutes les couches. Prenez un exemple simple de deux couches comme le montre la Figure 6.6. Chaque couche a 3 fichiers, mais l'image globale a 6 fichiers car elle est la combinaison des deux couches.![Figure 6.6](https://github.com/user-attachments/assets/12dc3b7b-55b7-4e7a-8ad3-d92abddfb35b)<p align="center">Figure 6.6</p>
Nous avons montré les couches de l'image dans la Figure 6.6 d'une manière légèrement différente des figures précédentes. C'est juste pour faciliter la présentation des fichiers.

Dans l'exemple légèrement plus complexe de l'image à trois couches de la Figure 6.7, l'image globale ne présente que 6 fichiers dans la vue unifiée. C'est parce que le fichier 7 dans la couche supérieure est une version mise à jour du fichier 5 juste en dessous (en ligne). Dans cette situation, le fichier de la couche supérieure masque le fichier directement en dessous. Cela permet d'ajouter des versions mises à jour de fichiers en tant que nouvelles couches à l'image.![Figure 6.7](https://github.com/user-attachments/assets/b9179cb8-c7dd-44ad-8ce9-9079edb3f22c)<p align="center">Figure 6.7</p>
Docker emploie un **pilote de stockage** (*storage driver*) qui est responsable de l'empilement des couches et de leur présentation comme un système de fichiers unifié unique. Des exemples de pilotes de stockage sur Linux incluent AUFS, overlay2, devicemapper, btrfs et zfs. Comme leurs noms le suggèrent, chacun est basé sur un système de fichiers ou une technologie de périphérique bloc Linux, et chacun a ses propres caractéristiques de performance uniques. Le seul pilote pris en charge par Docker sur Windows est `windowsfilter` et il implémente la gestion des couches et le CoW (Copy-on-Write) par-dessus NTFS.

#### Partage des couches d'images

Plusieurs images peuvent partager, et partagent effectivement, des couches. Cela conduit à des gains d'efficacité en termes d'espace et de performance.

Jetons un second coup d'œil à la commande `docker image pull` avec l'option `-a` que nous avons exécutée il y a une minute ou deux pour télécharger toutes les images étiquetées dans le dépôt `nigelpoulton/tu-demo`.

```shell
$ docker image pull -a nigelpoulton/tu-demo
latest: Pulling from nigelpoulton/tu-demo
237d5fcd25cf: Pull complete
a3ed95caeb02: Pull complete
<Snip>
Digest: sha256:42e34e546cee61adb100...a0c5b53f324a9e1c1aae451e9
v1: Pulling from nigelpoulton/tu-demo
237d5fcd25cf: Already exists
a3ed95caeb02: Already exists
<Snip>
Digest: sha256:9ccc0c67e5c5eaae4beb...24c1d5c80f2c9623cbcc9b59a
v2: Pulling from nigelpoulton/tu-demo
237d5fcd25cf: Already exists
a3ed95caeb02: Already exists
<Snip>
eab5aaac65de: Pull complete
Digest: sha256:d3c0d8c9d5719d31b79c...fef58a7e038cf0ef2ba5eb74c
Status: Downloaded newer image for nigelpoulton/tu-demo
$
$ docker image ls
REPOSITORY              TAG                 IMAGE ID            CREATED             SIZE
nigelpoulton/tu-demo    v2                  6ac...ead           4 months ago        211.6 MB
nigelpoulton/tu-demo    latest              9b9...e29           4 months ago        211.6 MB
nigelpoulton/tu-demo    v1                  9b9...e29           4 months ago        211.6 MB
```

Remarquez les lignes se terminant par **Already exists** (*Existe déjà*).

Ces lignes nous indiquent que Docker est assez intelligent pour reconnaître quand on lui demande de télécharger une couche d'image dont il a déjà une copie. Dans cet exemple, Docker a d'abord téléchargé l'image étiquetée `latest`. Ensuite, lorsqu'il a voulu télécharger les images `v1` et `v2`, il a remarqué qu'il possédait déjà certaines des couches qui composent ces images. Cela se produit parce que les trois images de ce dépôt sont presque identiques et partagent donc de nombreuses couches.

Comme mentionné précédemment, Docker sur Linux prend en charge de nombreux systèmes de fichiers et pilotes de stockage différents. Chacun est libre d'implémenter la gestion des couches d'images, le partage de couches et le comportement de copie sur écriture (Copy-on-Write) à sa manière. Cependant, le résultat global et l'expérience utilisateur sont essentiellement les mêmes. Bien que Windows ne prenne en charge qu'un seul pilote de stockage, ce pilote offre la même expérience que sous Linux.

#### Télécharger des images par *digest*

Jusqu'à présent, nous vous avons montré comment télécharger des images par *tag*, et c'est de loin la manière la plus courante. Mais elle a un problème - les *tags* sont **mutables** ! Cela signifie qu'il est possible d'étiqueter accidentellement une image avec un *tag* incorrect. Il est même parfois possible d'étiqueter une image avec le même *tag* qu'une image existante, mais différente. Cela peut causer des problèmes !

Par exemple, imaginez que vous avez une image appelée `golftrack:1.5` qui a un bogue connu. Vous téléchargez l'image, appliquez un correctif, et renvoyez (*push*) l'image mise à jour vers son dépôt avec le même *tag*.

Prenez une seconde pour comprendre ce qui vient de se passer... Vous avez une image appelée `golftrack:1.5` qui a un bogue. Cette image est utilisée dans votre environnement de production. Vous téléchargez l'image et appliquez un correctif. Puis vient l'erreur... vous renvoyez l'image corrigée à son dépôt avec le même *tag* que l'image vulnérable ! Comment allez-vous savoir lesquels de vos systèmes de production exécutent l'image vulnérable et lesquels exécutent l'image corrigée ? Les deux images ont le même *tag* !

C'est là que les **digests** d'images viennent à la rescousse.

Docker 1.10 a introduit un nouveau modèle de stockage adressable par le contenu. Dans le cadre de ce nouveau modèle, toutes les images obtiennent désormais un hachage cryptographique de leur contenu. Pour les besoins de cette discussion, nous appellerons ce hachage le **digest**. Parce que le *digest* est un hachage du contenu de l'image, il n'est pas possible de modifier le contenu de l'image sans que le *digest* ne change également. Cela signifie que les *digests* sont **immuables**. Cela aide à éviter le problème dont nous venons de parler.

Chaque fois que vous téléchargez une image, la commande `docker image pull` inclura le *digest* de l'image dans son code de retour. Vous pouvez également afficher les *digests* des images dans le dépôt local de votre hôte Docker en ajoutant l'option `--digests` à la commande `docker image ls`. Ces deux cas sont montrés dans l'exemple suivant.

```shell
$ docker image pull alpine
Using default tag: latest
latest: Pulling from library/alpine
e110a4a17941: Pull complete
Digest: sha256:3dcdb92d7432d56604d...6d99b889d0626de158f73a
Status: Downloaded newer image for alpine:latest
$
$ docker image ls --digests alpine
REPOSITORY   TAG      DIGEST                                                                    IMAGE ID      CREATED        SIZE
alpine       latest   sha256:3dcdb92d7432d56604d...6d99b889d0626de158f73a        4e38e38c8ce0  10 weeks ago   4.8 MB
```

La sortie tronquée ci-dessus montre le *digest* pour l'image alpine comme étant `sha256:3dcdb92d7432d56604d...6d99b889d0626de158f73a`.

Maintenant que nous connaissons le *digest* de l'image, nous pouvons l'utiliser pour la télécharger à nouveau. Cela garantira que nous obtenons exactement l'image que nous attendons !

> Au moment de la rédaction, il n'y a pas de commande Docker native qui récupère le *digest* d'une image depuis un registre distant tel que Docker Hub. Cela signifie que la seule façon de déterminer le *digest* d'une image est de la télécharger par *tag*, puis de noter son *digest*. Cela changera sans aucun doute à l'avenir.

L'exemple ci-dessous supprime l'image `alpine:latest` de votre hôte Docker, puis montre comment la télécharger à nouveau en utilisant son *digest* au lieu de son *tag*.

```shell
$ docker image rm alpine:latest
Untagged: alpine:latest
Untagged: alpine@sha256:c0537...7c0a7726c88e2bb7584dc96
Deleted: sha256:02674b9cb179d...abff0c2bf5ceca5bad72cd9
Deleted: sha256:e154057080f40...3823bab1be5b86926c6f860
$
$ docker image pull alpine@sha256:c0537...7c0a7726c88e2bb7584dc96
sha256:c0537...7726c88e2bb7584dc96: Pulling from library/alpine
cfc728c1c558: Pull complete
Digest: sha256:c0537ff6a5218...7c0a7726c88e2bb7584dc96
Status: Downloaded newer image for alpine@sha256:c0537...bb7584dc96
```

#### Un peu plus sur les hachages d'images (*digests*)

Depuis la version 1.10 de Docker, une image est une collection très lâche de couches indépendantes.

L'image elle-même n'est en réalité qu'un objet de configuration qui liste les couches ainsi que certaines métadonnées.

Les couches qui composent une image sont totalement indépendantes et n'ont aucune notion de faire partie d'une image collective.

Chaque image est identifiée par un ID crypto qui est un hachage de l'objet de configuration. Chaque couche est identifiée par un ID crypto qui est un hachage du contenu qu'elle contient.

Cela signifie que la modification du contenu de l'image, ou de l'une de ses couches, entraînera la modification des hachages crypto associés. Par conséquent, les images et les couches sont immuables.

Nous appelons ces hachages des **hachages de contenu** (*content hashes*).

Jusqu'à présent, les choses sont assez simples. Mais elles sont sur le point de se compliquer un peu.

Lorsque nous envoyons (*push*) et téléchargeons (*pull*) des images, nous compressons leurs couches pour économiser de la bande passante ainsi que de l'espace dans le magasin de blobs (*blob store*) du Registre.

Cool, mais compresser une couche modifie son contenu ! Cela signifie que son hachage de contenu ne correspondra plus après l'opération de *push* ou de *pull* ! C'est évidemment un problème.

Pour contourner cela, chaque couche obtient également quelque chose appelé un **hachage de distribution** (*distribution hash*). Il s'agit d'un hachage de la version compressée de la couche. Lorsqu'une couche est envoyée et téléchargée depuis le registre, son hachage de distribution est inclus, et c'est ce qui est utilisé pour vérifier que la couche est arrivée sans avoir été altérée.

Ce modèle de stockage adressable par le contenu améliore considérablement la sécurité en nous donnant un moyen de vérifier les données des images et des couches après les opérations de *push* et de *pull*. Il évite également les collisions d'ID qui pourraient se produire si les ID d'images et de couches étaient générés de manière aléatoire.

#### Images multi-architectures

Docker inclut désormais le support des images multi-plateformes et multi-architectures. Cela signifie qu'un seul dépôt et *tag* d'image peut avoir une image pour Linux sur x64 et Linux sur PowerPC, etc.

Pour ce faire, l'API du Registre prend en charge un **manifeste unifié** (*fat manifest*) ainsi qu'un manifeste d'image. Les manifestes unifiés listent les architectures prises en charge par une image particulière, tandis que les manifestes d'image listent les couches qui composent une image particulière.

Si vous exécutez Docker sur Linux x64 et que vous téléchargez une image, votre client Docker fait une requête à l'API du Registre. Si un manifeste unifié existe pour cette image, il sera analysé pour voir s'il existe une entrée pour Linux sur x64. Si c'est le cas, le manifeste d'image pour cette architecture est récupéré et analysé pour obtenir les couches réelles qui composent l'image.

#### Supprimer des Images

Lorsque vous n'avez plus besoin d'une image, vous pouvez la supprimer de votre hôte Docker avec la commande `docker image rm`. `rm` est l'abréviation de *remove* (supprimer).

Supprimez les images téléchargées dans les étapes précédentes avec la commande `docker image rm`. L'exemple ci-dessous supprime une image par son ID, celui-ci peut être différent sur votre système.

```shell
$ docker image rm 02674b9cb179
Untagged: alpine@sha256:c0537ff6a5218...c0a7726c88e2bb7584dc96
Deleted: sha256:02674b9cb179d57...31ba0abff0c2bf5ceca5bad72cd9
Deleted: sha256:e154057080f4063...2a0d13823bab1be5b86926c6f860
```

Un raccourci pratique pour nettoyer un système et supprimer toutes les images sur un hôte Docker est d'exécuter la commande `docker image rm` et de lui passer une liste de tous les ID d'image sur le système en appelant `docker image ls` avec l'option `-q`. Ceci est montré ci-dessous.

> Si vous exécutez la commande suivante sur un système Windows, elle ne fonctionnera que dans un terminal PowerShell. Elle ne fonctionnera pas dans une invite de commandes CMD.

```shell
$ docker image rm $(docker image ls -q) -f
```

Pour comprendre comment cela fonctionne, téléchargez quelques images, puis exécutez `docker image ls -q`.

```shell
$ docker image pull alpine
...
$ docker image pull ubuntu
...
$ docker image ls -q
bd3d4369aebc
4e38e38c8ce0
```

Voyez comment `docker image ls -q` renvoie une liste contenant uniquement les ID des images téléchargées localement sur le système. Passer cette liste à `docker image rm` supprimera toutes les images du système.

```shell
$ docker image rm $(docker image ls -q) -f
Untagged: ubuntu:latest
...
Deleted: sha256:c8a75145fcc4e1a...4129005e461a43875a094b93412
Untagged: alpine:latest
...
Deleted: sha256:4fe15f8d0ae69e1...eeeeebb265cd2e328e15c6a869f
$
$ docker image ls
REPOSITORY          TAG                 IMAGE ID            CREATED             SIZE
```

#### Images - Les commandes

Rappelons-nous les principales commandes que nous utilisons pour travailler avec les images Docker.

*   `docker image pull` est la commande pour télécharger des images. Nous téléchargeons des images depuis des dépôts à l'intérieur de registres distants. Par défaut, les images seront téléchargées depuis des dépôts sur Docker Hub. Cette commande téléchargera l'image avec le *tag* `latest` depuis le dépôt `alpine` sur Docker Hub : `docker image pull alpine:latest`.
*   `docker image ls` liste toutes les images stockées dans le cache local de votre hôte Docker. Pour voir les *digests* SHA256 des images, ajoutez l'option `--digests`.
*   `docker image inspect` est une pure merveille ! Elle vous donne tous les détails glorieux d'une image - données des couches et métadonnées.
*   `docker image rm` est la commande pour supprimer des images. Cette commande montre comment supprimer l'image `alpine:latest` : `docker image rm alpine:latest`. Vous ne pouvez pas supprimer une image qui est associée à un conteneur à l'état en cours d'exécution (*Up*) ou arrêté (*Exited*).

### Résumé du chapitre

Dans ce chapitre, nous avons découvert les images Docker. Nous avons appris qu'elles sont comme des modèles de machines virtuelles et sont utilisées pour démarrer des conteneurs. Sous le capot, elles sont composées d'une ou plusieurs couches en lecture seule qui, une fois empilées, constituent l'image globale.

Nous avons utilisé la commande `docker image pull` pour télécharger quelques images dans le registre local de notre hôte Docker.

Nous avons couvert le nommage des images, les dépôts officiels et non officiels, la gestion des couches, le partage et les ID crypto.

Nous avons terminé en examinant certaines des commandes les plus courantes utilisées pour travailler avec les images.

Dans le prochain chapitre, nous ferons un tour similaire des conteneurs - le cousin d'exécution des images.
