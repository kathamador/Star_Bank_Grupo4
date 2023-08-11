using Star_Bank_Grupo4.Clases;
using System;
using System.Collections.Generic;
using System.Text;

namespace Star_Bank_Grupo4.ViewModels
{
    public class ConfirmarTransferenciaViewModel : RespuestaApi
    {
        public string NumeroCuentaOrigen { get; set; }
        public string NombreClienteOrigen { get; set; }
        public string NumeroCuentaDestino { get; set; }
        public string NombreClienteDestino { get; set; }
        public decimal Monto { get; set; }
        public string Comentario { get; set; }
    }
}
