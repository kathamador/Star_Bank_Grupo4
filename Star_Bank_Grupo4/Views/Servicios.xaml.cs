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
    public partial class Servicio : ContentPage
    {
        public List<Clases.ClaseDatos> Datos { get; set; }
        //string cuenta = "20230102";
        public Servicio()
        {
            InitializeComponent();
            Datos = new List<Clases.ClaseDatos>();
            listViewServicios.ItemsSource = Datos;
            OnAppearing();
        }

        private void CargarServicios()
        {
            
            WebClient client = new WebClient();

            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                string response = client.DownloadString("https://finalproyect.com/StartBarkPHP/MostrarServicios.php");

                // Parsear la respuesta JSON utilizando Newtonsoft.Json
                Datos = JsonConvert.DeserializeObject<List<Clases.ClaseDatos>>(response);

                listViewServicios.ItemsSource = Datos;
            }
            catch (WebException ex)
            {
                // Manejar la excepción adecuadamente
                Console.WriteLine("Error al obtener los datos: " + ex.Message);
            }
        }
        //cargar eventos
        private void CargarEventos()
        {

            WebClient client = new WebClient();

            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                string response = client.DownloadString("https://finalproyect.com/StartBarkPHP/MostrarEventos.php");

                // Parsear la respuesta JSON utilizando Newtonsoft.Json
                Datos = JsonConvert.DeserializeObject<List<Clases.ClaseDatos>>(response);

                listViewEventos.ItemsSource = Datos;
            }
            catch (WebException ex)
            {
                // Manejar la excepción adecuadamente
                Console.WriteLine("Error al obtener los datos: " + ex.Message);
            }
        }
     

        //para actualizar los campos al momento de cambiar pantallas
        protected override void OnAppearing()
        {
            base.OnAppearing();
            CargarServicios();
            CargarEventos();
        }
        private async void ListViewServicios_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is ClaseDatos selectedItem)
            {
                string descrip = selectedItem.descripcion;
                int Id = selectedItem.id;
                int cuentaS = selectedItem.CuentaServicio;
                //string cuenta = selectedItem.numero_cuenta;
                string cuenta = DatosCuentaSingleton.Instance.NumeroCuenta;
                PagoServicios paginaVideo = new PagoServicios(descrip, cuenta,cuentaS,Id);
                await Navigation.PushAsync(paginaVideo);
            }
        }
        private async void ListViewEventos_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is ClaseDatos selectedItem)
            {
                string descrip = selectedItem.descripcion;
                int Id = selectedItem.id;
                double costo = selectedItem.costo_boleto;
                //string cuenta = selectedItem.numero_cuenta;
                string cuenta = DatosCuentaSingleton.Instance.NumeroCuenta;
                PagoEventos paginaVideo = new PagoEventos(descrip, cuenta,costo,Id);
                await Navigation.PushAsync(paginaVideo);
            }
        }
     
    }

}