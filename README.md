# Aportes en Línea Backend

Este es el backend de la aplicación **Aportes en Línea**, desarrollado en **.NET Core**.

## 📌 Tecnologías utilizadas
- **C# (.NET Core 8.0)**
- **Entity Framework Core**
- **xUnit** (para pruebas unitarias)
- **AutoMapper**
- **MoQ** (para pruebas unitarias)

## 📋 Requisitos previos
Antes de comenzar, asegúrate de tener instalado:
- **.NET SDK 8.0**
- **SQL Server** (para la base de datos)
- **Visual Studio 2022** o **Visual Studio Code**

## 🚀 Configuración del proyecto

### 1️⃣ Clonar el repositorio
```sh
git clone https://github.com/tu-usuario/aportes-en-linea-backend.git
cd aportes-en-linea-backend
```

### 2️⃣ Configurar la base de datos
En `appsettings.json`, ajusta la cadena de conexión:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=TU_SERVIDOR;Database=AEL_DB;User Id=TU_USUARIO;Password=TU_CONTRASEÑA;"
}
```

### 3️⃣ Aplicar migraciones
```sh
dotnet ef database update
```

## ▶️ Ejecutar la aplicación
```sh
dotnet run
```

La API estará disponible en `http://localhost:5000`.

## 🛠️ Ejecutar pruebas
Para ejecutar las pruebas unitarias con **xUnit**, usa el siguiente comando:
```sh
dotnet test
```

## 📁 Estructura del proyecto
```
📦 aportes-en-linea-backend
├── 📂 AEL.Application     # Lógica de negocio y servicios
├── 📂 AEL.Domain          # Entidades del dominio
├── 📂 AEL.Infrastructure  # Acceso a datos y persistencia
├── 📂 AEL.Tests           # Pruebas unitarias con xUnit
├── 📂 AEL.WebAPI          # API REST con controladores
├── appsettings.json       # Configuración de la aplicación
├── README.md              # Documentación del proyecto
```
