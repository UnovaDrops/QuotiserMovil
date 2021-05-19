using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_MSPT.Models.ViewModels
{
    public class ListaUsuarioViewModel
    {
        public bool status { get; set; }
        public int UsuarioID { get; set; }
        public int IDCompany { get; set; }
        public string Mensaje { get; set; }

    }
}