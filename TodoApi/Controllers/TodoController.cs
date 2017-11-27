using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Model;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        private readonly TodoContext _context;
        public TodoController (TodoContext context)
        {
            _context = context;
            if(_context.Items.Count() == 0)
            {
                _context.Items.Add(new TodoItem {
                    Name = "Estudiar en casa"
                });
                _context.Items.Add(new TodoItem
                {
                    Name = "Hacer ejercicios en casa"
                });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<TodoItem> Get()
        {
            return _context.Items;
        }
        [HttpGet("{id}",Name = "GetTodoItem")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = _context.Items.SingleOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            else
            {

                return Ok(item);
            }
           
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TodoItem item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();
            return CreatedAtRoute("GetTodoItem", new { id = item.Id });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            
            var item = await _context.Items.SingleOrDefaultAsync(x => x.Id == id);
            if(item == null)
            {
                return NotFound();
            }
            else
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
                return Ok(item);
            }
       
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute]int id,[FromBody]TodoItem item)
        {
            if (!ModelState.IsValid || id != item.Id)
            {
                return BadRequest(ModelState);
            }
            _context.Entry(item).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(_context.Items.Any(x => x.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            }
            return NoContent();
           
        }

    }
  
}
