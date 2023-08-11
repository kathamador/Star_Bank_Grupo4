using System;
using System.Collections.Generic;
using System.Text;

namespace Star_Bank_Grupo4.Clases
{
    public class CuentaUsuario
    {
        public CuentaUsuario()
        {

        }
        public CuentaUsuario(string cuenta, string nombreCliente)
        {
            Cuenta = cuenta;
            NombreCliente = nombreCliente;
        }

        public string Cuenta { get; set; }
        public string NombreCliente { get; set; }
    }
}
