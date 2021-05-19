using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using Database.Models;
using Newtonsoft.Json;
using QuotiserAppMobile.Models.QuotiserDashboard;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;
using Menu = Database.Models.Menu;
using QuotiserAppMobile.Views.PorEnviar;
using QuotiserAppMobile.Helpers;
using QuotiserAppMobile.Helpers.Traducciones;

namespace QuotiserAppMobile.ViewModels
{
    /// <summary>
    /// ViewModel for stock overview page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class PanelViewModel : BaseViewModel
    {
        LlamadasApi ApiCall = new LlamadasApi();

        public ImageSource Banner { get; set; }

        private ObservableCollection<Datos3Objetos> listItems;
        public ObservableCollection<Menu> MenuDatos { get; set; }
        public Command<object> SelectionCommand { get; set; }
        public Command<object> SelectMenu { get; set; }
        
        public PanelViewModel()
        {
            
            TresObjetosClase();
            Traducciones();
            TraduccionesMenu();
          
        }

       
        private async void listView_ItemSelected(object obj)
        {
            
            var menu = (obj as Syncfusion.ListView.XForms.ItemTappedEventArgs).ItemData as Menu;
            if (menu.MenuDetail.Equals("Send"))
            {
                await App.Current.MainPage.Navigation.PushModalAsync(new PorEnviarVista());
                            
            }
            else if (menu.MenuDetail.Equals("DashBoard"))
            {
                CloseOpen = false;
            }
            else if (menu.MenuDetail.Equals("Language"))
            {
                
                //Al entrar aqui falla
                if (App.Current.Properties["Traducciones"].Equals("Ingles"))
                {
                    //Este Display Alert esta creaedo de esta manera porque esta ubicado en un ViewModel
                    bool answer = await Application.Current.MainPage.DisplayAlert("English to Spanish", "¿Are you sure to change the language?", "Yes", "No");
                    if (answer.Equals(true))
                    {
                        App.Current.Properties["Traducciones"] = "Español";
                        Traducciones();
                        await Task.Run(() => TresObjetosClase());
                        await Task.Run(() => TraduccionesMenu());                       
                    }
                }
                else if (App.Current.Properties["Traducciones"].Equals("Español"))
                {
                    bool answer = await Application.Current.MainPage.DisplayAlert("Español a Ingles", "¿Estas seguro de cambiar el lenguaje?", "Si", "No");
                    if (answer.Equals(true))
                    {
                        App.Current.Properties["Traducciones"] = "Ingles";
                        Traducciones();
                        await Task.Run(() => TresObjetosClase());
                        await Task.Run(() => TraduccionesMenu());
                        

                    }
                }
                await Application.Current.SavePropertiesAsync();
            }
        }
        public void TresObjetosClase()
        {
            string url = "http://velarde3600-001-site6.etempurl.com/api/AdministradorCotizaciones/SolicitudAdministradorCotizaciones";
            TraerCompañia Conexion = new TraerCompañia();
            Conexion.CompañiaID = int.Parse(App.Current.Properties["CompañiaID"].ToString());
            string resultado = ApiCall.ReadStringJson<TraerCompañia>(url, Conexion, "POST");
            var jVariable = JsonConvert.DeserializeObject<List<DashBoard>>(resultado);


            string UrlClientes = "http://velarde3600-001-site6.etempurl.com/api/TotalClientes/TotalClientes";
            InsertClientes ConexionClientes = new InsertClientes();
            ConexionClientes.CompanyId = int.Parse(App.Current.Properties["CompañiaID"].ToString());
            ConexionClientes.Activo = 1;
            string resultadoClientes = ApiCall.ReadStringJson<InsertClientes>(UrlClientes, ConexionClientes, "POST");
            var ListaClientes = JsonConvert.DeserializeObject<List<ListaClientes>>(resultadoClientes);
           
            int TotalClientes = ListaClientes.Count();


            int TotalActivos = jVariable.Where(d=> d.Enviada ==1 && d.Activo ==1).Sum(item => item.Activo);
            var TotalCotizacion = jVariable.Where(d => d.Aceptado == 1 && d.Activo == 1).ToList();
            int GananciasTotales = 0;

            foreach (var Numero in TotalCotizacion)
            {
                GananciasTotales = GananciasTotales + (Int32)Numero.CostoTotal;
            }

            NumberFormatInfo formato = new CultureInfo("es-AR").NumberFormat;
            formato.NumberDecimalSeparator = ".";
            formato.NumberGroupSeparator = ",";

            Español TraduccionEspañol = new Español();
            Ingles TraduccionIngles = new Ingles();


            //Condicion de traducciones
            if (App.Current.Properties["Traducciones"].Equals("Ingles"))
            {
                this.ListItems = new ObservableCollection<Datos3Objetos>()
                {
                new Datos3Objetos()
                {
                    TextoInferior = TraduccionIngles.CuotasAceptadas_Ingles, //CuotasAceptadas ---Identificador de traduccion
                    CostoTotal = "$ " + GananciasTotales.ToString("N", formato),
                    Icono = "costocotizacion.jpg",
                    BackgroundGradientStart = "#99CC33"
                },
                new Datos3Objetos()
                {
                    TextoInferior = TraduccionIngles.TotalClientes_Ingles,//TotalClientes ---Identificador de traduccion
                    CostoTotal = TotalClientes.ToString(),
                    Icono = "clientescotizacion.jpg",
                    BackgroundGradientStart = "#00B1F5"
                },
                new Datos3Objetos()
                {
                    TextoInferior = TraduccionIngles.CotizacionesActivas_Ingles,//CotizacionesActivas --- Identificador de traduccion
                    CostoTotal = TotalActivos.ToString(),
                    Icono = "activas.jpg",
                    BackgroundGradientStart = "#FFC859"
                }
                };

            }
            else if (App.Current.Properties["Traducciones"].Equals("Español"))
            {
                this.ListItems = new ObservableCollection<Datos3Objetos>()
                {
                new Datos3Objetos()
                {
                    TextoInferior = TraduccionEspañol.CuotasAceptadas_Español, //CuotasAceptadas ---Identificador de traduccion
                    CostoTotal = "$ " + GananciasTotales.ToString("N", formato),
                    Icono = "costocotizacion.jpg",
                    BackgroundGradientStart = "#99CC33"
                },
                new Datos3Objetos()
                {
                    TextoInferior = TraduccionEspañol.TotalClientes_Español,//TotalClientes ---Identificador de traduccion
                    CostoTotal = TotalClientes.ToString(),
                    Icono = "clientescotizacion.jpg",
                    BackgroundGradientStart = "#00B1F5"
                },
                new Datos3Objetos()
                {
                    TextoInferior = TraduccionEspañol.CotizacionesActivas_Español,//CotizacionesActivas --- Identificador de traduccion
                    CostoTotal = TotalActivos.ToString(),
                    Icono = "activas.jpg",
                    BackgroundGradientStart = "#FFC859"
                }
                };
            }
            
        }

        public void TraduccionesMenu()
        {
            SelectMenu = new Command<object>(listView_ItemSelected);
            Español TraduccionEspañol = new Español();
            Ingles TraduccionIngles = new Ingles();
            //Condicion de traducciones
            if (App.Current.Properties["Traducciones"].Equals("Ingles"))
            {

                this.ListaMenu = new ObservableCollection<Menu>()
                    {
                    new Menu{ MenuTitle= TraduccionIngles.MenuDashboard_Ingles, MenuDetail="DashBoard",icon="speed.png"},
                    new Menu{ MenuTitle= TraduccionIngles.MenuSend_Ingles,  MenuDetail="Send",icon="avion.png"},
                    new Menu{ MenuTitle= TraduccionIngles.Language_Ingles,  MenuDetail="Language",icon="estadosunidos.png"},
                    };

            }
            else if (App.Current.Properties["Traducciones"].Equals("Español"))
            {

                this.ListaMenu = new ObservableCollection<Menu>()
                    {
                    new Menu{ MenuTitle= TraduccionEspañol.MenuDashboard_Español, MenuDetail="DashBoard",icon="speed.png"},
                    new Menu{ MenuTitle= TraduccionEspañol.MenuSend_Español,  MenuDetail="Send",icon="avion.png"},
                    new Menu{ MenuTitle= TraduccionEspañol.Language_Español,  MenuDetail="Language",icon="mexico.png"},
                    };

            }
        }

        public void Traducciones()
        {
            Español TraduccionEspañol = new Español();
            Ingles TraduccionIngles = new Ingles();


            //Condicion de traducciones
            if (App.Current.Properties["Traducciones"].Equals("Ingles"))
            {
                    Banner = "bannercotizacioningles.jpg";
                    TituloDashboard = TraduccionIngles.TituloDashboard_Ingles;
                    AcumuladosPorMes = TraduccionIngles.AcumuladosPorMes_Ingles;
                    Aceptado = TraduccionIngles.Aceptado_Ingles;
                    Cancelado = TraduccionIngles.Cancelado_Ingles;
                    CerrarSesionTexto = TraduccionIngles.CerrarSesionTexto_Ingles;
               

            }
            else if (App.Current.Properties["Traducciones"].Equals("Español"))
            {
                    Banner = "bannercotizacionespañol.jpg";
                    TituloDashboard = TraduccionEspañol.TituloDashboard_Español;
                    AcumuladosPorMes = TraduccionEspañol.AcumuladosPorMes_Español;
                    Aceptado = TraduccionEspañol.Aceptado_Español;
                    Cancelado = TraduccionEspañol.Cancelado_Español;
                    CerrarSesionTexto = TraduccionEspañol.CerrarSesionTexto_Español;
              

            }

        }




        public ObservableCollection<Datos3Objetos> ListItems
        {
            get
            {
               
                return this.listItems;               
            }

            set
            {
               
                this.listItems = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Menu> ListaMenu
        {
            get
            {
                return this.MenuDatos;
            }

            set
            {

                this.MenuDatos = value;
                OnPropertyChanged();
            }
        }

        private bool closeopen;
        private string TituloDashboard_Cambio;
        private string AcumuladosPorMes_Cambio;
        private string Aceptado_Cambio;
        private string Cancelado_Cambio;
        private string CerrarSesionTexto_Cambio;
        public bool CloseOpen
        {
            get { return closeopen; }
            set {closeopen = value; OnPropertyChanged();}
        }
        public string TituloDashboard
        {
            get { return TituloDashboard_Cambio; }
            set { TituloDashboard_Cambio = value; OnPropertyChanged(); }
        }
        public string AcumuladosPorMes
        {
            get { return AcumuladosPorMes_Cambio; }
            set { AcumuladosPorMes_Cambio = value; OnPropertyChanged(); }
        }
        public string Aceptado
        {
            get { return Aceptado_Cambio; }
            set { Aceptado_Cambio = value; OnPropertyChanged(); }
        }
        public string Cancelado
        {
            get { return Cancelado_Cambio; }
            set { Cancelado_Cambio = value; OnPropertyChanged(); }
        }
        public string CerrarSesionTexto
        {
            get { return CerrarSesionTexto_Cambio; }
            set { CerrarSesionTexto_Cambio = value; OnPropertyChanged(); }
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
            await Task.Run(() => TresObjetosClase());
                        
            this.IsRefreshing = false;
            
        }

      
    }
}
