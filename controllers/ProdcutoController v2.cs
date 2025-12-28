using Microsoft.AspNetCore.Mvc;
using model;

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


    //GET api/producto
    [HttpGet]
    public IActionResult Get() => Ok(productos);

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
