using Star_Bank_Grupo4.Clases;
using System;
using System.Collections.Generic;
using System.Text;

namespace Star_Bank_Grupo4.ViewModels
{
    public class IniciarTransferenciaViewModel
    {

        public IniciarTransferenciaViewModel()
        {
            CuentaOrigenSeleccionada = CuentasUsuarioLogeado[0];
        }
        public List<CuentaUsuario> CuentasUsuarioLogeado => App.UsuarioSesion.CuentasUsuario;
        public CuentaUsuario CuentaOrigenSeleccionada { get; set; }
        public string NumeroCuentaDestino { get; set; } = string.Empty;
        public decimal Monto { get; set; } = 0;
        public string Comentario { get; set; } = string.Empty;
    }
}
