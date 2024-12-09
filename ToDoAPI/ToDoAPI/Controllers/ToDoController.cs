using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using ToDoAPI.Models;

namespace ToDoAPI.Controllers
{
    public class ToDoController : ApiController
    {
        private string jsonFilePath = HttpContext.Current.Server.MapPath("~/App_Data/todo.json");

        // GET: api/ToDo
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            if (!File.Exists(jsonFilePath))
            {
                File.WriteAllText(jsonFilePath, "[]");
            }

            string jsonData = File.ReadAllText(jsonFilePath);
            var todoList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ToDoItem>>(jsonData) ?? new List<ToDoItem>();
            return Ok(todoList);
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            if (!File.Exists(jsonFilePath))
            {
                return NotFound();
            }

            string jsonData = File.ReadAllText(jsonFilePath);
            var todoList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ToDoItem>>(jsonData);
            var item = todoList?.FirstOrDefault(t => t.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        // POST: api/ToDo
        [HttpPost]
        public IHttpActionResult Add([FromBody] ToDoItem newItem, [FromUri] string apiKey)
        {
            if (apiKey != "your-api-key")
            {
                return Unauthorized();
            }

            string jsonData = File.ReadAllText(jsonFilePath);
            var todoList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ToDoItem>>(jsonData) ?? new List<ToDoItem>();

            newItem.Id = todoList.Count > 0 ? todoList.Max(t => t.Id) + 1 : 1;
            newItem.Metadata = new Metadata { CreatedDate = DateTime.Now.ToString("yyyy-MM-dd") };

            todoList.Add(newItem);
            File.WriteAllText(jsonFilePath, Newtonsoft.Json.JsonConvert.SerializeObject(todoList));

            return Ok(newItem);
        }

        // PUT: api/ToDo
        [HttpPut]
        public IHttpActionResult Update(int id, [FromBody] ToDoItem updatedItem, [FromUri] string apiKey)
        {
            if (apiKey != "your-api-key")
            {
                return Unauthorized();
            }

            string jsonData = File.ReadAllText(jsonFilePath);
            var todoList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ToDoItem>>(jsonData);
            var item = todoList?.FirstOrDefault(t => t.Id == id);

            if (item != null)
            {
                item.Title = updatedItem.Title;
                File.WriteAllText(jsonFilePath, Newtonsoft.Json.JsonConvert.SerializeObject(todoList));
                return Ok(item);
            }

            return NotFound();
        }

        // DELETE: api/ToDo
        [HttpDelete]
        public IHttpActionResult Delete(int id, [FromUri] string apiKey)
        {
            if (apiKey != "your-api-key")
            {
                return Unauthorized();
            }

            string jsonData = File.ReadAllText(jsonFilePath);
            var todoList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ToDoItem>>(jsonData);
            var item = todoList?.FirstOrDefault(t => t.Id == id);

            if (item != null)
            {
                todoList.Remove(item);
                File.WriteAllText(jsonFilePath, Newtonsoft.Json.JsonConvert.SerializeObject(todoList));
                return Ok(new { message = "Deleted successfully" });
            }

            return NotFound();
        }
    }
}