[![](https://img.shields.io/nuget/v/soenneker.extensions.iformfile.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.iformfile/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.iformfile/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.iformfile/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.extensions.iformfile.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.iformfile/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.iformfile/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.iformfile/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Extensions.IFormFile
Copies an ASP.NET Core `IFormFile` into an independently owned `MemoryStream` positioned for reading.

## Installation

```bash
dotnet add package Soenneker.Extensions.IFormFile
```

## Buffer an upload

```csharp
using Soenneker.Extensions.IFormFile;

const long maxUploadBytes = 10 * 1024 * 1024;

if (formFile.Length > maxUploadBytes)
    return Results.BadRequest("The file is too large");

await using MemoryStream stream = await formFile.ToMemoryStream(cancellationToken);
await ProcessUpload(stream, cancellationToken);
```

Both overloads copy the entire file and reset the returned stream to position zero. They do not impose a size limit, validate the declared content type, scan the content, or sanitize `IFormFile.FileName`. Enforce those policies before buffering or persisting an untrusted upload.

The default overload pre-sizes a normal `MemoryStream` when the declared file length fits in an `int`:

```csharp
await using MemoryStream stream = await formFile.ToMemoryStream(cancellationToken);
```

Use the utility overload when a scoped recyclable-stream utility is already available:

```csharp
await using MemoryStream stream =
    await formFile.ToMemoryStream(memoryStreamUtil, cancellationToken);
```

The utility-provided stream is cleared before copying, so bytes from an earlier use are not retained in the result.

The caller owns the returned stream and must dispose it. If copying fails or is cancelled, the extension disposes the stream before propagating the failure. The input file and utility must be non-null.
