using System;

public enum MediaType
{
    Image,
    Video
}

public class MediaFile
{
    public string FilePath { get; set; }
    public MediaType Type { get; set; }
    public DateTime DateCreated { get; set; }

    public MediaFile(string filePath, MediaType type, DateTime dateCreated)
    {
        FilePath = filePath;
        Type = type;
        DateCreated = dateCreated;
    }
}

