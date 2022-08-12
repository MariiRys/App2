using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BL.Models
{
    public partial class Categoria
    {
        public Categoria()
        {
            Peliculas = new HashSet<Pelicula>();
        }
        public int Id { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "El nombre de la categoria no puede contener más de 100 caracteres.")]
        [MinLength(2, ErrorMessage = "El nombre de la categoria no puede contener menos de 2 caracteres.")]
        public string Nombre { get; set; } = null!;
        public bool Estado { get; set; }

        public virtual ICollection<Pelicula> Peliculas { get; set; }
    }
}
