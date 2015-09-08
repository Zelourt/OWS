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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WeatherAppDesktop
{
    /// <summary>
    /// Логика взаимодействия для TextBoxFlyout.xaml
    /// </summary>
    public partial class TextBoxFlyout : UserControl
    {
        DoubleAnimation da;
        Weather wt;
        MainWindow Mw;
        public Cities сities;
        public TextBoxFlyout()
        {
            InitializeComponent();
            сities = new Cities();
        }

        public void SetMainWnd(MainWindow wnd)
        {
            Mw = wnd;
        }

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            CloseTextBoxvoxFl();
        }

        private void CloseTextBoxvoxFl ()
        {
            da = new DoubleAnimation();
            da.To = 0;
            da.Duration = TimeSpan.FromSeconds(0.2);
            SearchFlyout.BeginAnimation(Border.WidthProperty, da);
            MainWindow.IsSearch = false;
        }

        private void btn_searchfortexbox_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TbCitySearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            LboxSearchCities.Items.Clear();
            if (TbCitySearch.Text.Length > 3)
            {
                //foreach (var p in сities.CityList)
                //{
                //    if (p.name.Contains(TbCitySearch.Text))
                //    {
                //        if (!LboxSearchCities.Items.Contains(p.name + ", " + p.country))
                //        {
                //            LboxSearchCities.Items.Add(p.name + ", " + p.country);
                //        }
                //    }
                //}

                сities.CityList.FindAll(item => item.name.IndexOf(TbCitySearch.Text, StringComparison.InvariantCultureIgnoreCase) >= 0)
                    .ForEach(city => LboxSearchCities.Items.Add(city.name + ", " + city.country));
            }
        }

        private void LboxSearchCities_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Weather wtp = new Weather();
                string location = LboxSearchCities.SelectedValue.ToString();
                string city, country;
                int comaIndex = location.IndexOf(',');
                country = location.Substring(comaIndex + 1);
                city = location.Remove(comaIndex);

                City cityFound = сities.CityList.FirstOrDefault<City>(c => c.name == city | c.country == country);

                wtp.GetWeatherById(cityFound._id);

                Mw.SetWeatherData(wtp);

                TbCitySearch.Clear();

                CloseTextBoxvoxFl();

            }
            catch (Exception ex)
            {
                ShowInfo sg = new ShowInfo("Ошибка", ex.ToString());
                sg.ShowDialog();
            }


        }
    }
}
