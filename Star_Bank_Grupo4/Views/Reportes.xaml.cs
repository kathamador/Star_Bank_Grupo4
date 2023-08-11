using Microcharts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SkiaSharp;
using Star_Bank_Grupo4.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Star_Bank_Grupo4.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Reportes : ContentPage
	{
        private Random random = new Random();
        private HttpClient _httpClient;
        public Reportes ()
		{
			InitializeComponent ();
            NavigationPage.SetBackButtonTitle(this, " ");

            _httpClient = new HttpClient();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            string cuenta = DatosCuentaSingleton.Instance.NumeroCuenta;
            txtcuenta.Text = cuenta;
            GetGraficoTransferencias();
            await CargarCuentaCliente();
        }
        private async void GetGraficoEventos()
        {
            HttpClient client = new HttpClient();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            string url = "https://finalproyect.com/StartBarkPHP/reporte/grafico-eventos.php?cuenta=" + txtcuenta.Text; // URL de tu REST API

            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                List<Eventos> chartDataList = ParseChartData3(result); 

                // Crear una lista de entradas de gráfico para Microcharts
                List<ChartEntry> entries = new List<ChartEntry>();


                foreach (var data in chartDataList)
                {
                    string color = String.Format("#{0:X6}", random.Next(0x1000000));
                    entries.Add(new ChartEntry((float)data.monto)
                    {
                        ValueLabelColor = SKColor.Parse(color),
                        Label = data.fecha,
                        ValueLabel = "L " + data.monto.ToString(),
                        Color = SKColor.Parse(color), // Color personalizable
                        TextColor = SKColor.Parse("#000")
                    });
                }
                Chart chart = new PieChart
                {
                    Entries = entries,
                    LabelTextSize = 22
                };
                charView.Chart = chart;
            }
            else
                await DisplayAlert("Aviso", "Sin resultados para mostrar", "OK");
        }
        private async void GetGraficoTransferencias()
        {

            HttpClient client2 = new HttpClient();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            string url = "https://finalproyect.com/StartBarkPHP/reporte/grafico-transferencias.php?cuenta=" + txtcuenta.Text; // URL de tu REST API

            var response = await client2.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {

                var result = await response.Content.ReadAsStringAsync();
                List<Transferencias> chartDataList = ParseChartData(result);

                // Crear una lista de entradas de gráfico para Microcharts
                List<ChartEntry> entries = new List<ChartEntry>();

                foreach (var data in chartDataList)
                {
                    string color = String.Format("#{0:X6}", random.Next(0x1000000));

                    entries.Add(new ChartEntry((float)data.monto)
                    {
                        ValueLabelColor = SKColor.Parse(color),
                        Label = data.fecha,
                        ValueLabel = "L " + data.monto.ToString(),
                        Color = SKColor.Parse(color), // Color personalizable
                        TextColor = SKColor.Parse("#000")
                    });
                }
                Chart chart = new LineChart
                {
                    Entries = entries,
                    LabelTextSize = 22,
                    LabelOrientation = Orientation.Horizontal,
                    ValueLabelOrientation = Orientation.Horizontal
                };
                charView.Chart = chart;
            }
            else
                await DisplayAlert("Aviso", "Sin resultados para mostrar", "OK");

        }
        private async void GetGraficoPagosServicios()
        {

            HttpClient client3 = new HttpClient();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            string url = "https://finalproyect.com/StartBarkPHP/reporte/grafico-pagos.php?cuenta=" + txtcuenta.Text; // URL de tu REST API

            var response = await client3.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {

                var result = await response.Content.ReadAsStringAsync();
                List<Servicios> chartDataList = ParseChartData2(result);

                // Crear una lista de entradas de gráfico para Microcharts
                List<ChartEntry> entries = new List<ChartEntry>();

                foreach (var data in chartDataList)
                {
                    string color = String.Format("#{0:X6}", random.Next(0x1000000));
                    entries.Add(new ChartEntry((float)data.monto)
                    {
                        ValueLabelColor = SKColor.Parse(color),
                        Label = data.fecha,
                        ValueLabel = "L " + data.monto.ToString(),
                        Color = SKColor.Parse(color), // Color personalizable
                        TextColor = SKColor.Parse("#000")
                    });
                }
                Chart chart = new BarChart
                {
                    Entries = entries,
                    LabelTextSize = 22,
                    LabelOrientation = Orientation.Vertical,
                    ValueLabelOrientation = Orientation.Horizontal
                };
                charView.Chart = chart;
            }
            else
                await DisplayAlert("Aviso", "Sin resultados para mostrar", "OK");
        }
        private List<Transferencias> ParseChartData(string json)
        {
            List<Transferencias> chartDataList = new List<Transferencias>();

            JArray jsonArray = JArray.Parse(json);
            foreach (var item in jsonArray)
            {
                Transferencias data = new Transferencias
                {
                    fecha = item["fecha"].ToString(),
                    monto = double.Parse(item["monto"].ToString())
                };

                chartDataList.Add(data);
            }//cierre de aplicacion
            return chartDataList;

        }
        private List<Servicios> ParseChartData2(string json)
        {
            List<Servicios> chartDataList2 = new List<Servicios>();

            JArray jsonArray = JArray.Parse(json);
            foreach (var item in jsonArray)
            {
                Servicios data = new Servicios
                {
                    fecha = item["fecha"].ToString(),
                    monto = double.Parse(item["monto"].ToString())
                };

                chartDataList2.Add(data);
            }//cierre de aplicacion
            return chartDataList2;

        }
        private List<Eventos> ParseChartData3(string json)
        {


            List<Eventos> chartDataList3 = new List<Eventos>();

            JArray jsonArray = JArray.Parse(json);
            foreach (var item in jsonArray)
            {
                Eventos data = new Eventos
                {
                    fecha = item["fecha"].ToString(),
                    monto = double.Parse(item["monto"].ToString())
                };

                chartDataList3.Add(data);
            }//cierre de aplicacion
            return chartDataList3;

        }
        async Task CargarCuentaCliente()
        {
            // Realizar una solicitud HTTP a tu API REST para obtener el JSON
            HttpClient httpClient = new HttpClient();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            string json = await httpClient.GetStringAsync("https://finalproyect.com/StartBarkPHP/reporte/consulta.php?cuenta=" + txtcuenta.Text);

            List<ClaseDatos> clientes = JsonConvert.DeserializeObject<List<ClaseDatos>>(json); // Deserializar el JSON
            ClaseDatos cliente = clientes[0];

            nombre.Text = cliente.nombre;
            txtcuenta.Text = cliente.numero_cuenta;
            saldo.Text = cliente.saldo.ToString();

        }
        private async void btn_image_Clicked(object sender, EventArgs e)
        {
            string alerta = await DisplayActionSheet(
                "Opciones A Mostrar De Grafico", "Cancelar", null,
                "Transferencias", "Pagos servicios", "Pagos eventos"
                );

            switch (alerta)
            {
                case "Transferencias":
                    GetGraficoTransferencias();
                    break;
                case "Pagos servicios":
                    GetGraficoPagosServicios();
                    break;
                case "Pagos eventos":
                    GetGraficoEventos();
                    break;
            }
        }

    }
}