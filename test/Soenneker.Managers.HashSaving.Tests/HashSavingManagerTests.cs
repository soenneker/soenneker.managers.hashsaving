using Soenneker.Managers.HashSaving.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Managers.HashSaving.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class HashSavingManagerTests : HostedUnitTest
{
    private readonly IHashSavingManager _util;

    public HashSavingManagerTests(Host host) : base(host)
    {
        _util = Resolve<IHashSavingManager>(true);
    }

    [Test]
    public void Default()
    {

    }
}
