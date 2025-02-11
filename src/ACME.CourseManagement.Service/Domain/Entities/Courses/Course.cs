using ACME.CourseManagement.Service.Domain.Helpers;

namespace ACME.CourseManagement.Service.Domain.Entities.Courses;

public class Course
{
    public long Id { get; set; }
    public string Name { get; set; }
    public decimal EnrollmentFee { get; set; }
    public int Capacity { get; set; }
    public bool Enable { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public Course()
    {

    }
    public Course(string name, decimal enrollmentFee, int capacity, DateTime startDate, DateTime endDate)
    {
        Id = SnowflakeIdGenerator.GenerateId();
        Name = name;
        EnrollmentFee = enrollmentFee;
        StartDate = startDate;
        EndDate = endDate;
        Capacity = capacity;
        Enable = true;
    }
}