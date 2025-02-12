using System.Reflection;

namespace ACME.CourseManagement.Service;

/// <summary>
/// Proporciona una referencia est�tica a la ensambladura (assembly) de la aplicaci�n.
/// </summary>
public class ApplicationAssemblyReference
{
    /// <summary>
    /// Referencia est�tica a la ensambladura actual de la aplicaci�n.
    /// </summary>
    internal static readonly Assembly Assembly = typeof(ApplicationAssemblyReference).Assembly;
}