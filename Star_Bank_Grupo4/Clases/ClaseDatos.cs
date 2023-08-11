using System;
using System.Collections.Generic;
using System.Text;

namespace Star_Bank_Grupo4.Clases
{
    public class ClaseDatos
    {
        public int id { get; set; }
        public string descripcion { get; set; }

        public string transferencia_monto { get; set; }
        public string transferencia_fecha { get; set; }
        public string pago_monto { get; set; }
        public string pago_fecha { get; set; }
        public string titulo { get; set; }
        public string eventos_monto { get; set; }
        public string eventos_fecha { get; set; }


        ///para eventos
        /*public string idE { get; set; }
        public string descripcionE { get; set; }*/
        public double costo_boleto { get; set; }
        //cuenta de servisio
        public int CuentaServicio { get; set; }
        //datos de usuario
        public string numero_cuenta { get; set; }
        public string saldo { get; set; }
        public string nombre { get; set; }
        public string estado { get; set; }
        public string correo { get; set; }

    }
}
