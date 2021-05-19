using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;

namespace Database.Models
{
    public class DatosUsuario
    {

        public int IdUser { get; set; }


        public string Name { get; set; }


        public string LastName { get; set; }

        public string EmailUser { get; set; }


        public string Password { get; set; }


        public string ImagePath { get; set; }


        public string FechaRegistro { get; set; }


        public int CompanyId { get; set; }

       
        public int IsAdministrador { get; set; }

       
        public int IsOperating { get; set; }

        
        public int Active { get; set; }

        public int SendReport { get; set; }


        public int IdLenguaje { get; set; }
    }
}