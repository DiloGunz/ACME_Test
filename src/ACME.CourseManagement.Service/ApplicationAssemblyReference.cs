using System.Reflection;

namespace ACME.CourseManagement.Service;

public class ApplicationAssemblyReference
{
    internal static readonly Assembly Assembly = typeof(ApplicationAssemblyReference).Assembly;
}