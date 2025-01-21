namespace AdventureWorks_Frontend.Services;

public interface ITokenService
{
    public Task<string> GetTokenAsync();
}
