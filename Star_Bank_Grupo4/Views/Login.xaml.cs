using Newtonsoft.Json;
using SendGrid.Helpers.Mail;
using SendGrid;
using Star_Bank_Grupo4.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Acr.UserDialogs;

namespace Star_Bank_Grupo4.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login 
    {
        public string correoC;
        public Login()
        {
            InitializeComponent();
        }
        async void SigninClick(System.Object sender, System.EventArgs e)
        {
        }
        async void SignupClick(System.Object sender, System.EventArgs e)

        {

            await Navigation.PushAsync(new PageCrearCuenta(), true);

        }
        private async void Ingresar_Clicked_1(object sender, EventArgs e)
        {
            // Navegar a la página de inicio
            //await Navigation.PushAsync(new PageMain());
            // Configura la PageMain como la nueva MainPage de la aplicación
            //App.Current.MainPage = new PageMain();
            if (String.IsNullOrWhiteSpace(correo.Text) || String.IsNullOrWhiteSpace(contrasena.Text))
            {
                await DisplayAlert("AVISO", "Debe de llenar los campos vacios!", "OK");
            }
            else
            {
                WebClient client = new WebClient();
                ServicePointManager.ServerCertificateValidationCallback = (s, cert, chain, sslPolicyErrors) => true;

                string respuestaHttp = await client.DownloadStringTaskAsync("https://finalproyect.com/StartBarkPHP/login.php" +
                $"?correo={correo.Text.Trim()}&clave={contrasena.Text.Trim()}");

                var respuesta = JsonConvert.DeserializeObject<Cliente>(respuestaHttp);
                if (respuesta.Ok)
                {
                    if (respuesta.Mensaje != "temp")
                    {
                        App.UsuarioSesion = respuesta;
                        var config = new AlertConfig
                        {
                            Title = $"                   Star Bank",  // Agrega espacios al principio del título para centrarlo
                            Message = $"{respuesta.Nombre} Accediendo a tu Cuenta",
                            OkText = "Ok"
                    };
                        UserDialogs.Instance.Alert(config);
                        App.Current.MainPage = new PageMain
                        {
                            NombreUsuario = respuesta.Nombre,
                            CorreoUsuario = correo.Text.Trim(),
                        };
                    }
                    else
                        await Navigation.PushAsync(new PageRecuperarClave(correo.Text.Trim()));
                }
                else
                {
                    await DisplayAlert("Aviso", respuesta.Mensaje, "Aceptar");
                }   
            }
        }

        private async void btnCrearCuenta_Clicked(object sender, EventArgs e)
        {
              await Navigation.PushAsync(new PageCrearCuenta());
        }

        private async void btnRecuperarPass_Clicked(object sender, EventArgs e)
        {
            correoC = await DisplayPromptAsync("Correo", "Ingrese su dirección de correo electrónico");

            if (String.IsNullOrWhiteSpace(correoC))
            {
                await DisplayAlert("Aviso", "Ingrese un correo!!!", "Aceptar");
            }
            else
            {
                WebClient client = new WebClient();
                ServicePointManager.ServerCertificateValidationCallback = (s, cert, chain, sslPolicyErrors) => true;
                string respuestaHttp = await client.DownloadStringTaskAsync("https://finalproyect.com/StartBarkPHP/set-clave-temporal.php" +
                    $"?correo={correoC.Trim()}");

                var respuesta = JsonConvert.DeserializeObject<Cliente>(respuestaHttp);

                if (!respuesta.Ok)
                {
                    await DisplayAlert("Aviso", "Correo no registrado!!!", "Aceptar");
                }
                else
                {
                    await DisplayAlert("Aviso", "Recibiras un correo con tu clave temporal, inicia sesión con ella", "Aceptar");
                    await EnviarCorreoClaveTemp(respuesta);
                }
            }

        }
        private async Task EnviarCorreoClaveTemp(Cliente cliente)
        {
            try
            {
                var client = new SendGridClient("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");
                var from = new EmailAddress("calonaisaac@gmail.com", "Star Bank");
                var subject = "Clave temporal";
                var to = new EmailAddress(correoC);

                // Crear el contenido HTML del correo
                string htmlContent = string.Format(@"
                <html>
                <head>
                    <style>
                        /* Estilos personalizados */
                        .container1 {{
                            background-color: #f2f2f2;
                            padding: 20px;
                        }}
                        .container {{
                            background-color: #FFFFFF;
                            padding: 20px;
                            font-family: Arial, sans-serif;
                        }}
                        .header {{
                            text-align: left;
                            background-color: #E4002B;
                            color: #fff;
                            padding: 10px;
                            display: flex;
                            align-items: center;
                        }}
                        .logo {{
                            width: 100px;
                            height: 100px;
                            margin-right: 10px;
                        }}
                        .message {{
                            margin-top: 20px;
                            font-size: 18px;
                            color: #333;
                        }}
                        .footer {{
                            text-align: center;
                            font-size: 14px;
                            margin-top: 30px;
                        }}
                        .footer p {{
                            margin: 0;
                        }}
                        .whatsapp-link {{
                            color: #007bff;
                            text-decoration: none;
                        }}
                        .banner-image {{
                            width: 100%;
                            height: 250px;
                        }}
                    </style>
                </head>
                <body>
            <div class='container1'>
                    <div class='container'>
                        <div class='header'>
                            <img src='https://icones.pro/wp-content/uploads/2021/10/icone-de-banque-rouge.png' alt='Logo' class='logo'>
                            <h1>Star Bank</h1>
                        </div>

                        <img src='https://sscommonsense.org/wp-content/uploads/2020/09/Proceso-de-transacci%C3%B3n-con-tarjeta-de-cr%C3%A9dito-2.png' alt='Banner' class='banner-image'>

                        <div class='message'>
                            <p>Estimado usuario, {0}</p> 
                            <p> Hemos recibido su solicitud de restablecer su contraseña.  
                             Por favor, utilice esta contraseña temporal para iniciar sesión en su cuenta y establecer 
                             una nueva contraseña de su elección. 
                             Por favor recuerda la nueva contraseña que establezcas. Tu contraseña temporal es: {1} </p>               
                        </div>
                    </div>
            </div>
                    <div class='footer'>
                        <p>¡Gracias por tu preferencia!</p>
                        <p>Para consultas sobre esta factura, por favor, comunícate con nuestro equipo.</p>
                        <a href='https://chat.whatsapp.com/LMOxoWARZtx40VueyTkgNd' class='whatsapp-link'>Equipo de Soporte en WhatsApp</a>
                    </div>
                </body>
                </html>", cliente.Nombre, cliente.Mensaje);

                var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);

                var respuesta = await client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Error al enviar el correo: " + ex.Message, "Aceptar");
            }
        }
    }
}