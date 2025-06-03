# Connexion SSH à une VM Debian depuis votre machine physique (sans environnement graphique)

Ce guide explique comment accéder à votre VM Debian via SSH directement depuis le terminal de votre machine physique.  
Cela facilite le travail sans interface graphique et permet d’utiliser le copier/coller, le transfert de fichiers, etc.

---

## 1. Installer le serveur SSH sur la VM Debian

**Pourquoi ?**  
Permet à votre VM de recevoir des connexions SSH.

**Étapes :**
```bash
sudo apt update
sudo apt install -y openssh-server
```

---

## 2. Vérifier que le service SSH fonctionne

```bash
sudo systemctl status ssh
```
- Si besoin, démarrez et activez le service :
    ```bash
    sudo systemctl enable --now ssh
    ```

---

## 3. Déterminer l’adresse IP de votre VM Debian

- **Dans la VM, tapez :**
    ```bash
    ip a
    ```
    ou
    ```bash
    hostname -I
    ```
- Notez l’adresse IP affichée (exemple : `192.168.56.101`).

---

## 4. Configurer le réseau de la VM pour l’accès SSH

- **Si vous utilisez VirtualBox :**
    - Pour se connecter facilement, le mode “Réseau : Accès par pont (Bridge)” est le plus simple.  
      - Éteignez la VM.
      - Dans VirtualBox, sélectionnez votre VM > "Configuration" > "Réseau".
      - Adaptez “Mode d’accès réseau” sur “Accès par pont”.
      - Démarrez la VM.
      - Refaites `ip a` pour obtenir l’adresse IP donnée par votre réseau local.

    - **Si vous gardez le mode NAT,** il faut configurer une redirection de port :
      - VM éteinte, dans VirtualBox > Configuration > Réseau > Avancé > Redirection de ports.
      - Ajoutez une règle :  
        - Protocole : TCP  
        - Hôte IP : 127.0.0.1  
        - Port hôte : 2222  
        - IP invité : (laissez vide ou 10.0.2.15, par défaut)  
        - Port invité : 22  
      - Démarrez la VM.

---

## 5. Se connecter en SSH depuis la machine physique

### **Si vous êtes en mode Bridge :**
- Dans un terminal sur votre machine physique :
    ```bash
    ssh <utilisateur>@<ip_de_la_vm>
    ```
    _Exemple :_
    ```bash
    ssh c-andriam@192.168.1.45
    ```

### **Si vous êtes en mode NAT avec redirection de port :**
- Sur votre machine physique :
    ```bash
    ssh -p 2222 <utilisateur>@127.0.0.1
    ```
    _Exemple :_
    ```bash
    ssh -p 2222 c-andriam@127.0.0.1
    ```

- **Acceptez la clé si c’est la première connexion (tapez `yes`).**
- Entrez le mot de passe de l’utilisateur de la VM.

---

## 6. (Optionnel) Sécurisation et confort

- **Changer le port SSH (ex : 2222) ou désactiver le login root** dans `/etc/ssh/sshd_config`.
- **Copier votre clé publique SSH** pour éviter d’avoir à taper le mot de passe à chaque fois :
    ```bash
    ssh-copy-id <utilisateur>@<ip_de_la_vm>
    ```
    ou (si NAT avec port 2222) :
    ```bash
    ssh-copy-id -p 2222 <utilisateur>@127.0.0.1
    ```
- **Redémarrer le service SSH après modification de la configuration :**
    ```bash
    sudo systemctl restart ssh
    ```

---

## 7. Résumé des commandes utiles

```bash
# Sur la VM Debian
sudo apt update
sudo apt install -y openssh-server
sudo systemctl enable --now ssh
ip a  # Pour obtenir l'IP

# Depuis la machine physique
ssh <utilisateur>@<ip_vm>              # Mode Bridge
ssh -p 2222 <utilisateur>@127.0.0.1    # Mode NAT avec redirection de port
```

---

## 8. Dépannage

- **Impossible de se connecter ?**
  - Vérifiez que le firewall (ufw) autorise le port 22 :  
    ```bash
    sudo ufw allow 22/tcp
    sudo ufw reload
    ```
  - Vérifiez l’IP de la VM et le mode réseau.
  - Vérifiez que le service SSH fonctionne :  
    `sudo systemctl status ssh`

---

**Vous êtes maintenant prêt à travailler sur votre VM Debian en SSH, directement depuis votre terminal !**