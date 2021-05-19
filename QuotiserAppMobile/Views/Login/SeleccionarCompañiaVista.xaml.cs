using Database.Models;
using Nancy.Json;
using Newtonsoft.Json;
using QuotiserAppMobile.Helpers;
using QuotiserAppMobile.Helpers.Traducciones;
using QuotiserAppMobile.Views.Panel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using WS_MSPT.Models.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace QuotiserAppMobile.Views.Login
{
    /// <summary>
    /// Page to login with user name and password
    /// </summary>
    [DesignTimeVisible(false)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [Preserve(AllMembers = true)]
    public partial class SeleccionarCompañiaVista : ContentPage
    {
       
        LlamadasApi ApiCall = new LlamadasApi();
        List<ListaCompañiasUsuario> Pepe = new List<ListaCompañiasUsuario>();
        public SeleccionarCompañiaVista(List<ListaCompañiasUsuario> Lista)
        {
            
            InitializeComponent();
            Traducciones();
            NavigationPage.SetHasNavigationBar(this, false);
            InitApp(Lista);

        }

       

        public void InitApp(List<ListaCompañiasUsuario> LISTAUsuariosCompañias)
        {
            foreach (var item in LISTAUsuariosCompañias)
            {
                Pepe.Add(new ListaCompañiasUsuario
                {
                    IdCompañia = item.IdCompañia,
                    Compañia = item.Compañia,
                    Usuario = item.Usuario
                });
                PickerCompany.Items.Add(item.Compañia);
                
            }
        }
       
        private void PickerChangeCompany(object sender, EventArgs e)
        {
            int position = PickerCompany.SelectedIndex;
            if (position > -1)
            {
                TresObjetos.UsuarioEstatico = Pepe[position].Usuario;
            }
        }

        private async void SelectCompany(object sender, EventArgs e)
        {
            

            if (TresObjetos.UsuarioEstatico == 0)
            {
                //Usuario incorrecto
                await Task.Delay(10);
                OnAlertYesNoClicked();
                IndicatorCompañia.IsRunning = false;
            }
            else
            {
                IndicatorCompañia.IsRunning = true;
                await Task.Delay(10);
                string url = "http://velarde3600-001-site6.etempurl.com/api/Usuarios/SolicitudUsuarios";
                UserID Conexion = new UserID();
                Conexion.UsuarioID = TresObjetos.UsuarioEstatico;
                string resultado = ApiCall.ReadStringJson<UserID>(url, Conexion, "POST");
                List<DatosUsuario> CostoTotal = JsonConvert.DeserializeObject<List<DatosUsuario>>(resultado);
                foreach (var item in CostoTotal)
                {
                    App.Current.Properties["Administrador"] = item.IsAdministrador;
                    App.Current.Properties["CompañiaID"] = item.CompanyId;
                    App.Current.Properties["NombreUsuario"] = item.Name + " " + item.LastName;
                }
               
                //Usuario correcto
                App.Current.Properties["IsLogin"] = "true";
                IndicatorCompañia.IsRunning = false;
                await App.Current.MainPage.Navigation.PushModalAsync(new PanelVista());
            }           
        }

        async void OnAlertYesNoClicked()
        {
            if (App.Current.Properties["Traducciones"].Equals("Ingles"))
            {
                await App.Current.MainPage.DisplayAlert("No company was selected", "Please select it ", "Ok");

            }
            else if (App.Current.Properties["Traducciones"].Equals("Español"))
            {
                await App.Current.MainPage.DisplayAlert("No se seleccionó ninguna empresa", "Por favor selecciónalo", "Ok");

            }
            
        }

        public void Traducciones()
        {
            Español TraduccionEspañol = new Español();
            Ingles TraduccionIngles = new Ingles();
           
          
            //Condicion de traducciones
            if (App.Current.Properties["Traducciones"].Equals("Ingles"))
            {
                Device.BeginInvokeOnMainThread(() => {
                    SeleccionarCompañia.Text = TraduccionIngles.SeleccionarCompañia_Ingles;
                    PickerCompany.Title = TraduccionIngles.PickerCompany_Ingles;
                    SeleccionarCompañiaBoton.Text = TraduccionIngles.SeleccionarCompañiaBoton_Ingles;
                });

            }
            else if (App.Current.Properties["Traducciones"].Equals("Español"))
            {
                Device.BeginInvokeOnMainThread(() => {
                    SeleccionarCompañia.Text = TraduccionEspañol.SeleccionarCompañia_Español;
                    PickerCompany.Title = TraduccionEspañol.PickerCompany_Español;
                    SeleccionarCompañiaBoton.Text = TraduccionEspañol.SeleccionarCompañiaBoton_Español;
                });

            }

        }
    }
}