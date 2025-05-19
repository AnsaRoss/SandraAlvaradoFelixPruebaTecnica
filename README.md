API de Halterofilia - Juegos Olímpicos
Este proyecto es una API construida con .NET 7 Web API y SQL Server, desarrollada para gestionar los resultados de la disciplina de halterofilia en los Juegos Olímpicos.
Permite registrar intentos por modalidad (arranque y envión), obtener los mejores resultados por deportista, paginar los resultados y registrar los logs en archivo o base de datos.

Estructura del Proyecto
- Controllers/: Controladores de la API (Deportista, Levantamiento, Login, Resultados)
- Models/: Modelos y clases de respuesta
- Utils/: Funciones auxiliares (JWT, Logging, Validaciones, etc.)
- Logs/: Carpeta donde se almacenan los logs si LogToFile está activado
- BdJuegosOlimpicos.sql: Script SQL para crear la base de datos
- appsettings.json: Configuración del proyecto (conexión DB, JWT, logs, etc.)

Requisitos Previos
- .NET 7 SDK
- SQL Server 2018+
- Postman o Swagger UI para pruebas
- Editor de código (Visual Studio, VS Code, Rider)

⚙️ Configuración del Proyecto
1. Clona el repositorio
   git clone https://github.com/AnsaRoss/SandraAlvaradoFelixPruebaTecnica.git
   cd tu-repo
2. Restaurar la base de datos
   Ejecuta el script BdJuegosOlimpicos.sql en tu SQL Server local.
3. Verifica el archivo appsettings.json
4. Asegúrate de que las cadenas de conexión coincidan con tu entorno local:
   "LogToDatabase": {
      "Enabled": false,
      "ConnectionString": "Data source=CHORLITA\\SQLEXPRESS;initial catalog=BdJuegosOlimpicos;Trusted_Connection=true",
      "TableName": "tbAuditoria"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "conexion": "Data source=CHORLITA\\SQLEXPRESS;initial catalog=BdJuegosOlimpicos;Trusted_Connection=true"
  },
5. Ejecutar el proyecto
6. Accede a Swagger UI
   Una vez ejecutada, accede a: https://localhost:7048/swagger
7. Ejecutar el endpoint login y obtener el token, utilizar las siguientes credenciales
   {
      "usuario": "easy",
      "contrasena": "12345"
    }
9. Autenticación JWT
   Agrega este token en la cabecera de cada solicitud:
10. Endpoints
   POST	/api/login	Login con usuario y clave
   POST	/api/deportista/crear	Crear deportista
   POST	/api/levantamiento/crear	Registrar intento de levantamiento
   GET	/api/resultados/listar	Obtener resultados con paginación
   GET	/api/levantamiento/intentos	Ver intentos por deportista
11. Logs
    El sistema permite registrar eventos en archivo o base de datos, configurable desde appsettings.json.
    "Logging": {
      "LogToFile": { "Enabled": true },
      "LogToDatabase": { "Enabled": false }
    }
    Los logs por archivo se guardan en logs/JuegosOlimpicosLog-YYYYMMDD.txt.
