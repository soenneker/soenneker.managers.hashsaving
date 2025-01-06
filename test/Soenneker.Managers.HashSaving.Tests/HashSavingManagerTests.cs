using Soenneker.Managers.HashSaving.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Managers.HashSaving.Tests;

[Collection("Collection")]
public class HashSavingManagerTests : FixturedUnitTest
{
    private readonly IHashSavingManager _util;

    public HashSavingManagerTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<IHashSavingManager>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
