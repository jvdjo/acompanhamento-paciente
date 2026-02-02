# PsicoNotes - Sistema de Acompanhamento de Pacientes

Sistema para psicólogos gerenciarem pacientes e sessões de terapia.

## 🛠️ Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js 20+](https://nodejs.org/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

## 🚀 Execução Rápida

### Modo Debug (Desenvolvimento)
```bash
# Windows - duplo clique ou execute:
run-debug.bat
```

### Modo Release (Produção)
```bash
# Windows - duplo clique ou execute:
run-release.bat
```

### Parar a Aplicação
```bash
stop.bat
```

---

## 📋 Execução Manual

### 1. Iniciar o Banco de Dados
```bash
docker-compose up -d
```

### 2. Backend

#### Modo Debug
```bash
cd backend/AcompanhamentoPaciente.Api
dotnet run --launch-profile Development
```

#### Modo Release
```bash
cd backend/AcompanhamentoPaciente.Api
dotnet run --launch-profile Production -c Release
```

### 3. Frontend

#### Modo Debug (com hot reload)
```bash
cd frontend
npm install
npm run dev
```

#### Modo Release (build otimizado)
```bash
cd frontend
npm install
npm run build
npm run preview
```

---

## 🌐 URLs

| Ambiente | Backend | Frontend |
|----------|---------|----------|
| Debug    | http://localhost:5008 | http://localhost:5173 |
| Release  | http://localhost:5008 | http://localhost:4173 |

---

## 🔐 Autenticação

O sistema suporta duas formas de login:

### Login Tradicional (Email/Senha)
- **Email:** admin@clinica.com
- **Senha:** admin123

### Login com Google OAuth2

Para habilitar o login com Google, você precisa configurar as credenciais OAuth2:

1. Acesse o [Google Cloud Console](https://console.cloud.google.com/)
2. Crie um novo projeto ou selecione um existente
3. Vá em **APIs e Serviços** > **Credenciais**
4. Clique em **Criar Credenciais** > **ID do cliente OAuth**
5. Selecione **Aplicativo da Web**
6. Adicione a URI de redirecionamento autorizada:
   - `http://localhost:5008/signin-google`
7. Copie o **Client ID** e **Client Secret**
8. Configure no arquivo `backend/AcompanhamentoPaciente.Api/appsettings.Development.json`:

```json
{
  "Google": {
    "ClientId": "SEU_GOOGLE_CLIENT_ID",
    "ClientSecret": "SEU_GOOGLE_CLIENT_SECRET"
  }
}
```

---

## 📁 Estrutura do Projeto

```
acompanhamento-paciente/
├── backend/
│   ├── AcompanhamentoPaciente.Api/          # API Web
│   ├── AcompanhamentoPaciente.Application/  # Serviços e DTOs
│   ├── AcompanhamentoPaciente.Domain/       # Entidades
│   └── AcompanhamentoPaciente.Infrastructure/ # Repositórios e EF
├── frontend/                                # React + Vite
├── docker-compose.yml                       # PostgreSQL
├── run-debug.bat                            # Script modo debug
├── run-release.bat                          # Script modo release
└── stop.bat                                 # Parar containers
```

---

## ⚙️ Configurações por Ambiente

### Backend
- `appsettings.Development.json` - Debug (logging detalhado)
- `appsettings.Production.json` - Release (logging mínimo)

### Frontend
- `.env.development` - Variáveis para desenvolvimento
- `.env.production` - Variáveis para produção

---

## 🧪 Testes

```bash
# Backend
cd backend
dotnet test

# Frontend
cd frontend
npm run lint
```

---

## 📝 Funcionalidades

- ✅ Autenticação JWT
- ✅ **Login com Google OAuth2**
- ✅ Gerenciamento de Pacientes
- ✅ Registro de Sessões
- ✅ Notas de Texto nas Sessões
- ✅ Canvas de Desenho para Anotações
- ✅ Interface Responsiva (Dark Mode)
