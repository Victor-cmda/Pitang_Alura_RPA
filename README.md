# Pitang Alura RPA

Este projeto combina automação RPA (Robotic Process Automation) com Internet das Coisas (IoT) para coletar e gerenciar informações sobre cursos da plataforma Alura. A aplicação implementa automação via Selenium para buscar cursos e fornece uma API robusta para interação.

## Funcionalidades

- **Automação RPA**: Realiza web scraping para obter informações de cursos da Alura.
- **Listagem de Cursos**: Armazena e lista os cursos capturados pelo RPA.
- **Persistência de Dados**: Utiliza SQLite para armazenar os cursos capturados.
- **Injeção de Dependências**: Usa IoC (Inversão de Controle) para gerenciar serviços de maneira eficiente.

## Tecnologias Utilizadas

- **ASP.NET Core**: Backend para a API.
- **Entity Framework Core**: ORM para integração com SQLite.
- **Selenium WebDriver**: Utilizado para automação web.
- **IoC (Inversão de Controle)**: Facilita a gestão de dependências na aplicação.
- **Swagger**: Ferramenta de documentação e teste da API.

## Estrutura do Projeto

1. **Application**: Camada responsável pela lógica de negócios e serviços de aplicação.
    - `ICourseService` e `IRpaService`: Interfaces de serviços para cursos e automação.
    - `CourseService` e `RpaService`: Implementações dos serviços de cursos e automação RPA.

2. **Domain**: Define as entidades e interfaces de repositórios.
    - `Course`: Entidade que representa os cursos.
    - `ICourseRepository`: Interface para o repositório de cursos.

3. **Infrastructure**: Responsável pelo acesso a dados e integração com o banco de dados.
    - `AppDbContext`: Contexto do Entity Framework.
    - `CourseRepository`: Implementação de repositório para a entidade `Course`.

4. **IoC (Inversão de Controle)**: 
    - `DependencyInjection.cs`: Configuração de serviços e injeção de dependências para registrar e configurar o uso de serviços como `IRpaService`, `ICourseService`, e o repositório de cursos.

5. **Presentation**: Controladores e endpoints expostos pela API.
    - `RpaController`: Controlador que gerencia a automação e consulta de cursos.
    - `Program.cs`: Configurações da API e setup de serviços.

## Como Executar

### Pré-requisitos

- .NET 8.0 SDK
- SQLite
- ChromeDriver (para Selenium)
- Dispositivos IoT (opcional)

### Passos

1. Clone o repositório:
   ```bash
   git clone https://github.com/Victor-cmda/Pitang_Alura_RPA.git
   ```

2. Navegue até o diretório do projeto:
   ```bash
   cd Pitang_Alura_RPA
   ```

3. Restaure as dependências:
   ```bash
   dotnet restore
   ```

4. Execute a aplicação:
   ```bash
   dotnet run --project Presentation
   ```

A API estará disponível em: `https://localhost:5016/swagger`

## Endpoints

### POST `/api/rpa/execute-service`
Executa a automação para buscar cursos com base em um termo de busca.

#### Exemplo de body:
```json
{
  "filter": "Python"
}
```

### GET `/api/rpa/courses`
Retorna todos os cursos capturados pelo RPA.

## Autor

Desenvolvido por Victor Hugo Somavilla.
