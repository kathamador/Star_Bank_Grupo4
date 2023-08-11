 using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SendGrid.Helpers.Mail;
using SendGrid;
using Star_Bank_Grupo4.Clases;

namespace Star_Bank_Grupo4.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PagoEventos : ContentPage
    {
        double costoBoleto = 0;
        double TotalPagar = 0;
        int idE = 0;
        string fechaFormateada;
        string fecha;
        string correo = DatosCuentaSingleton.Instance.Correo;
        string saldo = DatosCuentaSingleton.Instance.Saldo;
        
        double saldousuario;
        double saldoFinal = 0.0;

        public PagoEventos(string descrip, string cuentaC,double costo,int id)
        {
            InitializeComponent();
            // Asignar el Entry "boletosEntry" al campo de clase correspondiente
            boletosEntry = (Entry)FindByName("boletosEntry");
            boletosEntry.Text = "0";
            descripcion.Text = descrip;
            cuenta.Text = cuentaC;
            costoBoleto = costo;
            idE = id;

            // Obtener la fecha y hora actual
            DateTime fechaActual = DateTime.Now;

            // Formatear la fecha en el formato deseado ("yyyy-MM-dd HH:mm:ss")
            fechaFormateada = fechaActual.ToString("yyyy-MM-dd HH:mm:ss");

            // Guardar la fecha formateada en una variable
            fecha = fechaFormateada;
            saldousuario = double.Parse(saldo);

        }
        private void OnMinusClicked(object sender, EventArgs e)
        {

            if (boletosEntry != null && int.TryParse(boletosEntry.Text, out int boletos))
            {
                boletos--;
                boletosEntry.Text = boletos.ToString();
                CalcularPago();
            }
        }
        private void OnPlusClicked(object sender, EventArgs e)
        {
            if (boletosEntry != null && int.TryParse(boletosEntry.Text, out int boletos))
            {
                boletos++;
                boletosEntry.Text = boletos.ToString();     
                CalcularPago();
            }
        }
        public void CalcularPago()
        {
            TotalPagar = (costoBoleto * (double.Parse(boletosEntry.Text)));
            total.Text = TotalPagar.ToString();
        }
        //boton guardar
        public void guardarDatos() 
        {      
            WebClient cliente = new WebClient();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            var parametros = new System.Collections.Specialized.NameValueCollection();
            parametros.Add("evento_id", idE.ToString());
            parametros.Add("cuenta_cliente", cuenta.Text);
            parametros.Add("cantidad_boletos", boletosEntry.Text);
            parametros.Add("precio_unidad", costoBoleto.ToString());
            parametros.Add("total", total.Text);
            parametros.Add("fecha", fecha);

            cliente.UploadValues("https://finalproyect.com/StartBarkPHP/CompraEventos.php", "POST", parametros);
        }
        private async void EnviarCorreo()
        {
            try
            {
                string nombre = DatosCuentaSingleton.Instance.Nombre;
                string evento = descripcion.Text;
                string cantidadB = boletosEntry.Text;
                // Datos de la transacción
                string TotalA = total.Text;
                // Obtener la fecha y hora actual
                DateTime fechaActual = DateTime.Now;
                // Formatear la fecha en el formato deseado ("yyyy-MM-dd HH:mm:ss")
                string fecha = fechaActual.ToString("yyyy-MM-dd HH:mm:ss");
                //var apiKey = "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";
                var client = new SendGridClient("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");

                var from = new EmailAddress("calonaisaac@gmail.com", "Star Bank");
                var subject = "COMPRA DE BOLETOS";
                var to = new EmailAddress(correo, "Star Bank");

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
                            background-color: #E4002B;
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
                        .transaction-details {{
                            margin-top: 30px;
                            border: 1px solid #ccc;
                            padding: 10px;
                            background-color: #fff;
                        }}
                        .detail-item {{
                            display: table-row;
                        }}
                        .detail-item p {{
                            display: table-cell;
                            padding: 5px;
                            border-bottom: 1px solid #ccc;
                        }}
                        .detail-item p:last-child {{
                            border-bottom: none;
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
                            <p>Gracias por Preferirnos como tu método de confianza para adquisición de Boletos. 
                            Nos complace informarte que se ha realizado de manera exitosa la compra de tus boletos. 
                            Recuerda que cada evento esta sujeto a cambios de fecha o suspenciones por lo tanto recuerda 
                            verificar bien la información del evento antes de realizar la compra de tus boletos.  
                            Stark Bank tu boleteria al alcance de tus manos.</p>
                            <h3>DETALLE COMPRA DE BOLETOS</h3>
                            <div class='transaction-details'>
                                <div class='detail-item'>
                                    <strong>Cliente:</strong>
                                    <p>{0}</p>
                                </div>
                                <div class='detail-item'>
                                    <strong>Evento:</strong>
                                    <p>{1}</p>
                                </div>
                                <div class='detail-item'>
                                    <strong>Cantidad Boletos:</strong>
                                    <p>{2}</p>
                                </div>
                                <div class='detail-item'>
                                    <strong>Fecha Compra:</strong>
                                    <p>{3}</p>
                                </div>
                                    <div class='detail-item'>
                                    <strong>Total:</strong>
                                    <p>L {4}</p>
                                </div>
                            </div>
                        </div>
                    </div>
            </div>
                    <div class='footer'>
                        <p>¡Gracias por tu preferencia!</p>
                        <p>Para consultas sobre esta factura, por favor, comunícate con nuestro equipo.</p>
                        <a href='https://chat.whatsapp.com/LMOxoWARZtx40VueyTkgNd' class='whatsapp-link'>Equipo de Soporte en WhatsApp</a>
                    </div>
                </body>
                </html>", nombre, evento, cantidadB, fecha, TotalA);

                var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);

                var response = await client.SendEmailAsync(msg);

                //await DisplayAlert("Éxito", "El correo se envió correctamente.", "Aceptar");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Error al enviar el correo: " + ex.Message, "Aceptar");
            }
        }

        private void pagar_Clicked(object sender, EventArgs e)
        {
            if(boletosEntry.Text == "0")
            {
                DisplayAlert("Aviso", "Por favor ingrese la cantidad de boletos que desea comprar", "OK");
            }
            else {
                if (saldousuario > double.Parse(total.Text))
                {
                    guardarDatos();
                    RestarSaldo(correo);
                    EnviarCorreo();
                    DisplayAlert("Aviso", "'GRACIAS POR SU COMPRA'\n Por favor, revisa tu correo para obtener mas informacion de su compra", "OK");
                    Navigation.PopToRootAsync();
                }
                else
                {
                    DisplayAlert("Aviso", "No cuenta con suficiente saldo en su cuenta para realizar su compra", "OK");
                    Navigation.PopToRootAsync();
                }
            }
        }
        public void RestarSaldo(string correo)
        {
            saldoFinal = saldousuario - (double.Parse(total.Text));
            WebClient cliente = new WebClient();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            var parametros = new System.Collections.Specialized.NameValueCollection();
            parametros.Add("correo", correo); // Aquí se debe usar el nombre "correo"
            parametros.Add("nuevo_saldo", saldoFinal.ToString()); // Y aquí el nombre "nuevo_saldo"

            cliente.UploadValues("https://finalproyect.com/StartBarkPHP/ActualizarSaldo.php", "POST", parametros);
        }

    }
}