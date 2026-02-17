using WPM.Core.Models;

namespace WPM.Core.Interfaces;

public interface IContentPublisher
{
    string DomainId { get; }

    Task<IReadOnlyList<PublishableFile>> PublishAsync(
        SiteContext context,
        CancellationToken ct = default);

    Task<IReadOnlyList<PublishableFile>> PublishItemAsync(
        SiteContext context,
        string itemId,
        CancellationToken ct = default);
}
