using Application.Contracts.Localization;
using Microsoft.Extensions.Localization;

namespace Infrastructure.Localization;

public sealed class TextLocalizer(IStringLocalizer<SharedResource> localizer) : ITextLocalizer
{
    public string Get(string key) => localizer[key];

    public string Get(string key, params object[] args) => localizer[key, args];
}
