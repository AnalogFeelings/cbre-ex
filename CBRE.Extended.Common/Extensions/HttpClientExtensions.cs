namespace CBRE.Extended.Common.Extensions;

// https://gist.github.com/dalexsoto/9fd3c5bdbe9f61a717d47c5843384d11
public static class HttpClientExtensions
{
    public static async Task DownloadDataAsync(this HttpClient Client, string RequestUrl, Stream Destination, IProgress<float>? Progress = null, CancellationToken CancellationToken = default(CancellationToken))
    {
        using (HttpResponseMessage response = await Client.GetAsync(RequestUrl, HttpCompletionOption.ResponseHeadersRead, CancellationToken))
        {
            long? contentLength = response.Content.Headers.ContentLength;

            using (Stream contentStream = await response.Content.ReadAsStreamAsync(CancellationToken))
            {
                if (Progress == null || !contentLength.HasValue)
                {
                    await contentStream.CopyToAsync(Destination, CancellationToken);
                    return;
                }

                Progress<long> progressWrapper = new Progress<long>(totalBytes => Progress.Report(GetProgressPercentage(totalBytes, contentLength.Value)));
                await contentStream.CopyToAsync(Destination, contentLength.Value, progressWrapper, CancellationToken);
            }
        }

        float GetProgressPercentage(float TotalBytes, float CurrentBytes) => (TotalBytes / CurrentBytes) * 100f;
    }

    private static async Task CopyToAsync(this Stream Source, Stream Destination, long BufferSize, IProgress<long>? Progress = null, CancellationToken CancellationToken = default(CancellationToken))
    {
        if (BufferSize < 0)
            throw new ArgumentOutOfRangeException(nameof(BufferSize));
        if (Source is null)
            throw new ArgumentNullException(nameof(Source));
        if (!Source.CanRead)
            throw new InvalidOperationException($"'{nameof(Source)}' is not readable.");
        if (Destination == null)
            throw new ArgumentNullException(nameof(Destination));
        if (!Destination.CanWrite)
            throw new InvalidOperationException($"'{nameof(Destination)}' is not writable.");

        byte[] buffer = new byte[BufferSize];
        long totalBytesRead = 0;
        int bytesRead;

        while ((bytesRead = await Source.ReadAsync(buffer, 0, buffer.Length, CancellationToken).ConfigureAwait(false)) != 0)
        {
            await Destination.WriteAsync(buffer, 0, bytesRead, CancellationToken).ConfigureAwait(false);
            totalBytesRead += bytesRead;
            Progress?.Report(totalBytesRead);
        }
    }
}