namespace Application.Interfaces;

public interface IFileStorage
{
    Task<string> SaveAsync(
        Stream content,
        string fileName,
        string? contentType,
        CancellationToken cancellationToken = default);

    Task<Stream?> OpenReadAsync(string storedPath, CancellationToken cancellationToken = default);
}

