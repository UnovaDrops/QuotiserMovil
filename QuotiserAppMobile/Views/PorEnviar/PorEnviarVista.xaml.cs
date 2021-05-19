using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using Database.Models;
using System;
using Syncfusion.XForms.Buttons;
using System.Threading.Tasks;
using System.ComponentModel;
using QuotiserAppMobile.Helpers.Traducciones;
using QuotiserAppMobile.Views.Cotizacion;

namespace QuotiserAppMobile.Views.PorEnviar
{
    /// <summary>
    /// Page to show the health care details.
    /// </summary>
    [DesignTimeVisible(false)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [Preserve(AllMembers = true)]
    public partial class PorEnviarVista : ContentPage
    {   
        List<CotizacionesAceyDen> ListaDeMeses = new List<CotizacionesAceyDen>();
       
        public PorEnviarVista()
        {                       
            InitializeComponent();
            Traducciones();
            NavigationPage.SetHasNavigationBar(this, false);
            Device.BeginInvokeOnMainThread(() => {
                UserImage.Source = "WhitelogoQ.png";               
                BackButton.BackgroundImage = "backbutton.png";
            });
            Task.Run(() => MostrarObjetos());
            
        }
      
        protected async void BotonDeRetroceso(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PopModalAsync();
        }
        public async void MostrarObjetos()
        {
            
                await Task.Delay(4000);
                Device.BeginInvokeOnMainThread(() => {                   
                    IndicatorCarga.IsRunning = false;
                    TablaVisible.IsVisible = true;
                });

        }

        public void Traducciones()
        {
            Español TraduccionEspañol = new Español();
            Ingles TraduccionIngles = new Ingles();
            //Condicion de traducciones
            if (App.Current.Properties["Traducciones"].Equals("Ingles"))
            {

                SearchEntry.Placeholder = TraduccionIngles.SubTituloBarraBusqueda_Ingles;
             
            }
            else if (App.Current.Properties["Traducciones"].Equals("Español"))
            {

                SearchEntry.Placeholder = TraduccionEspañol.SubTituloBarraBusqueda_Español;
                
            }

        }
        protected async void BotonOpciones(object sender, EventArgs e)
        {
                
                DatosClienteStataicos.BanderaTablaCotizacion = 1;
                await Task.Delay(100);
                SfButton button = sender as SfButton;
                string ID = button.BindingContext.ToString();
                TresObjetos.IDCotizacionEstatico = ID;
                await App.Current.MainPage.Navigation.PushModalAsync(new CotizacionVista());
           
        }


  
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (width > height)
            {
                if (Search.IsVisible)
                {
                    Search.WidthRequest = width;
                }
            }
        }

       
        private void SearchButton_Clicked(object sender, EventArgs e)
        {
            this.SearchButton.IsVisible = false;
            this.Search.IsVisible = true;
            this.Title.IsVisible = false;

            if (this.TitleView != null)
            {
                double opacity;

               
                var expandAnimation = new Animation(
                    property =>
                    {
                        Search.WidthRequest = property;
                        opacity = property / TitleView.Width;
                        Search.Opacity = opacity;
                    }, 0, TitleView.Width, Easing.Linear);
                expandAnimation.Commit(Search, "Expand", 16, 250, Easing.Linear);
            }

            SearchEntry.Focus();
        }

       
        private void BackToTitle_Clicked(object sender, EventArgs e)
        {
            this.SearchButton.IsVisible = true;
            if (this.TitleView != null)
            {
                double opacity;

                // Animating Width of the search box, from full width to 0 before it removed from view.
                var shrinkAnimation = new Animation(property =>
                {
                    Search.WidthRequest = property;
                    opacity = property / TitleView.Width;
                    Search.Opacity = opacity;
                },
                TitleView.Width, 0, Easing.Linear);
                shrinkAnimation.Commit(Search, "Shrink", 16, 250, Easing.Linear, (p, q) => this.SearchBoxAnimationCompleted());
            }

            SearchEntry.Text = string.Empty;
        }

        
        private void SearchBoxAnimationCompleted()
        {
            this.Search.IsVisible = false;
            this.Title.IsVisible = true;
        }


       


    }
}
