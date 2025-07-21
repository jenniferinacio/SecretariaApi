
# SecretariaApi

API desenvolvida em ASP.NET Core com o objetivo de gerenciar funcionalidades relacionadas a uma secretaria (educacional, administrativa, etc.).

## ✅ Requisitos

Antes de começar, verifique se você tem os seguintes itens instalados:

- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/pt-br/) com suporte para ASP.NET e desenvolvimento web
- Git (opcional, para controle de versão)
- Banco de dados configurado (detalhes abaixo)

## 🚀 Instalação

1. Clone ou extraia este repositório:
   ```bash
   git clone https://github.com/jenniferinacio/SecretariaApi.git
   ```
   ou apenas extraia o arquivo `.zip`.

2. Abra a solução no Visual Studio:
   - Arquivo → Abrir → Projeto/Solução
   - Selecione `SecretariaApi.sln`

## ⚙️ Configuração

A aplicação utiliza o arquivo `appsettings.json` para configuração do ambiente.

Verifique o arquivo:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SecretariaDb;Trusted_Connection=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

> 🔒 **Importante:** Ajuste a string de conexão com base no seu ambiente (servidor SQL, usuário, senha, etc.).

Se necessário, crie o banco de dados manualmente ou configure a aplicação para aplicar as `migrations`.

## 🗄️ Banco de Dados

Para criar as tabelas necessárias, execute o script SQL disponível no arquivo `dump.sql` antes de rodar o projeto.

- Esse projeto utiliza Sql Server.

### Como executar o script `dump.sql`:

1. Abra seu cliente de banco de dados ou terminal.
2. Conecte-se ao banco de dados configurado na string de conexão.
3. Execute script.

## ▶️ Execução

Para rodar o projeto em modo de desenvolvimento:

1. No Visual Studio, selecione `SecretariaApi` como projeto de inicialização.
2. No topo da janela, selecione o perfil IIS Express no menu suspenso de execução.
3. Pressione **F5** ou clique em **Iniciar Debugging**.
4. A API será iniciada em um endereço como:
   ```
   https://localhost:44352/swagger/index.html
   ```

Você pode testar os endpoints com ferramentas como:

- [Postman](https://www.postman.com/)

## 📁 Estrutura Básica do Projeto

```
SecretariaApi/
│
├── appsettings.json          # Configurações principais
├── Program.cs                # Ponto de entrada da aplicação
├── SecretariaApi.csproj      # Arquivo de projeto
├── Properties/
│   └── launchSettings.json   # Configurações de perfil de execução
├── Controllers/              # Controladores da API
├── Models/                   # Modelos de dados
└── ...                       # Outros arquivos e serviços
```

## 💡 Observações

- Este projeto utiliza o .NET 8.0.

Desenvolvido com ❤️ em ASP.NET Core.
