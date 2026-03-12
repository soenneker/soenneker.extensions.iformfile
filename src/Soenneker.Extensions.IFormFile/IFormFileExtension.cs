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
    /// Asynchronously copies the contents of the specified form file to a new memory stream using the provided memory
    /// stream utility.
    /// </summary>
    /// <remarks>The returned memory stream's position is set to 0. The caller is responsible for disposing
    /// the returned stream when it is no longer needed.</remarks>
    /// <param name="formFile">The form file whose contents will be copied to the memory stream. Cannot be null.</param>
    /// <param name="memoryStreamUtil">An implementation of a memory stream utility used to obtain the target memory stream. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The result contains a memory stream with the form file's
    /// contents, positioned at the beginning.</returns>
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