using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;

namespace Database.Models
{
    public class DatosEstado
    {      
        public int IdEstadoCompany { get; set; }

        public string NombreEstado { get; set; }

      
        public float Porcenetaje { get; set; }

       
        public int CompanyId { get; set; }

       
        public int Activo { get; set; }
    }
}