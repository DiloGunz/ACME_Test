# ACME School Management System

Este proyecto es una prueba de concepto (PoC) para la gesti�n de cursos y estudiantes en ACME School. Se ha desarrollado utilizando C# con una arquitectura basada en DDD, CQRS y patrones modernos de dise�o, asegurando flexibilidad y escalabilidad para futuras expansiones.

## Caracter�sticas Implementadas

- **Registro de Estudiantes**: Solo adultos pueden registrarse.
- **Registro de Cursos**: Incluye nombre, tarifa de inscripci�n, fecha de inicio y fin.
- **Inscripci�n de Estudiantes**: Un estudiante puede inscribirse en un curso si paga la tarifa correspondiente.
- **Consulta de Cursos y Estudiantes**: Listado de cursos y estudiantes inscritos en un rango de fechas.
- **Generaci�n de IDs �nicos**: Se utiliza el algoritmo **Twitter Snowflake** para asegurar unicidad y orden de los identificadores.


## Arquitectura

Se ha implementado una arquitectura **Domain-Driven Design (DDD)** con patrones CQRS y Mediator.
La estructura principal del proyecto es:

- Application
- Domain
- Infraestructure


### Patrones y Principios Utilizados

- **CQRS**: Separaci�n de comandos y consultas para una mejor escalabilidad.
- **Unit of Work**: Para gestionar transacciones de manera eficiente.
- **Mediator (MediatR)**: Desacopla la comunicaci�n entre componentes de la aplicaci�n.
- **Validaciones con FluentValidation**: Asegura datos correctos en las operaciones.
- **Automapper**: Facilita la conversi�n de DTOs y entidades de dominio.
- **ErrorOr**: Manejo estructurado de errores.

## Librer�as Utilizadas

| Librer�a | Uso |
|----------|-----|
| MediatR | Implementaci�n del patr�n Mediator para CQRS |
| ErrorOr | Gesti�n estructurada de errores en la aplicaci�n |
| FluentValidation | Validaciones de entrada de datos |
| AutoMapper | Mapeo autom�tico entre DTOs y entidades |

## Pruebas

Se han implementado pruebas unitarias usando **xUnit.net**, cubriendo los casos principales.

## Consideraciones y Mejoras Futuras

### Cosas por hacer:
- Integraci�n con una base de datos (actualmente en memoria).
- Implementaci�n de una API REST para exponer los endpoints.
- Integraci�n con una pasarela de pagos real.
- Agregar Auditor�a de tablas con campos como CreatedAt, CreatedBy, UpdateAt, UpdatedBy.
- Manejar excepciones de forma global con un Middleware por ejemplo.
- Generar UnitTest de m�todos faltantes incluyendo excepciones por parametros nulos en constructores.
- Comentar el c�digo faltante para que sea m�s comprensible para los desarrolladores.

### Mejoras Potenciales:
- Optimizaci�n de consultas para grandes vol�menes de datos.
- Implementaci�n de event sourcing para auditor�a de cambios.

## Tiempo y Aprendizajes

El desarrollo tom� aproximadamente **8 horas**.



