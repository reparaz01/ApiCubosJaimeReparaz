using ApiCubosJaimeReparaz.Models;
using ApiCubosJaimeReparaz.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ApiCubosJaimeReparaz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComprasController : ControllerBase
    {
        private RepositoryCubos repo;

        public ComprasController(RepositoryCubos repo)
        {
            this.repo = repo;
        }

        [Authorize]
        [HttpPost("[action]")]
        public async Task<IActionResult> RealizarPedido(Compra compra)
        {
            string jsonUsuario = HttpContext.User.FindFirst(x => x.Type == "UserData").Value;
            Usuario usuarioLogeado = JsonConvert.DeserializeObject<Usuario>(jsonUsuario);

            //LO CORRECTO SERIA HACER UN MODEL ADICIONAL PARA REALIZAR COMPRA Y QUE ESTE NO TENGA EL IDUSUARIO. PERO NO ME DA TIEMPO
            //DE ESTA FORMA DA IGUAL EL ID QUE PONGAS QUE OBTIENE EL DEL USUARIO LOGEADO

            compra.IdUsuario = usuarioLogeado.IdUsuario;
            

            
            await this.repo.InsertarCompraAsync(compra);
            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public ActionResult<List<Compra>> GetPedidosUsuario()
        {
            string jsonUsuario = HttpContext.User.FindFirst(x => x.Type == "UserData").Value;
            Usuario usuarioLogeado = JsonConvert.DeserializeObject<Usuario>(jsonUsuario);


            return this.repo.GetComprasUsuario(usuarioLogeado.IdUsuario);
        }
    }
}
