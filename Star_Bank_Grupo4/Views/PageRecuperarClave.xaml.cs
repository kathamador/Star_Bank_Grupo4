using Newtonsoft.Json;
using Star_Bank_Grupo4.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Star_Bank_Grupo4.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageRecuperarClave : ContentPage
    {
        private readonly string correo;
        public PageRecuperarClave(string correo)
        {
            InitializeComponent();
            this.correo = correo;
        }

        private async void btnRecuperarClave_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(contrasenaNueva.Text)
                || String.IsNullOrWhiteSpace(confirmarcontrasena.Text))
            {
                await DisplayAlert("AVISO", "No dejar campos vacios!", "OK");
            }
            else if (contrasenaNueva.Text != confirmarcontrasena.Text)
            {
                await DisplayAlert("AVISO", "Las constraseñas no coinciden!", "OK");
                contrasenaNueva.Text = String.Empty;
                confirmarcontrasena.Text = String.Empty;
            }
            else
            {
                WebClient client = new WebClient();
                ServicePointManager.ServerCertificateValidationCallback += (s, certificate, chain, sslPolicyErrors) => true;
                string respuestaHttp = await client.DownloadStringTaskAsync("https://finalproyect.com/StartBarkPHP/set-clave-nueva.php" +
                $"?correo={correo.Trim()}&claveNueva={contrasenaNueva.Text}");

                var respuestaApi = JsonConvert.DeserializeObject<RespuestaApi>(respuestaHttp);

                if (respuestaApi.Ok)
                {
                    await DisplayAlert("Notificación", "Su clave ha sido cambiada, inicie sesión con su clave nueva", "Aceptar");
                    await Navigation.PopAsync();
                }

            }
        }
    }
}