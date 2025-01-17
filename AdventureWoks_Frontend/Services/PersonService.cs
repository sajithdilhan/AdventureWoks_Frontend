using AdventureWoks_Frontend.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace AdventureWoks_Frontend.Services
{
    public class PersonService : IPersonService
    {
        ILogger<PersonService> _logger;
        IConfiguration _configuration;

        public PersonService(ILogger<PersonService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

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
            try
            {
                string baseUrl = _configuration.GetValue<string>("AdventureWorksApiBaseUrl") ?? string.Empty;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await client.GetAsync($"api/Person/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadFromJsonAsync<Person>();
                        return json ?? new Person();
                    }
                    else
                    {
                        _logger.LogError("Failed to get persons: {StatusCode} {Reason}", response.StatusCode, response.ReasonPhrase);
                        return new Person();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Person>> GetPersonsAsync(int pageNumber, int pageSize)
        {
            try
            {
                string baseUrl = _configuration.GetValue<string>("AdventureWorksApiBaseUrl") ?? string.Empty;

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await client.GetAsync($"api/Person?PageSize={pageSize}&PageNumber={pageNumber}");

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadFromJsonAsync<ApiResponse>();
                        if (json != null && json.Data?.Persons?.Count > 0)
                        {
                            return json.Data.Persons;
                        }
                        else
                        {
                            return Enumerable.Empty<Person>();
                        }
                    }
                    else
                    {
                        _logger.LogError("Failed to get persons: {StatusCode} {Reason}", response.StatusCode, response.ReasonPhrase);
                        return Enumerable.Empty<Person>();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get persons");
                return Enumerable.Empty<Person>();
            }

        }
        public Task<Person> UpdatePersonAsync(int id, Person person)
        {
            throw new NotImplementedException();
        }
    }
}
