using ACME.CourseManagement.Service.Application.Enrollments.Common;

namespace ACME.CourseManagement.Service.Application.Enrollments.GetByStudentAndCourse;

public record GetEnrollmentByStudentAndCourseQuery : IRequest<ErrorOr<EnrollmentDto>>
{
    public GetEnrollmentByStudentAndCourseQuery(long studentId, long courseId)
    {
        StudentId = studentId;
        CourseId = courseId;
    }
    public GetEnrollmentByStudentAndCourseQuery()
    {

    }

    public long StudentId { get; set; }
    public long CourseId { get; set; }
}