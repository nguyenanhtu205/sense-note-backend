namespace Application.Common.Interfaces;

public record MedicalSensitivityScoresResponse
{
    public int SoundSensitivity { get; init; }
    public int LightSensitivity { get; init; }
    public int TemperatureSensitivity { get; init; }
    public int TouchSensitivity { get; init; }
    public int Distractibility { get; init; }
}

public interface IExtractScores
{
    Task<MedicalSensitivityScoresResponse?> ExtractAsync(string medicalNote);
}
