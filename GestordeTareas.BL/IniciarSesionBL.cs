using GestordeTaras.EN;
using GestordeTareas.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTareas.BL
{
    public class IniciarSesionBL
    {
        private readonly IniciarSesionDAL _iniciarSesionDAL;

        public IniciarSesionBL(IniciarSesionDAL iniciarSesionDAL)
        {
            _iniciarSesionDAL = iniciarSesionDAL;
        }
        public async Task<int> RegistrarUsuarioAsync(IniciarSesionEN inicioSesion)
        {
            return await _iniciarSesionDAL.RegistrarUsuarioAsync(inicioSesion);
        }
        public async Task<IniciarSesionEN> GetNombreUsuario(IniciarSesionEN nombreUsuario)
        {
            return await _iniciarSesionDAL.GetNombreUsuario(nombreUsuario);
        }
        public async Task<bool> VerificarCredencialesAsync(string nombreUsuario, string contrasena)
        {
            return await _iniciarSesionDAL.VerificarCredencialesAsync(nombreUsuario, contrasena);
        }
     
    }
}
