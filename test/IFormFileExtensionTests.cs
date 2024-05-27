using Soenneker.Tests.FixturedUnit;
using Xunit;
using Xunit.Abstractions;

namespace Soenneker.Extensions.IFormFile.Tests;

[Collection("Collection")]
// ReSharper disable once InconsistentNaming
public class IFormFileExtensionTests : FixturedUnitTest
{
    public IFormFileExtensionTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
    }
}
