using Soenneker.Stytch.HttpClients.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Stytch.HttpClients.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class StytchOpenApiHttpClientTests : HostedUnitTest
{
    private readonly IStytchOpenApiHttpClient _httpclient;

    public StytchOpenApiHttpClientTests(Host host) : base(host)
    {
        _httpclient = Resolve<IStytchOpenApiHttpClient>(true);
    }

    [Test]
    public void Default()
    {

    }
}
