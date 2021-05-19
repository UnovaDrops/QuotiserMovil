using System.Collections.Generic;
using System.Collections.ObjectModel;
using Database.Models;
using Newtonsoft.Json;
using QuotiserAppMobile.Models;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using System.Threading.Tasks;
using System.Globalization;
using QuotiserAppMobile.Helpers;
using QuotiserAppMobile.Helpers.Traducciones;

namespace QuotiserAppMobile.ViewModels
{
    [Preserve(AllMembers = true)]
    public class PorEnviarViewModel : BaseViewModel
    {
        LlamadasApi ApiCall = new LlamadasApi();

        private ObservableCollection<DataTableCotizacion> items;
        public ImageSource Banner { get; set; }
        public PorEnviarViewModel()
        {
            
            if (DatosClienteStataicos.BanderaTablaCotizacion.Equals(0))
            {
                Task.Run(() => Traducciones());
                Task.Run(() => TablaCotizaciones());                
            }                                                   
        }
     

        private string ConexionServicio(int CotizacionID)
        {           
            string url = "http://velarde3600-001-site6.etempurl.com/api/Servicio/SolicitudServicio";
            TraerServicio Conexion = new TraerServicio();
            Conexion.CotizacionID = CotizacionID;
            string resultado = ApiCall.ReadStringJson<TraerServicio>(url, Conexion, "POST");
            List<DatosServicio> Compañias = JsonConvert.DeserializeObject<List<DatosServicio>>(resultado);
            string NombreServicio = "";
            foreach (var item in Compañias)
            {
                NombreServicio = item.DescripcionServicio;
            }
            return NombreServicio;

        }
        public async void TablaCotizaciones()
        {
                await Task.Delay(1);
               
                if (App.Current.Properties["Administrador"].Equals(1))
                {

                    
                    string resultado = "";
                    string url = "http://velarde3600-001-site6.etempurl.com/api/AdministradorCotizaciones/SolicitudAdministradorCotizaciones";
                    TraerCompañia Conexion = new TraerCompañia();
                    Conexion.CompañiaID = int.Parse(App.Current.Properties["CompañiaID"].ToString());
                    resultado = ApiCall.ReadStringJson<TraerCompañia>(url, Conexion, "POST");//Checar AQUI                 
                    var jVariable = JsonConvert.DeserializeObject<List<DashBoard>>(resultado);
                    jVariable.Reverse();
                    this.Items = new ObservableCollection<DataTableCotizacion>();
                    NumberFormatInfo formato = new CultureInfo("es-AR").NumberFormat;
                    formato.NumberDecimalSeparator = ".";
                    formato.NumberGroupSeparator = ",";
                    if (jVariable.Count == 0)
                    {
                        MensajeNoCotizacion();
                    }
                    else
                    {
                    
                        

                        foreach (var item in jVariable)
                        {
                            if (item.Activo.Equals(1) && item.Enviada.Equals(0) && item.Deleted.Equals(0))
                            {
                                if (item.Autorizar.Equals(1))
                                {

                                    this.Items.Add(new DataTableCotizacion
                                    {
                                        ProductoServicio = ConexionServicio(item.IdCotizacion),
                                        SerialNumber = item.Consecutivo.ToString(),
                                        ClubName = item.NombreProyecto,
                                        GoldPoints = "Yes", // Identificador de traduccion - CuotaAceptada_SI
                                        Points = "$" + item.CostoTotal.ToString("N", formato),
                                        ValorBoton = item.IdCotizacion,
                                        ColorText = "#28D808"
                                    });

                                }
                                else
                                {
                                    this.Items.Add(new DataTableCotizacion
                                    {
                                        ProductoServicio = ConexionServicio(item.IdCotizacion),
                                        SerialNumber = item.Consecutivo.ToString(),
                                        ClubName = item.NombreProyecto,
                                        GoldPoints = "No", // Identificador de traduccion - CuotaAceptada_NO
                                        Points = "$" + item.CostoTotal.ToString("N", formato),
                                        ValorBoton = item.IdCotizacion,
                                        ColorText = "#E70C0C",
                                        
                                    });
                                }

                            }
                        }
                    }

                }
                else
                {

                    string url = "http://velarde3600-001-site6.etempurl.com/api/DashBoardInfo/SolicitudUser";
                    UserID Conexion = new UserID();
                    Conexion.UsuarioID = int.Parse(App.Current.Properties["UsuarioID"].ToString());
                    string resultado = ApiCall.ReadStringJson<UserID>(url, Conexion, "POST");
                    var jVariable = JsonConvert.DeserializeObject<List<DashBoard>>(resultado);
                    this.Items = new ObservableCollection<DataTableCotizacion>();
                    jVariable.Reverse();
                    if (jVariable.Count == 0)
                    {
                        MensajeNoCotizacion();
                    }
                    else
                    {
                    //Condicion de traducciones
                       
                        foreach (var item in jVariable)
                        {
                            if (item.Activo.Equals(1) && item.Enviada.Equals(0) && item.Deleted.Equals(0))
                            {

                                if (item.Autorizar.Equals(1))
                                {

                                    this.Items.Add(new DataTableCotizacion
                                    {
                                        ProductoServicio = ConexionServicio(item.IdCotizacion),
                                        SerialNumber = item.Consecutivo.ToString(),
                                        ClubName = item.NombreProyecto,
                                        GoldPoints = "Yes", // Identificador de traduccion - CuotaAceptada_SI
                                        Points = "$" + item.CostoTotal.ToString(),
                                        ValorBoton = item.IdCotizacion,
                                        ColorText = "#28D808"
                                    });
                                }
                                else
                                {
                                    this.Items.Add(new DataTableCotizacion
                                    {
                                        ProductoServicio = ConexionServicio(item.IdCotizacion),
                                        SerialNumber = item.Consecutivo.ToString(),
                                        ClubName = item.NombreProyecto,
                                        GoldPoints = "No",  // Identificador de traduccion - CuotaAceptada_NO
                                        Points = "$" + item.CostoTotal.ToString(),
                                        ValorBoton = item.IdCotizacion,
                                        ColorText = "#E70C0C",
                                        

                                    });
                                }

                            }


                        }
                    }

                }
        }
        public void Traducciones()
        {
            
            Español TraduccionEspañol = new Español();
            Ingles TraduccionIngles = new Ingles();
            if (App.Current.Properties["Traducciones"].Equals("Ingles"))
            {
                TituloSend = TraduccionIngles.TituloSend_Ingles;
                TituloBarraBusqueda = TraduccionIngles.TituloBarraBusqueda_Ingles;
                Banner = "bannercotizacioningles.jpg";

                NombreProyecto = TraduccionIngles.NombreProyecto_Ingles;
                Servicio = TraduccionIngles.Servicio_Ingles;
                Autorizado = TraduccionIngles.Autorizado_Ingles;
                Costo = TraduccionIngles.Costo_Ingles;
                Opcion = TraduccionIngles.Opcion_Ingles;
                
            }
            else if (App.Current.Properties["Traducciones"].Equals("Español"))
            {
                TituloSend = TraduccionEspañol.TituloSend_Español;
                TituloBarraBusqueda = TraduccionEspañol.TituloBarraBusqueda_Español;
                Banner = "bannercotizacionespañol.jpg";

                NombreProyecto = TraduccionEspañol.NombreProyecto_Español;
                Servicio = TraduccionEspañol.Servicio_Español;
                Autorizado = TraduccionEspañol.Autorizado_Español;
                Costo = TraduccionEspañol.Costo_Español;
                Opcion = TraduccionEspañol.Opcion_Español;
                
            }
        }
        public void MensajeNoCotizacion()
        {
            if (App.Current.Properties["Traducciones"].Equals("Ingles"))
            {
                Device.BeginInvokeOnMainThread(() => {
                    App.Current.MainPage.DisplayAlert("Sorry!", "There is no Quotes pending.", "OK");
                    App.Current.MainPage.Navigation.PopModalAsync();
                });

            }
            else if (App.Current.Properties["Traducciones"].Equals("Español"))
            {
                Device.BeginInvokeOnMainThread(() => {
                    App.Current.MainPage.DisplayAlert("¡Lo siento! ", "No hay cotizaciones pendientes.", "OK");
                    App.Current.MainPage.Navigation.PopModalAsync();
                });
            }


        }


        private string TituloSend_Cambio;
        
        private string TituloBarraBusqueda_Cambio;
       
        public string TituloSend
        {
            get { return TituloSend_Cambio; }
            set { TituloSend_Cambio = value; OnPropertyChanged(); }
        }
        public string TituloBarraBusqueda
        {
            get { return TituloBarraBusqueda_Cambio; }
            set { TituloBarraBusqueda_Cambio = value; OnPropertyChanged(); }
        }

        public static string NombreProyecto { get; set; }

        public static string Servicio { get; set; }

        public static string Autorizado { get; set; }

        public static string Costo { get; set; }

        public static string Opcion { get; set; }



        public ObservableCollection<DataTableCotizacion> Items
        {
            get
            {                
                return this.items;
            }
            set
            {               
                this.items = value;
                OnPropertyChanged(); //Actualiza los datos de la tabla
            }
        }

       


        private bool _isRefreshing;
        private Command _refreshViewCommand;      
        public bool IsRefreshing
        {
        
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }
        public Command RefreshViewCommand
        {
            get
            {
                return _refreshViewCommand ?? (_refreshViewCommand = new Command(() =>
                {
                    this.RefreshData();
                }));
            }
        }
        private async void RefreshData()
        {
            
            await Task.Run(() => TablaCotizaciones());             
            this.IsRefreshing = false;
            
        }
    }
}
