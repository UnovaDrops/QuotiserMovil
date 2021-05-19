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
    public class CotizacionViewModel : BaseViewModel
    {     
        public CotizacionViewModel()
        {
            ListaDiseñosPDF();
        }

        public void ListaDiseñosPDF()
        {       
            //Condicion de traducciones
            if (App.Current.Properties["Traducciones"].Equals("Ingles"))
            {

                this.ListaDiseños = new ObservableCollection<DiseñosPDF>()
                {
                
                    new DiseñosPDF{DiseñoPdf = "Breakdown 1"},
                    new DiseñosPDF{DiseñoPdf = "Breakdown 2"},
              
                };

            }
            else if (App.Current.Properties["Traducciones"].Equals("Español"))
            {
                this.ListaDiseños = new ObservableCollection<DiseñosPDF>()
                {
                    new DiseñosPDF{DiseñoPdf = "Desglose 1"},
                    new DiseñosPDF{DiseñoPdf = "Desglose 2"},
                };
            }
            
        }


        private ObservableCollection<DiseñosPDF> listadiseños;
        public ObservableCollection<DiseñosPDF> ListaDiseños
        {
            get
            {

                return this.listadiseños;
            }

            set
            {

                this.listadiseños = value;
                OnPropertyChanged();
            }
        }

    }
}
