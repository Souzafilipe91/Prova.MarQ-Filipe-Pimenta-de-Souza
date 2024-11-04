using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prova.MarQ.Domain.Entities;
using Prova.MarQ.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Prova.MarQ.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly ProvaMarqDbContext _context;

        public EmployeeController(ProvaMarqDbContext context)
        {
            _context = context;
        }

        // GET: api/Employee
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees.Where(e => !e.IsDeleted).ToListAsync();
        }

        // GET: api/Employee/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null || employee.IsDeleted)
            {
                return NotFound();
            }

            return employee;
        }

        // POST: api/Employee
        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
        {
            // Verifica se o documento e o PIN são únicos
            if (_context.Employees.Any(e => e.Documento == employee.Documento))
                return BadRequest("Documento já existe.");

            if (_context.Employees.Any(e => e.PIN == employee.PIN))
                return BadRequest("PIN já existe.");

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeId }, employee);
        }

        // PUT: api/Employee/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null || employee.IsDeleted)
            {
                return NotFound();
            }

            // Soft delete
            employee.IsDeleted = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Employee/register-point
        [HttpPost("register-point")]
        public async Task<IActionResult> RegisterPoint([FromBody] string pin)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.PIN == pin && !e.IsDeleted);

            if (employee == null)
            {
                return NotFound("Funcionário não encontrado ou excluído.");
            }

            var timeEntry = new TimeEntry
            {
                EmployeeId = employee.EmployeeId,
                Timestamp = DateTime.UtcNow
            };

            _context.TimeEntries.Add(timeEntry);
            await _context.SaveChangesAsync();

            return Ok("Ponto registrado com sucesso.");
        }

        
        [HttpGet("report")]
        public async Task<IActionResult> GetReport(DateTime startDate, DateTime endDate, string? document = null)
        {
            var query = _context.TimeEntries
                                .Where(te => te.Timestamp.Date >= startDate && te.Timestamp.Date <= endDate)
                                .Join(_context.Employees,
                                      te => te.EmployeeId,
                                      e => e.EmployeeId,
                                      (te, e) => new { TimeEntry = te, Employee = e });

            if (!string.IsNullOrEmpty(document))
            {
                query = query.Where(q => q.Employee.Documento == document);
            }

            var report = await query
                .GroupBy(q => new { Date = q.TimeEntry.Timestamp.Date, Employee = q.Employee })
                .Select(g => new
                {
                    Date = g.Key.Date,
                    EmployeeName = g.Key.Employee.Name,
                    Documento = g.Key.Employee.Documento,
                    PointsPerDay = g.Count(),
                    TotalWorked = g.Sum(x => (DateTime.UtcNow - x.TimeEntry.Timestamp).TotalHours), 
                    ExtraHours = g.Sum(x => (DateTime.UtcNow - x.TimeEntry.Timestamp).TotalHours) - 8,
                    DayOfWeek = g.Key.Date.DayOfWeek.ToString()
                })
                .ToListAsync();

            return Ok(report);
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
