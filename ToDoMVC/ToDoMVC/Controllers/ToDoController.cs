using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using ToDoMVC.Models;

namespace ToDoMVC.Controllers
{
    public class ToDoController : Controller
    {
        private string apiUrl = "https://localhost:44350/api/ToDo";
        private string apiKey = "your-api-key";

        public async Task<ActionResult> Index()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var todoList = await response.Content.ReadAsAsync<List<ToDoItem>>();
                    return View(todoList);
                }
                return View(new List<ToDoItem>());
            }
        }
        public async Task<ActionResult> Edit(int id)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{apiUrl}/{id}?apiKey={apiKey}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var todoItem = Newtonsoft.Json.JsonConvert.DeserializeObject<ToDoItem>(jsonString);
                    return View(todoItem);
                }
                return HttpNotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Add(ToDoItem item)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync($"{apiUrl}?apiKey={apiKey}", item);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return View("Index");
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditConfirmed(ToDoItem updatedItem)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PutAsJsonAsync($"{apiUrl}?id={updatedItem.Id}&apiKey={apiKey}", updatedItem);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return View("Edit", updatedItem);
            }
        }



        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            using (var client = new HttpClient())
            {
                var response = await client.DeleteAsync($"{apiUrl}?id={id}&apiKey={apiKey}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return View("Index");
            }
        }
    }
}