using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : Controller
    {
        private bool isNotEmptyHeader()
        {
            if (Request.Headers["userId"] != StringValues.Empty && Request.Headers["userId"].ToString() != String.Empty)
            {
                return true;
            }
            return false;
        }

        // Create
        [HttpPost]
        public IActionResult Create([FromBody] TodoItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            if (isNotEmptyHeader())
            {
                int UserId = Convert.ToInt32(Request.Headers["userId"]);

                User user = context.Users.Find(UserId);

                if (user != null)
                {

                    item.UserId = user.Id;
                    context.TodoItems.Add(item);
                    context.SaveChanges();
                }
                else
                {
                    return Unauthorized();
                }

            }

            return StatusCode(201);
        }

        // Update
        [HttpPatch]
        public IActionResult Update([FromBody] TodoItem item, string id)
        {
            if (item == null)
            {
                return BadRequest();
            }

            if (isNotEmptyHeader())
            {
                int ItemId = Convert.ToInt32(id);

                var todo = context.TodoItems.Find(ItemId);

                if (todo == null)
                {
                    return NotFound();
                }

                todo.Name = item.Name;
                todo.IsComplete = item.IsComplete;

                context.TodoItems.Update(todo);
                context.SaveChanges();

                return new NoContentResult();

            }

            return Unauthorized();
        }

        [HttpPatch("SetStatus")]
        public IActionResult ChangeStatus(bool newStatus, string id)
        {
            if (isNotEmptyHeader())
            {
                int ItemId = Convert.ToInt32(id);

                var todo = context.TodoItems.Find(ItemId);

                if (todo == null)
                {
                    return NotFound();
                }

                todo.IsComplete = newStatus;

                context.TodoItems.Update(todo);
                context.SaveChanges();

                return new NoContentResult();

            }

            return Unauthorized();
        }

        // Delete
        [HttpDelete]
        public IActionResult Delete(string id)
        {
            if (isNotEmptyHeader())
            {
                var todo = context.TodoItems.Find(Convert.ToInt32(id));
                if (todo == null)
                {
                    return NotFound();
                }

                context.TodoItems.Remove(todo);
                context.SaveChanges();

                return new NoContentResult();

            }
            return Unauthorized();


        }

        public TodoController(ApplicationContext context)
        {
            this.context = context;
        }

        public ApplicationContext context { get; set; }

        // Read
        [HttpGet]
        public IActionResult GetAll()
        {
            if (isNotEmptyHeader())
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



        
        [HttpGet("admin")]
        public IActionResult GetAllAdmin()
        {
            if (isNotEmptyHeader())
            {
                int userId = Convert.ToInt32(Request.Headers["userId"]);

                User user = context.Users.Include(u => u.TodoItems).Where(u => u.Id == userId).FirstOrDefault();

                if (user.RoleId != 1) return StatusCode(403);

                var todoItems = context.TodoItems.ToList();

                if (todoItems != null)
                {
                    return View(todoItems);
                }
            }

            return Unauthorized();
        }
        
    }
}