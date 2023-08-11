using Acr.UserDialogs;
using SendGrid.Helpers.Mail;
using SendGrid;
using Star_Bank_Grupo4.Controllers;
using Star_Bank_Grupo4.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Star_Bank_Grupo4.Clases;
using System.Threading;

namespace Star_Bank_Grupo4.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfirmarTransferencia : ContentPage
    {
        public ConfirmarTransferenciaViewModel ViewModel;
        private TransferenciasService transferenciasService = DependencyService.Get<TransferenciasService>();
        string correo = DatosCuentaSingleton.Instance.Correo;
        public ConfirmarTransferencia(ConfirmarTransferenciaViewModel confirmarTransferenciaViewModel)
        {
            InitializeComponent();
            ViewModel = confirmarTransferenciaViewModel;
            BindingContext = ViewModel;
        }

        private async void btntransferencia_Clicked(object sender, EventArgs e)
        {
            using (UserDialogs.Instance.Loading("Realizando transferencia", maskType: MaskType.Black))
            {
                await Task.Delay(1000);
                var datosParaResumen = await transferenciasService.RegistrarTransferencia(ViewModel);

                await DisplayAlert("Aviso", datosParaResumen.Mensaje, "Aceptar");

                if (datosParaResumen.Ok)
                {
                    MessagingCenter.Send<Application>(Application.Current, "transferencia-realizada");
                    EnviarCorreo();
                    await Navigation.PopToRootAsync();
                }
            }

        }
        private async void EnviarCorreo()
        {
            try
            {
                string nombreUsuario = txtnombre.Text;
                string montoTransferencia = txtmonto.Text;
                // Datos de la transacción
                string destino = txtnombredestino.Text;
                // Obtener la fecha y hora actual
                DateTime fechaActual = DateTime.Now;
                // Formatear la fecha en el formato deseado ("yyyy-MM-dd HH:mm:ss")
                string fecha = fechaActual.ToString("yyyy-MM-dd HH:mm:ss");
                string descripcion = txtcomentario.Text;
                //var apiKey = "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";
                var client = new SendGridClient("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");

                var from = new EmailAddress("calonaisaac@gmail.com", "Star Bank");
                var subject = "TRANSFERENCIAS";
                var to = new EmailAddress(correo, "Star Bank");
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
                            <p>Estimado {0},</p>
                            <p>Nos complace informarte que se ha realizado una transacción exitosa en tu cuenta por un monto total de {4}</p>
                            <h3>DETALLE DE LA TRANSACCION</h3>
                            <div class='transaction-details'>
                                <div class='detail-item'>
                                    <strong>Cliente Origen:</strong>
                                    <p>{0}</p>
                                </div>
                                <div class='detail-item'>
                                    <strong>Cliente Destino:</strong>
                                    <p>{1}</p>
                                </div>
                                <div class='detail-item'>
                                    <strong>Fecha:</strong>
                                    <p>{2}</p>
                                </div>
                            </div>
                            <div class='transaction-details'>
                                <div class='detail-item'>
                                    <strong>Descripción:</strong>
                                    <p>{3}</p>
                                </div>
                                <div class='detail-item'>
                                    <strong>Monto:</strong>
                                    <p>{4}</p>
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
                </html>", nombreUsuario, destino, fecha, descripcion, montoTransferencia);

                var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);

                var response = await client.SendEmailAsync(msg);

                //await DisplayAlert("Éxito", "El correo se envió correctamente.", "Aceptar");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Error al enviar el correo: " + ex.Message, "Aceptar");
            }
        }


    }
}