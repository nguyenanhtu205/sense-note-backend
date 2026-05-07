namespace Application.Students.Commands.AddStudent;

public class AddStudentCommandValidator : AbstractValidator<AddStudentCommand>
{
    public AddStudentCommandValidator()
    {
        RuleFor(x => x.ClassId)
            .GreaterThan(0).WithMessage("Class id must be greater than 0");

        RuleFor(x => x.TeachingContextId)
            .GreaterThan(0).WithMessage("Teaching context id must be greater than 0");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required")
            .MaximumLength(150).WithMessage("Full name must not exceed 150 characters");

        RuleFor(x => x.DisplayName)
            .NotEmpty().WithMessage("Display name is required")
            .MaximumLength(100).WithMessage("Display name must not exceed 100 characters");

        RuleFor(x => x.BirthDay)
            .LessThan(DateTime.Today).When(x => x.BirthDay.HasValue)
            .WithMessage("Birthday must be in the past");
        
        RuleFor(x => x.StudentSensitivityProfile)
            .NotNull().WithMessage("Student sensitivity profile must not be null");
        
        RuleFor(x => x.StudentSensitivityProfile.SoundSensitivity)
            .InclusiveBetween(0, 10).WithMessage("Sound sensitivity must be between 0 and 10");
        
        RuleFor(x => x.StudentSensitivityProfile.LightSensitivity)
            .InclusiveBetween(0, 10).WithMessage("Light sensitivity must be between 0 and 10");
        
        RuleFor(x => x.StudentSensitivityProfile.TemperatureSensitivity)
            .InclusiveBetween(0, 10).WithMessage("Temperature sensitivity must be between 0 and 10");
        
        RuleFor(x => x.StudentSensitivityProfile.TouchSensitivity)
            .InclusiveBetween(0, 10).WithMessage("Touch sensitivity must be between 0 and 10"); 
        
        RuleFor(x => x.StudentSensitivityProfile.Distractibility)
            .InclusiveBetween(0, 10).WithMessage("Distractibility must be between 0 and 10");
   
        RuleFor(x => x.StudentSensitivityProfile.MedicalNotes)
            .NotNull().WithMessage("Medical notes must not be null")
            .NotEmpty().WithMessage("Medical notes must not be empty")
            .MaximumLength(500).WithMessage("Medical notes must not exceed 500 characters");
        
        RuleFor(x => x.StudentSensitivityProfile.OverallSensitivityLevel)
            .InclusiveBetween(0, 10).WithMessage("Overall sensitivity level must be between 0 and 10");
    }
}
