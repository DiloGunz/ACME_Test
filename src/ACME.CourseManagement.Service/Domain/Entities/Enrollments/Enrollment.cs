using ACME.CourseManagement.Service.Domain.Helpers;

namespace ACME.CourseManagement.Service.Domain.Entities.Enrollments;

public class Enrollment
{
    public long Id { get; set; }
    public long StudentId { get; set; }
    public long CourseId { get; set; }
    public DateTime EnrollmentDate { get; set; }

    public Enrollment()
    {

    }

    public Enrollment(long studentId, long courseId, DateTime enrollmentDate)
    {
        Id = SnowflakeIdGenerator.GenerateId();
        StudentId = studentId;
        CourseId = courseId;
        EnrollmentDate = enrollmentDate;
    }
}