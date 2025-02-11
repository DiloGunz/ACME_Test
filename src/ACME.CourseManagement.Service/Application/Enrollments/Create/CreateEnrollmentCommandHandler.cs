using ACME.CourseManagement.Service.Application.Courses.GetById;
using ACME.CourseManagement.Service.Application.Enrollments.GetEnrolledByCourse;
using ACME.CourseManagement.Service.Application.Enrollments.GetNumberEnrolledByCourse;
using ACME.CourseManagement.Service.Application.Payments.GetByStudentAndCourse;
using ACME.CourseManagement.Service.Application.Students.GetById;
using ACME.CourseManagement.Service.Domain.Entities.Enrollments;
using ACME.CourseManagement.Service.Domain.Primitives;

namespace ACME.CourseManagement.Service.Application.Enrollments.Create;

public class CreateEnrollmentCommandHandler : IRequestHandler<CreateEnrollmentCommand, ErrorOr<long>>
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public CreateEnrollmentCommandHandler(IUnitOfWork unitOfWork, IEnrollmentRepository enrollmentRepository, IMediator mediator)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _enrollmentRepository = enrollmentRepository ?? throw new ArgumentNullException(nameof(enrollmentRepository));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<ErrorOr<long>> Handle(CreateEnrollmentCommand request, CancellationToken cancellationToken)
    {
        var student = await _mediator.Send(new GetStudentByIdQuery(request.StudentId));
        if (student.IsError)
        {
            return Errors.Student.IdNotFound;
        }

        var course = await _mediator.Send(new GetCourseByIdQuery(request.CourseId));
        if (course.IsError)
        {
            return Errors.Course.IdNotFound;
        }

        if (!course.Value.Enable)
        {
            return Errors.Course.Disable;
        }

        if (course.Value.EnrollmentFee > 0)
        {
            // validar que el estudiante ya tenga un pago registrado
            var queryPayment = new GetPaymentByStudentAndCourseQuery()
            {
                CourseId = course.Value.Id,
                StudentId = student.Value.Id
            };
            var payment = await _mediator.Send(queryPayment);
            if (payment.IsError)
            {
                return payment.FirstError;
            }
        }

        // Validar que haya vacantes disponibles en el curso
        var enrolledStudents = await _mediator.Send(new GetNumberStudentEnrolledByCourseQuery(course.Value.Id));
        if (enrolledStudents.IsError)
        {
            return enrolledStudents.FirstError;
        }
        if (enrolledStudents.Value >= course.Value.Capacity)
        {
            return Errors.Course.InsufficientVacancies;
        }

        // validar que no esté matriculado
        var enrolledCourse = await _enrollmentRepository.GetByStudentAndCourseAsync(student.Value.Id, course.Value.Id);
        if (enrolledCourse is not null)
        {
            return Errors.Enrollment.StudentHasAlreadyBeenEnrolled;
        }

        var enrollment = new Enrollment(request.StudentId, request.CourseId, request.EnrollmentDate);
        await _enrollmentRepository.AddAsync(enrollment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return enrollment.Id;
    }
}
