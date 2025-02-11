using ACME.CourseManagement.Service.Domain.Helpers;

namespace ACME.CourseManagement.Service.Domain.Entities.Students;

public class Student
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DocumentNumber { get; set; }
    public int Age { get; set; }


    public Student()
    {

    }

    public Student(string firstName, string lastName, int age, string documentNumber)
    {
        Id = SnowflakeIdGenerator.GenerateId();
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        DocumentNumber = documentNumber;
    }
}