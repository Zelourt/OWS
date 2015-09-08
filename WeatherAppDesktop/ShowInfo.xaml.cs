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
using System.Windows.Shapes;

namespace WeatherAppDesktop
{
    /// <summary>
    /// Логика взаимодействия для ShowInfo.xaml
    /// </summary>
    public partial class ShowInfo : Window
    {
        public ShowInfo(string header, string info)
        {
            InitializeComponent();
            LbHeader.Content = header;
            TbInfo.Text = info;
        }

        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
