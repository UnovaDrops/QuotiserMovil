using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Entry = Microcharts.ChartEntry;
using SkiaSharp;
using System.Collections.Generic;
using Microcharts;
using Database.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using System.ComponentModel;
using QuotiserAppMobile.Helpers;
using QuotiserAppMobile.Views.Login;

namespace QuotiserAppMobile.Views.Panel
{
    [DesignTimeVisible(false)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [Preserve(AllMembers = true)]

    public partial class PanelVista : ContentPage
    {   
        List<CotizacionesAceyDen> ListaDeMeses = new List<CotizacionesAceyDen>();
        LlamadasApi ApiCall = new LlamadasApi();
        public PanelVista()
        {            
            //Mejorar rendimiento de la grafica
            InitializeComponent();           
            NavigationPage.SetHasNavigationBar(this, false);
            if(App.Current.Properties["Administrador"].Equals(1)){
            Task.Run(() => (GraficasBarras.Chart = new BarChart { Entries = GetChart(), LabelTextSize = 25 }));
            }
            else
            {
                Device.BeginInvokeOnMainThread(() => {
                    IndicatorCarga.IsRunning = false;
                    IndicatorCarga.IsVisible = false;
                    Aceptado.IsVisible = false;
                    Cancelado.IsVisible = false;
                    AcumuladosPorMes.IsVisible = false;
                });
            }                     
            NombreUsuario.Text = App.Current.Properties["NombreUsuario"].ToString();
            
        }      
        protected void BotonMenu(object sender, EventArgs e)
        {           
            navigationDrawer.IsOpen = true;
        }

        protected async void BotonLogOut(object sender, EventArgs e)
        {
            if (App.Current.Properties["Traducciones"].Equals("Ingles"))
            {
                bool answer = await DisplayAlert("ALERT", "¿Are you sure to log out?", "Yes", "No");
                if (answer.Equals(true))
                {
                    App.Current.Properties["IsLogin"] = "false";
                    App.Current.MainPage = new LoginVista();
                }
            }
            else if (App.Current.Properties["Traducciones"].Equals("Español"))
            {
                bool answer = await DisplayAlert("ALERTA", "¿Estás seguro de cerrar sesión?", "Si", "No");
                if (answer.Equals(true))
                {
                    App.Current.Properties["IsLogin"] = "false";
                    App.Current.MainPage = new LoginVista();
                }
            }
                  
        }

        
        private double MainConexionAceptada(int Year, int Month)
        {
            double Suma = 0;
            string url = "http://velarde3600-001-site6.etempurl.com/api/AdministradorCotizaciones/SolicitudAdministradorCotizaciones";
            TraerCompañia Conexion = new TraerCompañia();
            Conexion.CompañiaID = int.Parse(App.Current.Properties["CompañiaID"].ToString());
            string resultado = ApiCall.ReadStringJson<TraerCompañia>(url, Conexion, "POST");
            var jVariable = JsonConvert.DeserializeObject<List<DashBoard>>(resultado);
            var CotizacionesAceptadas = jVariable.Where(d => d.Aceptado == 1 && d.Enviada == 1 && d.Activo == 1 && d.FechaCreacion.Year == Year && d.FechaCreacion.Month == Month).Sum(d => d.CostoTotal);
            Suma = CotizacionesAceptadas;
            return Suma;
        }

        private double MainConexionDenegada(int Year, int Month)
        {
            double Suma = 0;
            string url = "http://velarde3600-001-site6.etempurl.com/api/AdministradorCotizaciones/SolicitudAdministradorCotizaciones";
            TraerCompañia Conexion = new TraerCompañia();
            Conexion.CompañiaID = int.Parse(App.Current.Properties["CompañiaID"].ToString());
            string resultado = ApiCall.ReadStringJson<TraerCompañia>(url, Conexion, "POST");
            var jVariable = JsonConvert.DeserializeObject<List<DashBoard>>(resultado);
            var CotizacionesDenegadas = jVariable.Where(d => d.Aceptado == 0 && d.Enviada == 1 && d.Activo == 0 && d.FechaCreacion.Year == Year && d.FechaCreacion.Month == Month).Sum(d => d.CostoTotal);
            Suma = CotizacionesDenegadas;
            return Suma;
        }
        public void GetMontosPorFecha()
        {
            DateTime thisDay = DateTime.Today;
            string fecha = "";

            fecha = thisDay.AddYears(-1).ToString("dd/MM/yyyy") + " - " + thisDay.ToString("dd/MM/yyyy");
            // convertir a date es-MX//

            String[] separador = { "-" };
            String[] Lista = fecha.Split(separador, StringSplitOptions.RemoveEmptyEntries);

            var culture = new CultureInfo("es-MX");

            DateTime FechaInicio = Convert.ToDateTime(Lista[0].Trim(), culture);

            Lista[1] = Lista[1].Replace(" ", string.Empty).TrimStart();
            DateTime FechaFinal = Convert.ToDateTime(Lista[1], culture);

            List<CotizacionesAceyDen> TablaMontos = ListaDeMesesMetodo(FechaInicio, FechaFinal);

        }
        public List<CotizacionesAceyDen> ListaDeMesesMetodo(DateTime Inicio, DateTime Final)
        {
            if (Inicio > Final) return new List<CotizacionesAceyDen>();

            var monthDiff = Math.Abs((Final.Year * 12 + (Final.Month - 1)) - (Inicio.Year * 12 + (Inicio.Month - 1)));

            if (Inicio.AddMonths(monthDiff) > Final || Final.Day < Inicio.Day)
            {
                monthDiff -= 1;
            }

            List<DateTime> results = new List<DateTime>();
            for (int i = monthDiff; i >= 0; i--)
            {
                results.Add(Final.AddMonths(-i));
            }

            foreach (var dateTime in results)
            {
                CotizacionesAceyDen item = new CotizacionesAceyDen
                {
                    Month = dateTime.ToString("MMM"),
                    Year = dateTime.Year,
                    Fecha = dateTime,
                    MontoAceptado = (float)MainConexionAceptada(dateTime.Year, dateTime.Month),
                    MontoCancelado = (float)MainConexionDenegada(dateTime.Year, dateTime.Month),
                };
                ListaDeMeses.Add(item);
            }
            return ListaDeMeses;
        }

        


        public List<Entry> GetChart()
        {
            GetMontosPorFecha();
            List<Entry> data = new List<Entry>();
            ListaDeMeses.Reverse();
            
            foreach (var item in ListaDeMeses)
            {
                //10 = a 6 meses
                if (data.Count <= 10)
                {
                    data.Add(new Entry(item.MontoCancelado)
                    {
                        Label = item.Month + " " + item.Year,
                        ValueLabel = "$" + item.MontoCancelado.ToString(),
                        Color = SKColor.Parse("#E70C0C"),
                        ValueLabelColor = SKColor.Parse("#000000"),
                    });

                    data.Add(new Entry(item.MontoAceptado)
                    {
                       
                        Label = item.Month + " " + item.Year,
                        ValueLabel = "$" + item.MontoAceptado.ToString(),
                        Color = SKColor.Parse("#28D808"),
                        ValueLabelColor = SKColor.Parse("#000000"),
                    });
                }
                
            }

            data.Reverse();
            Device.BeginInvokeOnMainThread(() => {
                IndicatorCarga.IsRunning = false;
                IndicatorCarga.IsVisible = false;
            });
            
            return data;
        }

       

    }
}
