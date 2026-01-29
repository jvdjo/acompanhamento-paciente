# PsicoNotes - Sistema de Acompanhamento de Pacientes

Aplicação web para psicólogos gerenciarem seus pacientes, com registro de sessões e anotações à mão livre.

## 🛠️ Tecnologias

- **Backend**: .NET 9 Web API, Entity Framework Core, PostgreSQL
- **Frontend**: React 18 + Vite, React Router
- **Autenticação**: JWT Bearer Token

## 📋 Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)
- [PostgreSQL 14+](https://www.postgresql.org/download/)

## 🚀 Como executar

### 1. Configurar o banco de dados

Certifique-se que o PostgreSQL está rodando. A connection string padrão é:

```
Host=localhost;Port=5432;Database=acompanhamento_paciente;Username=postgres;Password=postgres
```

Altere em `backend/AcompanhamentoPaciente.Api/appsettings.json` se necessário.

### 2. Executar o Backend

```powershell
cd backend/AcompanhamentoPaciente.Api
dotnet run
```

O backend rodará em `http://localhost:5143` e aplicará as migrations automaticamente.

### 3. Executar o Frontend

```powershell
cd frontend
npm install  # apenas na primeira vez
npm run dev
```

O frontend rodará em `http://localhost:5173`

## 🔐 Credenciais padrão

- **Email**: admin@clinica.com
- **Senha**: admin123

## 📁 Estrutura do Projeto

```
acompanhamento-paciente/
├── backend/
│   └── AcompanhamentoPaciente.Api/
│       ├── Controllers/        # Controllers da API
│       ├── Data/               # DbContext
│       ├── DTOs/               # Data Transfer Objects
│       ├── Models/             # Entidades
│       └── Services/           # Serviços (JWT)
├── frontend/
│   └── src/
│       ├── components/         # Componentes reutilizáveis
│       ├── contexts/           # React Contexts
│       ├── pages/              # Páginas da aplicação
│       └── services/           # API client
└── README.md
```

## 🎨 Funcionalidades

- ✅ Login com autenticação JWT
- ✅ Listagem de pacientes com busca
- ✅ Cadastro de novos pacientes
- ✅ Visualização de detalhes do paciente
- ✅ Gerenciamento de sessões por data
- ✅ Canvas para anotações à mão livre
- ✅ Interface moderna com tema escuro

## 📝 Licença

Este projeto está sob a licença MIT.
