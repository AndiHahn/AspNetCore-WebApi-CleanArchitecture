#nullable enable

namespace CleanArchitecture.Shared.Core.Models
{
    public class Blob : IAsyncDisposable, IDisposable
    {
        public Blob(string contentType, DateTime? lastModified = null)
            : this(new MemoryStream(), contentType, lastModified)
        {
        }

        public Blob(Stream content, string contentType, DateTime? lastModified = null)
        {
            this.Content = content;
            this.ContentType = contentType;
            this.LastModified = lastModified ?? DateTime.UtcNow;
        }

        public Stream? Content { get; set; }
        public string ContentType { get; private set; }
        public DateTime LastModified { get; private set; }

        public void Reset()
        {
            if (this.Content is not null)
            {
                this.Content.Position = 0;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();

            Dispose(false);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Content?.Dispose();
                this.Content = null;
            }
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (this.Content is not null)
            {
                await this.Content.DisposeAsync().ConfigureAwait(false);
            }

            this.Content = null;
        }
    }
}
