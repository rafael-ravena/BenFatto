## Arquitetura e Design Patterns Aplicados
1. DDD
2. Micro Services
3. N-tier
4. Generics
5. Facade
6. Singleton
7. Encapsulamento
8. Reutilização de código
9. Regras de legibilidade / manutenibilidade de código

## Tecnologias / Linguagens / Ferramentas
1. Asp.Net Core
2. Web Api (REST)
3. Asp.Net MVC
4. Razor
5. Entity Framework
6. PostgreSQL
7. User registration 
    - Sem utilizar o MS-Identity:
        > O Uso de Session é muito comum para quem programa na plataforma MS por pelo menos 15 anos.
        > 
        > Porém, o uso de Session em Net Core está atrelado ao controller, dificultando a implementação do Pattern Facade (encapsulamento do acesso para simplificar e evitar bugs em runtime [¹ Bugs Runtime Bugs Reference](#bugs)). Essa implementação ficou muito nebulosa, com configuração muito complicada, com entendimento complexo, apresentando muitos erros, e sem garantias de que conseguiria fazê-lo em tempo.
        > 
        > Quando você cria um projeto web sem se preparar para utilizar o Identity, incluir depois é bem complicado e com muitos passos.
        > 
        > Não existe a opção de alterar a autenticação, então seria necessário  remover todo o projeto, refazê-lo, importar tudo que já havia sido feito.
        > 
        > Ainda que o package do Identity gere bastante código, diminuindo o esforço (menos programação, alguma customização), a opção por registrar sem MS-Identity com cookies acabou ficando muito mais leve e objetiva.
8. Bootstrap
9. JQuery
10. Font-awesome
11. Toastr

## Manual:
1. Criar users (superuser) / databases 

### Users
    - Execute o arquivo "/SQL/Users.sql"

### App database
```json
    {
        "DataBaseConfiguration": 
        {
            "Host": "localhost",
            "Port": 2021,
            "Database": "BenFatto-FrontEnd",
            "Username": "BenFattoUsr-FrontEnd",
            "Password": "B3nF4tt0#2021"
        }
    }
```
### Api database
```json
{
    "DataBaseConfiguration": 
    {
        "Host": "localhost",
        "Port": 2021,
        "Database": "BenFatto-CLF",
        "Username": "BenFattoUsr-CLF",
        "Password": "B3nF4tt0#2021"
    }
}
```
2. Criar Tabelas
    - Execute os arquivos "/SQL/Api-schema.sql" & "/SQL/App-schema.sql"
3. Inserir "bogus data"
    1. Restaure os pacotes NUGET
    2. Inicie uma instância de "HandyMan" console application;
    3. Ao ser prompted, digite o comando "file" (case insensitive)
    4. Se desejar, acompanhe o processo por Debug
    5. Ao completar, uma mensagem será exibida, e 5 arquivos foram devidamente criados, importados (com linhas válidas e inválidas) e persistidos.
    6. Se desejar, repita a operação para alimentar o DB com mais massa de dados.
4. Iniciar a aplicação
5. Acesse o item "Files"
    - Cada linha representa um arquivo importado ("File").
    - Não é possível  Incluir, Editar ou Excluir "Files" manualmente. 
    - Novos "Files" são incluídos somente mediante Importação.
    - Aplique os filtros (não exclusivos mutuamente)
    - Ações:
        - Ver detalhes do "File"
            - Acesse os detalhes de um "File" qualquer.
        - Ver inconsistências na importação
            - Acesse as inconsistências de um arquivo qualquer.
                - Aplique os filtros (não exclusivos mutuamente)
                - Exclua uma inconsistência ("Row with error")
                - Altere uma inconsistência
                    - Note que apenas é possível alterar a linha originalmente contida no arquivo.
                    - Caso a alteração seja sufuciente para que a linha se torne um registro, isso ocorrerá automaticamente.
        - Ver linhas importadas com sucesso
            - Acesse as linhas importadas de um arquivo qualquer
                - Aplique os filtros (não exclusivos mutuamente)
                - Exclua uma linha ("Row")
                - Altere uma linha
                - Inclua uma linha válida
                - Inclua uma linha com inconsistência

## Melhorias
- Implementar OAuth para previnir DOS Attacks, Melhorar a segurança e restringir acesso
- Implementar controle de usuários (acesso, gerenciamento) -> já está planejado e "projetado"
- Implementar APIs de outros tipos de arquivos (fixed lenght / Char-separated)
- Implementar telas de gerenciamento dos outros tipos de arquivo
- Todos os forms de Edição, inclusão e exclusão enviados via ajax

# Bugs
- É bem complicado implementar Controle de Usuários com Session e o Design Pattern "Facade" (extraindo para uma classe que encapsula o controle das chaves da sessão), para evitar bugs em runtime.
    - por exemplo, ao atribuir objeto à sessão "Cary",  geraria um NullReferenceException ao tentar utilizar o valor da sessão "Cart", uma vez que as chaves são diferentes. Identificar esse bug demanda muito tempo, muita atenção e uma grande familiaridade do dev com a implementação como um todo.
