# ERP Acadêmico — Documentação Mestre de Arquitetura

## Objetivo do Projeto

Construir um ERP Acadêmico funcional focado em:

* Controle de Estoque
* Vendas
* Orçamentos
* Dashboard Gerencial
* Relatórios
* Auditoria de Estoque
* Autenticação JWT

O projeto prioriza:

* simplicidade arquitetural;
* clareza de código;
* baixo consumo de memória;
* aprendizado progressivo;
* organização profissional.

---

# Stack Técnica

## Backend

* C#
* .NET 8
* ASP.NET Core Web API
* Entity Framework Core
* PostgreSQL
* JWT Authentication
* FluentValidation
* ResponseDto Pattern
---

# Estrutura do Projeto

```txt
ErpAcademico.Domain
ErpAcademico.Application
ErpAcademico.Infrastructure
ErpAcademico.WebApi
```

---

# Regras Arquiteturais

## Domain

Responsável por:

* entidades;
* enums;
* regras centrais de domínio.

### NÃO pode:

* acessar banco;
* usar Entity Framework;
* referenciar Infrastructure;
* depender da WebApi.

---

## Application

Responsável por:

* DTOs;
* contratos futuros;
* validações;
* objetos de transferência.

### NÃO deve:

* conter acesso direto ao banco;
* conter lógica pesada de infraestrutura.

---

## Infrastructure

Responsável por:

* AppDbContext;
* Entity Framework;
* Services;
* autenticação;
* acesso a dados;
* integrações.

### Contém:

* ProdutoService
* VendaService
* OrcamentoService
* DashboardService
* EstoqueService
* RelatorioService
* AuthService
* MovimentacaoEstoqueService

---

## WebApi

Responsável por:

* Controllers;
* Swagger;
* JWT middleware;
* configuração da aplicação;
* endpoints.

### Controllers devem ser:

* finos;
* simples;
* sem regra de negócio.

Toda lógica deve ficar:

* nas entidades;
* ou nos services.

---

# Convenções do Projeto

## Entidades

Sempre singular.

Exemplos:

```txt
Produto
Venda
ItemVenda
Orcamento
MovimentacaoEstoque
```

---

## DbSet

Sempre plural.

Exemplos:

```txt
Produtos
Vendas
ItensVenda
Orcamentos
MovimentacoesEstoque
```

---

## Controllers

Sempre plural.

Exemplos:

```txt
ProdutosController
VendasController
RelatoriosController
```

---

## DTOs

Separados por responsabilidade.

Exemplos:

```txt
CriarProdutoDto
RealizarVendaDto
DashboardDto
EntradaEstoqueDto
```

---

# Regras de Negócio

# Produto

Todo produto possui:

* PrecoVenda
* PrecoCusto
* QuantidadeAtual
* EstoqueMinimo
* EstoqueIdeal

---

# Estoque

## Venda

Venda:

* reduz estoque;
* gera movimentação;
* valida saldo disponível.

---

## Entrada de Estoque

Entrada:

* aumenta estoque;
* gera movimentação tipo Entrada.

---

## Ajuste de Estoque

Ajuste:

* corrige divergência física;
* pode ser positivo ou negativo;
* gera movimentação tipo Ajuste.

---

# Movimentação de Estoque

Toda alteração de estoque deve gerar histórico.

Tipos:

```txt
Entrada
Saida
Ajuste
Venda
```

---

# Orçamento

Orçamento:

* NÃO altera estoque;
* pode ser convertido em venda.

---

# Venda

Venda:

* possui itens;
* é transacional;
* baixa estoque;
* gera movimentações;
* calcula valor total.

---

# Dashboard

O dashboard deve fornecer:

* faturamento total;
* valor total em estoque;
* produtos abaixo do mínimo;
* produtos mais vendidos;
* produtos com maior estoque;
* últimas movimentações.

---

# Relatórios

Relatórios devem:

* usar filtros;
* usar IQueryable;
* evitar carregar dados desnecessários;
* priorizar performance.

---

# Autenticação

Sistema usa:

* JWT Bearer Token.

Endpoints protegidos usam:

```csharp
[Authorize]
```

---

# Padrões Proibidos Atualmente

NÃO introduzir:

* MediatR;
* CQRS complexo;
* Repository Pattern desnecessário;
* Unit of Work customizado;
* AutoMapper;
* microsserviços;
* abstrações excessivas.

Motivos:

* projeto educacional;
* notebook com pouca RAM;
* foco em aprendizado sólido;
* simplicidade e clareza.

---

# Diretrizes de Código

## Prioridades

1. Clareza
2. Simplicidade
3. Legibilidade
4. Performance
5. Escalabilidade gradual

---

# Convenções de Services

Services:

* usam AppDbContext diretamente;
* usam async/await;
* validam regras de negócio;
* não devem conter lógica de apresentação.

---

# Convenções de Controllers

Controllers:

* apenas recebem requests;
* chamam services;
* retornam IActionResult.

Controllers NÃO devem:

* acessar banco diretamente;
* conter regra de negócio;
* conter cálculos.

---

# Convenções de Entidades

Entidades:

* possuem construtores;
* controlam integridade;
* possuem métodos de domínio.

Exemplos:

```txt
BaixarEstoque()
AdicionarEstoque()
AjustarEstoque()
```

---

# Banco de Dados

Banco:

* PostgreSQL.

ORM:

* Entity Framework Core.

Precisão decimal padrão:

```txt
18,2
```

---

# Estrutura de Pastas Recomendada

```txt
Domain
 ├── Entities
 └── Enums

Application
 ├── DTOs
 └── Validators

Infrastructure
 ├── Data
 ├── Services
 └── Migrations

WebApi
 ├── Controllers
 ├── Middleware
 └── Configurations
```

---

# Endpoints Existentes

## Auth

```txt
POST /api/auth/register
POST /api/auth/login
```

---

## Produtos

```txt
GET /api/produtos
POST /api/produtos
```

---

## Vendas

```txt
POST /api/vendas
```

---

## Orçamentos

```txt
POST /api/orcamentos
POST /api/orcamentos/{id}/converter
```

---

## Estoque

```txt
POST /api/estoque/entrada
POST /api/estoque/ajuste
```

---

## Dashboard

```txt
GET /api/dashboard
GET /api/dashboard/detalhado
```

---

## Movimentações

```txt
GET /api/movimentacoes
```

---

## Relatórios

```txt
GET /api/relatorios/vendas
```

---

# Estado Atual do Projeto

O sistema atualmente possui:

* autenticação JWT;
* CRUD de produtos;
* vendas transacionais;
* orçamentos;
* movimentação de estoque;
* dashboard;
* relatórios;
* auditoria básica.
* Paginação e Filtros de Busca (ILike)
* Padronização de resposta
* Middleware de exceção customizado

---

# Próximas Etapas Recomendadas

## Curto Prazo

* Exclusão lógica
* Soft delete
* Logs
* Melhorias de validação
* Repository Pattern
---

## Médio Prazo

* Clientes
* Fornecedores
* Compras
* Upload de imagens
* Permissões por perfil
* Inventário completo

---

## Longo Prazo

* Front-end React
* Docker
* Deploy cloud
* Cache
* Redis
* Testes automatizados
* CI/CD
* Multiempresa

---

# Instruções para Outras IAs

Antes de sugerir código:

1. Ler este documento completamente.
2. Respeitar a arquitetura atual.
3. NÃO adicionar patterns complexos.
4. NÃO reestruturar o projeto.
5. NÃO mudar stack.
6. NÃO adicionar abstrações desnecessárias.
7. Seguir naming conventions existentes.
8. Priorizar simplicidade.

---

# Filosofia do Projeto

Este ERP segue:

```txt
Clean Architecture simplificada
```

com foco em:

* aprendizado sólido;
* organização profissional;
* escalabilidade gradual;
* clareza arquitetural.

O projeto NÃO busca:

* hiper abstração;
* arquitetura enterprise exagerada;
* complexidade prematura.

---

# Regra Final

Sempre preferir:

```txt
código simples e correto
```

em vez de:

```txt
arquitetura excessivamente complexa
```
