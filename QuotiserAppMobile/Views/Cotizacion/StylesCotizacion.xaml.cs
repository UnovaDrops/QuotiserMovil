using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace QuotiserAppMobile.Views.Cotizacion
{
    /// <summary>
    /// Class helps to reduce repetitive markup, and allows an apps appearance to be more easily changed.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StylesCotizacion
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StylesCotizacion"/> class.
        /// </summary>
		public StylesCotizacion()
        {
            InitializeComponent();
        }
    }
}