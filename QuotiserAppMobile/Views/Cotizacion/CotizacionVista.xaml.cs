using Database.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using System.Globalization;
using Xamarin.Essentials;
using System.ComponentModel;
using QuotiserAppMobile.Helpers;
using QuotiserAppMobile.Helpers.Traducciones;

namespace QuotiserAppMobile.Views.Cotizacion
{
    [DesignTimeVisible(false)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [Preserve(AllMembers = true)]
    public partial class CotizacionVista : ContentPage
    {
        string telefono = "";
        string DesglosePDF;
        LlamadasApi ApiCall = new LlamadasApi();
        public CotizacionVista()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            DesglosePDF = "PDFFormato1_Ingles";
            UserImage.Source = "WhitelogoQ.png";              
            BackButton.BackgroundImage = "backbutton.png";
            Traducciones();
            DatosClienteStataicos.BanderaTablaCotizacion = 0;           
            Task.Run(() => ConexionCliente());
            Task.Run(() => MostrarObjetos());
        }


        public async void MostrarObjetos()
        {

            await Task.Delay(2000);
            Device.BeginInvokeOnMainThread(() => {
                IndicatorCarga.IsRunning = false;
                MostrarDatosCotizacion.IsVisible = true;                
                PDF.Source = "pdfdownload.png";
                LLAMAR.Source = "llamar.png";               
            });

            if (!App.Current.Properties["Administrador"].Equals(1))
            {
                Device.BeginInvokeOnMainThread(() => {
                    AutorizarBoton.IsVisible = false;
                });
            }
               

        }
        protected async void BotonEnviarCotizacion(object sender, EventArgs e)
        {
            if (App.Current.Properties["Administrador"].Equals(1))
            {
                IndicatorAutorizacion.IsRunning = true;
                await Task.Delay(1000);
                string url = "http://velarde3600-001-site6.etempurl.com/api/Autorizar/SolicitudCompañias";
                AutorizarCotizacion Conexion = new AutorizarCotizacion();
                Conexion.CotizacionID = int.Parse(TresObjetos.IDCotizacionEstatico);
                Conexion.Autorizacion = 1;
                string resultado = ApiCall.ReadStringJson<AutorizarCotizacion>(url, Conexion, "PATCH");

                
                string url2 = "http://velarde3600-001-site6.etempurl.com/api/Envio/SolicitudEnvio";
                EnviarCotizacion Conexion2 = new EnviarCotizacion();
                Conexion2.CotizacionID = int.Parse(TresObjetos.IDCotizacionEstatico);
                Conexion2.Enviado = 1;
                string resultado2 = ApiCall.ReadStringJson<EnviarCotizacion>(url2, Conexion2, "PATCH");
                MensajeEnviado();
                
            }
            else
            {
                IndicatorAutorizacion.IsRunning = true;
                await Task.Delay(1000);
                if (DatosClienteStataicos.Autorizado.Equals("1"))
                {
                    string url = "http://velarde3600-001-site6.etempurl.com/api/Envio/SolicitudEnvio";
                    EnviarCotizacion Conexion = new EnviarCotizacion();
                    Conexion.CotizacionID = int.Parse(TresObjetos.IDCotizacionEstatico);
                    Conexion.Enviado = 1;
                    string resultado = ApiCall.ReadStringJson<EnviarCotizacion>(url, Conexion, "PATCH");
                    MensajeEnviado();
                }
                else
                {
                    MensajeEnvioNoAutorizado();
                }
            }

            
            
        }

        private void ComboBox_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            var SelectedItem = e.Value;
            if (App.Current.Properties["Traducciones"].Equals("Ingles"))
            {

                if (SelectedItem.ToString().Equals("Breakdown 2"))
                {
                    DesglosePDF = "PDFFormato2_Ingles";
                }
                else
                {
                    DesglosePDF = "PDFFormato1_Ingles";
                }

            }
            else if (App.Current.Properties["Traducciones"].Equals("Español"))
            {
                if (SelectedItem.ToString().Equals("Desglose 2"))
                {
                    DesglosePDF = "PDFFormato2_Español";
                }
                else
                {
                    DesglosePDF = "PDFFormato1_Español";
                }
            }
               
        }

        protected async void BotonAutorizar(object sender, EventArgs e)
        {
           
                IndicatorAutorizacion.IsRunning = true;
                await Task.Delay(1000);
                string url = "http://velarde3600-001-site6.etempurl.com/api/Autorizar/SolicitudCompañias";
                AutorizarCotizacion Conexion = new AutorizarCotizacion();
                Conexion.CotizacionID = int.Parse(TresObjetos.IDCotizacionEstatico);
                Conexion.Autorizacion = 1;
                string resultado = ApiCall.ReadStringJson<AutorizarCotizacion>(url, Conexion, "PATCH");
                MensajeAutorizacion();
           
           
        }

        protected async void BotonDelate(object sender, EventArgs e)
        {
            IndicatorAutorizacion.IsRunning = true;
            await Task.Delay(1000);
            string url = "http://velarde3600-001-site6.etempurl.com/api/Delate/SolicitudCompañias";
            BorrarCotizacion Conexion = new BorrarCotizacion();
            Conexion.CotizacionID = int.Parse(TresObjetos.IDCotizacionEstatico);
            Conexion.Enviado = 1;
            Conexion.Activo = 0;
            Conexion.Delate = 1;
            string resultado = ApiCall.ReadStringJson<BorrarCotizacion>(url, Conexion, "PATCH");
            MensajeBorrado();
        }

        private async void ConexionCliente()
        {
            await Task.Delay(1);
            string url = "http://velarde3600-001-site6.etempurl.com/api/ClientesCotizacion/SolicitudClientes";
            TraerServicio Conexion = new TraerServicio();
            Conexion.CotizacionID = int.Parse(TresObjetos.IDCotizacionEstatico);
            string resultado = ApiCall.ReadStringJson<TraerServicio>(url, Conexion, "POST");
            List<DatosCliente> Clientes = JsonConvert.DeserializeObject<List<DatosCliente>>(resultado);

            
            foreach (var item in Clientes)
            {

                Device.BeginInvokeOnMainThread(() => {
                    Contacto.Text = item.NombreCliente + " " + item.ApellidoCliente;
                    Telefono.Text = item.Telefono;
                    Empresa.Text = item.EmpresaNombre;
                    ProductosServicios.Text = ConexionServicio();
                    telefono = item.Telefono;
                });
                
                                                               
            }
            ConexionCostoTotalUbicacion();
        }

        private void ConexionCostoTotalUbicacion()
        {
            Español TraduccionEspañol = new Español();
            Ingles TraduccionIngles = new Ingles();
            string url = "http://velarde3600-001-site6.etempurl.com/api/TraerCostoTotalCotizacion/SolicitudCostoTotal";
            TraerServicio Conexion = new TraerServicio();
            Conexion.CotizacionID = int.Parse(TresObjetos.IDCotizacionEstatico);
            string resultado = ApiCall.ReadStringJson<TraerServicio>(url, Conexion, "POST");
            List<DatosUbicacionCostoTotal> CostoTotal2 = JsonConvert.DeserializeObject<List<DatosUbicacionCostoTotal>>(resultado);

            NumberFormatInfo formato = new CultureInfo("es-AR").NumberFormat;
            formato.NumberDecimalSeparator = ".";
            formato.NumberGroupSeparator = ",";
            //Condicion de traducciones
            if (App.Current.Properties["Traducciones"].Equals("Ingles"))
            {
                foreach (var item in CostoTotal2)
                {
                    Device.BeginInvokeOnMainThread(() => {
                        NombreProyecto.Text = TraduccionIngles.NombreProyectoAutorizacion_Ingles + item.NombreProyecto; //Identificador de traduccion - NombreProyecto

                        Ubicacion.Text = ConexionEstado(item.EstadoId);

                        CostoTotal.Text = "$" + item.CostoTotal.ToString("N", formato);

                        DatosClienteStataicos.Autorizado = item.Autorizar.ToString();
                    });


                }

            }
            else if (App.Current.Properties["Traducciones"].Equals("Español"))
            {

                foreach (var item in CostoTotal2)
                {
                    Device.BeginInvokeOnMainThread(() => {
                        NombreProyecto.Text = TraduccionEspañol.NombreProyectoAutorizacion_Español + item.NombreProyecto; //Identificador de traduccion - NombreProyecto

                        Ubicacion.Text = ConexionEstado(item.EstadoId);

                        CostoTotal.Text = "$" + item.CostoTotal.ToString("N", formato);

                        DatosClienteStataicos.Autorizado = item.Autorizar.ToString();
                    });


                }
            }
           

        }

        private string ConexionServicio()
        {


            string url = "http://velarde3600-001-site6.etempurl.com/api/Servicio/SolicitudServicio";
            TraerServicio Conexion = new TraerServicio();
            Conexion.CotizacionID = int.Parse(TresObjetos.IDCotizacionEstatico); 
            string resultado = ApiCall.ReadStringJson<TraerServicio>(url, Conexion, "POST");
            List<DatosServicio> Compañias = JsonConvert.DeserializeObject<List<DatosServicio>>(resultado);
            string NombreServicio = "";
            foreach (var item in Compañias)
            {
                NombreServicio = item.DescripcionServicio;
                Device.BeginInvokeOnMainThread(() => {                   
                    TipoProductos.Text = item.CantidadServicios.ToString();
                });
              

            }
            return NombreServicio;

        }

        private string ConexionEstado(int EstadoId)
        {


            string url = "http://velarde3600-001-site6.etempurl.com/api/Estados/SolicitudEstados";
            EstadoID Conexion = new EstadoID();
            Conexion.EstadosID = EstadoId;
            string resultado = ApiCall.ReadStringJson<EstadoID>(url, Conexion, "POST");
            List<DatosEstado> Estados = JsonConvert.DeserializeObject<List<DatosEstado>>(resultado);
            string NombreEstado = "";
            foreach (var item in Estados)
            {

                NombreEstado = item.NombreEstado;


            }
            return NombreEstado;

        }

        public void Traducciones()
        {
            Español TraduccionEspañol = new Español();
            Ingles TraduccionIngles = new Ingles();
           
           
            //Condicion de traducciones
            if (App.Current.Properties["Traducciones"].Equals("Ingles"))
            {
                TituloAutorizacion.Text = TraduccionIngles.TituloAutorizacion_Ingles;
                ContactoTexto.Text = TraduccionIngles.ContactoTexto_Ingles;
                TelefonoTexto.Text = TraduccionIngles.TelefonoTexto_Ingles;
                EmpresaTexto.Text = TraduccionIngles.EmpresaTexto_Ingles;
                UbicacionTexto.Text = TraduccionIngles.UbicacionTexto_Ingles;
                TipoProductosTexto.Text = TraduccionIngles.TipoProductosTexto_Ingles;
                ProductosServiciosTexto.Text = TraduccionIngles.ProductosServiciosTexto_Ingles;
                CostoTotalTexto.Text = TraduccionIngles.CostoTotalTexto_Ingles;
                comboBox.Text = TraduccionIngles.comboBox_Ingles;
                EnviarBoton.Text = TraduccionIngles.EnviarBoton_Ingles;
                BorrarBoton.Text = TraduccionIngles.BorrarBoton_Ingles;
                AutorizarBoton.Text = TraduccionIngles.AutorizarBoton_Ingles;

            }
            else if (App.Current.Properties["Traducciones"].Equals("Español"))
            {
                TituloAutorizacion.Text = TraduccionEspañol.TituloAutorizacion_Español;
                ContactoTexto.Text = TraduccionEspañol.ContactoTexto_Español;
                TelefonoTexto.Text = TraduccionEspañol.TelefonoTexto_Español;
                EmpresaTexto.Text = TraduccionEspañol.EmpresaTexto_Español;
                UbicacionTexto.Text = TraduccionEspañol.UbicacionTexto_Español;
                TipoProductosTexto.Text = TraduccionEspañol.TipoProductosTexto_Español;
                ProductosServiciosTexto.Text = TraduccionEspañol.ProductosServiciosTexto_Español;
                CostoTotalTexto.Text = TraduccionEspañol.CostoTotalTexto_Español;
                comboBox.Text = TraduccionEspañol.comboBox_Español;
                EnviarBoton.Text = TraduccionEspañol.EnviarBoton_Español;
                BorrarBoton.Text = TraduccionEspañol.BorrarBoton_Español;
                AutorizarBoton.Text = TraduccionEspañol.AutorizarBoton_Español;

            }

        }



        async void MensajeEnvioNoAutorizado()
        {
            IndicatorAutorizacion.IsRunning = false;
            
            if (App.Current.Properties["Traducciones"].Equals("Ingles"))
            {
                await App.Current.MainPage.DisplayAlert("Unauthorized Quote", "Check if this quote is authorized", "OK");
            }
            else if (App.Current.Properties["Traducciones"].Equals("Español"))
            {
                await App.Current.MainPage.DisplayAlert("Cotización no autorizada", "Comprueba si esta cotización está autorizada ", "OK");
            }
        }

        async void MensajeAutorizacion()
        { 
            
            ConexionCliente();                             
            IndicatorAutorizacion.IsRunning = false;
            if (App.Current.Properties["Traducciones"].Equals("Ingles"))
            {
                await App.Current.MainPage.DisplayAlert("Authorized Quote", "Your quote has been satisfactorily authorized", "OK");
            }
            else if (App.Current.Properties["Traducciones"].Equals("Español"))
            {
                await App.Current.MainPage.DisplayAlert("Cotización autorizada", "Su cotización ha sido autorizada satisfactoriamente", "OK");
            }
                       
        }

        async void MensajeBorrado()
        {              
            IndicatorAutorizacion.IsRunning = false;
            
            if (App.Current.Properties["Traducciones"].Equals("Ingles"))
            {
                await App.Current.MainPage.DisplayAlert("Quote Deleted", "Your quote has been deleted", "OK");
            }
            else if (App.Current.Properties["Traducciones"].Equals("Español"))
            {
                await App.Current.MainPage.DisplayAlert("Cotizacion eliminada", "Tu cotizacion a sido eliminada", "OK");
            }
            await App.Current.MainPage.Navigation.PopModalAsync();
        }
        async void MensajeEnviado()
        {            
            IndicatorAutorizacion.IsRunning = false;
            
            if (App.Current.Properties["Traducciones"].Equals("Ingles"))
            {
                await App.Current.MainPage.DisplayAlert("Quote Sent", "Your quote has been sent successfully", "OK");
            }
            else if (App.Current.Properties["Traducciones"].Equals("Español"))
            {
                await App.Current.MainPage.DisplayAlert("Cotización enviada ", "Su cotización ha sido enviada con éxito", "OK");
            }
            await App.Current.MainPage.Navigation.PopModalAsync();
           
        }

        protected async void VerPDF(object sender, EventArgs e)
        {
            IndicatorAutorizacion.IsRunning = true;
            await Task.Delay(100);
            string url = "http://velarde3600-001-site6.etempurl.com/api/PDF/"+ DesglosePDF;
            PDFCotizacion Conexion = new PDFCotizacion();
            Conexion.CotizacionID = int.Parse(TresObjetos.IDCotizacionEstatico);
            byte [] rest = ApiCall.ReadByteJson<PDFCotizacion>(url, Conexion, "POST");//Metodo super lento
            MemoryStream stream = new MemoryStream(rest);          
            await DependencyService.Get<ISave>().SaveAndView("Cotizacion.pdf", "application/pdf", stream);
            IndicatorAutorizacion.IsRunning = false;

        }
        protected void Llamar(object sender, EventArgs e)

        {
            Launcher.OpenAsync("tel:" + telefono);

        }

        protected async void BotonDeRetroceso(object sender, EventArgs e)
        {
           await App.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}