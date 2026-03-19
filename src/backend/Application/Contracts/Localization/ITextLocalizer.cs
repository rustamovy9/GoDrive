namespace Application.Contracts.Localization;

public interface ITextLocalizer
{
    string Get(string key);
    string Get(string key, params object[] args);
}
