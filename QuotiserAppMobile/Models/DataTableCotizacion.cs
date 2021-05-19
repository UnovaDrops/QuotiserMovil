using Xamarin.Forms.Internals;

namespace QuotiserAppMobile.Models
{
    /// <summary>
    /// Model for Data table
    /// /// </summary>
    [Preserve(AllMembers = true)]
    public class DataTableCotizacion
    {
     
        public string SerialNumber { get; set; }

        /// <summary>
        /// Gets or sets the club name.
        /// </summary>
        public string ClubName { get; set; }

        /// <summary>
        /// Gets or sets the gold points.
        /// </summary>
        public string GoldPoints { get; set; }

        
        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        public string Points { get; set; }

        /// <summary>
        /// Gets or sets the match results.
        /// </summary>
        public int ValorBoton { get; set; }

        public string ProductoServicio { get; set; }

        public string ColorText { get; set; }

       


    }
}
