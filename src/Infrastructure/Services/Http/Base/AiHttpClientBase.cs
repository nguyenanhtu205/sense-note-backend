using System.Net.Http.Json;
using Application.Common.Exceptions;

namespace Infrastructure.Services.Http.Base;

public abstract class AiHttpClientBase(HttpClient httpClient)
{
    protected async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest request,
        CancellationToken cancellationToken)
    {
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(url, request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken);
        }

        string error = await response.Content.ReadAsStringAsync(cancellationToken);
        throw new AiServiceException($"AI Error: {response.StatusCode} - {error}");
    }
}
