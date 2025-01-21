using AdventureWoks_Frontend.Models;

namespace AdventureWorks_Frontend.Services
{
    public interface IPersonService
    {
        Task<(IEnumerable<Person> persons, int total)> GetPersonsAsync(int pageNumber, int pageSize);
        Task<Person> GetPersonAsync(int id);
        Task<Person> CreatePersonAsync(Person person);
        Task<Person> UpdatePersonAsync(int id, Person person);
        Task DeletePersonAsync(int id);
    }
}