## Sense Note Application

This repository contains the backend for the Sense Note application. It is built using ASP.NET for the main Web API, FastAPI for the AI service, PostgreSQL as the database, and Ollama for running local AI models. The entire system is containerized using Docker.

## Prerequisites

Before running the application, make sure you have the following installed:

- Docker Desktop (with Docker Compose support)
- Git

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/nguyenanhtu205/sense-note-backend.git
cd sense-note-backend
```

### 2. Configure Environment Variables

Rename the `.env.example` file to `.env`:

```bash
cp .env.example .env
```

Then update the values inside `.env`:

```env
# Database config
POSTGRES_USER=postgres
POSTGRES_PASSWORD=your_password
POSTGRES_DB=sense_note_db
DB_CONNECTION=Host=db;Port=5432;Database=sense_note_db;Username=postgres;Password=your_password

# JWT config
ISSUER=sense-note-api
AUDIENCE=dev-api
SIGNING_KEY=your_signing_key
```

Make sure to replace:
- `your_password` with a secure database password
- `your_signing_key` with a secure JWT signing key

### 3. Build the Docker Containers

Navigate to the directory containing `compose.yaml` and run:

```bash
docker compose build
```

### 4. Pull Required AI Model

After building, pull the required model into the Ollama container:

```bash
docker exec -it ollama ollama pull qwen2.5:1.5b
```

### 5. Start the Application

```bash
docker compose up
```

## Important Notes

### Port Conflicts

Ensure that the following ports are not being used by other processes on your machine:

- `5432` (PostgreSQL)
- `11434` (Ollama)
- `8000` (AI Service)
- `8080` (Web API)

Stop any services that are using these ports before running the application.

## Accessing the Application

### Web API (Scalar UI)

Once the application is running, you can access the Web API via:

```
http://localhost:8080
```

The endpoints are exposed using Scalar UI.

### AI Service (Swagger UI)

You can explore the AI service endpoints using Swagger at:

```
http://localhost:8000
```

### Database Connection

You can connect to the PostgreSQL database using:

**From host machine:**
```
Host=localhost;Port=5432;Database=sense_note_db;Username=postgres;Password=your_password
```

**From within Docker network:**
```
Host=db;Port=5432;Database=sense_note_db;Username=postgres;Password=your_password
```

## Stopping the Application

To stop all running containers:

```bash
docker compose down
```