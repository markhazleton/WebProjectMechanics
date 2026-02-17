namespace WPM.Core.Models;

public record PublishableFile(
    string RelativePath,
    string Content,
    string ContentType);
