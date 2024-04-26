using ApiCubosJaimeReparaz.Models;
using ApiCubosJaimeReparaz.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ApiCubosJaimeReparaz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private RepositoryCubos repo;

        public UsuariosController(RepositoryCubos repo)
        {
            this.repo = repo;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Registro(Usuario user)
        {
            await this.repo.InsertarUsuarioAsync(user);
            return Ok();
        }

        [Authorize]
        [HttpPost("[action]")]
        public async Task<IActionResult> Detalles()
        {

            string jsonUsuario = HttpContext.User.FindFirst(x => x.Type == "UserData").Value;
            Usuario usuarioLogeado = JsonConvert.DeserializeObject<Usuario>(jsonUsuario);

            var usuario = await this.repo.FindUsuario(usuarioLogeado.Email);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

    }
}