namespace Application.TeachingContexts.Commands.CreateTeachingContext;

public class CreateTeachingContextCommandValidator : AbstractValidator<CreateTeachingContextCommand>
{
    public CreateTeachingContextCommandValidator()
    {
        RuleFor(v => v.ClassName)
            .MaximumLength(150).WithMessage("Class name can't be more than 150 characters")
            .NotEmpty().WithMessage("Class name is required");

        RuleFor(v => v.TeachingContextName)
            .MaximumLength(255).WithMessage("Teaching context name can't be more than 255 characters")
            .NotEmpty().WithMessage("Teaching context name is required");

        RuleFor(v => v.NumCols)
            .GreaterThan(0).WithMessage("Number of columns must be greater than 0");

        RuleFor(v => v.NumRows)
            .GreaterThan(0).WithMessage("Number of rows must be greater than 0");

        RuleFor(v => v.SeatsPerTable)
            .GreaterThan(0).WithMessage("Seats per table must be greater than 0");

        RuleForEach(v => v.EnvironmentalAssets).ChildRules(asset =>
        {
            asset.RuleFor(x => x.AssetType)
                .NotEmpty().WithMessage("Asset type is required")
                .MaximumLength(50).WithMessage("Asset type can't exceed 50 characters");

            asset.RuleFor(x => x.ImpactType)
                .NotNull().WithMessage("Impact type is required")
                .IsInEnum().WithMessage("Impact type is invalid");

            asset.RuleFor(x => x.X)
                .GreaterThanOrEqualTo(0).WithMessage("X must be greater than 0");

            asset.RuleFor(x => x.Y)
                .GreaterThanOrEqualTo(0).WithMessage("Y must be greater than 0");

            asset.RuleFor(x => x.InfluenceRadius)
                .GreaterThan(0).WithMessage("Influence radius must be greater than 0");
        });
    }
}
