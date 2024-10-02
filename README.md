# Carnet de Vaccination Électronique

## Description

Ce projet est un carnet de vaccination électronique où les utilisateurs peuvent créer un compte, ajouter des vaccins, et recevoir des notifications sur les prochains vaccins à prendre en fonction d'un calendrier de vaccinations.

## Technologie

- **Backend** : API .NET 8
- **Frontend** : Angular v18.2.5 avec Angular Material
- **Base de Données** : SQLite via Entity Framework Core

## Fonctionnalités

- Création et gestion de comptes utilisateurs (créer, modifier et supprimer)
- Ajout et suivi des vaccins (ajout et modifier)
- Notifications pour les prochains vaccins basées sur un calendrier prédéfini

## Installation

### Backend

1. **Cloner le repository** :
    ```sh
    git clone https://github.com/OlivierWan/Vaccination.git
    ```
2. **Naviguer dans le répertoire backend** :
    ```sh
    cd Vaccination.Backend
    ```
    
3. **Migration base de données** :
- Automatique ( voir dans le program.cs)
- Insertion de données par défaut (calendrier de vaccination et roles utilisateur)
- Base de données dans le projet Infrastructure/Data/Database/vaccination.db 

### Frontend

1. **Naviguer dans le répertoire frontend** :
    ```sh
    cd Vaccination.Frontend
    ```
2. **Installer Angular CLI** :
    ```sh
    npm install -g @angular/cli
    ```
3. **Installer les dépendances** :
    ```sh
    npm install
    ```
4. **Exécuter le serveur de développement** :
    ```sh
    npm start
    ```
5. **Ouvrir l'application** :
    Ouvrir votre navigateur et naviguer vers `http://localhost:4200/`.
