namespace Application.Students.Queries.GetStudentInfo;

public record GetStudentInfoQuery(int TeachingContextId, int StudentId) : IRequest<StudentInfoVm>;

public class GetStudentInfoQueryHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetStudentInfoQuery, StudentInfoVm>
{
    public async Task<StudentInfoVm> Handle(GetStudentInfoQuery request, CancellationToken cancellationToken)
    {
        StudentInfoDto? studentInfo = await context.Students
            .AsNoTracking()
            .Where(s => s.Id == request.StudentId)
            .ProjectTo<StudentInfoDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (studentInfo == null)
        {
            throw new NotFoundException($"Student with id {request.StudentId} was not found");
        }

        Lesson? latestLesson = await context.Lessons
            .AsNoTracking()
            .Where(l => l.TeachingContextId == request.TeachingContextId)
            .OrderByDescending(l => l.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (latestLesson == null || latestLesson.LessonStatus == LessonStatus.Inactive)
        {
            return new StudentInfoVm { FinalScore = 0, StudentInfo = studentInfo };
        }

        int finalScore = await context.LessonSummaries
            .AsNoTracking()
            .Where(ls => ls.StudentId == request.StudentId && ls.LessonId == latestLesson.Id)
            .Select(ls => ls.FinalScore)
            .FirstOrDefaultAsync(cancellationToken) ?? 0;

        return new StudentInfoVm { FinalScore = finalScore, StudentInfo = studentInfo };
    }
}
