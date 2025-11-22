# AllGoals API

## Projeto: API RESTful de Gestão de Metas e Recompensas

Este projeto foi desenvolvido para o **Global Solution** da disciplina **Advanced Business Development with .NET**. O objetivo é fornecer uma **API RESTful** robusta e escalável para gamificação de tarefas, utilizando **.NET 8** e **Oracle**.

## 👨‍💻 Integrantes

- Felipe Levy Stephens Fidelix - RM556426
- Jennifer Kaori Suzuki - RM554661
- Pedro Henrique Jorge de Paula - RM558833

---

## 🎯 Proposta do Projeto

O **AllGoals** é uma plataforma de gamificação onde usuários podem definir metas, ganhar experiência (XP) e moedas virtuais ao completá-las, e trocar essas moedas por itens em uma loja virtual.

A solução foi desenhada seguindo os princípios da **Clean Architecture** para garantir desacoplamento e testabilidade.

---

## 🏗️ Arquitetura e Estrutura

O projeto segue a **Clean Architecture**, dividido em 4 camadas principais:

- **AllGoals.API**: Entrada da aplicação, Controllers, Configuração de DI e Middlewares.
- **AllGoals.Application**: Regras de negócio, Serviços, DTOs e Interfaces de Serviço.
- **AllGoals.Domain**: Núcleo do projeto. Entidades, Value Objects (Email) e Interfaces de Repositório.
- **AllGoals.Infrastructure**: Implementação de acesso a dados (EF Core), Repositórios e Migrations.

---

## 🚀 Tecnologias e Práticas Utilizadas

- **.NET 8** (C#)
- **Entity Framework Core** (Code First)
- **Oracle** (Banco de Dados)
- **xUnit + Moq** (Testes Unitários)
- **Serilog** (Logging Estruturado)
- **OpenTelemetry** (Tracing Distribuído)

### Diferenciais Implementados:
- **HATEOAS**: Links de navegação hipermídia nas respostas.
- **API Versioning**: Suporte a múltiplas versões (`v1` e `v2`).
- **Paginação**: Endpoints de listagem otimizados.
- **Health Checks**: Monitoramento de saúde da API e do Banco (`/health`).

---
## ❓​ Como rodar o projeto

```bash
# 1. Clonar o repositório
git clone https://github.com/jenniesuzuki/AllGoals.git
cd AllGoals

# 2. Restaurar e dar build no projeto
dotnet restore
dotnet build

# 3. Rodar a API
dotnet run --project AllGoals.API
```

---

## 🧪​ Execução dos testes:

Para executar os testes, use o seguinte comando:

```bash
dotnet test
```

---

## 🔗 Links do Deploy

- **Swagger UI (Documentação)**: [https://allgoalsapp.azurewebsites.net/swagger](https://allgoalsapp.azurewebsites.net/swagger)
- **Health Check**: [https://allgoalsapp.azurewebsites.net/health](https://allgoalsapp.azurewebsites.net/health)

---

## 📝 Endpoints Principais

### 1. Usuários (Users)
*Suporta Versionamento (v1 e v2)*

| Verbo  | Rota (v1) | Rota (v2) | Descrição |
| --- | --- | --- | --- |
| GET | `/api/v1/user` | `/api/v2/user` | Lista usuários (Paginado) |
| GET | `/api/v1/user/{id}` | `/api/v2/user/{id}` | Busca usuário por ID |
| POST | `/api/v1/user` | `/api/v2/user` | Cria um novo usuário |
| PUT | `/api/v1/user/{id}` | `/api/v2/user/{id}` | Atualiza dados do usuário |
| POST | **N/A** | `/api/v2/user/{id}/promote` | **(V2)** Promove a Admin |
| POST | **N/A** | `/api/v2/user/{id}/revoke` | **(V2)** Revoga Admin |

**Exemplo POST:**
```json
{
  "nome": "Jennifer Suzuki",
  "email": "jennifer@fiap.com.br"
}
```

### 2. Metas (Goals)
| Verbo | Rota | Descrição |
| :--- | :--- | :--- |
| `GET` | `/api/v1/goals` | Lista todas as metas disponíveis (Paginado) |
| `GET` | `/api/v1/goals/{id}` | Consulta detalhes de uma meta |
| `POST` | `/api/v1/goals` | Cria uma nova meta |
| `PUT` | `/api/v1/goals/{id}` | Atualiza a meta |
| `DELETE` | `/api/v1/goals/{id}` | Remove a meta |

**Exemplo POST:**
```json
{
  "titulo": "Vender 100 perfumes",
  "descricao": "Vender 100 perfumes da nova linha",
  "xp": 500,
  "moedas": 100
}
```

### 3. Itens da Loja (StoreItems)
| Verbo | Rota | Descrição |
| :--- | :--- | :--- |
| `GET` | `/api/v1/storeitems` | Lista o catálogo da loja (Paginado) |
| `GET` | `/api/v1/storeitems/{id}` | Detalhes do item |
| `POST` | `/api/v1/storeitems` | Adiciona um novo item à loja |
| `PUT` | `/api/v1/storeitems/{id}` | Atualiza preço ou descrição do item |
| `DELETE` | `/api/v1/storeitems/{id}` | Remove item do catálogo |

**Exemplo POST:**
```json
{
  "nome": "Voucher iFood",
  "descricao": "Vale refeição de R$ 50,00",
  "valor": 50
}
```
