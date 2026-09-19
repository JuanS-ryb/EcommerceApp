# Ecommerce App — Prueba Técnica Desarrollador Full Stack (Grupo Merpes)

Aplicación móvil híbrida tipo e-commerce, desarrollada como prueba técnica.
Incluye catálogo de productos, carrito de compras, registro/login de usuarios,
simulación de compra, y un módulo adicional de análisis de puntos por
cumplimiento de cuota de venta.

## Stack tecnológico

| Capa       | Tecnología                                      |
|------------|--------------------------------------------------|
| Frontend   | Ionic 7 + Angular 17 (standalone) + Cordova       |
| Backend    | ASP.NET Core 10 (.NET 10) Web API                 |
| Base de datos | SQL Server                                     |
| Autenticación | JWT (JSON Web Token) + BCrypt para contraseñas |

## Estructura del repositorio

```
/
├── Ecommerce.client/                  # App Ionic + Angular (código fuente)
│   └── src/app/
│       ├── core/
│       │   ├── http/           # Wrapper de HttpClient (HttpService)
│       │   ├── interceptors/   # authInterceptor (agrega JWT, maneja 401)
│       │   ├── models/         # Interfaces TypeScript (DTOs del backend)
│       │   └── services/       # AuthService, CartService, ProductsService...
│       └── pages/
│           ├── login/
│           ├── register/
│           ├── products/
│           ├── cart/
│           ├── checkout-success/
│           └── points/         # Módulo de análisis de puntos
│
├── Ecommerce.server/                   # API ASP.NET Core (código fuente)
│   ├── Controllers/
│   ├── Services/
│   ├── Dto/
│   ├── Models/                 # Entidades generadas con Scaffold-DbContext
│   ├── Context/                # AppDbContext
│   └── Program.cs
│
├── database/
│   └── database_schema.sql    # Script de creación de la BD + datos semilla
│
└── README.md                  # Este archivo
```

## Funcionalidades

- **Login / Registro** de usuarios con validaciones básicas y JWT.
- **Catálogo** de 3 productos (nombre, imagen, precio).
- **Carrito de compras**: agregar, quitar, cambiar cantidades, ver total.
- Si el usuario intenta finalizar la compra sin sesión iniciada, se redirige a Login.
- **Confirmación de compra**: mensaje de éxito y pedido guardado en la base de datos.
- **Módulo de puntos** (prueba de análisis): el usuario ingresa cuánto lleva
  ejecutado de su cuota trimestral (en pesos y en unidades) y el sistema
  calcula los puntos ganados según las tablas de cumplimiento del enunciado.

## Cómo correr el proyecto

### 1. Base de datos

```bash
# Desde SQL Server Management Studio, Azure Data Studio, o sqlcmd:
# ejecutar database/database_schema.sql
```

Esto crea la base `EcommerceApp` con las tablas `Users`, `Products`,
`CartItems`, `Orders`, `OrderItems`, `SalesGoals`, y siembra los 3 productos
del catálogo.

Ademas, con el siguiente comando que se debe ejecutar en la raiz del proyecto 
Ecommerce.server se va a sincronizar el backend con la base de datos hablando 
de modelos y contextos.

```bash
dotnet ef dbcontext scaffold "Server=(localdb)\MSSQLLocalDB;Database=EcommerceApp;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -o Models --context AppDbContext --context-dir Context --project Ecommerce.server.csproj --force
```

### 2. Backend

Requiere el **SDK de .NET 10** (`dotnet --version` debe mostrar `10.x`).

```bash
cd backend
dotnet restore
```

Edita `appsettings.json` con tu cadena de conexión real y una clave `Jwt:Secret`
propia.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=EcommerceApp;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Secret": "tu-clave-secreta",
    "Issuer": "EcommerceApi",
    "Audience": "EcommerceAppClient",
    "ExpiryMinutes": 120
  }
}
```

```bash
dotnet run
```

La API queda disponible en `https://localhost:7002` (el puerto puede variar,
revisa la consola al arrancar). Con el proyecto en modo Development, Swagger
queda en `https://localhost:7002/swagger` para probar los endpoints sin el
frontend o Scalar con `https://localhost:7002/scalar`

### 3. Frontend

Requiere Node.js, y las CLI globales:

```bash
npm install -g @ionic/cli @angular/cli cordova
```

```bash
cd frontend
npm install
```

Si el puerto de tu backend no es `7002`, ajústalo en
`Ecommerce.client/src/environments/environments.ts`, aqui
se puede ajustar la conexion al backend.

```bash
ionic serve
```

La app queda disponible en `http://localhost:4200` (navegador, para desarrollo).

## Generar el APK

```bash
cd frontend
ionic build --prod
ionic cordova platform add android   # solo la primera vez
ionic cordova build android
```

El APK queda en:
```
frontend/platforms/android/app/build/outputs/apk/debug/app-debug.apk
```

Requiere JDK 17, Android Studio (Android SDK) y la variable de entorno
`ANDROID_HOME` configurada.

## Endpoints de la API

Todas las rutas protegidas requieren el header:
```
Authorization: Bearer {token}
```

| Método | Ruta                       | Auth | Descripción                               |
|--------|----------------------------|------|--------------------------------------------|
| POST   | /api/Auth/register         | No   | Crea un usuario nuevo                       |
| POST   | /api/Auth/login            | No   | Devuelve `{ token, user }`                  |
| GET    | /api/Products               | No   | Lista el catálogo                           |
| GET    | /api/Products/{id}          | No   | Un producto puntual                         |
| GET    | /api/Cart                   | Sí   | Carrito del usuario autenticado             |
| POST   | /api/Cart                   | Sí   | Agrega `{ productId, quantity }` al carrito |
| PUT    | /api/Cart/{productId}       | Sí   | Cambia la cantidad de un ítem               |
| DELETE | /api/Cart/{productId}       | Sí   | Quita un ítem del carrito                   |
| POST   | /api/Orders/checkout        | Sí   | Convierte el carrito actual en un pedido    |
| GET    | /api/Orders                 | Sí   | Historial de pedidos del usuario            |
| GET    | /api/SalesGoals/current     | Sí   | Meta/puntos del trimestre actual            |
| PUT    | /api/SalesGoals/current     | Sí   | Actualiza lo ejecutado y recalcula puntos   |

## Prueba de análisis — módulo de puntos

**Regla de negocio:** cada trimestre el usuario tiene una cuota fija
($11.000.000 en pesos y 6.000 unidades). El usuario reporta cuánto lleva
ejecutado hasta el momento, y el sistema calcula cuántos puntos gana según
el % de cumplimiento (tablas de rangos del enunciado). 1 punto = $1.500.

**Dónde vive la lógica:**
- `backend/Services/SalesPointsCalculator.cs` — las reglas de negocio puras
  (las tablas de rangos), separadas de todo lo que toca base de datos.
- `backend/Services/SalesGoalService.cs` — orquesta: busca o crea la meta
  del usuario para el trimestre actual, guarda lo ejecutado, y arma la
  respuesta con los puntos ya calculados.
- `frontend/src/app/pages/points/` — pantalla donde el usuario ingresa lo
  ejecutado y ve el desglose de puntos.

**Nota sobre el enunciado:** el rango "-999 productos vendidos = 100 puntos"
del documento original parece un error de digitación (valdría más puntos que
el rango de 1.000-2.999 unidades = 50 puntos). Se implementó como 0 puntos;
el ajuste está aislado a una sola línea en `SalesPointsCalculator.cs` si el
evaluador confirma que el valor correcto es otro.

## Decisiones técnicas y por qué

- **DTOs en vez de exponer entidades de EF directamente**: evita que datos
  sensibles (como `PasswordHash`) viajen al cliente por accidente.
- **JWT + BCrypt**: contraseñas nunca se guardan en texto plano; las rutas
  protegidas validan el token en cada request, y el `userId` siempre sale
  del token (nunca de un parámetro que mande el cliente).
- **Transacción en el checkout**: crear el pedido, descontar stock y vaciar
  el carrito ocurre en una sola transacción de base de datos — si algo
  falla a mitad de camino, no queda un pedido a medias.
- **Angular standalone components**: sin `NgModule`, cada página declara
  sus propios imports — es el estilo que genera Ionic 7+ por defecto.
- **CORS abierto (`AllowAnyOrigin`)**: como la autenticación es por JWT (no
  cookies de sesión), no hay riesgo de robo de sesión vía CORS; se dejó así
  para simplificar las pruebas desde el navegador y el APK.

## Autor

Prueba técnica desarrollada para el proceso de selección de Grupo Merpes —
puesto de Desarrollador Full Stack.
