using System;
using System.Collections.Generic;
using System.Text;

namespace Star_Bank_Grupo4.Clases
{
    public class Transferencia
    {
        public string NumeroCuentaOrigen { get; set; }
        public string NumeroCuentaDestino { get; set; }
        public decimal Monto { get; set; }
        public string Comentario { get; set; }
        public DateTime Fecha { get; set; }
    }
}
