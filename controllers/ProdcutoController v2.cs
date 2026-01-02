using Microsoft.AspNetCore.Mvc;
using model;
using System;
using System.Linq;

namespace Controllers.V2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProductoController : ControllerBase
{
    private static List<Producto> productos = new List<Producto>
    {
        new Producto { Id = 1, Nombre = "Producto A", Precio = 10.5m, Stock = 100 },
        new Producto { Id = 2, Nombre = "Producto B", Precio = 20.0m, Stock = 50 },
        new Producto { Id = 3, Nombre = "Producto C", Precio = 15.75m, Stock = 75 }
    };


    //GET api/producto?nombre=&minPrecio=&maxPrecio=&pageNumber=1&pageSize=10
    [HttpGet]
    public IActionResult Get([FromQuery] string? nombre = null,
                             [FromQuery] decimal? minPrecio = null,
                             [FromQuery] decimal? maxPrecio = null,
                             [FromQuery] int pageNumber = 1,
                             [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        var query = productos.AsQueryable();

        if (!string.IsNullOrWhiteSpace(nombre))
            query = query.Where(p => p.Nombre != null && p.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase));

        if (minPrecio.HasValue)
            query = query.Where(p => p.Precio >= minPrecio.Value);
        if (maxPrecio.HasValue)
            query = query.Where(p => p.Precio <= maxPrecio.Value);

        var total = query.Count();
        var items = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        var result = new PagedResult<Producto>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = total,
            TotalPages = (int)Math.Ceiling(total / (double)pageSize)
        };

        return Ok(result);
    }

    //GET api/producto/1
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var prod = productos.FirstOrDefault(p => p.Id == id);
        if (prod == null) return NotFound();
        return Ok(prod);
    }

    //POST api/producto
    [HttpPost]
    public IActionResult Post([FromBody] Producto nuevo)
    {
        nuevo.Id = productos.Max(p => p.Id) + 1;
        productos.Add(nuevo);
        return Created($"api/v2/producto/{nuevo.Id}", nuevo);
    }

    // PUT api/productos/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Producto updated)
        {
            var prod = productos.FirstOrDefault(p => p.Id == id);
            if (prod == null) return NotFound();

            prod.Nombre = updated.Nombre;
            prod.Precio = updated.Precio;
            prod.Stock = updated.Stock;

            return Ok(prod);
        }

        // DELETE api/productos/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var prod = productos.FirstOrDefault(p => p.Id == id);
            if (prod == null) return NotFound();
            productos.Remove(prod);

            return NoContent();
        }

}
