1. **Certifique-se de que o .NET SDK esteja instalado**  
   Na máquina destino, instale o .NET SDK (pode ser baixado da [página oficial do .NET](https://dotnet.microsoft.com/download)). Assim, você terá à disposição os comandos de linha de comando como `dotnet restore`, `dotnet build` e `dotnet run`.

2. **Transfira o projeto para a nova máquina**

   - Se estiver utilizando um sistema de versionamento como o Git, basta clonar o repositório:
     ```bash
     git clone https://github.com/danlimax/back-end-econlsulta
     ```
   - Se não estiver usando Git, copie os arquivos e pastas do projeto (incluindo o arquivo de solução _.sln_ e os projetos _.csproj_).

3. **Restaure as dependências do projeto**  
   No terminal, navegue até a pasta do projeto (ou da solução) e execute:
   ```bash
   dotnet restore
   ```
4. **Compile e execute o projeto**  
    Após restaurar as dependências, compile o projeto com:
   ```bash
   dotnet build
   ```
   E, em seguida, execute-o (se for um aplicativo console, web ou outro tipo) usando:
   ```bash
   dotnet watch run
   ```
   **Para acessar o swagger**
   ```bash
   Acesse a rota do localhost sem o endpoint /swagger
   ```

**Para acessar o banco de dados**  
 Com o docker instalado.

```bash
use o comando docker compose up -d
```

As credênciais estão no arquivo docker compose.

**Comandos da migration**  
 Instale a ferramenta ef core https://learn.microsoft.com/pt-br/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli
 e rode os comandos abaixo.

```bash
dotnet ef migrations add NomeQuePreferir
dotnet dataase update
```
Lembre-se de fazer o passo anterior do docker compose para atualizar o banco de dados.
