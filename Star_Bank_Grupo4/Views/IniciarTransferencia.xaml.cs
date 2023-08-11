using Acr.UserDialogs;
using Star_Bank_Grupo4.Controllers;
using Star_Bank_Grupo4.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Star_Bank_Grupo4.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IniciarTransferencia : ContentPage
    {
        public IniciarTransferenciaViewModel ViewModel = new IniciarTransferenciaViewModel();
        private TransferenciasService transferenciasService = DependencyService.Get<TransferenciasService>();
        public IniciarTransferencia()
        {
            BindingContext = ViewModel;
            InitializeComponent();

            MessagingCenter.Subscribe<Application>(Application.Current, "transferencia-realizada", (sender) =>
            {
                ViewModel = new IniciarTransferenciaViewModel();
                BindingContext = ViewModel;
            });
        }
        private void FCuentaDestino_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex regex = new Regex("^[0-9]$", RegexOptions.Compiled);

            // Permite vacío
            if (string.IsNullOrEmpty(e.NewTextValue)) return;

            // Si hay un valor que no sea número, se regresa al valor anterior
            if (!e.NewTextValue.All(c => regex.IsMatch(c.ToString())))
            {
                ((Entry)sender).Text = e.OldTextValue;
            }
        }
        private void FMonto_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Validar que solo se puedan ingresar 2 decimales
            if (e.NewTextValue.Contains("."))
            {
                if (e.NewTextValue.Length - 1 - e.NewTextValue.IndexOf(".") > 2)
                {
                    var s = e.NewTextValue.Substring(0, e.NewTextValue.IndexOf(".") + 2 + 1);
                    ((Entry)sender).Text = s;
                    ((Entry)sender).SelectionLength = s.Length;
                }
            }
        }

        private async void btnirResumen_Clicked(object sender, EventArgs e)
        {
            using (UserDialogs.Instance.Loading("Validando los datos ingresados", maskType: MaskType.Black))
            {
                await Task.Delay(1000);
                var datosParaResumen = await transferenciasService.ObtenerResumenTransferencia(ViewModel);

                if (!datosParaResumen.Ok)
                {
                    await DisplayAlert("Aviso", datosParaResumen.Mensaje, "Aceptar");
                    return;
                }

                datosParaResumen.Monto = ViewModel.Monto;
                datosParaResumen.Comentario = ViewModel.Comentario;
                await Navigation.PushAsync(new ConfirmarTransferencia(datosParaResumen));
            }

        }

        private void FMonto_Unfocused(object sender, FocusEventArgs e)
        {
            // Si el campo queda vacío se pone el valor '0'
            if (string.IsNullOrEmpty(((Entry)sender).Text))
                ((Entry)sender).Text = "0";
        }
        
    }
}