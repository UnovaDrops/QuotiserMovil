using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;

namespace Database.Models
{
    public class DatosServicio
    {
        
        public string DescripcionServicio { get; set; }       
        public double PrecioServicio { get; set; }       
        public int CompanyId { get; set; }
        public int Activo { get; set; }
        public int CantidadServicios { get; set; }
        

    }
}