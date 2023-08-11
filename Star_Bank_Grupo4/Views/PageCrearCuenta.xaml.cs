using Newtonsoft.Json;
using Plugin.Media;
using SendGrid.Helpers.Mail;
using SendGrid;
using Star_Bank_Grupo4.Clases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Collections.Specialized;

namespace Star_Bank_Grupo4.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageCrearCuenta 
    {
        Plugin.Media.Abstractions.MediaFile photo = null;

        public PageCrearCuenta()
        {
            InitializeComponent();
        }
        async void SignupClick(System.Object sender, System.EventArgs e)
        {
        }
        async void SigninClick(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopAsync(true);
        }

        private async void btnRegistrarse_Clicked(object sender, EventArgs e)
        {
            bool isValidEmail = await ValidarCorreo();

            if (isValidEmail)
            {
                Validaciones();
            }
        }

        private async Task<bool> RequestCameraPermissionAsync()
        {
            var status = await Permissions.RequestAsync<Permissions.Camera>();
            return status == PermissionStatus.Granted;
        }

        private async Task<bool> RequestStoragePermissionAsync()
        {
            var status = await Permissions.RequestAsync<Permissions.StorageWrite>();
            return status == PermissionStatus.Granted;
        }

        private async void btnTomarFoto_Clicked(object sender, EventArgs e)
        {
            bool cameraPermissionGranted = await RequestCameraPermissionAsync();
            bool storagePermissionGranted = await RequestStoragePermissionAsync();

            if (cameraPermissionGranted && storagePermissionGranted)
            {
                // Continuar con la lógica para tomar la foto
                photo = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Login",
                    Name = "Foto.jpg",
                    SaveToAlbum = true,
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small
                });

                if (photo != null)
                {
                    foto.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
                }
            }
            else
            {
                await DisplayAlert("Permiso denegado", "No se otorgaron los permisos necesarios.", "OK");
            }
        }

        private void btnCancelar_Clicked(object sender, EventArgs e)
        {
            LimpiarCampo();
            Navigation.PopToRootAsync();
        }

        public byte[] GetimageBytes()
        {
            if (photo != null)
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    Stream stream = photo.GetStream();
                    stream.CopyTo(memory);
                    byte[] fotobyte = memory.ToArray();

                    return fotobyte;
                }
            }
            return null;
        }

        public string Getimage64()
        {
            if (photo != null)
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    Stream stream = photo.GetStream();
                    stream.CopyTo(memory);
                    byte[] fotobyte = memory.ToArray();

                    string Base64 = Convert.ToBase64String(fotobyte);
                    return Base64;
                }
            }

            return null;
        }

        public async Task<bool> ValidarCorreo()
        {
            string emailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            bool isValidEmail = System.Text.RegularExpressions.Regex.IsMatch(correo.Text, emailPattern);

            if (!isValidEmail)
            {
                await DisplayAlert("Error", "El correo electrónico no es válido.", "Aceptar");
            }

            return isValidEmail;
        }

        public async void Validaciones()
        {
            if (String.IsNullOrWhiteSpace(nombre.Text)
                || String.IsNullOrWhiteSpace(correo.Text) || String.IsNullOrWhiteSpace(contrasena.Text)
                || String.IsNullOrWhiteSpace(telefono.Text))
            {
                await DisplayAlert("Advertencia", "Debe de llenar los campos vacios!", "OK");
            }
            else
            {
                await EnviarCodigoVerificacion();
            }
        }

        private async Task EnviarCodigoVerificacion()
        {
            try
            {
                // Generate a random verification code
                Random random = new Random();
                string codigoVerificacion = random.Next(100000, 999999).ToString();

                // Send the verification code via email
                await EnviarCorreo(codigoVerificacion);

                // Prompt the user to enter the verification code
                var codigo = await DisplayPromptAsync("Codigo de Verificación", "Ingrese el código enviado a " + correo.Text);

                // Check if the entered code matches the generated code
                if (codigo == codigoVerificacion)
                {
                    await DisplayAlert("Código verificado", "Código de verificación correcto", "OK");

                    // Register the user after email verification is successful
                    await RegistrarDatos();
                }
                else
                {
                    await DisplayAlert("Aviso", "Código no coincide", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Error al enviar el código de verificación: " + ex.Message, "Aceptar");
            }
        }

        private async Task RegistrarDatos()
        {
            WebClient cliente = new WebClient();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            var parametros = new NameValueCollection();
            parametros.Add("nombre", nombre.Text);
            parametros.Add("correo", correo.Text);
            parametros.Add("contrasena", contrasena.Text);
            parametros.Add("telefono", telefono.Text);
            parametros.Add("foto", Getimage64());

            var responseHttp = cliente.UploadValues("https://finalproyect.com/StartBarkPHP/registrar.php", "POST", parametros);
            var respuestaApi = JsonConvert.DeserializeObject<RespuestaApi>(cliente.Encoding.GetString(responseHttp));

            if (respuestaApi.Ok)
            {
                await DisplayAlert("Guardado", "Registrado exitosamente", "OK");

                LimpiarCampo();
                await Navigation.PushAsync(new Login());
            }
            else
            {
                await DisplayAlert("Aviso", respuestaApi.Mensaje, "OK");
            }
        }

        private async Task EnviarCorreo(string codigo)
        {
            try
            {
                //var apiKey = "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";
                var client = new SendGridClient("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");
                var from = new EmailAddress("calonaisaac@gmail.com", "Star Bank");
                var subject = "Código de Verificación";
                var to = new EmailAddress(correo.Text);

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
                            background-color: #93172D;
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
                            <p>Estimado cliente, {0},</p>
                            <p>Gracias por preferirnos como tú banca móvil.  Para completar el proceso de registro, 
                            por favor verifica tu cuenta haciendo uso del siguiente código de verificación :{1}
                            Al verificar su cuenta, podrás acceder a todas las funciones y beneficios de nuestra aplicación.
                            </p>
                            <h3>Gracias por elegirnos.</h3>
                            <h3>Atentamente: Star Bank</h3>

                        </div>
                    </div>
            </div>
                    <div class='footer'>
                        <p>¡Gracias por tu preferencia!</p>
                        <p>Para consultas sobre esta factura, por favor, comunícate con nuestro equipo.</p>
                        <a href='https://chat.whatsapp.com/LMOxoWARZtx40VueyTkgNd' class='whatsapp-link'>Equipo de Soporte en WhatsApp</a>
                    </div>
                </body>
                </html>", nombre.Text, codigo);

                var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);

                var respuesta = await client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Error al enviar el correo: " + ex.Message, "Aceptar");
            }
        }

        public void LimpiarCampo()
        {
            this.nombre.Text = String.Empty;
            this.correo.Text = String.Empty;
            this.contrasena.Text = String.Empty;
            this.telefono.Text = String.Empty;
        }
    }

}