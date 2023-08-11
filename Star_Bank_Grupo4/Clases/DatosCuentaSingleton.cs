using System;
using System.Collections.Generic;
using System.Text;

namespace Star_Bank_Grupo4.Clases
{
    public class DatosCuentaSingleton
    {
        private static DatosCuentaSingleton _instance;

        public string NumeroCuenta { get; set; }
        public string Saldo { get; set; }
        public string Estado { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public static DatosCuentaSingleton Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DatosCuentaSingleton();

                return _instance;
            }
        }
    }
}
