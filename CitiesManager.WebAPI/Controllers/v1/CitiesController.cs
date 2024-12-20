using CitiesManager.WebAPI.DatabaseContext;
using CitiesManager.WebAPI.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CitiesManager.WebAPI.Controllers.v1;

[ApiVersion("1.0")]
[EnableCors("4100Client")]
public class CitiesController : CustomControllerBase
{
    private readonly ApplicationDbContext _context;

    public CitiesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Cities
    /// <summary>
    /// To get list of cities (including city id and city name) from "cities" table
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    // [Produces("application/xml")]
    public async Task<ActionResult<IEnumerable<City>>> GetCities()
    {
        if (_context.Cities == null) return NotFound();

        return await _context.Cities.ToListAsync();
    }

    // GET: api/Cities/5
    [HttpGet("{id}")]
    public async Task<ActionResult<City>> GetCity(Guid id)
    {
        if (_context.Cities == null) return NotFound();

        var city = await _context.Cities.FindAsync(id);

        if (city == null) return NotFound();

        return city;
    }

    // PUT: api/Cities/5
    // To protect from over posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCity(Guid id, [Bind(nameof(City.Id), nameof(City.Name))] City city)
    {
        // if (id != city.Id) return BadRequest();
        if (id != city.Id) return Problem(detail: "Invalid city Id", statusCode: 400, title: "City Update");

        var existingCity = await _context.Cities.FindAsync(id);

        if (existingCity is null) return NotFound();

        existingCity.Name = city.Name;

        // _context.Entry(city).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CityExists(id)) return NotFound();

            throw;
        }

        return NoContent();
    }

    // POST: api/Cities
    // To protect from over posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<City>> PostCity([Bind(nameof(City.Id), nameof(City.Name))] City city)
    {
        if (_context.Cities == null) return Problem("Entity set 'ApplicationDbContext.Cities'  is null.");

        _context.Cities.Add(city);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCity", new { id = city.Id }, city);
    }

    // DELETE: api/Cities/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCity(Guid id)
    {
        if (_context.Cities == null) return NotFound();

        var city = await _context.Cities.FindAsync(id);

        if (city == null) return NotFound();

        _context.Cities.Remove(city);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CityExists(Guid id)
    {
        return (_context.Cities?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}