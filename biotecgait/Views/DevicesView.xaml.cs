using biotecgait.ViewModels;
using System.Windows.Controls;

namespace biotecgait.Views
{
    /// <summary>
    /// Lógica de interacción para DevicesView.xaml
    /// </summary>
    public partial class DevicesView : UserControl
    {
        public DevicesView(DevicesVM vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
