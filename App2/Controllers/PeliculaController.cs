using App2.DB;
using BL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeliculaController : ControllerBase
    {
        private readonly PeliculasDBContext _db;
        public PeliculaController(PeliculasDBContext db)
        {
            _db = db;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<PaginadorGenerico<Pelicula>>> GetPeli(int pagina = 1,
                                                                                int registros_por_pagina = 10)
        {
            List<Pelicula> _Pelicula;
            PaginadorGenerico<Pelicula> _PaginadorPeli;
            // Recuperamos el 'DbSet' completo
            _Pelicula = await _db.Peliculas.OrderBy(p => p.NombrePelicula).Include(p => p.Categoria).ToListAsync();

            ///////////////////////////
            // SISTEMA DE PAGINACIÓN //
            ///////////////////////////
            int _TotalRegistros = 0;
            int _TotalPaginas = 0;
            // Número total de registros de la tabla Customers
            _TotalRegistros = _Pelicula.Count();
            // Obtenemos la 'página de registros' de la tabla Customers
            _Pelicula = _Pelicula.Skip((pagina - 1) * registros_por_pagina)
                                             .Take(registros_por_pagina)
                                             .ToList();
            // Número total de páginas de la tabla Customers
            _TotalPaginas = (int)Math.Ceiling((double)_TotalRegistros / registros_por_pagina);

            // Instanciamos la 'Clase de paginación' y asignamos los nuevos valores
            _PaginadorPeli = new PaginadorGenerico<Pelicula>()
            {
                RegistrosPorPagina = registros_por_pagina,
                TotalRegistros = _TotalRegistros,
                TotalPaginas = _TotalPaginas,
                PaginaActual = pagina,
                Resultado = _Pelicula
            };

            return _PaginadorPeli;
        }
      

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPelicula(int id)
        {
            var obj = await _db.Peliculas.Include(p => p.Categoria).FirstOrDefaultAsync(p => p.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CrearPelicula([FromBody] Pelicula pelicula)
        {
            if (pelicula == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!ValidaPeli(pelicula.NombrePelicula))
            {

                await _db.AddAsync(pelicula);
                await _db.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        private bool ValidaPeli(string nom_categoria)
        {
            return _db.Peliculas.Any(c => c.NombrePelicula == nom_categoria);
        }
    }
}

