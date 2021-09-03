using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EatCleanAPI.Models;

namespace EatCleanAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenusDetailsController : ControllerBase
    {
        private readonly VegafoodBotContext _context;

        public MenusDetailsController(VegafoodBotContext context)
        {
            _context = context;
        }

        // GET: api/MenusDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenusDetail>>> GetMenusDetails()
        {
            return await _context.MenusDetails
                                            .Include(md => md.Product)
                                            .ToListAsync();
        }

        // GET: api/MenusDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<MenusDetail>>> GetMenusDetail(int id)
        {
            var menusDetail = await _context.MenusDetails
                                            .Include(md => md.Product)
                                            .Where(md => md.MenuId == id)
                                            .ToListAsync();

            if (menusDetail == null)
            {
                return NotFound();
            }

            return menusDetail;
        }

        // PUT: api/MenusDetails/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMenusDetail(int id, MenusDetail menusDetail)
        {
            if (id != menusDetail.MenuId)
            {
                return BadRequest();
            }

            _context.Entry(menusDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MenusDetailExists(id))
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

        // POST: api/MenusDetails
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<MenusDetail>> PostMenusDetail(MenusDetail menusDetail)
        {
            _context.MenusDetails.Add(menusDetail);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MenusDetailExists(menusDetail.MenuId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMenusDetail", new { id = menusDetail.MenuId }, menusDetail);
        }

        // DELETE: api/MenusDetails/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MenusDetail>> DeleteMenusDetail(int id)
        {
            var menusDetail = await _context.MenusDetails.FindAsync(id);
            if (menusDetail == null)
            {
                return NotFound();
            }

            _context.MenusDetails.Remove(menusDetail);
            await _context.SaveChangesAsync();

            return menusDetail;
        }

        private bool MenusDetailExists(int id)
        {
            return _context.MenusDetails.Any(e => e.MenuId == id);
        }
    }
}
