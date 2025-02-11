namespace ACME.CourseManagement.Service.Application.Students.Create;

public record CreateStudentCommand : IRequest<ErrorOr<long>>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public string DocumentNumber { get; set; }
}
