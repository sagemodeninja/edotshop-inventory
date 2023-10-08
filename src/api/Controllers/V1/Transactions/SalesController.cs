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
/// Controller for sale items.
/// </summary>
[ApiVersion(1.0)]
public class SaleItemsController : ODataController
{
    private readonly ServiceContext _context;

    /// <summary>
    /// Creates an instance of <see cref="SaleItemsController"/>.
    /// </summary>
    /// <param name="context">The database context.</param>
    public SaleItemsController(ServiceContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets all active sale items.
    /// </summary>
    /// <returns>All active sale items.</returns>
    /// <response code="200">SaleItems were successfully retrieved.</response>
    [EnableQuery]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ODataValue<IEnumerable<SaleItem>>), 200)]
    public IActionResult Get()
    {
        var saleItems = _context.SaleItems.Where(c => c.Status != GenericEntityStatus.Inactive);
        return Ok(saleItems);
    }

    /// <summary>
    /// Gets a single sale item.
    /// </summary>
    /// <param name="key">Identifier of sale item.</param>
    /// <returns>The sale item.</returns>
    /// <response code="200">SaleItem was successfully retrieved.</response>
    /// <response code="404">The sale item does not exist.</response>
    [EnableQuery]
    [Produces("application/json")]
    [ProducesResponseType(typeof(SaleItem), 200)]
    [ProducesResponseType(404)]
    public IActionResult Get(Guid key)
    {
        var saleItem = _context.SaleItems.FirstOrDefault(c => c.ObjectId == key
                                                                        && c.Status != GenericEntityStatus.Inactive);

        if (saleItem is null)
            return NotFound(key);

        return Ok(saleItem);
    }

    /// <summary>
    /// Creates a new sale item.
    /// </summary>
    /// <param name="saleItem">The sale item to create.</param>
    /// <returns>The created sale item.</returns>
    /// <response code="201">The sale item was successfully created.</response>
    /// <response code="400">The sale item is invalid.</response>
    [Produces("application/json")]
    [ProducesResponseType(typeof(SaleItem), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Post([FromBody] SaleItem saleItem)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.SaleItems.Add(saleItem);
        await _context.SaveChangesAsync();

        return Created(saleItem);
    }

    /// <summary>
    /// Updates an existing sale item.
    /// </summary>
    /// <param name="key">Identifier of sale item.</param>
    /// <param name="delta">The updated sale item.</param>
    /// <returns>The updated sale item.</returns>
    /// <response code="204">The sale item was successfully updated.</response>
    /// <response code="400">The sale item is invalid.</response>
    /// <response code="404">The sale item does not exist.</response>
    [Produces("application/json")]
    [ProducesResponseType(typeof(SaleItem), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Patch(Guid key, [FromBody] Delta<SaleItem> delta)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var saleItem = _context.SaleItems.FirstOrDefault(c => c.ObjectId == key);

        if (saleItem is null)
            return NotFound(key);

        delta.Patch(saleItem);
        await _context.SaveChangesAsync();

        return Ok(saleItem);
    }

    /// <summary>
    /// Removes a sale item.
    /// </summary>
    /// <param name="key">Identifier of sale item to remove.</param>
    /// <returns>None</returns>
    /// <response code="204">The sale item was successfully removed.</response>
    /// <response code="404">The sale item does not exist.</response>
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(Guid key)
    {
        var saleItem = _context.SaleItems.FirstOrDefault(c => c.ObjectId == key);

        if (saleItem is null)
            return NotFound(key);

        saleItem.Status = GenericEntityStatus.Inactive;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
