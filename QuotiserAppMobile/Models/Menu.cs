
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Database.Models
{
    public class Menu
    {

        public string MenuTitle
        {
            get;
            set;
        }
        public string MenuDetail
        {
            get;
            set;
        }

        public ImageSource icon
        {
            get;
            set;
        }

        public Page Page
        {
            get;
            set;
        }

    }

   
}
