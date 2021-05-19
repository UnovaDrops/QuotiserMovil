using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;

namespace Database.Models
{
    public class DatosCliente
    {
       
        public int ClienteId { get; set; }

       
        public string RFC { get; set; }

       
        public string EmpresaNombre { get; set; }

        public string NombreCliente { get; set; }

       
        public string ApellidoCliente { get; set; }

       
        public string CorreoCliente { get; set; }

       
        public string Telefono { get; set; }

        
        public string Giro { get; set; }

       
        public string Extension { get; set; }

       
        public int Activo { get; set; }

        
        public int SubCliente { get; set; }
    }
}