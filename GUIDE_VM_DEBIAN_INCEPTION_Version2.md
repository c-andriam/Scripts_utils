# Guide détaillé — Installation de Debian sur une machine virtuelle pour le projet Inception

Ce guide explique comment installer une machine virtuelle Debian, la configurer, et installer Docker dessus afin de préparer l’environnement pour le projet Inception.  
Chaque étape est détaillée pour que vous puissiez comprendre ce que vous faites et pourquoi.

---

## 1. Télécharger l’image ISO de Debian

**Pourquoi ?**  
Il vous faut l’image d’installation de Debian pour créer la VM.

**Étapes :**
- Allez sur le site officiel : [https://www.debian.org/distrib/](https://www.debian.org/distrib/)
- Téléchargez l’image “netinst” (installation réseau), adaptée à l’architecture de votre machine (généralement “amd64”).

---

## 2. Créer une machine virtuelle

**Pourquoi ?**  
La VM vous permet de travailler dans un environnement isolé et propre.

**Étapes (exemple avec VirtualBox) :**
1. **Ouvrez VirtualBox** (ou VMware, ou votre gestionnaire de VM préféré).
2. **Cliquez sur “Nouvelle”** pour créer une nouvelle VM.
3. **Nom de la VM** : Par exemple `debian-inception`.
4. **Type** : Linux
5. **Version** : Debian (64-bit)
6. **Mémoire vive (RAM)** : Minimum 2048 Mo (2 Go). Recommandé : 4096 Mo (4 Go) ou plus si possible.
7. **Disque dur virtuel** :  
   - Créez un disque dur virtuel maintenant.
   - Type VDI (VirtualBox Disk Image).
   - Stockage dynamique ou taille fixe selon vos préférences.
   - Taille : Minimum 20 Go (plus si vous comptez faire beaucoup d’essais ou bonus).

---

## 3. Monter l’ISO et démarrer l’installation

**Pourquoi ?**  
Démarrer la VM avec l’image ISO permet de lancer l’installation de Debian.

**Étapes :**
1. Sélectionnez la VM nouvellement créée dans VirtualBox.
2. Cliquez sur “Configuration” > “Stockage” > “Vide” sous “Contrôleur IDE” > “Choisir un disque” > Sélectionnez l’ISO téléchargé.
3. Cliquez sur “Démarrer” pour lancer la VM.

---

## 4. Installer Debian (dans la VM)

**Pourquoi ?**  
Pour disposer d’un système de base propre et stable pour travailler.

**Étapes principales :**
1. **Choisissez “Install” ou “Graphical Install”** (vous pouvez prendre le mode texte, il est plus rapide).
2. **Sélectionnez la langue** : Français ou Anglais selon vos préférences.
3. **Sélectionnez votre pays** et le clavier.
4. **Configuration réseau** :
   - Laissez par défaut si vous êtes en NAT (VirtualBox règle tout seul).
   - Donnez un nom à votre machine (ex : `inception-vm`).
5. **Définissez un utilisateur root (mot de passe)**.  
   **Note de sécurité :** choisissez un mot de passe fort et ne l’oubliez pas.
6. **Créez un utilisateur standard** (ex : votre login 42, pour la cohérence avec les volumes).
7. **Partitionnement du disque** :
   - Pour un projet/test, choisissez “Guidé - utiliser tout le disque”.
   - Partition unique ou séparée selon vos besoins (le plus simple : tout dans une partition).
8. **Attendez la copie des fichiers** (peut prendre quelques minutes).
9. **Choisissez d’utiliser un miroir réseau** (recommandé pour avoir les paquets à jour).
10. **Sélection des logiciels** :
    - Décochez “Environnement de bureau” (inutile pour ce projet).
    - Laissez “Utilitaires usuels du système”, “Serveur SSH” si vous voulez accéder à distance.
11. **Installez le chargeur de démarrage GRUB** sur le disque proposé.
12. **Retirez l’ISO** du lecteur virtuel (menu “Périphériques” > “Lecteur optique” > “Retirer le disque”).
13. **Redémarrez la VM**.

---

## 5. Premiers réglages après installation

**Pourquoi ?**  
Pour s’assurer que la VM est utilisable et prête pour accueillir Docker.

**Étapes :**
1. **Connectez-vous** avec l’utilisateur créé.
2. **Mettez à jour le système :**
    ```bash
    sudo apt update
    sudo apt upgrade -y
    ```
3. **Installez quelques outils utiles :**
    ```bash
    sudo apt install -y vim git curl wget ca-certificates lsb-release apt-transport-https
    ```

---

## 6. Installation de Docker

**Pourquoi ?**  
Docker est essentiel pour le projet Inception (toutes les images et containers seront gérés via Docker).

**Étapes officielles pour Debian :**
1. **Supprimer d’anciennes versions de Docker (si besoin) :**
    ```bash
    sudo apt remove docker docker-engine docker.io containerd runc
    ```
2. **Installer les dépendances :**
    ```bash
    sudo apt update
    sudo apt install -y ca-certificates curl gnupg
    ```
3. **Ajouter la clé GPG officielle de Docker :**
    ```bash
    sudo install -m 0755 -d /etc/apt/keyrings
    curl -fsSL https://download.docker.com/linux/debian/gpg | sudo gpg --dearmor -o /etc/apt/keyrings/docker.gpg
    sudo chmod a+r /etc/apt/keyrings/docker.gpg
    ```
4. **Ajouter le dépôt Docker au sources.list :**
    ```bash
    echo \
      "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/debian \
      $(lsb_release -cs) stable" | \
      sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
    ```
5. **Mettre à jour et installer Docker :**
    ```bash
    sudo apt update
    sudo apt install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin
    ```
6. **Vérifier l’installation :**
    ```bash
    sudo docker run hello-world
    ```
    Si vous voyez un message de bienvenue, Docker fonctionne bien !

7. **(Optionnel, mais recommandé) Ajouter votre utilisateur au groupe docker :**
    ```bash
    sudo usermod -aG docker $USER
    ```
    > **Déconnectez-vous puis reconnectez-vous** pour que cela prenne effet.

8. **Vérifier que vous pouvez lancer Docker sans sudo :**
    ```bash
    docker ps
    ```
    Si la commande fonctionne sans erreur, c’est bon.

---

## 7. Installation de Docker Compose (plugin)

**Pourquoi ?**  
Docker Compose est obligatoire pour orchestrer les différents services dans le projet Inception.

**Étape :**
- Depuis Debian 12, Docker Compose s’installe en tant que plugin avec la commande ci-dessus.  
- Vérifiez la version :
    ```bash
    docker compose version
    ```
    Si la version s’affiche, tout est prêt.

---

## 8. (Optionnel) Configuration réseau de la VM

**Pourquoi ?**  
Pour accéder à vos services depuis l’hôte ou Internet.

**Étape :**
- Par défaut, la VM est en mode NAT : c’est suffisant pour ce projet.
- Pour accéder à vos services via le navigateur de votre machine hôte, redirigez les ports ou configurez le mode “Accès par pont” (Bridge) dans les réglages de la VM.
- **Attention :** ne jamais exposer une VM non sécurisée sur Internet !

---

## 9. Prêt pour le projet Inception

Vous avez maintenant :
- Une machine virtuelle Debian propre et à jour.
- Docker et Docker Compose installés et fonctionnels.
- Un environnement prêt à accueillir la structure du projet Inception.

**Vous pouvez maintenant suivre le guide principal d’Inception pour commencer la création de vos dossiers, Dockerfiles, etc.**

---

## Résumé des commandes pour l’installation de Docker

```bash
sudo apt update && sudo apt upgrade -y
sudo apt install -y ca-certificates curl gnupg lsb-release
sudo install -m 0755 -d /etc/apt/keyrings
curl -fsSL https://download.docker.com/linux/debian/gpg | sudo gpg --dearmor -o /etc/apt/keyrings/docker.gpg
sudo chmod a+r /etc/apt/keyrings/docker.gpg
echo \
  "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/debian \
  $(lsb_release -cs) stable" | \
  sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
sudo apt update
sudo apt install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin
sudo usermod -aG docker $USER  # (Optionnel, pour éviter sudo)
```

---

## Documentation officielle

- [Documentation officielle Debian](https://www.debian.org/doc/)
- [Documentation officielle Docker pour Debian](https://docs.docker.com/engine/install/debian/)
- [Documentation Docker Compose](https://docs.docker.com/compose/)