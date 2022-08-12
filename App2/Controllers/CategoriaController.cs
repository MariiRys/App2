using App2.DB;
using BL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace App1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly PeliculasDBContext _db;
        public CategoriaController(PeliculasDBContext db)
        {
            _db = db;
        }
        //metodo get (Todo el contenido)

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Categoria>))]
        [ProducesResponseType(400)] //BadRequest
        public async Task<IActionResult> GetCategorias()
        {
            var lista = await _db.Categorias.OrderBy(c => c.Nombre).ToListAsync();
            return Ok(lista);
        }

        //Un registro en especifico
        [HttpGet("{id:int}", Name = "GetCategoria")]
        [ProducesResponseType(200, Type = typeof(Categoria))]
        [ProducesResponseType(400)] //BadRequest
        [ProducesResponseType(404)] //Not found
        public async Task<IActionResult> GetCategoria(int id)
        {
            var obj = await _db.Categorias.FirstOrDefaultAsync(c => c.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }

        //Metodo Post(Agregar registros)
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)] //Error interno
        public async Task<IActionResult> CrearCategoria([FromBody] Categoria categoria)
        {
            if (categoria == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!ValidaCategoria(categoria.Nombre))
            {

                await _db.AddAsync(categoria);
                await _db.SaveChangesAsync();

                return CreatedAtRoute("GetCategoria", new { id = categoria.Id }, categoria);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        private bool ValidaCategoria(string nom_categoria )
        {
            return _db.Categorias.Any(c => c.Nombre == nom_categoria);
        }
    }

}


