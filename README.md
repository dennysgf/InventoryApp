# InventoryApp

Aplicación web fullstack para la **gestión de inventarios** desarrollada como parte de la evaluación técnica.  
Incluye **arquitectura de microservicios en .NET 8** para backend y **Angular** para frontend, con soporte para productos y transacciones de inventario.

---

## Arquitectura del Proyecto

- **Backend (.NET 8 / C#)**
  - Microservicio **ProductService**: gestión de productos (CRUD).
  - Microservicio **TransactionService**: gestión de transacciones (compras/ventas).
  - Comunicación síncrona entre microservicios mediante APIs REST.
  - Persistencia con **SQL Server**.

- **Frontend (Angular)**
  - Vista de **Productos**: creación, edición, listado y eliminación.
  - Vista de **Transacciones**: registro de compras/ventas, historial, filtros por fechas y tipo.
  - Diseño con **SCSS**, enrutamiento con `app.routes.ts`.

- **Colección Postman**
  - Ubicada en la raíz: `InventoryApp API.postman_collection.json`.
  - Incluye todos los endpoints de ProductService y TransactionService.

---

## Requisitos

- **Backend**
  - .NET 8 SDK
  - PostgreSQL (local o en contenedor Docker)

- **Frontend**
  - Node.js 18+
  - Angular CLI 17+

- **DevOps**
  - Docker & Docker Compose (opcional para levantar todo junto)

---

## Ejecución del Backend

1. Clonar el repositorio:
   ```bash
   git clone https://github.com/dennysgf/InventoryApp.git
   cd InventoryApp
   ```

2. Restaurar dependencias:
   ```bash
   dotnet restore
   ```

3. Crear base de datos ejecutando el script:
   ```bash
   sqlcmd -S localhost -U sa -P your_password -i database.sql
   ```

4. Ejecutar servicios:
   ```bash
   cd ProductService
   dotnet run --urls http://localhost:8080

   cd ../TransactionService
   dotnet run --urls http://localhost:8082
   ```

---

## Ejecución del Frontend

1. Entrar en carpeta `frontend`:
   ```bash
   cd frontend
   ```

2. Instalar dependencias:
   ```bash
   npm install
   ```

3. Levantar servidor Angular:
   ```bash
   ng serve -o
   ```

   Por defecto corre en `http://localhost:4200`

---

## Ejecución con Docker

1. Construir imágenes:
   ```bash
   docker-compose build
   ```

2. Levantar servicios:
   ```bash
   docker-compose up
   ```

   - ProductService: [http://localhost:8080/swagger](http://localhost:8080/swagger)  
   - TransactionService: [http://localhost:8082/swagger](http://localhost:8082/swagger)  
   - Frontend: [http://localhost:4200](http://localhost:4200)

---

## Endpoints Principales

### ProductService
- `GET /api/products`
- `GET /api/products/{id}`
- `POST /api/products`
- `PUT /api/products/{id}`
- `DELETE /api/products/{id}`

### TransactionService
- `GET /api/transactions`
- `GET /api/transactions/{id}`
- `POST /api/transactions`
- `PUT /api/transactions/{id}`
- `DELETE /api/transactions/{id}`
- `GET /api/transactions/history?productId=&from=&to=&type=`

 Ver colección de Postman: `InventoryApp API.postman_collection.json`

---

##  Estructura de Carpetas

### Backend
```
InventoryApp/
 ├── ProductService/
 │   ├── Controllers/
 │   ├── Models/
 │   ├── Repositories/
 │   ├── Services/
 │   └── Dockerfile
 ├── TransactionService/
 │   ├── Controllers/
 │   ├── Models/
 │   ├── Repositories/
 │   ├── Services/
 │   └── Dockerfile
```

### Frontend
```
frontend/
 ├── src/app/
 │   ├── core/
 │   ├── features/
 │   │   └── products/
 │   │   └── transactions/
 │   └── layout/dashboard/
 ├── assets/
 └── Dockerfile
```

---

## Evidencias

- Listado dinámico de productos y transacciones con paginación  
- Pantalla de creación y edición de productos  
- Pantalla de creación y edición de transacciones  
- Filtros dinámicos por fechas y tipo de transacción  

---

## Evaluación cumplida

- Gestión de productos (CRUD completo)  
- Registro de transacciones (compras/ventas con validación de stock)  
- Filtros dinámicos (fechas y tipo)  
- Comunicación entre microservicios vía APIs REST  
- Frontend Angular con rutas, validaciones y feedback al usuario  
- Evidencias y colección Postman incluidas  
