using Star_Bank_Grupo4.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Star_Bank_Grupo4.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PagoServicios : ContentPage
    {
        double pagoser = 600;
        double total = 0;
        int cuentaSer;
        //string nombre = "mirian";
        string nombre = DatosCuentaSingleton.Instance.Nombre;
        string saldo = DatosCuentaSingleton.Instance.Saldo;
        double saldousuario;
        int ids;
        public PagoServicios(string descri, string cuentaC,int cuentaS,int idS)
        {
            InitializeComponent();
            descripcion.Text = descri;
            cuenta.Text = cuentaC;
            cuentaSer = cuentaS;
            Random random = new Random();
            double precio = random.Next(50, 1001);
            // Mostrar el número en el Label
            total = pagoser + precio;
            saldoT.Text = "L " + total.ToString();
            ids = idS;
            saldousuario= double.Parse(saldo);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if(saldousuario > total)
            {
                Navigation.PushAsync(new Views.ConfirmacionPago(nombre, descripcion.Text, cuenta.Text, cuentaSer, total, ids));
            }
            else
            {
                DisplayAlert("Aviso","No cuenta con el saldo sufuciente en su cuenta para pagar el servicio", "OK");
                Navigation.PushAsync(new Views.Servicio());
            }

        }

    }
}