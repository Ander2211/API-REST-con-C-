# ApiC#

Aplicación de ejemplo en ASP.NET Core que expone un CRUD simple para `Producto`.

**Qué es**
- API REST minimal/Controller que gestiona productos con propiedades `Id`, `Nombre`, `Precio`, `Stock`.
- Implementa versionado de API (v1 y v2) usando segmentos de URL: `/api/v1/producto` y `/api/v2/producto`.

**Estructura básica**
- `Program.cs` - configuración de la aplicación y registro de `ApiVersioning`.
- `controllers/ProdcutoController.cs` - controlador versión 1 (`Controllers.V1`).
- `controllers/ProdcutoController v2.cs` - controlador versión 2 (`Controllers.V2`).
- `model/Producto.cs` - modelo de datos.

Requisitos
- .NET SDK compatible (el proyecto está dirigido a `net9.0` según la carpeta `bin`). Instale .NET 9 SDK desde https://dotnet.microsoft.com si corresponde.
- Git (opcional)

Instalación y ejecución (paso a paso)

1) Clonar o copiar el repositorio y entrar al directorio del proyecto:

```bash
cd C:\Users\Ander\Desktop\React\ApiC#
```

2) (Opcional) Si aún no has añadido el paquete de versionado de API, añade el paquete NuGet:

```bash
dotnet add package Microsoft.AspNetCore.Mvc.Versioning
```

3) Restaurar dependencias:

```bash
dotnet restore
```

4) Ejecutar la aplicación:

```bash
dotnet run
```

Observa la salida en consola: ahí verás las URLs donde está escuchando la app (por ejemplo `https://localhost:5001`).

Uso / Endpoints

- Listar productos (v1):

```bash
curl https://localhost:5001/api/v1/producto -k
```

- Listar productos (v2):

```bash
curl https://localhost:5001/api/v2/producto -k
```

- Obtener producto por id (ej. id=1):

```bash
curl https://localhost:5001/api/v1/producto/1 -k
```

- Crear producto (POST):

```bash
curl -X POST https://localhost:5001/api/v2/producto -H "Content-Type: application/json" -d '{"Nombre":"Nuevo","Precio":9.9,"Stock":10}' -k
```

Notas sobre versionado
- `Program.cs` está configurado para aceptar versiones por segmento de URL, cabecera `x-api-version` y query string `api-version`.
- Sin embargo, los controladores actuales usan rutas con segmento de versión (`api/v{version:apiVersion}/[controller]`), por lo que la forma práctica de invocar es mediante la URL con versión (`/api/v1/...` o `/api/v2/...`).

OpenAPI / Swagger
- La app puede exponer Swagger UI y los documentos OpenAPI por versión.

- Paquetes recomendados (si no están instalados):

```bash
dotnet add package Microsoft.AspNetCore.Mvc.Versioning
dotnet add package Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer
dotnet add package Swashbuckle.AspNetCore
```

- Después de `dotnet run`, abre la URL que indica la consola (ej. `https://localhost:5001`).

- Endpoints útiles:

	- Interfaz Swagger UI:

		- `https://localhost:5001/swagger` (UI que lista los documentos disponibles)

	- Documentos JSON OpenAPI por versión:

		- `https://localhost:5001/swagger/v1/swagger.json`
		- `https://localhost:5001/swagger/v2/swagger.json`

- Ejemplos (entorno local, omitiendo verificación de certificado):

```bash
curl https://localhost:5001/swagger/v1/swagger.json -k
curl https://localhost:5001/swagger/v2/swagger.json -k
```

- Nota: `Program.cs` en este proyecto ya registra `AddVersionedApiExplorer` y `Swagger` (ver código). Si no ves Swagger UI tras `dotnet run`, asegúrate de haber instalado `Swashbuckle.AspNetCore` y de ejecutar en `Development` o habilitar los middlewares en `Program.cs`.

Siguientes mejoras (opcional)
- Registrar `AddVersionedApiExplorer` y configurar Swagger para mostrar versiones separadas.
- Extraer lógica compartida en servicios y añadir persistencia (BD) en lugar de lista en memoria.

Si quieres, puedo:
- añadir automáticamente `AddVersionedApiExplorer` y configurar Swagger para versiones, o
- ejecutar aquí `dotnet add package Microsoft.AspNetCore.Mvc.Versioning` y `dotnet run` por ti.
