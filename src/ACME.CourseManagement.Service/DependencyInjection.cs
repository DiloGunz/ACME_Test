﻿using ACME.CourseManagement.Service.Application.Common.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace ACME.CourseManagement.Service;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<ApplicationAssemblyReference>();
        });

        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>)
        );

        services.AddAutoMapper(ApplicationAssemblyReference.Assembly);

        services.AddValidatorsFromAssemblyContaining<ApplicationAssemblyReference>();

        return services;
    }
}