using System.Text.RegularExpressions;

namespace Application.AiServices.Commands.ExtractScores;

public static partial class RegexHelpers
{
    [GeneratedRegex(@"\s+")]
    public static partial Regex MultiWhitespace();

    [GeneratedRegex(@"[\u0000-\u001F]")]
    public static partial Regex ControlChars();
}

public record ExtractScoresCommand(string MedicalNote) : IRequest<MedicalSensitivityScoresResponse>;

public class ExtractScoresCommandHandler(IExtractScores extractScores)
    : IRequestHandler<ExtractScoresCommand, MedicalSensitivityScoresResponse>
{
    public async Task<MedicalSensitivityScoresResponse> Handle(ExtractScoresCommand request,
        CancellationToken cancellationToken)
    {
        string cleanedNote = request.MedicalNote;

        cleanedNote = RegexHelpers.MultiWhitespace().Replace(cleanedNote, " ").Trim();

        cleanedNote = RegexHelpers.ControlChars().Replace(cleanedNote, "");

        MedicalSensitivityScoresResponse? result = await extractScores.ExtractAsync(cleanedNote, cancellationToken);

        return result ?? throw new AiServiceException();
    }
}
