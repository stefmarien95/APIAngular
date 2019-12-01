﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIAngular.Models;

namespace APIAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PollUsersController : ControllerBase
    {
        private readonly PollContext _context;

        public PollUsersController(PollContext context)
        {
            _context = context;
        }

        // GET: api/PollUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PollUser>>> GetPollUsers()
        {
            return await _context.PollUsers.ToListAsync();
        }

        // GET: api/PollUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PollUser>> GetPollUser(int id)
        {
            var pollUser = await _context.PollUsers.FindAsync(id);

            if (pollUser == null)
            {
                return NotFound();
            }

            return pollUser;
        }

        // PUT: api/PollUsers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPollUser(int id, PollUser pollUser)
        {
            if (id != pollUser.PollUserID)
            {
                return BadRequest();
            }

            _context.Entry(pollUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PollUserExists(id))
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

        // POST: api/PollUsers
        [HttpPost]
        public async Task<ActionResult<PollUser>> PostPollUser(PollUser pollUser)
        {
            _context.PollUsers.Add(pollUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPollUser", new { id = pollUser.PollUserID }, pollUser);
        }

        // DELETE: api/PollUsers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PollUser>> DeletePollUser(int id)
        {
            var pollUser = await _context.PollUsers.FindAsync(id);
            if (pollUser == null)
            {
                return NotFound();
            }

            _context.PollUsers.Remove(pollUser);
            await _context.SaveChangesAsync();

            return pollUser;
        }

        private bool PollUserExists(int id)
        {
            return _context.PollUsers.Any(e => e.PollUserID == id);
        }
    }
}
