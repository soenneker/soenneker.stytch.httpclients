using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Soenneker.Dtos.HttpClientOptions;
using Soenneker.Extensions.Configuration;
using Soenneker.Stytch.HttpClients.Abstract;
using Soenneker.Utils.HttpClientCache.Abstract;

namespace Soenneker.Stytch.HttpClients;

/// <inheritdoc cref="IStytchOpenApiHttpClient" />
public sealed class StytchOpenApiHttpClient : IStytchOpenApiHttpClient
{
    private readonly IHttpClientCache _httpClientCache;
    private readonly IConfiguration _config;
    private readonly string _cacheKey = $"{nameof(StytchOpenApiHttpClient)}-{Guid.NewGuid():N}";

    private const string _prodBaseUrl = "https://api.stytch.com";

    public StytchOpenApiHttpClient(IHttpClientCache httpClientCache, IConfiguration config)
    {
        _httpClientCache = httpClientCache;
        _config = config;
    }

    public ValueTask<HttpClient> Get(CancellationToken cancellationToken = default)
    {
        return _httpClientCache.Get(_cacheKey, (config: _config, baseUrl: _config["Stytch:ClientBaseUrl"] ?? _prodBaseUrl), static state =>
        {
            string authHeaderName = state.config["Stytch:AuthHeaderName"] ?? "Authorization";
            string? projectId = state.config["Stytch:ProjectId"];
            string? secret = state.config["Stytch:Secret"];
            string authHeaderValue;

            if (!string.IsNullOrWhiteSpace(projectId) && !string.IsNullOrWhiteSpace(secret))
            {
                string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{projectId}:{secret}"));
                authHeaderValue = $"Basic {credentials}";
            }
            else
            {
                var apiKey = state.config.GetValueStrict<string>("Stytch:ApiKey");
                string authHeaderValueTemplate = state.config["Stytch:AuthHeaderValueTemplate"] ?? "Basic {token}";
                authHeaderValue = authHeaderValueTemplate.Replace("{token}", apiKey, StringComparison.Ordinal);
            }

            return new HttpClientOptions
            {
                BaseAddress = new Uri(state.baseUrl),
                DefaultRequestHeaders = new Dictionary<string, string>
                {
                    {authHeaderName, authHeaderValue},
                }
            };
        }, cancellationToken);
    }

    public void Dispose()
    {
        _httpClientCache.RemoveSync(_cacheKey);
    }

    public ValueTask DisposeAsync()
    {
        return _httpClientCache.Remove(_cacheKey);
    }
}
