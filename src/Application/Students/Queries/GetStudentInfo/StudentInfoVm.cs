namespace Application.Students.Queries.GetStudentInfo;

public class StudentInfoVm
{
    public required StudentInfoDto StudentInfo { get; init; }

    public int FinalScore { get; init; }
}
