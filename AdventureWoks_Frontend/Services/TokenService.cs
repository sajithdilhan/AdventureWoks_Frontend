using AdventureWoks_Frontend.Models;
using Serilog;
using System.Net.Http.Headers;
using System.Text.Json;
using ILogger = Serilog.ILogger;

namespace AdventureWorks_Frontend.Services;

public class TokenService(IConfiguration configuration, ILogger logger) : ITokenService
{
    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);
    private DateTime _expiryTime;
    private string _token = string.Empty;

    public async Task<string> GetTokenAsync() 
    {
        if (string.IsNullOrEmpty(_token) || _expiryTime < DateTime.UtcNow)
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                logger.Debug("Aquiring token from API..");
                if (!string.IsNullOrEmpty(_token) && _expiryTime >= DateTime.UtcNow)
                {
                    return _token;
                }

                var credentials = configuration.GetRequiredSection("BackendCredentials");
                if (credentials == null)
                {
                    Log.Error("Error reading API credentials");
                    return string.Empty;
                }

                var user = new
                {
                    userName = credentials.GetValue<string>("Username"),
                    password = credentials.GetValue<string>("Password")
                };

                using (var client = new HttpClient())
                {
                    string baseUrl = configuration.GetValue<string>("AdventureWorksApiBaseUrl") ?? string.Empty;
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var content = new StringContent(JsonSerializer.Serialize(user), System.Text.Encoding.UTF8, "application/json");
                    var response = await client.PostAsync($"api/user/token", content);
                    response.EnsureSuccessStatusCode();

                    var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
                    if (result != null && !string.IsNullOrEmpty(result.Token))
                    {
                        _token = result.Token;
                        _expiryTime = DateTime.UtcNow.AddMinutes(2);
                        logger.Debug("TOken aquired from API and will be expired on {expiry}", _expiryTime);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
        return _token;
    }
}