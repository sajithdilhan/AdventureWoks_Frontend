using AdventureWoks_Frontend.Models;
using AdventureWorks_Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace AdventureWorks_Frontend.Controllers
{
    public class PersonsController(IPersonService personService, IDistributedCache distributedCache, Serilog.ILogger logger) : Controller
    {
        // GET: PersonsController
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                pageNumber = 0;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            int pageSize = 10;
            (IEnumerable<Person> persons, int total) = await personService.GetPersonsAsync(pageNumber ?? 0, pageSize).ConfigureAwait(true);
            logger.Debug("Received persons list from api. Count:{count}, Total{total}", persons.Count(), total);

            return View(PaginatedList<Person>.Create(persons, pageNumber ?? 0, pageSize, total));
        }

        // GET: PersonsController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                string key = $"person-{id}";
                Person person = new();
                CancellationTokenSource cts = new(1000);
                string? personCache = await distributedCache.GetStringAsync(key, cts.Token);
                if (string.IsNullOrEmpty(personCache))
                {
                    person = await personService.GetPersonAsync(id);
                    if (person.BusinessEntityId != 0)
                    {
                        await distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(person));
                    }

                }
                else
                {
                    person = JsonConvert.DeserializeObject<Person>(personCache) ?? person;
                }

                return View(person);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return View(new Person());
            }            
        }

        // GET: PersonsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PersonsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PersonController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PersonController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PersonController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PersonController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
