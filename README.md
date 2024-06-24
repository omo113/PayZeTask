# PayZe Microservices

This repository contains the Docker Compose setup for the PayZe microservices application. This setup includes multiple services such as payment, orders, and identity APIs, along with their dependencies (PostgreSQL databases, RabbitMQ, and Redis).

## Prerequisites

Before you begin, ensure you have the following installed on your machine:

- [Docker](https://www.docker.com/get-started)

## Setup Instructions

1. **Clone the Repository**

    ```sh
    git clone https://github.com/your-username/your-repo.git
    cd your-repo
    ```


2. **Build and Start the Services**

    Use Docker Compose to build and start the services:

    ```sh
    docker-compose up -d --build
    ```

    This command will build the Docker images for each service and start the containers.

## Running the Application Locally

After running the `docker-compose up --build` command, the following services will be available:

- **PayZe Payment API**
  - URL: `http://localhost:8082/swagger/index.html`
- **PayZe Orders API**
  - URL: `http://localhost:8084/swagger/index.html`
- **PayZe Identity API**
  - URL: `http://localhost:8080/swagger/index.html`

## Stopping the Services

To stop and remove the containers, networks, and volumes defined in the `docker-compose.yml` file, run:

```sh
docker-compose down
