
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class DashBoard
    {
      
        public int IdCotizacion { get; set; }
        public string NombreProyecto { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int ClienteId { get; set; }
        public int UserId { get; set; }
        public double CostoBase { get; set; }
        public double CostoTotal { get; set; }
        public int NumeroPeticiones { get; set; }
        public int Enviada { get; set; }
        public int Aceptado { get; set; }
        public int Activo { get; set; }
        public int EstadoId { get; set; }
        public int MunicipioId { get; set; }
        public int Legacy { get; set; }
        public string FolioLegacy { get; set; }
        public int Consecutivo { get; set; }
        public string FolioTicket { get; set; }
        public int Autorizar { get; set; }
        public string DescripcionProyecto { get; set; }
        public int Deleted { get; set; }


    }
   
}
