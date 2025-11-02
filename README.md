# JWT Auth API

API REST .NET 9.0 avec authentification JWT (JSON Web Token) et documentation Swagger.

## Description

Cette API fournit un système d'authentification complet basé sur JWT, permettant la gestion sécurisée des utilisateurs et la protection des endpoints via des tokens Bearer.

## Technologies utilisées

- **.NET 9.0** - Framework principal
- **ASP.NET Core Web API** - Framework API REST
- **JWT Bearer Authentication** - Système d'authentification
- **Swagger/OpenAPI** - Documentation interactive de l'API
- **Microsoft.IdentityModel.Tokens** - Gestion des tokens JWT

## Packages NuGet

- `Microsoft.AspNetCore.Authentication.JwtBearer` (v9.0.10)
- `Microsoft.AspNetCore.OpenApi` (v9.0.6)
- `Swashbuckle.AspNetCore` (v9.0.6)

## Configuration

### appsettings.json

```json
{
  "JwtSettings": {
    "SecretKey": "VotreCleSecreteTresLongueEtSecurisee123456!",
    "Issuer": "JwtAuthApi",
    "Audience": "JwtAuthApiClients",
    "ExpirationInMinutes": 60
  }
}
```

**Important:** En production, déplacez la `SecretKey` vers des variables d'environnement ou un coffre-fort de secrets (Azure Key Vault, AWS Secrets Manager, etc.).

## Endpoints disponibles

### Authentification (AuthController)

#### POST /api/auth/login
Authentifie un utilisateur et retourne un token JWT.

**Body:**
```json
{
  "username": "admin",
  "password": "password"
}
```

**Réponse (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiration": "2025-11-02T15:30:00Z"
}
```

**Credentials de test:**
- Username: `admin`
- Password: `password`

#### POST /api/auth/register
Enregistre un nouvel utilisateur (endpoint de démonstration).

**Body:**
```json
{
  "username": "nouveauUtilisateur",
  "password": "motDePasse123"
}
```

### Endpoints protégés (SecureController)

Tous les endpoints nécessitent un token JWT valide dans le header:
```
Authorization: Bearer {votre_token}
```

#### GET /api/secure/profile
Retourne le profil de l'utilisateur authentifié avec ses claims.

**Réponse (200 OK):**
```json
{
  "message": "Bienvenue admin!",
  "username": "admin",
  "userId": "guid-value",
  "claims": [
    { "type": "sub", "value": "admin" },
    { "type": "jti", "value": "guid" },
    { "type": "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "value": "admin" },
    { "type": "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "value": "Admin" }
  ]
}
```

#### GET /api/secure/data
Retourne une liste de données protégées.

**Réponse (200 OK):**
```json
[
  { "id": 1, "name": "Item 1", "description": "Description de l'item 1" },
  { "id": 2, "name": "Item 2", "description": "Description de l'item 2" },
  { "id": 3, "name": "Item 3", "description": "Description de l'item 3" }
]
```

#### GET /api/secure/admin
Endpoint réservé aux utilisateurs avec le rôle "Admin".

**Réponse (200 OK):**
```json
{
  "message": "Ceci est une ressource protégée pour les admins uniquement"
}
```

## Installation et démarrage

### Prérequis
- .NET 9.0 SDK ou supérieur
- Visual Studio 2022 ou VS Code (optionnel)

### Étapes

1. **Cloner le projet**
   ```bash
   git clone <url-du-repo>
   cd JwtAuthApi
   ```

2. **Restaurer les packages NuGet**
   ```bash
   dotnet restore
   ```

3. **Compiler le projet**
   ```bash
   dotnet build
   ```

4. **Lancer l'application**
   ```bash
   dotnet run
   ```

5. **Accéder à Swagger**
   - En mode développement: `https://localhost:7264/swagger`
   - L'application démarrera automatiquement Swagger UI

## Utilisation avec Swagger

1. Ouvrez Swagger UI dans votre navigateur
2. Utilisez l'endpoint `/api/auth/login` pour obtenir un token
3. Cliquez sur le bouton "Authorize" en haut de la page
4. Entrez `Bearer {votre_token}` dans le champ (remplacez {votre_token} par le token reçu)
5. Vous pouvez maintenant accéder aux endpoints protégés

## Utilisation avec cURL

### Se connecter
```bash
curl -X POST "https://localhost:7264/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"password"}'
```

### Accéder à un endpoint protégé
```bash
curl -X GET "https://localhost:7264/api/secure/profile" \
  -H "Authorization: Bearer {votre_token}"
```

## Structure du projet

```
JwtAuthApi/
├── Controllers/
│   ├── AuthController.cs       # Gestion de l'authentification
│   └── SecureController.cs     # Endpoints protégés
├── Models/
│   ├── LoginModel.cs           # Modèle de connexion
│   ├── TokenResponse.cs        # Modèle de réponse token
│   └── User.cs                 # Modèle utilisateur
├── Program.cs                  # Point d'entrée et configuration
├── appsettings.json           # Configuration de l'application
└── JwtAuthApi.csproj          # Fichier de projet
```

## Fonctionnalités de sécurité

- ✅ Authentification JWT
- ✅ Validation des tokens (issuer, audience, lifetime, signature)
- ✅ Protection des endpoints avec `[Authorize]`
- ✅ Gestion des rôles (Role-Based Access Control)
- ✅ HTTPS redirection

## Points d'amélioration pour la production

- [ ] Implémenter une vraie base de données (SQL Server, PostgreSQL, etc.)
- [ ] Utiliser Entity Framework Core pour la persistance
- [ ] Hasher les mots de passe (BCrypt, PBKDF2, Argon2)
- [ ] Déplacer les secrets vers des variables d'environnement
- [ ] Ajouter le refresh token pour renouveler les tokens expirés
- [ ] Implémenter la limitation de débit (rate limiting)
- [ ] Ajouter la journalisation (logging) avec Serilog
- [ ] Ajouter la validation des données avec FluentValidation
- [ ] Mettre en place des tests unitaires et d'intégration
- [ ] Configurer CORS pour les applications front-end

## Licence

Ce projet est à but éducatif et de démonstration.

## Auteur

Projet d'exemple - JWT Auth API

## Support

Pour toute question ou problème, veuillez ouvrir une issue sur le repository GitHub.
