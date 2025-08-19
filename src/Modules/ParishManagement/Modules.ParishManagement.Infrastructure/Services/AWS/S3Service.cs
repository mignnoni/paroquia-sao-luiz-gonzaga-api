using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3;
using Modules.ParishManagement.Application.Abstractions;
using Microsoft.Extensions.Options;
namespace Modules.ParishManagement.Infrastructure.Services.AWS;

public class S3Service(
    IOptions<S3ServiceOptions> _options,
    IAmazonS3 _client) : IS3Service
{
    private readonly string _bucketName = _options.Value.BucketName;
    public async Task<string> UploadFileAsync(Stream file, string name, string contentType, string extension, CancellationToken cancellationToken)
    {
        var fileName = FormatFileName(name, extension);

        var transferUtility = new TransferUtility(_client);

        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = file,
            Key = fileName,
            BucketName = _bucketName,
            ContentType = contentType
        };

        await transferUtility.UploadAsync(uploadRequest, cancellationToken);

        return fileName;
    }

    public string GetPublicUrl(string fileName)
    {
        return $"https://{_bucketName}.s3.{_client.Config.RegionEndpoint.SystemName}.amazonaws.com/{fileName}";
    }

    public async Task<string> GetPreSignedUrlAsync(string fileName)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = fileName,
            Expires = DateTime.UtcNow.AddMinutes(5)
        };

        return await _client.GetPreSignedURLAsync(request);
    }

    private static string FormatFileName(string value, string extension)
    {
        string newValue = value.Replace(" ", "_").ToLowerInvariant();

        if (newValue.Length > 50)
            newValue = newValue[..50];

        return $"{newValue}_{DateTime.Now:yyyy-MM-dd-HH-mm-ss-fff}{extension}";
    }

    public async Task DeleteFileAsync(string uploadedName, CancellationToken cancellationToken)
    {
        var deleteOjbectRequest = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = uploadedName
        };

        await _client.DeleteObjectAsync(deleteOjbectRequest, cancellationToken);
    }

    public async Task DeleteFilesAsync(List<string> fileNames, CancellationToken cancellationToken)
    {
        var deleteObjectsRequest = new DeleteObjectsRequest
        {
            BucketName = _bucketName,
            Objects = [.. fileNames.Select(x => new KeyVersion { Key = x })]
        };

        await _client.DeleteObjectsAsync(deleteObjectsRequest, cancellationToken);
    }
}