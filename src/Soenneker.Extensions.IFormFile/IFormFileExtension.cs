using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Extensions.Stream;
using Soenneker.Extensions.Task;
using Soenneker.Extensions.ValueTask;
using Soenneker.Utils.MemoryStream.Abstract;

namespace Soenneker.Extensions.IFormFile;

/// <summary>
/// A collection of helpful IFormFile extension methods
/// </summary>
// ReSharper disable once InconsistentNaming
/// <summary>
/// Represents the i form file extension.
/// </summary>
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
        // Pre-size when possible to avoid repeated growth/copies.
        MemoryStream memoryStream = formFile.Length is > 0 and <= int.MaxValue
            ? new MemoryStream((int)formFile.Length)
            : new MemoryStream();

        await formFile.CopyToAsync(memoryStream, cancellationToken)
                      .NoSync();
        memoryStream.ToStart();
        return memoryStream;
    }

    /// <summary>
    /// Copies an uploaded form file into a recyclable memory stream positioned for reading.
    /// </summary>
    /// <param name="formFile">The uploaded file to copy.</param>
    /// <param name="memoryStreamUtil">The recyclable-memory-stream provider.</param>
    /// <param name="cancellationToken">Signals that the asynchronous operation should stop.</param>
    /// <returns>A readable memory stream containing the uploaded bytes.</returns>
    /// <remarks>The returned memory stream's position is set to 0. The caller is responsible for disposing
    /// the returned stream when it is no longer needed.</remarks>
    public static async ValueTask<MemoryStream> ToMemoryStream(this Microsoft.AspNetCore.Http.IFormFile formFile, IMemoryStreamUtil memoryStreamUtil,
        CancellationToken cancellationToken = default)
    {
        MemoryStream stream = await memoryStreamUtil.Get(cancellationToken)
                                                    .NoSync();

        await formFile.CopyToAsync(stream, cancellationToken)
                      .NoSync();

        stream.Position = 0;
        return stream;
    }
}