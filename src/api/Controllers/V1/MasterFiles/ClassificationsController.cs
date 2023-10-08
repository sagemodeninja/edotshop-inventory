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
/// Controller for classifications.
/// </summary>
[ApiVersion(1.0)]
public class ClassificationsController : ODataController
{
    private readonly ServiceContext _context;

    /// <summary>
    /// Creates an instance of <see cref="ClassificationsController"/>.
    /// </summary>
    /// <param name="context">The database context.</param>
    public ClassificationsController(ServiceContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets all active classifications.
    /// </summary>
    /// <returns>All active classifications.</returns>
    /// <response code="200">Classifications were successfully retrieved.</response>
    [EnableQuery]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ODataValue<IEnumerable<Classification>>), 200)]
    public IActionResult Get()
    {
        var classifications = _context.Classifications.Where(c => c.Status != GenericEntityStatus.Inactive);
        return Ok(classifications);
    }

    /// <summary>
    /// Gets a single classification.
    /// </summary>
    /// <param name="key">Identifier of classification.</param>
    /// <returns>The classification.</returns>
    /// <response code="200">Classification was successfully retrieved.</response>
    /// <response code="404">The classification does not exist.</response>
    [EnableQuery]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Classification), 200)]
    [ProducesResponseType(404)]
    public IActionResult Get(Guid key)
    {
        var classification = _context.Classifications.FirstOrDefault(c => c.ObjectId == key
                                                                        && c.Status != GenericEntityStatus.Inactive);

        if (classification is null)
            return NotFound(key);

        return Ok(classification);
    }

    /// <summary>
    /// Creates a new classification.
    /// </summary>
    /// <param name="classification">The classification to create.</param>
    /// <returns>The created classification.</returns>
    /// <response code="201">The classification was successfully created.</response>
    /// <response code="400">The classification is invalid.</response>
    [Produces("application/json")]
    [ProducesResponseType(typeof(Classification), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Post([FromBody] Classification classification)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Classifications.Add(classification);
        await _context.SaveChangesAsync();

        return Created(classification);
    }

    /// <summary>
    /// Updates an existing classification.
    /// </summary>
    /// <param name="key">Identifier of classification.</param>
    /// <param name="delta">The updated classification.</param>
    /// <returns>The updated classification.</returns>
    /// <response code="204">The classification was successfully updated.</response>
    /// <response code="400">The classification is invalid.</response>
    /// <response code="404">The classification does not exist.</response>
    [Produces("application/json")]
    [ProducesResponseType(typeof(Classification), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Patch(Guid key, [FromBody] Delta<Classification> delta)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var classification = _context.Classifications.FirstOrDefault(c => c.ObjectId == key);

        if (classification is null)
            return NotFound(key);

        delta.Patch(classification);
        await _context.SaveChangesAsync();

        return Ok(classification);
    }

    /// <summary>
    /// Removes a classification.
    /// </summary>
    /// <param name="key">Identifier of classification to remove.</param>
    /// <returns>None</returns>
    /// <response code="204">The classification was successfully removed.</response>
    /// <response code="404">The classification does not exist.</response>
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(Guid key)
    {
        var classification = _context.Classifications.FirstOrDefault(c => c.ObjectId == key);

        if (classification is null)
            return NotFound(key);

        classification.Status = GenericEntityStatus.Inactive;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
