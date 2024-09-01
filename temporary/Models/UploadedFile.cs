namespace temporary.Models;

public partial class UploadedFile
{
    public int Ufid { get; set; }

    public string? FileName { get; set; }

    public string? ContentType { get; set; }

    public long? FileSize { get; set; }

    public string? FolderName { get; set; }

    public string? Status { get; set; }

    public int? UserId { get; set; }

    public DateTime? DocumentAddedDate { get; set; }

    public DateTime? DocumentModifiedDate { get; set; }
}
