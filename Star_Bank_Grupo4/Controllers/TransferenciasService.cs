using RestSharp;
using Star_Bank_Grupo4.Clases;
using Star_Bank_Grupo4.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Star_Bank_Grupo4.Controllers
{
    public class TransferenciasService
    {
        private readonly RestClient restClient;

        public TransferenciasService()
        {
            var options = new RestClientOptions("https://finalproyect.com/StartBarkPHP")
            {
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
            // restClient = new RestClient(App.STARBANK_API_URL + "/transferencias");
            restClient = new RestClient(options);


        }

        public async Task<ConfirmarTransferenciaViewModel> ObtenerResumenTransferencia(IniciarTransferenciaViewModel realizarTransferenciaViewModel)
        {
            RestRequest restRequest = new RestRequest("obtener-datos.php", Method.Get);
            restRequest.AddJsonBody(new
            {
                NumeroCuentaOrigen = realizarTransferenciaViewModel.CuentaOrigenSeleccionada.Cuenta,
                NumeroCuentaDestino = realizarTransferenciaViewModel.NumeroCuentaDestino,
                SaldoATransferir = realizarTransferenciaViewModel.Monto
            });

            RestResponse<ConfirmarTransferenciaViewModel> response = await restClient.ExecuteAsync<ConfirmarTransferenciaViewModel>(restRequest);

            return response.Data;
        }

        public async Task<RespuestaApi> RegistrarTransferencia(ConfirmarTransferenciaViewModel realizarTransferenciaViewModel)
        {
            RestRequest restRequest = new RestRequest("registrar-transferencia.php", Method.Post);
            restRequest.AddJsonBody(new
            {
                NumeroCuentaOrigen = realizarTransferenciaViewModel.NumeroCuentaOrigen,
                NumeroCuentaDestino = realizarTransferenciaViewModel.NumeroCuentaDestino,
                SaldoATransferir = realizarTransferenciaViewModel.Monto,
                Comentarios = realizarTransferenciaViewModel.Comentario
            });

            RestResponse<RespuestaApi> response = await restClient.ExecuteAsync<RespuestaApi>(restRequest);

            return response.Data;
        }
    }
}
