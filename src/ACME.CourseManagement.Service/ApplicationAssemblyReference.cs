using System.Reflection;

namespace ACME.CourseManagement.Service;

/// <summary>
/// Proporciona una referencia estática a la ensambladura (assembly) de la aplicación.
/// </summary>
public class ApplicationAssemblyReference
{
    /// <summary>
    /// Referencia estática a la ensambladura actual de la aplicación.
    /// </summary>
    internal static readonly Assembly Assembly = typeof(ApplicationAssemblyReference).Assembly;
}