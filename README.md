# PintoresApp

Aplicación de consola en .NET 9 con Clean Architecture que implementa el CRUD de la entidad Pintor.

## Integrantes

| Integrante | Capa a cargo |
|---|---|
| Tomas Toledo 1 | Domain + Application |
| Bruno David | Infrastructure (AppDbContext, configuración, repositorio, migraciones) |
| Lucas Aguilar 3 | Presentation (consola + DI) |
| Abner Vasquez 4 | Tests xUnit sobre Application |

## Estructura

- `src/PintoresApp.Domain`: entidades `Pintor` y `MovimientoArtistico`, interfaces de repositorios, excepciones de dominio.
- `src/PintoresApp.Application`: casos de uso (commands y queries).
- `src/PintoresApp.Infrastructure`: EF Core con SQLite, `AppDbContext`, repositorios y migraciones.
- `src/PintoresApp.Presentation`: menú de consola y registro de dependencias.
- `tests/PintoresApp.Application.Tests`: tests xUnit con Moq.

## Requisitos

- .NET 9 SDK

## Cómo ejecutar

Desde la carpeta raíz de la solución:

    dotnet restore
    dotnet run --project src/PintoresApp.Presentation

La base de datos SQLite (`pintores.db`) se crea automáticamente al iniciar, aplicando las migraciones.

## Cómo correr los tests

    dotnet test