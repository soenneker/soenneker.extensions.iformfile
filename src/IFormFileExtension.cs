using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Extensions.Stream;

namespace Soenneker.Extensions.IFormFile;

/// <summary>
/// A collection of helpful IFormFile extension methods
/// </summary>
// ReSharper disable once InconsistentNaming
public static class IFormFileExtension
{
    /// <summary>
    /// Converts an <see cref="Microsoft.AspNetCore.Http.IFormFile"/> to a <see cref="MemoryStream"/>.
    /// </summary>
    /// <param name="formFile">The form file to be converted.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A <see cref="MemoryStream"/> containing the contents of the form file.</returns>
    /// <remarks>
    /// This method reads the contents of the <paramref name="formFile"/> into a <see cref="MemoryStream"/> and returns it.
    /// The stream's position is set to the beginning after the copy operation.
    /// Be sure to dispose of the <see cref="MemoryStream"/> after use.
    /// </remarks>
    public static async ValueTask<MemoryStream> ToMemoryStream(this Microsoft.AspNetCore.Http.IFormFile formFile, CancellationToken cancellationToken = default)
    {
        var memoryStream = new MemoryStream();
        await formFile.CopyToAsync(memoryStream, cancellationToken).ConfigureAwait(false);
        memoryStream.ToStart();
        return memoryStream;
    }
}
