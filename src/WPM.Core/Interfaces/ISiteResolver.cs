using WPM.Core.Models;

namespace WPM.Core.Interfaces;

public interface ISiteResolver
{
    Task<SiteContext?> ResolveAsync(string hostHeader, CancellationToken ct = default);
}
