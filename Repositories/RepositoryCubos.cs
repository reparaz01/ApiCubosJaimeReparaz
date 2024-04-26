using ApiCubosJaimeReparaz.Data;
using ApiCubosJaimeReparaz.Models;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;

namespace ApiCubosJaimeReparaz.Repositories
{
    public class RepositoryCubos
    {
        private CubosContext context;
        private string urlusuarios = "https://sacubos.blob.core.windows.net/usuarios/";
        private string urlcubos = "https://sacubos.blob.core.windows.net/cubos/" ;

        public RepositoryCubos(CubosContext context)
        {
            this.context = context;
        }

        #region CUBOS 

        public async Task<List<Cubo>> GetCubos()
        {
            string url = urlcubos;
            var cubos = await this.context.Cubos.ToListAsync();
            cubos.ForEach(c => c.Imagen = url + c.Imagen);

            return cubos;
        }

        public async Task<List<Cubo>> GetCubosMarca(string marca)
        {
            return await this.context.Cubos.Where(x => x.Marca == marca).ToListAsync();
        }


        #endregion

        #region USUARIOS

        public async Task<List<Usuario>> GetUsuarios()
        {
            return await this.context.Usuarios.ToListAsync();
        }

        public async Task<Usuario> FindUsuario(string email)
        {
            string url = urlusuarios;

            var usuario = (from u in context.Usuarios
                           where u.Email == email
                           select u).FirstOrDefault();

            usuario.Imagen = url + usuario.Imagen;

            return usuario;
        }


        public async Task InsertarUsuarioAsync(Usuario user)
        {
            this.context.Usuarios.Add(user);
            await this.context.SaveChangesAsync();
        }

        public async Task<Usuario> LoginAsync(string email, string password)
        {
            return await this.context.Usuarios.Where(x => x.Email == email && x.Password == password).FirstOrDefaultAsync();
        }

        #endregion

        #region COMPRAS

        public List<Compra> GetComprasUsuario(int idusuario)
        {
            return context.Compras
                .Where(p => p.IdUsuario == idusuario)
                .OrderByDescending(p => p.FechaPedido)
                .ToList();
        }

        public async Task InsertarCompraAsync(Compra compra)
        {
            this.context.Compras.Add(compra);
            await this.context.SaveChangesAsync();
        }

        #endregion







    }
}
