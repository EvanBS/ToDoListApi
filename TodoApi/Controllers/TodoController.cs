using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : Controller
    {
        // Create
        [HttpPost]
        public IActionResult Create([FromBody] TodoItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }


            if (Request.Headers["userId"] != StringValues.Empty && Request.Headers["userId"].ToString() != String.Empty)
            {
                int UserId = Convert.ToInt32(Request.Headers["userId"]);

                User user = context.Users.Find(UserId);

                if (user != null)
                {
                    user.TodoItems.Add(item);
                }

            }


            return CreatedAtRoute("GetTodo", new { id = item.Key }, item);
        }

        // Update
        [HttpPatch("{id}")]
        public IActionResult Update([FromBody] TodoItem item, string id)
        {
            if (item == null)
            {
                return BadRequest();
            }

            var todo = context.TodoItems.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            item.Key = todo.Key;

            context.Update(item);
            return new NoContentResult();
        }

        // Delete
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var todo = context.TodoItems.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            context.Remove(id);

            return new NoContentResult();
        }


        public TodoController(ApplicationContext context)
        {
            this.context = context;
        }

        public ApplicationContext context { get; set; }
        

        [HttpGet]
        public IActionResult GetAll()
        {
            if (Request.Headers["userId"] != StringValues.Empty && Request.Headers["userId"].ToString() != String.Empty)
            {
                int userId = Convert.ToInt32(Request.Headers["userId"]);
                
                User user = context.Users.Include(u => u.TodoItems).Where(u => u.Id == userId).FirstOrDefault();

                if (user != null)
                {
                    return View(user.TodoItems.ToList());
                }

            }

            return Unauthorized();
        }

        // Read
        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(string id)
        {
            var item = context.TodoItems.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }
    }
}