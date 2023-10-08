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
/// Controller for inventory items.
/// </summary>
[ApiVersion(1.0)]
public class InventoryItemsController : ODataController
{
    private readonly ServiceContext _context;

    /// <summary>
    /// Creates an instance of <see cref="InventoryItemsController"/>.
    /// </summary>
    /// <param name="context">The database context.</param>
    public InventoryItemsController(ServiceContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets all active inventory items.
    /// </summary>
    /// <returns>All active inventory items.</returns>
    /// <response code="200">Inventory items were successfully retrieved.</response>
    [EnableQuery]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ODataValue<IEnumerable<InventoryItem>>), 200)]
    public IActionResult Get()
    {
        var inventoryItems = _context.InventoryItems.Where(c => c.Status != GenericEntityStatus.Inactive);
        return Ok(inventoryItems);
    }

    /// <summary>
    /// Gets a single inventory item.
    /// </summary>
    /// <param name="key">Identifier of inventoryItem.</param>
    /// <returns>The inventory item.</returns>
    /// <response code="200">Inventory item was successfully retrieved.</response>
    /// <response code="404">The inventory item does not exist.</response>
    [EnableQuery]
    [Produces("application/json")]
    [ProducesResponseType(typeof(InventoryItem), 200)]
    [ProducesResponseType(404)]
    public IActionResult Get(Guid key)
    {
        var inventoryItem = _context.InventoryItems.FirstOrDefault(c => c.ObjectId == key
                                                                        && c.Status != GenericEntityStatus.Inactive);

        if (inventoryItem is null)
            return NotFound(key);

        return Ok(inventoryItem);
    }

    /// <summary>
    /// Creates a new inventory item.
    /// </summary>
    /// <param name="inventoryItem">The inventory item to create.</param>
    /// <returns>The created inventory item.</returns>
    /// <response code="201">The inventory item was successfully created.</response>
    /// <response code="400">The inventory item is invalid.</response>
    [Produces("application/json")]
    [ProducesResponseType(typeof(InventoryItem), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Post([FromBody] InventoryItem inventoryItem)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.InventoryItems.Add(inventoryItem);
        await _context.SaveChangesAsync();

        return Created(inventoryItem);
    }

    /// <summary>
    /// Updates an existing inventory item.
    /// </summary>
    /// <param name="key">Identifier of inventory item.</param>
    /// <param name="delta">The updated inventory item.</param>
    /// <returns>The updated inventory item.</returns>
    /// <response code="204">The inventory item was successfully updated.</response>
    /// <response code="400">The inventory item is invalid.</response>
    /// <response code="404">The inventory item does not exist.</response>
    [Produces("application/json")]
    [ProducesResponseType(typeof(InventoryItem), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Patch(Guid key, [FromBody] Delta<InventoryItem> delta)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var inventoryItem = _context.InventoryItems.FirstOrDefault(c => c.ObjectId == key);

        if (inventoryItem is null)
            return NotFound(key);

        delta.Patch(inventoryItem);
        await _context.SaveChangesAsync();

        return Ok(inventoryItem);
    }

    /// <summary>
    /// Removes a inventory item.
    /// </summary>
    /// <param name="key">Identifier of inventory item to remove.</param>
    /// <returns>None</returns>
    /// <response code="204">The inventory item was successfully removed.</response>
    /// <response code="404">The inventory item does not exist.</response>
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(Guid key)
    {
        var inventoryItem = _context.InventoryItems.FirstOrDefault(c => c.ObjectId == key);

        if (inventoryItem is null)
            return NotFound(key);

        inventoryItem.Status = GenericEntityStatus.Inactive;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
