using Newtonsoft.Json;
using Plugin.Media;
using Star_Bank_Grupo4.Clases;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Star_Bank_Grupo4.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActualizarFoto : ContentPage
    {
        Plugin.Media.Abstractions.MediaFile photo = null;

        public ActualizarFoto()
        {
            InitializeComponent();
        }
     
        private async void ActualizarFotoEnMenu()
        {
            WebClient client = new WebClient();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            string url = "https://finalproyect.com/StartBarkPHP/ObtenerFoto.php?correo=" + correo.Text;

            try
            {
                // Descargar la foto codificada en base64 desde el servidor
                string fotoBase64 = await client.DownloadStringTaskAsync(url);

                // Decodificar la foto desde base64 a bytes
                byte[] fotoBytes = Convert.FromBase64String(fotoBase64);

                // Asignar la foto al ImageButton en el menú
                foto.Source = ImageSource.FromStream(() => new System.IO.MemoryStream(fotoBytes));
            }
            catch (Exception ex)
            {
                // Manejar errores, por ejemplo, si el cliente no tiene una foto o si hay un problema con la conexión
                await DisplayAlert("Error", "No se pudo obtener la foto del cliente: " + ex.Message, "OK");
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
        private async void actualizar_Clicked(object sender, EventArgs e)
        {
            WebClient cliente = new WebClient();
            ServicePointManager.ServerCertificateValidationCallback = (s, cert, chain, sslPolicyErrors) => true;
            var parametros = new NameValueCollection();
            parametros.Add("correo", correo.Text);
            parametros.Add("foto", Getimage64());
            var responseHttp = cliente.UploadValues("https://finalproyect.com/StartBarkPHP/ActualizarFoto.php", "POST", parametros);
            var respuestaApi = JsonConvert.DeserializeObject<RespuestaApi>(cliente.Encoding.GetString(responseHttp));

            if (respuestaApi.Ok)
            {
                await DisplayAlert("Aviso", "Foto de perfil actualizada exitosamente", "OK");

            }
            else
            {
                await DisplayAlert("Aviso", respuestaApi.Mensaje, "OK");
            }
            MessagingCenter.Send(this, "FotoActualizada", foto.Source);
        }

        private async void CerrarCuenta_Clicked(object sender, EventArgs e)
        {
            string estado = "I";
            WebClient cliente = new WebClient();

            var parametros = new NameValueCollection();
            parametros.Add("correo", correo.Text);
            parametros.Add("estado", estado);

            var responseHttp = cliente.UploadValues("https://finalproyect.com/StartBarkPHP/CancelarCuenta.php", "POST", parametros);
            var respuestaApi = JsonConvert.DeserializeObject<RespuestaApi>(cliente.Encoding.GetString(responseHttp));

            if (respuestaApi.Ok)
            {
                await DisplayAlert("Aviso", "Estimado usuario, te informamos que tu cuenta ha sido cancelada exitosamente", "OK");
                App.Current.MainPage = new NavigationPage(new Login());
            }
            else
            {
                await DisplayAlert("Aviso", respuestaApi.Mensaje, "OK");
            }
        }
        private bool foto_actualizada = false;
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!foto_actualizada)
            {
                ActualizarFotoEnMenu();
                foto_actualizada = true;
            }

        }
    }
}