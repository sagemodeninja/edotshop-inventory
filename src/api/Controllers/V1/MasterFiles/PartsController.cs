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
/// Controller for parts.
/// </summary>
[ApiVersion(1.0)]
public class PartsController : ODataController
{
    private readonly ServiceContext _context;

    /// <summary>
    /// Creates an instance of <see cref="PartsController"/>.
    /// </summary>
    /// <param name="context">The database context.</param>
    public PartsController(ServiceContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets all active parts.
    /// </summary>
    /// <returns>All active parts.</returns>
    /// <response code="200">Parts were successfully retrieved.</response>
    [EnableQuery]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ODataValue<IEnumerable<Part>>), 200)]
    public IActionResult Get()
    {
        var parts = _context.Parts.Where(c => c.Status != GenericEntityStatus.Inactive);
        return Ok(parts);
    }

    /// <summary>
    /// Gets a single part.
    /// </summary>
    /// <param name="key">Identifier of part.</param>
    /// <returns>The part.</returns>
    /// <response code="200">Part was successfully retrieved.</response>
    /// <response code="404">The part does not exist.</response>
    [EnableQuery]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Part), 200)]
    [ProducesResponseType(404)]
    public IActionResult Get(Guid key)
    {
        var part = _context.Parts.FirstOrDefault(c => c.ObjectId == key
                                                      && c.Status != GenericEntityStatus.Inactive);

        if (part is null)
            return NotFound(key);

        return Ok(part);
    }

    /// <summary>
    /// Creates a new part.
    /// </summary>
    /// <param name="part">The part to create.</param>
    /// <returns>The created part.</returns>
    /// <response code="201">The part was successfully created.</response>
    /// <response code="400">The part is invalid.</response>
    [Produces("application/json")]
    [ProducesResponseType(typeof(Part), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Post([FromBody] Part part)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Parts.Add(part);
        await _context.SaveChangesAsync();

        return Created(part);
    }

    /// <summary>
    /// Updates an existing part.
    /// </summary>
    /// <param name="key">Identifier of part.</param>
    /// <param name="delta">The updated part.</param>
    /// <returns>The updated part.</returns>
    /// <response code="204">The part was successfully updated.</response>
    /// <response code="400">The part is invalid.</response>
    /// <response code="404">The part does not exist.</response>
    [Produces("application/json")]
    [ProducesResponseType(typeof(Part), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Patch(Guid key, [FromBody] Delta<Part> delta)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var part = _context.Parts.FirstOrDefault(c => c.ObjectId == key);

        if (part is null)
            return NotFound(key);

        delta.Patch(part);
        await _context.SaveChangesAsync();

        return Ok(part);
    }

    /// <summary>
    /// Removes a part.
    /// </summary>
    /// <param name="key">Identifier of part to remove.</param>
    /// <returns>None</returns>
    /// <response code="204">The part was successfully removed.</response>
    /// <response code="404">The part does not exist.</response>
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(Guid key)
    {
        var part = _context.Parts.FirstOrDefault(c => c.ObjectId == key);

        if (part is null)
            return NotFound(key);

        part.Status = GenericEntityStatus.Inactive;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
