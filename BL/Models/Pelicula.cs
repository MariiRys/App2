using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BL.Models
{
    public partial class Pelicula
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "El nombre de la pelicula no puede contener más de 100 caracteres.")]
        [MinLength(2, ErrorMessage = "El nombre de la pelicula no puede contener menos de 2 caracteres.")]
        public string NombrePelicula { get; set; } = null!;
        public int? CategoriaId { get; set; }
        public string Director { get; set; } = null!;

        public virtual Categoria? Categoria { get; set; }
    }
}
