using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Star_Bank_Grupo4.Clases
{
    public class Cliente : RespuestaApi
    {
        [JsonProperty("numero_cuenta")]
        public string NumeroCuenta { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Contraseña { get; set; }
        public string Telefono { get; set; }
        public decimal Saldo { get; set; }
        public string Foto { get; set; }
        public DateTime FechaUltimoLogin { get; set; }

        public string Estado { get; set; }
        
        public List<CuentaUsuario> CuentasUsuario => new List<CuentaUsuario> {
            new CuentaUsuario(NumeroCuenta, Nombre)};
    }
}
