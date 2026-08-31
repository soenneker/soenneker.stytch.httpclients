using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace Soenneker.Stytch.HttpClients.Abstract;

/// <summary>
/// Provides an authenticated, cached <see cref="HttpClient"/> for Stytch's backend API.
/// </summary>
public interface IStytchOpenApiHttpClient : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Removes and disposes the HTTP client owned by this provider.
    /// </summary>
    new void Dispose();

    /// <summary>
    /// Asynchronously removes and disposes the HTTP client owned by this provider.
    /// </summary>
    new ValueTask DisposeAsync();

    /// <summary>
    /// Gets the cached Stytch HTTP client, creating it on first use.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task containing the result of the operation.</returns>
    ValueTask<HttpClient> Get(CancellationToken cancellationToken = default);
}
