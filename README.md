# az400-project-online

## Descrição

Esse código é um exemplo de um programa em C# que interage com o Microsoft Project Online usando a API do Graph Microsoft.

1.	A classe Program é a classe principal do programa. Ela contém o método `Main`, que é o ponto de entrada do programa.
2.	As primeiras linhas do código definem algumas variáveis estáticas que serão usadas ao longo do programa. Essas variáveis armazenam informações como o ID do locatário `tenant`, ID do cliente `client`, segredo do cliente `client secret`, escopo de acesso, URL do site do projeto e Id do Projeto `projectId`.
3.	O método `Main` é definido como async porque ele faz uso de operações assíncronas. Ele começa chamando o método `GetAccessToken` para obter um token de acesso. Em seguida, imprime o token de acesso na tela.
4.	O método `GetAccessToken` é responsável por obter o token de acesso necessário para autenticar as chamadas à API do Graph Microsoft. Ele faz uma solicitação `HTTP POST` para o endpoint de autenticação do Azure AD usando as informações de autenticação fornecidas (ID do cliente, segredo do cliente, escopo etc.). Se a solicitação for bem-sucedida, o método retorna o token de acesso. Caso contrário, ele lança uma exceção.
5.	O método `GetProjectDetails` é responsável por obter os detalhes de um projeto específico do Microsoft Project Online. Ele faz uma solicitação `HTTP GET` para o endpoint da API do Project Server, passando o token de acesso no cabeçalho de autorização. Se a solicitação for bem-sucedida, o método retorna os detalhes do projeto. Caso contrário, ele lança uma exceção.
6.	O método UpdateProjectDatails é responsável por atualizar os detalhes de um projeto específico do Microsoft Project Online. Ele faz uma solicitação `HTTP PATCH` para o endpoint da API do Project Server, passando o token de acesso no cabeçalho de autorização e os dados de atualização no corpo da solicitação. Se a solicitação for bem-sucedida, o método retorna a resposta da API. Caso contrário, ele lança uma exceção.
No geral, esse código demonstra como autenticar, obter detalhes e atualizar projetos no Microsoft Project Online usando a API do Graph Microsoft. Ele usa a classe `HttpClient` para fazer solicitações `HTTP` e manipula as respostas usando as bibliotecas `Newtonsoft.Json` para desserializar os dados retornados pela API.

## Configuração

Para configurar o projeto, você precisará atualizar as seguintes variáveis no método `GetAccessToken`:

- `clientId`: O ID do cliente para a aplicação registrada no Azure.
- `clientSecret`: O segredo do cliente para a aplicação registrada no Azure.
- `tenantId`: O ID do locatário do Azure AD.
- `scope`: O escopo da permissão solicitada.

## Execução

Para executar o projeto, siga estas etapas:

1. Clone o repositório para a sua máquina local.
2. Abra o projeto no Visual Studio.
3. Atualize as variáveis conforme descrito na seção de configuração.
4. Execute o projeto pressionando `F5` ou clicando em `Debug > Start Debugging`.

## Contribuição

...

## Licença

...
