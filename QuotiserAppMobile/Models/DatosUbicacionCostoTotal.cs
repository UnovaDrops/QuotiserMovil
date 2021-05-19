using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;

namespace Database.Models
{
    public class DatosUbicacionCostoTotal
    {
       
        public int EstadoId { get; set; }
        public double CostoTotal { get; set; }
        public string NombreProyecto { get; set; }

        public int Autorizar { get; set; }



    }
}