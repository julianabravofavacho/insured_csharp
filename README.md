# WebApi_Coris

## Descrição

**WebApi_Coris** é uma API RESTful desenvolvida em C# e .NET 8.0 que expõe endpoints para autenticação, gerenciamento de usuários e segurados.  
Utiliza JWT para segurança, EF Core para acesso a dados e FluentValidation para validação de entrada.

---

## Funcionalidades

- **Autenticação**  
  - Login / Logout / Refresh Token via `AuthenticationController`  
  - Geração e validação de JWT (`JwtTokenService`)  
- **CRUD de Usuários** (`UserController`)  
- **CRUD de Segurados** (`InsuredController`)  
- **DTOs** para requests e responses (`Models/Dtos/*.cs`)  
- **Validações** com FluentValidation (`Validators/`)  
- **Métricas Prometheus** via App.Metrics  
- **Documentação Swagger** automática  
- **Integração QuickBooks** com `IppDotNetSdkForQuickBooksApiV3`  

---

## Tecnologias

- .NET 8.0 (net8.0)  
- C# 11  
- Entity Framework Core 9.0.4  
- SQL Server  
- JWT Bearer Authentication (`Microsoft.AspNetCore.Authentication.JwtBearer`)  
- FluentValidation.AspNetCore  
- Swashbuckle.AspNetCore (Swagger)  
- App.Metrics.Formatters.Prometheus  
- QuickBooks SDK (`IppDotNetSdkForQuickBooksApiV3`)  

---

## Estrutura do Projeto

```
WebApi_Coris/
├── Controllers/
│   ├── AuthenticationController.cs
│   ├── UserController.cs
│   └── InsuredController.cs
├── DataContext/                  # DbContext e configurações EF
├── Infrastructure/               # Configurações transientes, extensões
├── Migrations/                   # Migrações do EF Core
├── Models/
│   ├── Dtos/
│   │   ├── CreateUserDto.cs
│   │   ├── LoginRequestDto.cs
│   │   └── InsuredDto.cs
│   ├── ApiResponse.cs
│   └── UserModel.cs
├── Service/
│   ├── Abstractions/             # Interfaces (IUserService, ITokenService…)
│   ├── AuthenticationService/    # Implementações de autenticação
│   ├── UserService/              # Lógica de negócio de usuário
│   └── InsuredService/           # Lógica de negócio de segurados
├── Validators/                   # Regras de validação FluentValidation
├── appsettings.json
├── Program.cs                    # Pipeline de middleware e configuração
├── WebApi_Coris.csproj           # Definição de SDK e pacotes
└── .gitignore
```

---

## Configuração

1. **Clone** o repositório:  
   ```bash
   git clone https://github.com/seu-usuario/WebApi_Coris.git
   cd WebApi_Coris
   ```

2. **Ajuste** as strings de conexão e JWT em `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=insured;User Id=sa;Password=SenhaForte123;"
     },
     "JwtSettings": {
       "Key": "seu_segredo_jwt_aqui",
       "Issuer": "WebApi_Coris",
       "Audience": "WebApi_Coris"
     }
   }
   ```

3. **Instale** pacotes e ferramentas:
   ```bash
   dotnet tool install --global dotnet-ef
   dotnet restore
   ```

4. **Crie** e **aplique** as migrações:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

---

## Executando

```bash
dotnet run
```

Por padrão a API escuta em `https://localhost:5001` e `http://localhost:5000`.  
Acesse a documentação interativa em `https://localhost:5001/swagger`.

---

## Endpoints Principais

| Método | Rota                     | Descrição                      |
|:------:|:------------------------:|--------------------------------|
| POST   | `/api/auth/login`        | Autentica usuário e retorna JWT|
| POST   | `/api/auth/register`     | Cria novo usuário              |
| POST   | `/api/auth/refresh`      | Atualiza token JWT             |
| GET    | `/api/users`             | Lista todos os usuários        |
| POST   | `/api/users`             | Cria usuário                   |
| PUT    | `/api/users/{id}`        | Atualiza usuário               |
| DELETE | `/api/users/{id}`        | Remove usuário                 |
| GET    | `/api/insureds`          | Lista todos os segurados       |
| POST   | `/api/insureds`          | Cria segurado                  |
| PUT    | `/api/insureds/{id}`     | Atualiza segurado              |
| DELETE | `/api/insureds/{id}`     | Remove segurado                |

---

## Contribuição

1. Fork este repositório  
2. Crie uma branch: `git checkout -b feature/nome-da-feature`  
3. Faça commit: `git commit -m "feat: descrição da feature"`  
4. Envie para o repositório remoto e abra um Pull Request  

---

## Licença

Este projeto está licenciado sob a [MIT License](LICENSE).
