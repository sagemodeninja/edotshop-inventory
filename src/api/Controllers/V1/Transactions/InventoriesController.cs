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
/// Controller for inventories.
/// </summary>
[ApiVersion(1.0)]
public class InventoriesController : ODataController
{
    private readonly ServiceContext _context;

    /// <summary>
    /// Creates an instance of <see cref="InventoriesController"/>.
    /// </summary>
    /// <param name="context">The database context.</param>
    public InventoriesController(ServiceContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets all active inventories.
    /// </summary>
    /// <returns>All active inventories.</returns>
    /// <response code="200">Inventories were successfully retrieved.</response>
    [EnableQuery]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ODataValue<IEnumerable<Inventory>>), 200)]
    public IActionResult Get()
    {
        var inventories = _context.Inventories.Where(c => c.Status != GenericEntityStatus.Inactive);
        return Ok(inventories);
    }

    /// <summary>
    /// Gets a single inventory.
    /// </summary>
    /// <param name="key">Identifier of inventory.</param>
    /// <returns>The inventory.</returns>
    /// <response code="200">Inventory was successfully retrieved.</response>
    /// <response code="404">The inventory does not exist.</response>
    [EnableQuery]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Inventory), 200)]
    [ProducesResponseType(404)]
    public IActionResult Get(Guid key)
    {
        var inventory = _context.Inventories.FirstOrDefault(c => c.ObjectId == key
                                                                 && c.Status != GenericEntityStatus.Inactive);

        if (inventory is null)
            return NotFound(key);

        return Ok(inventory);
    }

    /// <summary>
    /// Creates a new inventory.
    /// </summary>
    /// <param name="inventory">The inventory to create.</param>
    /// <returns>The created inventory.</returns>
    /// <response code="201">The inventory was successfully created.</response>
    /// <response code="400">The inventory is invalid.</response>
    [Produces("application/json")]
    [ProducesResponseType(typeof(Inventory), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Post([FromBody] Inventory inventory)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Inventories.Add(inventory);
        await _context.SaveChangesAsync();

        return Created(inventory);
    }

    /// <summary>
    /// Updates an existing inventory.
    /// </summary>
    /// <param name="key">Identifier of inventory.</param>
    /// <param name="delta">The updated inventory.</param>
    /// <returns>The updated inventory.</returns>
    /// <response code="204">The inventory was successfully updated.</response>
    /// <response code="400">The inventory is invalid.</response>
    /// <response code="404">The inventory does not exist.</response>
    [Produces("application/json")]
    [ProducesResponseType(typeof(Inventory), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Patch(Guid key, [FromBody] Delta<Inventory> delta)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var inventory = _context.Inventories.FirstOrDefault(c => c.ObjectId == key);

        if (inventory is null)
            return NotFound(key);

        delta.Patch(inventory);
        await _context.SaveChangesAsync();

        return Ok(inventory);
    }

    /// <summary>
    /// Removes a inventory.
    /// </summary>
    /// <param name="key">Identifier of inventory to remove.</param>
    /// <returns>None</returns>
    /// <response code="204">The inventory was successfully removed.</response>
    /// <response code="404">The inventory does not exist.</response>
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(Guid key)
    {
        var inventory = _context.Inventories.FirstOrDefault(c => c.ObjectId == key);

        if (inventory is null)
            return NotFound(key);

        inventory.Status = GenericEntityStatus.Inactive;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
