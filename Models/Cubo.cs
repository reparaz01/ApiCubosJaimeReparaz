﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiCubosJaimeReparaz.Models
{
    [Table("CUBOS")]
    public class Cubo
    {
        [Key]
        [Column("id_cubo")]
        public int IdCubo { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; }

        [Column("marca")]
        public string Marca { get; set; }

        [Column("Imagen")]
        public string Imagen { get; set; }

        [Column("Precio")]
        public int Precio { get; set; }

    }
}