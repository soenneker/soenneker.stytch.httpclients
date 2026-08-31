[![](https://img.shields.io/nuget/v/soenneker.stytch.httpclients.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.stytch.httpclients/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.stytch.httpclients/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.stytch.httpclients/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.stytch.httpclients.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.stytch.httpclients/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.stytch.httpclients/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.stytch.httpclients/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Stytch.HttpClients

Provides a cached `HttpClient` configured with the base URL and Basic authentication required by Stytch's backend API.

## Installation

```bash
dotnet add package Soenneker.Stytch.HttpClients
```

## Configuration

Use the test API while developing:

```json
{
  "Stytch": {
    "ProjectId": "project-test-...",
    "Secret": "secret-test-...",
    "ClientBaseUrl": "https://test.stytch.com"
  }
}
```

For a live project, use its live credentials and `https://api.stytch.com` (the default base URL).

## Usage

```csharp
using Soenneker.Stytch.HttpClients.Abstract;
using Soenneker.Stytch.HttpClients.Registrars;

services.AddStytchOpenApiHttpClientAsSingleton();

HttpClient client = await stytchHttpClient.Get(cancellationToken);
HttpResponseMessage response = await client.GetAsync("/v1/users", cancellationToken);
response.EnsureSuccessStatusCode();
```

The provider owns its cached client. Disposing the provider removes and disposes that client; callers should not dispose the value returned by `Get` independently.

The legacy `ApiKey`, `AuthHeaderName`, and `AuthHeaderValueTemplate` settings remain available for pre-encoded or custom authorization values. The default legacy template is `Basic {token}`.
