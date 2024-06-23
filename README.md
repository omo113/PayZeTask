# PayZe Microservices

This repository contains the Docker Compose setup for the PayZe microservices application. This setup includes multiple services such as payment, orders, and identity APIs, along with their dependencies (PostgreSQL databases, RabbitMQ, and Redis).

## Prerequisites

Before you begin, ensure you have the following installed on your machine:

- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)

## Setup Instructions

1. **Clone the Repository**

    ```sh
    git clone https://github.com/your-username/your-repo.git
    cd your-repo
    ```

2. **Environment Variables**

    Ensure you have a `.env` file in the root of your project directory with the following content:

    ```env
    DOCKER_REGISTRY=
    ```

3. **Build and Start the Services**

    Use Docker Compose to build and start the services:

    ```sh
    docker-compose up --build
    ```

    This command will build the Docker images for each service and start the containers.

## Running the Application Locally

After running the `docker-compose up --build` command, the following services will be available:

- **PayZe Payment API**
  - URL: `http://localhost:8082`
- **PayZe Orders API**
  - URL: `http://localhost:8084`
- **PayZe Identity API**
  - URL: `http://localhost:8080`

### Service Details

1. **PayZe Payment API**

    - **Build Context:** `./PayZe.Payment.Api`
    - **Dockerfile:** `PayZe.Payment.Api/Dockerfile`
    - **Ports:** `8082:8080`
    - **Environment Variables:**
        - `ASPNETCORE_ENVIRONMENT=Development`
        - `settings__RabbitMqHost=payze-rabbitmq`
        - `settings__RabbitMqUser=admin`
        - `settings__RabbitMqPassword=admin`
        - `settings__DatabaseConnection=Host=payment-db;Database=payze-payment;Username=admin;Password=admin;Pooling=true;`
        - `settings__RedisConnectionString=payze-redis:6379`
        - `settings__IdentityUrl=payze.identity.api`

2. **PayZe Orders API**

    - **Build Context:** `./PayZe.Orders.Api`
    - **Dockerfile:** `PayZe.Orders.Api/Dockerfile`
    - **Ports:** `8084:8080`
    - **Environment Variables:**
        - `ASPNETCORE_ENVIRONMENT=Development`
        - `settings__RabbitMqHost=payze-rabbitmq`
        - `settings__RabbitMqUser=admin`
        - `settings__RabbitMqPassword=admin`
        - `settings__DatabaseConnection=Host=orders-db;Database=payze-orders;Username=admin;Password=admin;Pooling=true;`
        - `settings__RedisConnectionString=payze-redis:6379`
        - `settings__IdentityUrl=payze.identity.api`

3. **PayZe Identity API**

    - **Build Context:** `./PayZe.Identity.Api`
    - **Dockerfile:** `PayZe.Identity.Api/Dockerfile`
    - **Ports:** `8080:8080`
    - **Environment Variables:**
        - `ASPNETCORE_ENVIRONMENT=Development`
        - `settings__RabbitMqHost=payze-rabbitmq`
        - `settings__RabbitMqUser=admin`
        - `settings__RabbitMqPassword=admin`
        - `settings__DatabaseConnection=Host=identity-db;Database=payze-identity;Username=admin;Password=admin;Pooling=true;`
        - `settings__RedisConnectionString=payze-redis:6379`

4. **Identity Database**

    - **Image:** `postgres:latest`
    - **Ports:** `5432:5432`
    - **Environment Variables:**
        - `POSTGRES_DB=payze-identity`
        - `POSTGRES_USER=admin`
        - `POSTGRES_PASSWORD=admin`
    - **Volumes:**
        - `identity_db_data:/var/lib/postgresql/data`

5. **Orders Database**

    - **Image:** `postgres:latest`
    - **Ports:** `5441:5432`
    - **Environment Variables:**
        - `POSTGRES_DB=mydatabase`
        - `POSTGRES_USER=admin`
        - `POSTGRES_PASSWORD=admin`
    - **Volumes:**
        - `orders_db_data:/var/lib/postgresql/data`

6. **Payment Database**

    - **Image:** `postgres:latest`
    - **Ports:** `5433:5432`
    - **Environment Variables:**
        - `POSTGRES_DB=mydatabase`
        - `POSTGRES_USER=admin`
        - `POSTGRES_PASSWORD=admin`
    - **Volumes:**
        - `payment_db_data:/var/lib/postgresql/data`

7. **RabbitMQ**

    - **Image:** `docker.io/bitnami/rabbitmq:3.10`
    - **Ports:** 
        - `4369:4369`
        - `5551:5551`
        - `5552:5552`
        - `5672:5672`
        - `25672:25672`
        - `15672:15672`
    - **Environment Variables:**
        - `RABBITMQ_SECURE_PASSWORD=yes`
        - `RABBITMQ_USERNAME=admin`
        - `RABBITMQ_PASSWORD=admin`
        - `RABBITMQ_LOGS=-`
    - **Volumes:**
        - `rabbitmq_data:/bitnami/rabbitmq/mnesia`

8. **Redis**

    - **Image:** `redis:latest`
    - **Ports:** `6379:6379`
    - **Command:** `redis-server --appendonly yes`
    - **Volumes:**
        - `redis_data:/data`

### Network and Volumes

- **Network:** `backend`
- **Volumes:**
    - `identity_db_data`
    - `orders_db_data`
    - `payment_db_data`
    - `rabbitmq_data`
    - `redis_data`

## Stopping the Services

To stop and remove the containers, networks, and volumes defined in the `docker-compose.yml` file, run:

```sh
docker-compose down
