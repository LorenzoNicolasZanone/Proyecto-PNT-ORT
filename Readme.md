# Proyecto PNT: Web Examenes

Aplicación web desarrollada con ASP.NET Core MVC para la gestion y realización de exámenes online. 

## Funcionalidades

### Rol de Profesor
- **Creación de Exámenes:** Elaborar nuevos exámenes, definir sus propiedades (como título y fecha) y asociarlos a un curso.
- **Gestión de Preguntas:** Agregar, modificar y eliminar preguntas y sus respectivas opciones para cada examen.
- **Publicación de Exámenes:** Hacer visibles los exámenes para que los alumnos puedan rendirlos.
- **Seguimiento de Alumnos:** Ver la lista de alumnos inscritos en sus cursos y visualizar los resultados de los exámenes que han completado.

### Rol de Alumno
- **Inscripción a Cursos:** Visualizar y buscar cursos para inscribirse.
- **Mis Exámenes:** Acceder a un listado de los exámenes disponibles para rendir y los que ya ha completado.
- **Rendir Examen:** Realizar un examen, seleccionando las respuestas a las preguntas presentadas.
- **Ver Resultados:** Consultar la calificación obtenida en los exámenes ya rendidos.

---

## Instalación y Ejecución Local

### 1. Restaurar Dependencias
Para instalar todas las dependencias del proyecto, ejecuta el siguiente comando en la raíz del repositorio:

```bash
dotnet restore web-examenes-mvc.sln
```

### 2. Migraciones de la Base de Datos
La base de datos se gestiona con Entity Framework Core.

Creamos una migration con el comando:
```bash
dotnet ef migrations add <NombreDeLaMigracion> --project Examenes
```

Para aplicar las migraciones y crear o actualizar la base de datos ejecutamos el comando:
```bash
dotnet ef database update --project Examenes
```

### 3. Ejecutar la Aplicación
Para iniciar la aplicación en modo de desarrollo:
```bash
dotnet watch run --project Examenes
```
Utilizamos watch para que los cambios realizados impacten en ejecución.

---

## Ejecución con Docker

Para ejecutar la aplicación con Docker, necesitas dos contenedores: uno para la base de datos SQL Server y otro para la aplicación web.

### 1. Iniciar Contenedor de SQL Server

**a) Comando para Mac:**

```bash
docker run --name sql-server-db -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=TuPasswordFuerte!" -p 1433:1433 -d mcr.microsoft.com/azure-sql-edge
```

**b) Comando para Windows:**

```bash
docker run --name sql-server-db -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=TuPasswordFuerte!" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
```