using Google.Protobuf.Reflection;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
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
    public partial class AdministracionCuentas : ContentPage
    {       
        public List<Clases.ClaseDatos> Datos { get; set; }
        public AdministracionCuentas()
        {
            InitializeComponent();
            Datos = new List<Clases.ClaseDatos>();
            listViewTransacciones.ItemsSource = Datos;
        }
        private void CargarTransferencias(string cuenta)
        {
            WebClient client = new WebClient();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            string url = "https://finalproyect.com/StartBarkPHP/HistorialTransacciones.php?cuenta=" + cuenta;

            string response = client.DownloadString(url);

            // Parsear la respuesta JSON utilizando Newtonsoft.Json
            List<Clases.ClaseDatos> datos = JsonConvert.DeserializeObject<List<Clases.ClaseDatos>>(response);

            Datos.Clear(); // Limpiar los datos existentes en la lista
            foreach (var dato in datos)
            {
                // Agregar el título como propiedad adicional en ClaseDatos
                dato.titulo = GetTitulo(dato);
                Datos.Add(dato); // Agregar los datos obtenidos desde el servidor
            }

            // Actualizar el origen de datos del ListView
            listViewTransacciones.ItemsSource = Datos;
        }

        private string GetTitulo(Clases.ClaseDatos dato)
        {
            // Devolver el título correspondiente según el tipo de transacción
            if (!string.IsNullOrEmpty(dato.transferencia_monto) && !string.IsNullOrEmpty(dato.transferencia_fecha))
            {
                return "Transferencia";
            }
            else if (!string.IsNullOrEmpty(dato.pago_monto) && !string.IsNullOrEmpty(dato.pago_fecha))
            {
                return "Pago de Servicio";
            }
            else if (!string.IsNullOrEmpty(dato.eventos_monto) && !string.IsNullOrEmpty(dato.eventos_fecha))
            {
                return "Eventos";
            }

            return string.Empty;
        }
        //para actualizar los campos al momento de cambiar pantallas
       

        private void Mostrar_Lista_Clicked(object sender, EventArgs e)
        {
            ventana();
        }
        //desde aqui empieza codigo de katherin
            public async void ventana()
            {
            var titleLabel = new Label
            {
                Text = "Selecciona un mes",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                TextColor = Color.White// Add top margin of 10 to the title
            };
            // Get the names of the months
            List<string> meses = new List<string>
                {
                    "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                    "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"
                };

                // ListView to display the month buttons
                var listViewMeses = new ListView
                {
                    ItemsSource = meses,
                    BackgroundColor = Color.White
                };

                // Handle the item selection event
                listViewMeses.ItemSelected += (sender, e) =>
                {
                    if (e.SelectedItem != null)
                    {
                        // Get the selected month
                        var selectedMonth = e.SelectedItem.ToString();

                        // Print the selected month in the console
                        Console.WriteLine("Mes seleccionado: " + selectedMonth);
                        conexion(selectedMonth);

                        // Deselect the item
                        listViewMeses.SelectedItem = null;
                    }
                };

                // Create a button to close the popup
                var closeButton = new Button
                {
                    Text = "Cerrar",
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = new Thickness(10), // Add padding of 10 to the button
                    BackgroundColor = Color.FromHex("#921939"), // Blue color for the button (you can change it)
                    TextColor = Color.White, // Text color for the button (you can change it)
                    TextTransform = TextTransform.None,
                    CornerRadius = 5
                };

                closeButton.Clicked += async (sender, e) =>
                {
                    // Close the popup when the button is clicked
                    await PopupNavigation.Instance.PopAsync(); 
                };

                // Create a layout to hold the ListView and the close button
                var contentLayout = new StackLayout
                {
                    Children = { titleLabel, listViewMeses, closeButton },
                    Padding = new Thickness(10) // Add padding of 10 to the layout
                };

                // Create the popup window to display the month buttons
                var monthSelectionPopup = new PopupPage
                {
                    Content = contentLayout,
                    BackgroundColor = Color.FromHex("#80000000"), // Transparent color with darkening
                    CloseWhenBackgroundIsClicked = true // Close the popup window when clicking on the darkened background
                };

                // Show the month selection popup window
                await PopupNavigation.Instance.PushAsync(monthSelectionPopup);
            }
        public async void conexion(string mes)
        {
            string cuenta = DatosCuentaSingleton.Instance.NumeroCuenta;
            WebClient client = new WebClient();
        
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            string url = "https://finalproyect.com/StartBarkPHP/MostrarHistorialFecha.php?cuenta=" + cuenta + "&mes=" + mes;

            string response = client.DownloadString(url);
            List<Clases.ClaseDatos> datos = JsonConvert.DeserializeObject<List<Clases.ClaseDatos>>(response);

            if (datos != null && datos.Count > 0)
            {
                var listViewDatos = new ListView
                {
                    ItemsSource = datos,
                    ItemTemplate = new DataTemplate(() =>
                    {
                        var grid = new Grid();

                        // Define las columnas con el mismo ancho para cada campo
                        for (int i = 0; i < 3; i++)
                        {
                            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                        }

                        var tituloLabel = new Label();
                        tituloLabel.TextColor = Color.Black;
                        tituloLabel.SetBinding(Label.TextProperty, "titulo");//0 

                        var transferenciaLabel = new Label();
                        transferenciaLabel.TextColor = Color.Black;
                        transferenciaLabel.SetBinding(Label.TextProperty, "transferencia_monto");

                        var transferenciaFechaLabel = new Label();
                        transferenciaFechaLabel.TextColor = Color.Black;
                        transferenciaFechaLabel.SetBinding(Label.TextProperty, "transferencia_fecha");

                        var pagoLabel = new Label();
                        pagoLabel.TextColor = Color.Black;
                        pagoLabel.SetBinding(Label.TextProperty, "pago_monto");

                        var pagoFechaLabel = new Label();
                        pagoFechaLabel.TextColor = Color.Black;
                        pagoFechaLabel.SetBinding(Label.TextProperty, "pago_fecha");

                        var eventosLabel = new Label();
                        eventosLabel.TextColor = Color.Black;
                        eventosLabel.SetBinding(Label.TextProperty, "eventos_monto");

                        var eventosFechaLabel = new Label();
                        eventosFechaLabel.TextColor = Color.Black;
                        eventosFechaLabel.SetBinding(Label.TextProperty, "eventos_fecha");

                        // Agrega los Labels a la cuadrícula en la misma fila
                        grid.Children.Add(tituloLabel, 0, 0);
                        grid.Children.Add(transferenciaLabel, 1, 0);
                        grid.Children.Add(transferenciaFechaLabel, 2, 0);
                        grid.Children.Add(pagoLabel, 1, 0);
                        grid.Children.Add(pagoFechaLabel, 2, 0);
                        grid.Children.Add(eventosLabel, 1, 0);
                        grid.Children.Add(eventosFechaLabel, 2, 0);

                        var viewCell = new ViewCell { View = grid };
                        return viewCell;
                    }),
                     
                    BackgroundColor = Color.White,
                    Margin = new Thickness(10)
                };

                var closeButton = new Button
                {
                    Text = "Cerrar",
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = new Thickness(10),
                    BackgroundColor = Color.FromHex("#2196F3"),
                    TextColor = Color.White
                };

                closeButton.Clicked += async (sender, e) =>
                {
                    await PopupNavigation.Instance.PopAsync();
                };

                var contentLayout = new StackLayout
                {
                    Padding = new Thickness(10),
                    Children = { listViewDatos, closeButton }
                };

                var popupPage = new PopupPage
                {
                    Content = contentLayout,
                    BackgroundColor = Color.FromHex("#80000000"),
                    CloseWhenBackgroundIsClicked = true
                };

                await PopupNavigation.Instance.PushAsync(popupPage);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "No se encontraron datos para el mes de " + mes, "Aceptar");
                //Console.WriteLine("No se encontraron datos.");
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            string cuenta = DatosCuentaSingleton.Instance.NumeroCuenta;
            string saldo = DatosCuentaSingleton.Instance.Saldo;
            string estado = DatosCuentaSingleton.Instance.Estado;
            txtcuenta.Text = cuenta;
            txtsaldo.Text = saldo;
            CargarTransferencias(cuenta);
            if (estado == "A")
            {
                txtestado.Text = "Activo";
            }
            else
            {
                txtestado.Text = "Inactivo";
            }

           // CargarTransferencias(cuenta);
        }
    }
}