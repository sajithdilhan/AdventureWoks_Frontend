using AdventureWoks_Frontend.Models;

namespace AdventureWoks_Frontend.Services
{
    public interface IPersonService
    {
        Task<IEnumerable<Person>> GetPersonsAsync(int pageNumber, int pageSize);
        Task<Person> GetPersonAsync(int id);
        Task<Person> CreatePersonAsync(Person person);
        Task<Person> UpdatePersonAsync(int id, Person person);
        Task DeletePersonAsync(int id);
    }
}