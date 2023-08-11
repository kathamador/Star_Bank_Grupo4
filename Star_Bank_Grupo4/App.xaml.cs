using Star_Bank_Grupo4.Clases;
using Star_Bank_Grupo4.Controllers;
using Star_Bank_Grupo4.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Star_Bank_Grupo4
{
    public partial class App : Application
    {
        public static MasterDetailPage Menu { get; set;}
        public static Cliente UsuarioSesion { get; set; } = null;
        public App()
        {
            InitializeComponent();

            //MainPage = new Views.PageMain();
            // Establecer LoginPage como la MainPage
            // MainPage = new Login();
            DependencyService.Register<TransferenciasService>();
            MainPage = new NavigationPage(new Login());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
