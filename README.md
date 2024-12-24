# Cities Manager

**Cities Manager** é uma API desenvolvida com **ASP.NET Core Web API** para gerenciar informações de cidades. Este projeto foi criado para aplicar os conceitos aprendidos em **ASP.NET Core Web API**.


## 🛠️ Funcionalidades

- **Cadastro de Cidades**: Crie novos registros de cidades com informações como nome, estado e população.
- **Listagem de Cidades**: Consulte todas as cidades cadastradas ou filtre por critérios específicos.
- **Atualização de Cidades**: Altere informações de uma cidade já cadastrada.
- **Remoção de Cidades**: Exclua cidades do sistema.
- **Paginação e Ordenação**: Suporte a paginação e ordenação nos endpoints de listagem.
- **Validação de Dados**: Garantia de consistência nos dados enviados para a API.


## 🚀 Tecnologias Utilizadas

- **ASP.NET Core Web API**: Framework principal para desenvolvimento da API.
- **Entity Framework Core**: Para gerenciamento de banco de dados e ORM.
- **SQL Server**: Banco de dados leve para armazenamento local.
- **Swagger/OpenAPI**: Documentação interativa da API.


## 🔧 Configuração do Ambiente

Siga os passos abaixo para executar a API localmente:

### Pré-requisitos

- **.NET 8 SDK** 
- **Banco de dados SQL Server**
- **IDE**: [Rider](https://www.jetbrains.com/rider/) ou [Visual Studio Code](https://code.visualstudio.com/).

### Passos
1. Clone este repositório:
   ```bash
   git clone https://github.com/murilonicemento/cities-manager
   ```

2. Configure a string de conexão no arquivo `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Data Source=citiesmanager.db"
     }
   }
   ```

3. Aplique as migrações para configurar o banco de dados:
   ```bash
   dotnet ef database update
   ```

4. Execute o projeto:
   ```bash
   dotnet run
   ```

5. Acesse a documentação Swagger da API: `http://localhost:5265/swagger`


## 🌐 Endpoints Disponíveis

Acesse a documentação interativa em `/swagger` para detalhes completos sobre os endpoints. Exemplos de alguns disponíveis:

- `GET /api/cities` - Lista todas as cidades.
- `GET /api/cities/{id}` - Obtém detalhes de uma cidade específica.
- `POST /api/cities` - Cria uma nova cidade.
- `PUT /api/cities/{id}` - Atualiza uma cidade existente.
- `DELETE /api/cities/{id}` - Remove uma cidade do banco de dados.


## 🤝 Contribuição

Contribuições são bem-vindas! Siga os passos abaixo para colaborar:

1. Fork o repositório
2. Crie uma branch com sua feature: `git checkout -b minha-feature`
3. Faça commit das alterações: `git commit -m 'Adicionei uma nova feature'`
4. Envie a branch: `git push origin minha-feature`
5. Abra um Pull Request


## 📝 Licença

Este projeto está licenciado sob a [MIT License](LICENSE).
