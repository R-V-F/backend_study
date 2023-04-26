using ApiApp.Data;
using ApiApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ApiApp.ViewModel;
using System;

namespace ApiApp.Controller
{
    [ApiController]
    [Route(template:"v1")]
    public class TodoController : ControllerBase
    {
        [HttpGet]
        [Route(template: "todos")]
        public async Task<IActionResult> GetAsync(
            [FromServices] AppDbContext context)
        {
            var todos = await context.Todos.ToListAsync();
            return Ok(todos);
        }

        [HttpGet]
        [Route(template: "todos/{id}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromServices] AppDbContext context,
            [FromRoute] int id)
        {
            var todo = await context.Todos.FirstOrDefaultAsync(x => x.Id == id);
            return todo == null
                ? NotFound()
                : Ok(todo);
        }

        [HttpPost]
        [Route(template: "todos")]
        public async Task<IActionResult> PostAsync(
            [FromServices] AppDbContext context,
            [FromBody] CreateViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var todo = new Todo
            {
                Title = model.Title,
                Done = false,

            };
            await context.Todos.AddAsync( todo );
            await context.SaveChangesAsync();
            return Created($"v1/todos/{todo.Id}", todo);

        }

        [HttpPut]
        [Route(template: "todos/{id}")]
        public async Task<IActionResult> PutAsync(
            [FromServices] AppDbContext context,
            [FromBody] CreateViewModel model,
            [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var todo = await context
                .Todos
                .FirstOrDefaultAsync(x => x.Id == id);
            if (todo == null)
                return NotFound();

            try
            {
                todo.Title = model.Title;
                context.Todos.Update(todo);
                await context.SaveChangesAsync();

                return Ok(todo);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route(template: "todos/{id}")]
        public async Task<IActionResult> DeleteAsync(
            [FromServices] AppDbContext context,
            [FromRoute] int id)
        {
            var todo = await context
                .Todos
                .FirstOrDefaultAsync(x => x.Id == id);
            if (todo == null)
                return NotFound();

            try
            {
                context.Todos.Remove(todo);
                await context.SaveChangesAsync();

                return Ok(todo);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
