using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiCubosJaimeReparaz.Models
{
    [Table("COMPRACUBOS")]
    public class Compra
    {
        [Key]
        [Column("id_pedido")]
        public int IdPedido { get; set; }

        [Column("id_cubo")]
        public int IdCubo { get; set; }

        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Column("fechapedido")]
        public DateTime FechaPedido { get; set; }

    }
}