# GestionTareasAPI

API RESTful para gestión de tareas internas con integración de API externa y análisis de sentimiento con ML.NET.

## Stack tecnológico
- ASP.NET Core Web API (.NET 8)
- Entity Framework Core + SQLite
- ML.NET 3.0.1
- Swagger / OpenAPI

---

## Pasos para ejecutar localmente

### 1. Clonar el repositorio
```bash
git clone https://github.com/oescobarv78/GestionTareasAPI.git
cd GestionTareasAPI
```

### 2. Restaurar dependencias
```bash
dotnet restore
```

### 3. Aplicar migraciones y ejecutar
```bash
dotnet run
```

La API estará disponible en: http://localhost:5172

Swagger UI: http://localhost:5172/index.html

---

## Comandos de migración

```bash
# Crear una nueva migración
dotnet ef migrations add NombreMigracion

# Aplicar migraciones a la base de datos
dotnet ef database update

# Las migraciones también se aplican automáticamente al ejecutar dotnet run
```

---

## Endpoints implementados

### Tareas (CRUD)
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | /api/tareas | Obtener todas las tareas |
| GET | /api/tareas/{id} | Obtener tarea por ID |
| POST | /api/tareas | Crear nueva tarea |
| PUT | /api/tareas/{id} | Actualizar tarea |
| DELETE | /api/tareas/{id} | Eliminar tarea |

### Filtros disponibles en GET /api/tareas
| Parámetro | Ejemplo | Descripción |
|-----------|---------|-------------|
| estado | ?estado=Pendiente | Filtra por estado |
| prioridad | ?prioridad=Alta | Filtra por prioridad |
| fechaInicio | ?fechaInicio=2026-05-01 | Fecha inicio rango |
| fechaFin | ?fechaFin=2026-05-31 | Fecha fin rango |

### Tareas Externas
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | /api/tareas-externas | Obtener todas las tareas externas |
| GET | /api/tareas-externas/{id} | Obtener tarea externa por ID |

### ML.NET
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| POST | /api/ml/sentimiento | Analizar sentimiento de un comentario |

---

## Ejemplo de uso de la API externa

El endpoint GET /api/tareas-externas consume la API pública:
https://jsonplaceholder.typicode.com/todos

### Request
### Response
```json
{
  "externalId": 1,
  "titulo": "delectus aut autem",
  "completado": false
}
```

Si la API externa no responde devuelve error 503.
Si el ID no existe devuelve error 404.

---

## Modelo ML.NET — Análisis de Sentimiento

Se implementó la Opción A: Análisis de Sentimiento usando clasificación binaria.

### Algoritmo usado
SdcaLogisticRegression (Stochastic Dual Coordinate Ascent)

### Dataset
Archivo: MLModels/datos_sentimiento.csv
- 35 frases etiquetadas manualmente
- 15 frases positivas (true)
- 20 frases negativas (false)

### Ejemplo de uso

Request:
```json
{
  "comentario": "La tarea fue completada correctamente y el sistema funciona bien"
}
```

Response:
```json
{
  "comentario": "La tarea fue completada correctamente y el sistema funciona bien",
  "sentimiento": "Positivo"
}
```

### Pipeline del modelo
1. FeaturizeText: convierte el texto en vectores numéricos
2. SdcaLogisticRegression: clasifica como Positivo o Negativo

---

## Estructura del proyecto
## Ramas y Pull Requests

| Rama | Descripción |
|------|-------------|
| feature/api-tareas | CRUD de tareas con EF Core y SQLite |
| feature/filtros-tareas | Filtros por estado, prioridad y fechas |
| feature/api-externa-todos | Consumo de jsonplaceholder.typicode.com |
| feature/mlnet-basico | Análisis de sentimiento con ML.NET |

