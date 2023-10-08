using Asp.Versioning;
using Asp.Versioning.OData;
using EdotShop.Contracts.Inventory;
using EdotShop.Contracts.Inventory.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace EdotShop.InventoryServices.Controllers;

/// <summary>
/// Controller for manufacturers.
/// </summary>
[ApiVersion(1.0)]
public class ManufacturersController : ODataController
{
    private readonly ServiceContext _context;

    /// <summary>
    /// Creates an instance of <see cref="ManufacturersController"/>.
    /// </summary>
    /// <param name="context">The database context.</param>
    public ManufacturersController(ServiceContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets all active manufacturers.
    /// </summary>
    /// <returns>All active manufacturers.</returns>
    /// <response code="200">Manufacturers were successfully retrieved.</response>
    [EnableQuery]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ODataValue<IEnumerable<Manufacturer>>), 200)]
    public IActionResult Get()
    {
        var manufacturers = _context.Manufacturers.Where(c => c.Status != GenericEntityStatus.Inactive);
        return Ok(manufacturers);
    }

    /// <summary>
    /// Gets a single manufacturer.
    /// </summary>
    /// <param name="key">Identifier of manufacturer.</param>
    /// <returns>The manufacturer.</returns>
    /// <response code="200">Manufacturer was successfully retrieved.</response>
    /// <response code="404">The manufacturer does not exist.</response>
    [EnableQuery]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Manufacturer), 200)]
    [ProducesResponseType(404)]
    public IActionResult Get(Guid key)
    {
        var manufacturer = _context.Manufacturers.FirstOrDefault(c => c.ObjectId == key
                                                                        && c.Status != GenericEntityStatus.Inactive);

        if (manufacturer is null)
            return NotFound(key);

        return Ok(manufacturer);
    }

    /// <summary>
    /// Creates a new manufacturer.
    /// </summary>
    /// <param name="manufacturer">The manufacturer to create.</param>
    /// <returns>The created manufacturer.</returns>
    /// <response code="201">The manufacturer was successfully created.</response>
    /// <response code="400">The manufacturer is invalid.</response>
    [Produces("application/json")]
    [ProducesResponseType(typeof(Manufacturer), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Post([FromBody] Manufacturer manufacturer)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Manufacturers.Add(manufacturer);
        await _context.SaveChangesAsync();

        return Created(manufacturer);
    }

    /// <summary>
    /// Updates an existing manufacturer.
    /// </summary>
    /// <param name="key">Identifier of manufacturer.</param>
    /// <param name="delta">The updated manufacturer.</param>
    /// <returns>The updated manufacturer.</returns>
    /// <response code="204">The manufacturer was successfully updated.</response>
    /// <response code="400">The manufacturer is invalid.</response>
    /// <response code="404">The manufacturer does not exist.</response>
    [Produces("application/json")]
    [ProducesResponseType(typeof(Manufacturer), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Patch(Guid key, [FromBody] Delta<Manufacturer> delta)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var manufacturer = _context.Manufacturers.FirstOrDefault(c => c.ObjectId == key);

        if (manufacturer is null)
            return NotFound(key);

        delta.Patch(manufacturer);
        await _context.SaveChangesAsync();

        return Ok(manufacturer);
    }

    /// <summary>
    /// Removes a manufacturer.
    /// </summary>
    /// <param name="key">Identifier of manufacturer to remove.</param>
    /// <returns>None</returns>
    /// <response code="204">The manufacturer was successfully removed.</response>
    /// <response code="404">The manufacturer does not exist.</response>
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(Guid key)
    {
        var manufacturer = _context.Manufacturers.FirstOrDefault(c => c.ObjectId == key);

        if (manufacturer is null)
            return NotFound(key);

        manufacturer.Status = GenericEntityStatus.Inactive;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
