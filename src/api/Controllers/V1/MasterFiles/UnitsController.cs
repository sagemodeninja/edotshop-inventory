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
/// Controller for units.
/// </summary>
[ApiVersion(1.0)]
public class UnitsController : ODataController
{
    private readonly ServiceContext _context;

    /// <summary>
    /// Creates an instance of <see cref="UnitsController"/>.
    /// </summary>
    /// <param name="context">The database context.</param>
    public UnitsController(ServiceContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets all active units.
    /// </summary>
    /// <returns>All active units.</returns>
    /// <response code="200">Units were successfully retrieved.</response>
    [EnableQuery]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ODataValue<IEnumerable<Unit>>), 200)]
    public IActionResult Get()
    {
        var units = _context.Units.Where(c => c.Status != GenericEntityStatus.Inactive);
        return Ok(units);
    }

    /// <summary>
    /// Gets a single unit.
    /// </summary>
    /// <param name="key">Identifier of unit.</param>
    /// <returns>The unit.</returns>
    /// <response code="200">Unit was successfully retrieved.</response>
    /// <response code="404">The unit does not exist.</response>
    [EnableQuery]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Unit), 200)]
    [ProducesResponseType(404)]
    public IActionResult Get(Guid key)
    {
        var unit = _context.Units.FirstOrDefault(c => c.ObjectId == key
                                                      && c.Status != GenericEntityStatus.Inactive);

        if (unit is null)
            return NotFound(key);

        return Ok(unit);
    }

    /// <summary>
    /// Creates a new unit.
    /// </summary>
    /// <param name="unit">The unit to create.</param>
    /// <returns>The created unit.</returns>
    /// <response code="201">The unit was successfully created.</response>
    /// <response code="400">The unit is invalid.</response>
    [Produces("application/json")]
    [ProducesResponseType(typeof(Unit), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Post([FromBody] Unit unit)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Units.Add(unit);
        await _context.SaveChangesAsync();

        return Created(unit);
    }

    /// <summary>
    /// Updates an existing unit.
    /// </summary>
    /// <param name="key">Identifier of unit.</param>
    /// <param name="delta">The updated unit.</param>
    /// <returns>The updated unit.</returns>
    /// <response code="204">The unit was successfully updated.</response>
    /// <response code="400">The unit is invalid.</response>
    /// <response code="404">The unit does not exist.</response>
    [Produces("application/json")]
    [ProducesResponseType(typeof(Unit), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Patch(Guid key, [FromBody] Delta<Unit> delta)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var unit = _context.Units.FirstOrDefault(c => c.ObjectId == key);

        if (unit is null)
            return NotFound(key);

        delta.Patch(unit);
        await _context.SaveChangesAsync();

        return Ok(unit);
    }

    /// <summary>
    /// Removes a unit.
    /// </summary>
    /// <param name="key">Identifier of unit to remove.</param>
    /// <returns>None</returns>
    /// <response code="204">The unit was successfully removed.</response>
    /// <response code="404">The unit does not exist.</response>
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(Guid key)
    {
        var unit = _context.Units.FirstOrDefault(c => c.ObjectId == key);

        if (unit is null)
            return NotFound(key);

        unit.Status = GenericEntityStatus.Inactive;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
