using CBRE.Localization;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CBRE.Common.Extensions
{
    // https://gist.github.com/dalexsoto/9fd3c5bdbe9f61a717d47c5843384d11
    public static class HttpClientExtensions
    {
        public static async Task DownloadDataAsync(this HttpClient client, string requestUrl, Stream destination, IProgress<float> progress = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (HttpResponseMessage response = await client.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                long? contentLength = response.Content.Headers.ContentLength;

                using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                {
                    if (progress == null || !contentLength.HasValue)
                    {
                        await contentStream.CopyToAsync(destination);
                        return;
                    }

                    Progress<long> progressWrapper = new Progress<long>(totalBytes => progress.Report(GetProgressPercentage(totalBytes, contentLength.Value)));
                    await contentStream.CopyToAsync(destination, contentLength.Value, progressWrapper, cancellationToken);
                }
            }

            float GetProgressPercentage(float totalBytes, float currentBytes) => (totalBytes / currentBytes) * 100f;
        }

        private static async Task CopyToAsync(this Stream source, Stream destination, long bufferSize, IProgress<long> progress = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (!source.CanRead)
                throw new InvalidOperationException(Local.LocalString("exception.not_readable", nameof(source)));
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
            if (!destination.CanWrite)
                throw new InvalidOperationException(Local.LocalString("exception.not_writable", nameof(destination)));

            byte[] buffer = new byte[bufferSize];
            long totalBytesRead = 0;
            int bytesRead;

            while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false)) != 0)
            {
                await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken).ConfigureAwait(false);
                totalBytesRead += bytesRead;
                progress?.Report(totalBytesRead);
            }
        }
    }
}
