namespace Modules.ParishManagement.Application.Abstractions;

public interface IS3Service
{
    Task<string> UploadFileAsync(Stream file, string name, string contentType, string extension, CancellationToken cancellationToken = default);
    string GetPublicUrl(string fileName);
    Task<string> GetPreSignedUrlAsync(string fileName);
    Task DeleteFileAsync(string uploadedName, CancellationToken cancellationToken = default);
    Task DeleteFilesAsync(List<string> fileNames, CancellationToken cancellationToken = default);
}