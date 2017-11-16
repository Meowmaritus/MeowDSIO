using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MeowTaeEditorGui
{
    /// <summary>
    /// Interaction logic for TaeEventParamField.xaml
    /// </summary>
    public partial class TaeEventParamField : UserControl
    {
        public IList<string> Value
        {
            get => (IList<string>)GetValue(ValueProperty);
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(IList<string>), typeof(TaeEventParamField));

        public TaeEventParamField()
        {
            InitializeComponent();
        }
    }
}
