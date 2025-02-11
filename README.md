# ACME School Management System

Este proyecto es una prueba de concepto (PoC) para la gestión de cursos y estudiantes en ACME School. Se ha desarrollado utilizando C# con una arquitectura basada en DDD, CQRS y patrones modernos de diseño, asegurando flexibilidad y escalabilidad para futuras expansiones.

## Características Implementadas

- **Registro de Estudiantes**: Solo adultos pueden registrarse.
- **Registro de Cursos**: Incluye nombre, tarifa de inscripción, fecha de inicio y fin.
- **Inscripción de Estudiantes**: Un estudiante puede inscribirse en un curso si paga la tarifa correspondiente.
- **Consulta de Cursos y Estudiantes**: Listado de cursos y estudiantes inscritos en un rango de fechas.
- **Generación de IDs Únicos**: Se utiliza el algoritmo **Twitter Snowflake** para asegurar unicidad y orden de los identificadores.


## Arquitectura

Se ha implementado una arquitectura **Domain-Driven Design (DDD)** con patrones CQRS y Mediator.
La estructura principal del proyecto es:

- Application
- Domain
- Infraestructure


### Patrones y Principios Utilizados

- **CQRS**: Separación de comandos y consultas para una mejor escalabilidad.
- **Unit of Work**: Para gestionar transacciones de manera eficiente.
- **Mediator (MediatR)**: Desacopla la comunicación entre componentes de la aplicación.
- **Validaciones con FluentValidation**: Asegura datos correctos en las operaciones.
- **Automapper**: Facilita la conversión de DTOs y entidades de dominio.
- **ErrorOr**: Manejo estructurado de errores.

## Librerías Utilizadas

| Librería | Uso |
|----------|-----|
| MediatR | Implementación del patrón Mediator para CQRS |
| ErrorOr | Gestión estructurada de errores en la aplicación |
| FluentValidation | Validaciones de entrada de datos |
| AutoMapper | Mapeo automático entre DTOs y entidades |

## Pruebas

Se han implementado pruebas unitarias usando **xUnit.net**, cubriendo los casos principales.

## Consideraciones y Mejoras Futuras

### Cosas por hacer:
- Integración con una base de datos (actualmente en memoria).
- Implementación de una API REST para exponer los endpoints.
- Integración con una pasarela de pagos real.
- Agregar Auditoría de tablas con campos como CreatedAt, CreatedBy, UpdateAt, UpdatedBy.
- Manejar excepciones de forma global con un Middleware por ejemplo.
- Generar UnitTest de métodos faltantes incluyendo excepciones por parametros nulos en constructores.
- Comentar el código faltante para que sea más comprensible para los desarrolladores.

### Mejoras Potenciales:
- Optimización de consultas para grandes volúmenes de datos.
- Implementación de event sourcing para auditoría de cambios.

## Tiempo y Aprendizajes

El desarrollo tomó aproximadamente **8 horas**.



