using System.Net.Http.Headers;
using AdventureWoks_Frontend.Models;
using Newtonsoft.Json;
using ILogger = Serilog.ILogger;

namespace AdventureWorks_Frontend.Services;

public class PersonService(ITokenService tokenService, ILogger logger, IHttpClientFactory _httpClientFactory) : IPersonService
{
    public Task<Person> CreatePersonAsync(Person person)
    {
        throw new NotImplementedException();
    }

    public Task DeletePersonAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Person> GetPersonAsync(int id)
    {
        var token = await tokenService.GetTokenAsync();
        var client = _httpClientFactory.CreateClient("AdventureServiceClient");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync($"api/Person/{id}");
        response.EnsureSuccessStatusCode();
            
        var json = await response.Content.ReadFromJsonAsync<Person>();
        logger.Debug($"Received success response for GetPerson Id :{id}, Content:{JsonConvert.SerializeObject(json)}");
        return json ?? new Person();
    }

    public async Task<(IEnumerable<Person>, int)> GetPersonsAsync(int pageNumber, int pageSize)
    {
        try
        {
            var token = await tokenService.GetTokenAsync();
            var client = _httpClientFactory.CreateClient("AdventureServiceClient");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"api/Person?PageSize={pageSize}&PageNumber={pageNumber}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return json is { Data.Persons.Count: > 0 } ? (json.Data.Persons, json.MetaData?.Total ?? 0) : ([], 0);
        }
        catch (HttpRequestException ux)
        {
            logger.Error(ux,
                ux.StatusCode == System.Net.HttpStatusCode.Unauthorized
                    ? "Unauthorized access"
                    : $"Unexpected result:{ux.StatusCode}, {ux.Message}");

            return ([], 0);
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Failed to get persons");
            return ([], 0);
        }
    }
    public Task<Person> UpdatePersonAsync(int id, Person person)
    {
        throw new NotImplementedException();
    }
}