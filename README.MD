## Arquitetura e Design Patterns Aplicados
1. DDD
2. Micro Services
3. N-tier
4. Generics
5. Singleton
6. Encapsulamento
7. Reutilização de código
8. Regras de legibilidade / manutenibilidade de código

## Tecnologias / Linguagens / Ferramentas
1. Asp.Net Core
2. Web Api (REST)
3. Asp.Net MVC
4. Razor
5. Entity Framework
6. PostgreSQL

## Manual:
1. Criar users (superuser) / databases 

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
