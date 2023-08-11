using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Net;
using Star_Bank_Grupo4.Clases;

namespace Star_Bank_Grupo4.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfirmacionPago : ContentPage
    {
        int servicio_id;
        string cuentaCliente;
        string fecha;
        string correo = DatosCuentaSingleton.Instance.Correo;
        string saldo = DatosCuentaSingleton.Instance.Saldo;
        double saldoUsuario = 0.0;
        double saldoFinal = 0.0;

        public ConfirmacionPago(string nombre,string descrip,string cuenta,int cuentaS,double total,int idS)
        {
            InitializeComponent();
            nombreC.Text = nombre;
            cuentaCliente = cuenta;
            descripcion.Text = descrip;
            cuentaServicio.Text = cuentaS.ToString();
            totalS.Text = total.ToString();
            servicio_id = idS;
            // Obtener la fecha y hora actual
            DateTime fechaActual = DateTime.Now;

            // Formatear la fecha en el formato deseado ("yyyy-MM-dd HH:mm:ss")
            fecha = fechaActual.ToString("yyyy-MM-dd HH:mm:ss");
            saldoUsuario = double.Parse(saldo);

        }


        private async void EnviarCorreo()
        {
            try
            {
                //var apiKey = "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";
                string nombreUsuario = nombreC.Text;
                string montoTransferencia = totalS.Text;
                // Datos de la transacción
                string destino = descripcion.Text;
                // Obtener la fecha y hora actual
                DateTime fechaActual = DateTime.Now;
                // Formatear la fecha en el formato deseado ("yyyy-MM-dd HH:mm:ss")
                string fecha = fechaActual.ToString("yyyy-MM-dd HH:mm:ss");
                //string descripcion = txtcomentario.Text;
                var client = new SendGridClient("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");

                var from = new EmailAddress("calonaisaac@gmail.com", "Star Bank");
                var subject = "PAGO DE SERVICIOS";
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
                            <p>Gracias por Preferirnos como método de pago. Nos complace informarte que se ha realizado de manera exitosa el pago del servicio de {1}. 
                             Recuerda mantener tus cuotas al día utilizando nuestra aplicación para mayor facilidad al momento de efectuar tus pagos mensuales. 
                             A continuación los detalles del pago de servicio {1}.</p>
                            <h3>PAGO DE SERVICIO</h3>
                            <div class='transaction-details'>
                                <div class='detail-item'>
                                    <strong>Cliente Origen:</strong>
                                    <p>{0}</p>
                                </div>
                                <div class='detail-item'>
                                    <strong>Proveedor:</strong>
                                    <p>{1}</p>
                                </div>
                                <div class='detail-item'>
                                    <strong>Fecha:</strong>
                                    <p>{2}</p>
                                </div>
                                    <div class='detail-item'>
                                    <strong>Cuota:</strong>
                                    <p>{3} L.</p>
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
                </html>", nombreUsuario, destino, fecha, montoTransferencia);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);

                var response = await client.SendEmailAsync(msg);

                //await DisplayAlert("Éxito", "El correo se envió correctamente.", "Aceptar");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Error al enviar el correo: " + ex.Message, "Aceptar");
            }
        }
        //boton guardar
        public void guardarDatos()
        {
            WebClient cliente = new WebClient();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            var parametros = new System.Collections.Specialized.NameValueCollection();
            parametros.Add("cuenta_cliente", cuentaCliente);
            parametros.Add("servicio_id", servicio_id.ToString());
            parametros.Add("monto", totalS.Text);
            parametros.Add("fecha_pago", fecha);


            cliente.UploadValues("https://finalproyect.com/StartBarkPHP/PagoServicios.php", "POST", parametros);
        }
        public void RestarSaldo(string correo)
        {
            saldoFinal = saldoUsuario - (double.Parse(totalS.Text));
            WebClient cliente = new WebClient();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            var parametros = new System.Collections.Specialized.NameValueCollection();
            parametros.Add("correo", correo); // Aquí se debe usar el nombre "correo"
            parametros.Add("nuevo_saldo", saldoFinal.ToString()); // Y aquí el nombre "nuevo_saldo"

            cliente.UploadValues("https://finalproyect.com/StartBarkPHP/ActualizarSaldo.php", "POST", parametros);
        }

        private void Pagar_Clicked(object sender, EventArgs e)
        {
            guardarDatos();
            RestarSaldo(correo);
            EnviarCorreo();
            DisplayAlert("Aviso", "Su pago se a realizado correctamente, para mas detalle revise su correo", "OK");
            Navigation.PopToRootAsync();
        }

        private void cancelar_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Servicio());
        }
    }
}