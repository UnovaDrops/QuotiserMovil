
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class CotizacionesAceyDen
    {
        public string Month { get; set; }
        public DateTime Fecha { get; set; }
        public int Year { get; set; }
        public float MontoCancelado { get; set; }
        public float MontoAceptado { get; set; }       
    }
}
