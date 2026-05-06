namespace Infrastructure.Services.Http.Ai;

public class ExtractScores(HttpClient httpClient) : AiHttpClientBase(httpClient), IExtractScores
{
    public async Task<MedicalSensitivityScoresResponse?> ExtractAsync(string medicalNote,
        CancellationToken cancellationToken)
    {
        return await PostAsync<object, MedicalSensitivityScoresResponse>(
            "/api/v1/llm_request/medical/sensitivity-scores",
            new { medical_note = medicalNote },
            cancellationToken
        );
    }
}
