using AdventureWoks_Frontend.Models;
using AdventureWorks_Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdventureWorks_Frontend.Controllers
{
    public class PersonController(IPersonService personService) : Controller
    {
        // GET: PersonController
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
            (IEnumerable<Person> persons, int total) = await personService.GetPersonsAsync(pageNumber ?? 0, pageSize);

            return View(PaginatedList<Person>.Create(persons, pageNumber ?? 0, pageSize, total));
        }

        // GET: PersonController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var person = await personService.GetPersonAsync(id);
            return View(person);
        }

        // GET: PersonController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PersonController/Create
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
