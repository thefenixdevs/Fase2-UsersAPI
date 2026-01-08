# UsersAPI

## Visão Geral

A **UsersAPI** é o microsserviço responsável pelo gerenciamento de usuários na plataforma **FIAP Cloud Games (FCG)**. Suas responsabilidades incluem o cadastro de usuários, autenticação com geração de token JWT e autorização para acesso aos demais serviços do ecossistema.

Este serviço faz parte da refatoração de uma arquitetura monolítica para uma **arquitetura de microsserviços orientada a eventos**, conforme definido no Tech Challenge da Fase 2.

---

## Responsabilidades do Serviço

* Cadastro de novos usuários
* Autenticação de usuários
* Geração de tokens JWT
* Publicação de eventos de domínio relacionados a usuários

### Evento Publicado

* **UserCreatedEvent**
  Publicado após o cadastro bem-sucedido de um usuário. Este evento é consumido pelo microsserviço de **Notificações**, que realiza o envio (simulado) do e-mail de boas-vindas.

---

## Arquitetura e Tecnologias

* **Plataforma:** .NET 9.0
* **Arquitetura:** Microsserviços orientados a eventos
* **Mensageria:** RabbitMQ
* **Biblioteca de Mensageria:** MassTransit
* **Banco de Dados:** PostgreSQL
* **Containerização:** Docker (multi-stage build)
* **Orquestração:** Kubernetes (Docker Desktop)

---

## Comunicação Assíncrona

A comunicação entre os microsserviços ocorre de forma assíncrona utilizando RabbitMQ como broker de mensagens. O MassTransit é responsável por abstrair a comunicação, garantindo desacoplamento, confiabilidade e facilidade de manutenção.

Fluxo relacionado a este serviço:

1. O usuário é cadastrado via endpoint da UsersAPI.
2. A UsersAPI publica o evento **UserCreatedEvent** no broker.
3. O NotificationsAPI consome o evento e executa o envio do e-mail de boas-vindas.

---

## Estrutura do Projeto

```
UsersAPI
├── src
│   ├── UsersAPI
│   │   ├── Controllers
│   │   ├── Application
│   │   ├── Domain
│   │   ├── Infrastructure
│   │   └── Program.cs
├── k8s
│   ├── deployment.yaml
│   ├── service.yaml
│   ├── configmap.yaml
│   └── secret.yaml
├── Dockerfile
├── docker-compose.yml
└── README.md
```

---

## Variáveis de Ambiente

As configurações do serviço são gerenciadas via **ConfigMaps** e **Secrets** no Kubernetes.

### ConfigMap

* `RABBITMQ_HOST`
* `RABBITMQ_QUEUE_USER_CREATED`
* `JWT_ISSUER`
* `JWT_AUDIENCE`

### Secrets

* `POSTGRES_CONNECTION_STRING`
* `JWT_SECRET_KEY`

---

## Execução Local com Docker

Para executar o serviço localmente utilizando Docker:

```bash
docker build -t users-api .
docker run -p 5000:8080 users-api
```

Ou, quando integrado ao ambiente completo:

```bash
docker-compose up
```

---

## Deploy no Kubernetes (Docker Desktop)

Certifique-se de que o Kubernetes do Docker Desktop esteja habilitado.

Aplicar os manifestos:

```bash
kubectl apply -f k8s/
```

Verificar se o pod está em execução:

```bash
kubectl get pods
```

---

## Considerações Acadêmicas

Este microsserviço foi desenvolvido com foco educacional, aplicando conceitos de:

* Separação de responsabilidades
* Arquitetura orientada a eventos
* Comunicação assíncrona
* Containerização e orquestração

Atendendo integralmente aos requisitos propostos no **Tech Challenge – Fase 2** da FIAP.
