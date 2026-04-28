namespace Application.Students.Queries.GetStudentInfo;

public class StudentInfoDto
{
    public required string FullName { get; init; }

    public DateTime? Birthday { get; init; }

    public List<StudentSensitivityProfile> StudentSensitivityProfiles { get; init; } = [];

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Student, StudentInfoDto>();
        }
    }
}
