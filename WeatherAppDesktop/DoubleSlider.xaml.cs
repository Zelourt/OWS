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

namespace WeatherAppDesktop
{
    /// <summary>
    /// Логика взаимодействия для DoubleSlider.xaml
    /// </summary>
    public partial class DoubleSlider : UserControl
    {
        public int MaxCurrent { get; set; }
        public int MinCurrent { get; set; }
        public DoubleSlider()
        {
            InitializeComponent();
        }

        private void LowerSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //MessageBox.Show(Convert.ToInt32(LowerSlider.Value).ToString());
            //slider_temperature
            int minvalue = Convert.ToInt32(LowerSlider.Value);
            MinValue.Content = String.Format("Min: {0}", minvalue.ToString());
            MinCurrent = minvalue;
        }

        private void UpperSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int maxvalue = Convert.ToInt32(UpperSlider.Value);
            MaxValue.Content = String.Format("Max: {0}", maxvalue.ToString());
            MaxCurrent = maxvalue;
        }
    }
}
