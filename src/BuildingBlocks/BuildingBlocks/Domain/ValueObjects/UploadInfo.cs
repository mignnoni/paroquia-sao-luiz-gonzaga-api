namespace BuildingBlocks.Domain.ValueObjects;

public class UploadInfo : ValueObject
{
    private UploadInfo(string title, string fileName, string contentType)
    {
        Title = title;
        FileName = fileName;
        ContentType = contentType;
    }

    private UploadInfo()
    {

    }

    public string Title { get; private set; }
    public string FileName { get; private set; }
    public string ContentType { get; private set; }


    public static UploadInfo Create(string title, string fileName, string contentType)
    {
        return new UploadInfo(title, fileName, contentType);
    }

    public static UploadInfo Empty => new(string.Empty, string.Empty, string.Empty);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Title;
        yield return FileName;
        yield return ContentType;
    }
}
