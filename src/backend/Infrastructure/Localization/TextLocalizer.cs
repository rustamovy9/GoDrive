using Application.Contracts.Localization;
using Microsoft.Extensions.Localization;

namespace Infrastructure.Localization;

public sealed class TextLocalizer : ITextLocalizer
{
    private readonly IStringLocalizer _localizer;

    public TextLocalizer(IStringLocalizerFactory factory)
    {
        var assemblyName = typeof(SharedResource).Assembly.GetName().Name!;
        // Resources are located in Infrastructure/Resources/SharedResource*.resx
        _localizer = factory.Create("Resources.SharedResource", assemblyName);
    }

    public string Get(string key) => _localizer[key];

    public string Get(string key, params object[] args) => _localizer[key, args];
}
