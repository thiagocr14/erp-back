# ERP Acadêmico — Documentação Mestre de Arquitetura

# Objetivo do Projeto

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

# Filosofia do Projeto

Este ERP segue:

```txt
Clean Architecture pragmática
```

com foco em:

* aprendizado sólido;
* organização profissional;
* escalabilidade gradual;
* clareza arquitetural;
* simplicidade operacional.

O projeto NÃO busca:

* hiper abstração;
* arquitetura enterprise exagerada;
* complexidade prematura;
* patterns desnecessários.

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

# Estrutura de Pastas Recomendada

```txt
Domain
 ├── Entities
 ├── Enums
 └── Exceptions

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

# Regras Arquiteturais

# Domain

Responsável por:

* entidades;
* enums;
* exceções de domínio;
* regras centrais de negócio.

## NÃO pode:

* acessar banco;
* usar Entity Framework;
* referenciar Infrastructure;
* depender da WebApi.

---

# Application

Responsável por:

* DTOs;
* validações;
* contratos;
* objetos de transferência.

## NÃO deve:

* acessar banco diretamente;
* conter infraestrutura;
* conter lógica pesada de persistência.

---

# Infrastructure

Responsável por:

* AppDbContext;
* Entity Framework;
* Services;
* autenticação;
* acesso a dados;
* integrações.

## Contém:

* ProdutoService
* VendaService
* OrcamentoService
* DashboardService
* EstoqueService
* RelatorioService
* AuthService

---

# WebApi

Responsável por:

* Controllers;
* Swagger;
* configuração da aplicação;
* autenticação JWT;
* middlewares;
* endpoints.

## Controllers devem ser:

* finos;
* simples;
* sem regra de negócio.

Toda lógica deve ficar:

* nas entidades;
* ou nos services.

---

# Convenções do Projeto

# Entidades

Sempre singular.

## Exemplos

```txt
Produto
Venda
ItemVenda
Orcamento
MovimentacaoEstoque
```

---

# DbSet

Sempre plural.

## Exemplos

```txt
Produtos
Vendas
ItensVenda
Orcamentos
MovimentacoesEstoque
```

---

# Controllers

Sempre plural.

## Exemplos

```txt
ProdutosController
VendasController
RelatoriosController
```

---

# DTOs

Separados por responsabilidade.

## Exemplos

```txt
CriarProdutoDto
RealizarVendaDto
DashboardDto
EntradaEstoqueDto
FiltroMovimentacaoDto
```

---

# Convenções Obrigatórias

* Toda entrada deve usar DTO
* Toda saída deve usar ResponseDto
* Toda listagem deve ser paginada
* Nunca expor entidades diretamente
* Controllers não acessam DbContext
* Toda alteração de estoque deve gerar movimentação
* Toda venda deve ser transacional
* Toda validação deve usar FluentValidation
* Toda listagem deve permitir filtros quando necessário

---

# Response Pattern

Toda resposta da API deve seguir:

```json
{
  "sucesso": true,
  "mensagem": "Operação realizada.",
  "dados": {},
  "erros": []
}
```

---

# Regras do Response Pattern

* Controllers nunca retornam entidades diretamente
* Exceptions devem passar pelo middleware global
* Erros devem retornar lista padronizada
* Responses de listagem devem usar paginação
* Responses devem ser previsíveis para o frontend

---

# Paginação

Toda listagem deve utilizar:

* Pagina
* TamanhoPagina

## Estrutura padrão

```json
{
  "pagina": 1,
  "tamanhoPagina": 10,
  "totalRegistros": 100,
  "totalPaginas": 10,
  "dados": []
}
```

---

# Filtros

Filtros devem:

* utilizar IQueryable;
* evitar carregamento desnecessário;
* usar paginação obrigatoriamente;
* utilizar ILike para busca textual no PostgreSQL.

---

# Validação

Validações devem ser implementadas com:

```txt
FluentValidation
```

## Estrutura

```txt
Application
 └── Validators
```

## Evitar

* validações inline em controllers;
* validações repetidas em services;
* validações duplicadas.

---

# Middleware Global de Erros

A API possui middleware global para:

* tratamento de exceptions;
* padronização de erros;
* logging;
* respostas HTTP padronizadas.

## Tipos tratados

* NegocioException
* KeyNotFoundException
* UnauthorizedAccessException
* Exception genérica

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

## Tipos

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
* priorizar performance;
* retornar dados enxutos.

---

# Autenticação

Sistema utiliza:

```txt
JWT Bearer Token
```

Endpoints protegidos usam:

```csharp
[Authorize]
```

---

# Diretrizes de Performance

Preferir:

* AsNoTracking() em consultas;
* IQueryable para filtros;
* paginação em listagens;
* Select() para projeções;
* carregamento mínimo necessário.

Evitar:

* ToList() prematuro;
* Include() desnecessário;
* carregar entidades completas sem necessidade;
* consultas sem paginação.

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

## Controllers NÃO devem:

* acessar banco diretamente;
* conter regra de negócio;
* conter cálculos;
* conter validação complexa.

---

# Convenções de Entidades

Entidades:

* possuem construtores;
* controlam integridade;
* possuem métodos de domínio.

## Exemplos

```txt
BaixarEstoque()
AdicionarEstoque()
AjustarEstoque()
DebitarEstoque()
```

---

# Banco de Dados

Banco:

```txt
PostgreSQL
```

ORM:

```txt
Entity Framework Core
```

Precisão decimal padrão:

```txt
18,2
```

---

# Convenções de Migrations

## Criar migration

```powershell
dotnet ef migrations add NomeMigration --project ErpAcademico.Infrastructure --startup-project ErpAcademico.WebApi
```

## Aplicar migration

```powershell
dotnet ef database update --project ErpAcademico.Infrastructure --startup-project ErpAcademico.WebApi
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

## Motivos

* projeto educacional;
* notebook com pouca RAM;
* foco em aprendizado sólido;
* simplicidade e clareza;
* evitar overengineering.

---

# Diretrizes de Código

## Prioridades

1. Clareza
2. Simplicidade
3. Legibilidade
4. Performance
5. Escalabilidade gradual

---

# Endpoints Existentes

# Auth

```txt
POST /api/auth/register
POST /api/auth/login
```

---

# Produtos

```txt
GET /api/produtos
POST /api/produtos
GET /api/produtos/filtro
```

---

# Vendas

```txt
POST /api/vendas
```

---

# Orçamentos

```txt
POST /api/orcamentos
POST /api/orcamentos/{id}/converter
```

---

# Estoque

```txt
POST /api/estoque/entrada
POST /api/estoque/ajuste
GET /api/estoque/movimentacoes
```

---

# Dashboard

```txt
GET /api/dashboard
GET /api/dashboard/detalhado
```

---

# Relatórios

```txt
GET /api/relatorios/fechamento-dia
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
* auditoria básica;
* paginação;
* filtros com ILike;
* ResponseDto;
* middleware global;
* FluentValidation.

---

# Próximas Etapas Recomendadas

# Curto Prazo

* Soft delete
* Logs
* Melhorias de validação
* Exclusão lógica
* Ajustes de auditoria
* Melhorias de exceptions
* Padronização final de responses

---

# Médio Prazo

* Clientes
* Fornecedores
* Compras
* Upload de imagens
* Permissões por perfil
* Inventário completo

---

# Longo Prazo

* Front-end React
* Docker
* Deploy cloud
* Redis
* Cache
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
9. Evitar overengineering.
10. Respeitar ResponseDto e paginação.

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
