using ACME.CourseManagement.Service.Application.Common.Behaviors;
using ACME.CourseManagement.Service.Application.Interfaces;
using ACME.CourseManagement.Service.Domain.Entities.Courses;
using ACME.CourseManagement.Service.Domain.Entities.Enrollments;
using ACME.CourseManagement.Service.Domain.Entities.Payments;
using ACME.CourseManagement.Service.Domain.Entities.Students;
using ACME.CourseManagement.Service.Domain.Primitives;
using ACME.CourseManagement.Service.Infraestructure.Persistence;
using ACME.CourseManagement.Service.Infraestructure.Persistence.Repositories;
using ACME.CourseManagement.Service.Infraestructure.Services;

namespace ACME.CourseManagement.Service;

using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Proporciona métodos de extensión para la configuración de inyección de dependencias en la capa de aplicación.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Agrega y configura los servicios de la aplicación en el contenedor de inyección de dependencias.
    /// </summary>
    /// <param name="services">Colección de servicios de la aplicación.</param>
    /// <returns>La colección de servicios actualizada con las dependencias registradas.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Registra MediatR y configura la inyección de dependencias desde el ensamblado de la aplicación.
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<ApplicationAssemblyReference>();
        });

        // Agrega un comportamiento de validación a la canalización de MediatR.
        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>)
        );

        // Registra AutoMapper para la transformación de objetos.
        services.AddAutoMapper(ApplicationAssemblyReference.Assembly);

        // Registra los validadores de FluentValidation desde el ensamblado de la aplicación.
        services.AddValidatorsFromAssemblyContaining<ApplicationAssemblyReference>();

        // Registra los repositorios en el contenedor de dependencias.
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();

        // Registra la unidad de trabajo para la gestión transaccional.
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Registra la pasarela de pagos como una implementación transitoria.
        services.AddTransient<IPaymentGateway, StripePaymentGateway>();

        return services;
    }
}
