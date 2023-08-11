using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Star_Bank_Grupo4.Clases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Star_Bank_Grupo4.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class Inicio : ContentPage
    {
        public List<Clases.ClaseDatos> Datos { get; set; }

        public Inicio()
        {
            InitializeComponent();
            Datos = new List<Clases.ClaseDatos>();

            List<imagenes> imagenes = new List<imagenes>()
            {
                new imagenes() { title="image 1", item="prom1.png"},
                new imagenes() { title = "image 2", item = "prom2.png" },
                new imagenes() { title = "image 3", item = "prom3.png" }
            };

            Carousel.ItemsSource = imagenes;

            Device.StartTimer(TimeSpan.FromSeconds(6), (Func<bool>)(() =>
            {
                Carousel.Position = (Carousel.Position + 1) % imagenes.Count;

                return true;
            }));
        }

        private void OtenerDatosUsuario(string correo)  
        {
            WebClient client = new WebClient();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            string url = "https://finalproyect.com/StartBarkPHP/MostrarUsuario.php?correo=" + correo;
            string response = client.DownloadString(url);

            // Deserializar los datos recibidos desde el servidor en una lista de Clases.ClaseDatos
            List<Clases.ClaseDatos> usuarios = JsonConvert.DeserializeObject<List<Clases.ClaseDatos>>(response);

            // Verificar si se encontraron resultados
            if (usuarios.Count > 0)
            {
                // Tomar el primer elemento de la lista, ya que la consulta solo debe devolver un usuario
                Clases.ClaseDatos datos = usuarios[0];
                // Actualizar los Labels con los datos deserializados
                txtcuenta.Text = datos.numero_cuenta;
                txtsaldo.Text = datos.saldo;
                string nombre = datos.nombre;
                string estado = datos.estado;
                // Guardar los datos en el Singleton
                DatosCuentaSingleton.Instance.NumeroCuenta = datos.numero_cuenta;
                DatosCuentaSingleton.Instance.Saldo = datos.saldo;
                DatosCuentaSingleton.Instance.Nombre = datos.nombre;
                DatosCuentaSingleton.Instance.Correo = txtcorreo.Text;
                DatosCuentaSingleton.Instance.Estado = estado;
            }
            else
            {
                // No se encontraron resultados, puedes mostrar un mensaje de error o manejarlo según tus necesidades
            }
        }
        //para actualizar los campos al momento de cambiar pantallas
        protected override void OnAppearing()
        {
            base.OnAppearing();
            OtenerDatosUsuario(txtcorreo.Text);
        }
    }
}