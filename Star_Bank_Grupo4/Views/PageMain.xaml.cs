using Star_Bank_Grupo4.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Star_Bank_Grupo4.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageMain : MasterDetailPage, INotifyPropertyChanged
    {
        //prueba
        private string _numeroCuenta;
        public string Numerocuenta
        {
            get { return _numeroCuenta; }
            set
            {
                if (_numeroCuenta != value)
                {
                    _numeroCuenta = value;
                    OnPropertyChanged(nameof(Numerocuenta));
                }
            }
        }

        // Property to store the balance
        private string _saldo;
        public string Saldo
        {
            get { return _saldo; }
            set
            {
                if (_saldo != value)
                {
                    _saldo = value;
                    OnPropertyChanged(nameof(Saldo));
                }
            }
        }

        private string _nombreUsuario;
        public string NombreUsuario
        {
            get { return _nombreUsuario; }
            set
            {
                if (_nombreUsuario != value)
                {
                    _nombreUsuario = value;
                    OnPropertyChanged(nameof(NombreUsuario));
                }
            }
        }

        private string _correoUsuario;
        public string CorreoUsuario
        {
            get { return _correoUsuario; }
            set
            {
                if (_correoUsuario != value)
                {
                    _correoUsuario = value;
                    OnPropertyChanged(nameof(CorreoUsuario));
                }
            }
        }
        public PageMain()
        {
            InitializeComponent();
            // Definir LoginPage como la página de detalle (Detail) de la MasterDetailPage
         
            NavigationPage.SetHasNavigationBar(this, false);
            myPageMain();
            MessagingCenter.Subscribe<ActualizarFoto, ImageSource>(this, "FotoActualizada", (sender, arg) =>
            {
                // Actualizar la foto en el menú
                foto.Source = arg;
            });

            /*ToolbarItems.Add(new ToolbarItem
            {
                IconImageSource = "inicio.png",
                //Command = new Command(ImageButton_Clicked)
            });*/
            this.BindingContext = this;
           

        }
        public void myPageMain()
        {
            Detail = new NavigationPage(new Inicio());

            List<ClaseOption> options = new List<ClaseOption>
            {
                new ClaseOption{ page=new Inicio(),title="Inicio", detail="Promociones", image = "inicio1.png" },
                new ClaseOption{ page=new AdministracionCuentas(),title="Administracion de Cuentas", detail="Detalle de cuenta", image = "adminc.png" },
                new ClaseOption{ page=new Servicio(),title="Pagar Servicios", detail="Pagos públicos y eventos", image = "pagos.png" },
                new ClaseOption{ page=new IniciarTransferencia(),title="Transferencias", detail="Movimientos", image = "transf.png" },
                new ClaseOption{ page=new Reportes(),title="Control Presupuestario", detail="Resumen de gastos", image = "pres.png" },
                new ClaseOption{ page=new AcercaDe(),title="Acerca de", detail="Información", image = "acerca_de.png" },
                new ClaseOption{ page=new ActualizarFoto(),title="Gestion", detail="Gestión De Cuenta", image = "gestion.png" },
                new ClaseOption{ page=null,title="Cerrar Sesión", detail="Salir de la App", image = "salir.png" }
            };
            listPageMain.ItemsSource = options;
        }
        private async void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var option = e.SelectedItem as ClaseOption;
            if (option.page != null)
            {
                IsPresented = false;
                Detail = new NavigationPage(option.page);
            }
            else if (option.page == null || option.title == "Cerrar Sesión")
            {

                var result = await DisplayAlert("Confirmar", "Estas seguro de cerrar sesión", "SI", "NO");
                if (result)
                {
                    // Configura la MainPage de la aplicación como la página de Login
                    App.Current.MainPage = new NavigationPage(new Login());
                }
                else
                {
                    return;
                }
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Aquí, actualizamos la foto cada vez que se muestra la página
            ActualizarFotoEnMenu();
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
    }
}