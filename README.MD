## Arquitetura e Design Patterns Aplicados
1. DDD
2. Micro Services
3. N-tier
4. Generics
5. Facade
6. Singleton
7. Encapsulamento
8. Reutilização de código
9. Regras de legibilidade / manutenibilidade de código [<sup>1 Regras de programação segura</sup>](#safe-programming)

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
        > Porém, o uso de Session em Net Core está atrelado ao controller, dificultando a implementação do Pattern Facade (ver [<sup>2 Bugs em Runtime para session e outros</sup>](#bugs-em-runtime)). Essa implementação ficou muito nebulosa, com configuração muito complicada, com entendimento complexo, apresentando muitos erros, e sem garantias de que conseguiria fazê-lo em tempo.
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
- Implementar as APIs de outros tipos de arquivos (fixed lenght / Char-separated)
- Implementar telas de gerenciamento dos outros tipos de arquivo
___
### Safe Programming (Programação Segura)
#### Booleano X resultado
Em algumas linguagens não tipadas (javascript, por exemplo) ou fracamente tipadas (VB.Net sem Option Explicid e Option Strict), ao fazer uma condicional baseada numa comparação, podemos fazer uma condicional paseada numa atribuição.
``` Javascript
var strTxt = "It's me! Mario!";
if(strTxt = "It's me! Luigi!") {
    window.alert("It was, indeed, Luigi!");
}
```
Uma vez que você não consegue atribuir à uma constante, ou um readonly attribute, a identificação do bug fica mais simples. A situação com variáveis booleanas é mais fácil de vislumbrar o problema na prática.
``` C#
string any = "";
bool isTheSame = (string.empty = any); // a static readonly field cannot be assigned to
```
####Preventing Exception Hell
A utilização de blocos Try/Catch proporcionam tratamentos de erro de uma forma tipada, previsível e extremamente robusta.

Entretanto, a má utilização desse recurso pode levar a diversos problemas e práticas tenebrosas.
Dentre os inúmeros problemas, os principais que gostaria de apontar são:
- Exception Swalloing: quando o programa acaba "ocultando" uma excessão disparada em blocos de try/catch aninhados.
- Large resource consumption: um bloco try/catch é muito mais custoso em termos de processamento e memória que uma consulta / verificação prévia.
- Stacktrace loss: o stacktrace da Exception original é perdido quando se usa Catch(Exception ex) { throw ex; }
- Exceptions are not business logic: Excessões devem ser utilizadas para previnir encerramento abrupto de um programa, e não para programar regras de negócio.
- Dealing with specific Exceptions: No processo de criação de um arquivo texto, por exemplo, temos uma Exception se parte do caminho não existe. Se não há espaço em disco disponível, temos outra Exception. Se não há permissão de escrita, outra Exception. Tratar da mesma forma todas as diferentes situações pode gerar um bug de difícil identificação.

### Bugs em Runtime
#### Situação Key&lt;string&gt;/Value&lt;object&gt; pair

Considere que em certa rotina de um programa, você atribui à um cookie, chamado "UsuarioLogado", um valor inteiro.
``` C#
Context.Response.Cookies["UsuarioLogado"] = 1;
```

Em outra parte do programa, você tenta acessar este conteúdo, mas por um erro de digitação, consome o cookie "UsuaroLogado".
``` C#
int usrLgd = ParseInt(Context.Response.Cookies["UsuaroLogado"].Value.ToString()); //<<< NullReferenceException thrown here
```

Esse bug só seria "identificado" em runtime. O dev identificaria o NullReferenceException, com algum esforço perceberia a ausência do I, corrigiria e pronto!

Porém, se o typo está no caso da atribuição, e a Exception é disparada na leitura, e a identificação e correção do Bug se tornam, consequentemente, mais complexas numa escala geométrica.

Agora considere, por exemplo, uma lista qualquer sendo armazenada numa Sessão "FirstList"; Nela estão armazenados os primeiros requests do usuário ao ingressar, nessa ocasião, no sistema.

Existe também, contudo, uma lista de Primeiros colocados em alguns concursos. O desenvolvedor dessa feature, sem conhecer a outra feature, chamou a sessão que vai armazenar essa lista de "FirstList". Como a atribuição e a leitura acontecem em requests diferentes, a exception disparada é sobre a conversão dos objetos. É necessário identificar a origem do bug e corrigi-lo... Boa sorte para o dev.
##### Aplicação do Design Pattern "Facade"
Facade (lê-se "Fássêid": fəˈsɑːd) é um pattern de encapsulamento de funcionalidades de 1 ou vários subsistemas para simplificar o acesso e consumo deste sistema em sua complexidade.
O corpo humano, por exemplo, apresenta diversos exemplos desse pattern: O Sistema Digestivo tem como facade a boca e o ânus. Os olhos são facades para conversões de radiação em sinapses para a interpretação e armazenamento em memória pelo cérebro. (...)
Na aplicação para a questão do Key/Value pair, é uma simplificação deste. 
Com o pattern, é possivel aplicar encapsulamento, tipagem, unicidade, entre outros. É possível, inclusive, mensurar o tamanho em bytes que estes objetos estão / deverão ocupar em memória.
