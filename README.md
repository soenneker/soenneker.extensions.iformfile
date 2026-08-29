[![](https://img.shields.io/nuget/v/soenneker.extensions.iformfile.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.iformfile/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.iformfile/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.iformfile/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.extensions.iformfile.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.iformfile/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.iformfile/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.iformfile/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Extensions.IFormFile
Copies an ASP.NET Core uploaded file into a seekable `MemoryStream` ready to read.

## Installation

```bash
dotnet add package Soenneker.Extensions.IFormFile
```

## Usage

```csharp
using Soenneker.Extensions.IFormFile;

await using MemoryStream stream = await formFile.ToMemoryStream(cancellationToken);
// stream.Position == 0
```

Both overloads copy the entire file and reset the returned stream to position zero. The default overload pre-sizes a normal `MemoryStream` when the file length fits in an `int`. The overload accepting `IMemoryStreamUtil` obtains a recyclable stream from that provider, which is preferable for repeated or larger uploads.

The caller owns the returned stream and must dispose it. Cancellation and copy failures propagate; the input file must be non-null.
