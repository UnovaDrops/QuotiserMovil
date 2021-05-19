using Database.Models;
using Newtonsoft.Json;
using QuotiserAppMobile.Helpers;
using QuotiserAppMobile.Helpers.Traducciones;
using QuotiserAppMobile.Views.Panel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using WS_MSPT.Models.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace QuotiserAppMobile.Views.Login
{ 
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [Preserve(AllMembers = true)]
    public partial class LoginVista 
    {
        LlamadasApi ApiCall = new LlamadasApi();
        public LoginVista()
        {          
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            Traducciones();
            if (Application.Current.Properties.ContainsKey("EmailPermanente"))
            {
                CapturaEmail.Text = App.Current.Properties["EmailPermanente"].ToString();
                if (App.Current.Properties["IsChecked"].Equals("true"))
                {
                    CapturaContraseña.Text = App.Current.Properties["PasswordPermanente"].ToString();
                    radioButton.IsChecked = true;
                }
            }       
        }

        [Obsolete]
        private void Registro(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("http://quotiser.com/Registro"));
        }

        [Obsolete]
        private void RecuperarContraseñas(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("http://quotiser.com/Restore"));
        }
        private async void IniciarSesionBoton(object sender, EventArgs e)
        {
            
                IndicatorLogin.IsRunning = true;           
                App.Current.Properties["EmailPermanente"] = CapturaEmail.Text;
                App.Current.Properties["PasswordPermanente"] = CapturaContraseña.Text;
                if (radioButton.IsChecked == true)
                {
                    App.Current.Properties["IsChecked"] = "true";
                }
                else
                {
                    App.Current.Properties["IsChecked"] = "false";
                }
                await Application.Current.SavePropertiesAsync();
                var Password = CapturaContraseña.Text;
                var Email = CapturaEmail.Text;
                MainConexionLogin(Password,Email);
           
        }
          
        
        private async void MainConexionLogin(string Password ,string Email)
        {           
                    
            string url = "http://velarde3600-001-site6.etempurl.com/api/Login/SolicitudUser";
            LoginDatos ConexionLogin = new LoginDatos();
            ConexionLogin.EmailUsuario = Email;
            ConexionLogin.Contrasena = Password;
            ConexionLogin.Active = 1;            
            string resultado = ApiCall.ReadStringJson<LoginDatos>(url, ConexionLogin, "POST");
            List<ListaUsuarioViewModel> jVariable = JsonConvert.DeserializeObject<List<ListaUsuarioViewModel>>(resultado);
            List<ListaCompañiasUsuario> Lista = new List<ListaCompañiasUsuario>();
            
            if (jVariable.Count > 1)
            {
                foreach (var item in jVariable)
                {
                   
                    Lista.Add(new ListaCompañiasUsuario
                    {
                        IdCompañia = item.IDCompany,
                        Compañia = ConexionCompañia(item.IDCompany),
                        Usuario = item.UsuarioID
                    });

                }
                IndicatorLogin.IsRunning = false;
                
                await App.Current.MainPage.Navigation.PushModalAsync(new SeleccionarCompañiaVista(Lista));                
            }
            else
            {
                foreach(var item in jVariable)
                {
                    
                    if (item.UsuarioID == 0)
                    {                        
                        //Usuario incorrecto
                        OnAlertYesNoClicked();
                        IndicatorLogin.IsRunning = false;
                    }
                    else
                    {

                        App.Current.Properties["UsuarioID"] = item.UsuarioID;                                            
                        string url2 = "http://velarde3600-001-site6.etempurl.com/api/Usuarios/SolicitudUsuarios";
                        UserID Conexion = new UserID();
                        Conexion.UsuarioID = item.UsuarioID;
                        string User = ApiCall.ReadStringJson<UserID>(url2, Conexion, "POST");
                        List<DatosUsuario> CostoTotal = JsonConvert.DeserializeObject<List<DatosUsuario>>(User);
                        foreach (var item2 in CostoTotal)
                        {
                            App.Current.Properties["Administrador"] = item2.IsAdministrador;
                            App.Current.Properties["CompañiaID"] = item2.CompanyId;
                            App.Current.Properties["NombreUsuario"] = item2.Name + " " + item2.LastName;
                        }
                       
                        App.Current.Properties["IsLogin"] = "true";
                        await Application.Current.SavePropertiesAsync();
                        //Usuario correcto
                        IndicatorLogin.IsRunning = false;
                        
                        await Application.Current.MainPage.Navigation.PushModalAsync(new PanelVista());
                        

                    }
                }
            }
            
        }

        private string ConexionCompañia(int Compañia)
        {            
            string url = "http://velarde3600-001-site6.etempurl.com/api/TraerCompañias/SolicitudCompañia";
            TraerCompañia ConexionLogin = new TraerCompañia();
            ConexionLogin.CompañiaID = Compañia;         
            string resultado = ApiCall.ReadStringJson<TraerCompañia>(url, ConexionLogin, "POST");
            List<ListaCompañiasViewModel> Compañias = JsonConvert.DeserializeObject<List<ListaCompañiasViewModel>>(resultado);
            string NombreCompañia = "";
            foreach (var item in Compañias)
            {

                NombreCompañia = item.NameCompany;
                
                
            }
            return NombreCompañia;

        }

        async void OnAlertYesNoClicked()
        {

            if (App.Current.Properties["Traducciones"].Equals("Ingles"))
            {
                await App.Current.MainPage.DisplayAlert("The captured data is incorrect", "Please verify your data", "Ok");
            }
            else if (App.Current.Properties["Traducciones"].Equals("Español"))
            {
                await App.Current.MainPage.DisplayAlert("Los datos capturados son incorrectos", "Verifique sus datos", "Ok");
            }
            
        }

        public void Traducciones()
        {
            Español TraduccionEspañol = new Español();
            Ingles TraduccionIngles = new Ingles();
            //Condicion de que si no existe una llave llamada "Traducciones" , por default muestre la traduccion en ingles y crea la llave
            if (!Application.Current.Properties.ContainsKey("Traducciones"))
            {
                App.Current.Properties["Traducciones"] = "Ingles";
                Application.Current.SavePropertiesAsync();
            }
            //Condicion de traducciones
            if (App.Current.Properties["Traducciones"].Equals("Ingles"))
            {
                Device.BeginInvokeOnMainThread(() => {
                    BienvenidoQuotiser.Text = TraduccionIngles.BienvenidoQuotiser_Ingles;
                    PorfavorInicia.Text = TraduccionIngles.PorfavorInicia_Ingles;
                    CapturaEmail.Placeholder = TraduccionIngles.CapturaEmail_Ingles;
                    CapturaContraseña.Placeholder = TraduccionIngles.CapturaContraseña_Ingles;
                    IniciarSesionBotonTexto.Text = TraduccionIngles.IniciarSesionBotonTexto_Ingles;
                    radioButton.Text = TraduccionIngles.radioButton_Ingles;
                    NoTienesCuenta.Text = TraduccionIngles.NoTienesCuenta_Ingles;
                    RecuperarContraseña.Text = TraduccionIngles.RecuperarContraseña_Ingles;
                });
               
            }
            else if (App.Current.Properties["Traducciones"].Equals("Español"))
            {
                Device.BeginInvokeOnMainThread(() => {
                    BienvenidoQuotiser.Text = TraduccionEspañol.BienvenidoQuotiser_Español;
                    PorfavorInicia.Text = TraduccionEspañol.PorfavorInicia_Español;
                    CapturaEmail.Placeholder = TraduccionEspañol.CapturaEmail_Español;
                    CapturaContraseña.Placeholder = TraduccionEspañol.CapturaContraseña_Español;
                    IniciarSesionBotonTexto.Text = TraduccionEspañol.IniciarSesionBotonTexto_Español;
                    radioButton.Text = TraduccionEspañol.radioButton_Español;
                    NoTienesCuenta.Text = TraduccionEspañol.NoTienesCuenta_Español;
                    RecuperarContraseña.Text = TraduccionEspañol.RecuperarContraseña_Español;
                });
             
            }

        }

       
    }
}