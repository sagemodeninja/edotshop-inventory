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
/// Controller for suppliers.
/// </summary>
[ApiVersion(1.0)]
public class SuppliersController : ODataController
{
    private readonly ServiceContext _context;

    /// <summary>
    /// Creates an instance of <see cref="SuppliersController"/>.
    /// </summary>
    /// <param name="context">The database context.</param>
    public SuppliersController(ServiceContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets all active suppliers.
    /// </summary>
    /// <returns>All active suppliers.</returns>
    /// <response code="200">Suppliers were successfully retrieved.</response>
    [EnableQuery]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ODataValue<IEnumerable<Supplier>>), 200)]
    public IActionResult Get()
    {
        var suppliers = _context.Suppliers.Where(c => c.Status != GenericEntityStatus.Inactive);
        return Ok(suppliers);
    }

    /// <summary>
    /// Gets a single supplier.
    /// </summary>
    /// <param name="key">Identifier of supplier.</param>
    /// <returns>The supplier.</returns>
    /// <response code="200">Supplier was successfully retrieved.</response>
    /// <response code="404">The supplier does not exist.</response>
    [EnableQuery]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Supplier), 200)]
    [ProducesResponseType(404)]
    public IActionResult Get(Guid key)
    {
        var supplier = _context.Suppliers.FirstOrDefault(c => c.ObjectId == key
                                                              && c.Status != GenericEntityStatus.Inactive);

        if (supplier is null)
            return NotFound(key);

        return Ok(supplier);
    }

    /// <summary>
    /// Creates a new supplier.
    /// </summary>
    /// <param name="supplier">The supplier to create.</param>
    /// <returns>The created supplier.</returns>
    /// <response code="201">The supplier was successfully created.</response>
    /// <response code="400">The supplier is invalid.</response>
    [Produces("application/json")]
    [ProducesResponseType(typeof(Supplier), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Post([FromBody] Supplier supplier)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();

        return Created(supplier);
    }

    /// <summary>
    /// Updates an existing supplier.
    /// </summary>
    /// <param name="key">Identifier of supplier.</param>
    /// <param name="delta">The updated supplier.</param>
    /// <returns>The updated supplier.</returns>
    /// <response code="204">The supplier was successfully updated.</response>
    /// <response code="400">The supplier is invalid.</response>
    /// <response code="404">The supplier does not exist.</response>
    [Produces("application/json")]
    [ProducesResponseType(typeof(Supplier), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Patch(Guid key, [FromBody] Delta<Supplier> delta)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var supplier = _context.Suppliers.FirstOrDefault(c => c.ObjectId == key);

        if (supplier is null)
            return NotFound(key);

        delta.Patch(supplier);
        await _context.SaveChangesAsync();

        return Ok(supplier);
    }

    /// <summary>
    /// Removes a supplier.
    /// </summary>
    /// <param name="key">Identifier of supplier to remove.</param>
    /// <returns>None</returns>
    /// <response code="204">The supplier was successfully removed.</response>
    /// <response code="404">The supplier does not exist.</response>
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(Guid key)
    {
        var supplier = _context.Suppliers.FirstOrDefault(c => c.ObjectId == key);

        if (supplier is null)
            return NotFound(key);

        supplier.Status = GenericEntityStatus.Inactive;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
