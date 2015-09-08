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
using System.Xml.Linq;

namespace WeatherAppDesktop
{
    /// <summary>
    /// Логика взаимодействия для SettingsFlyout.xaml
    /// </summary>
    public partial class SettingsFlyout : UserControl
    {
        DoubleAnimation da;
        public int MinTemperature;
        public int MaxTemperature;
        public int MinWind;
        public int MaxWind;
        Weather wt;
        MainWindow Mw;
        public SettingsFlyout()
        {
            InitializeComponent();
        }

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            da = new DoubleAnimation();
            da.To = 0;
            da.Duration = TimeSpan.FromSeconds(0.2);
            SettingsControl.BeginAnimation(Border.WidthProperty, da);
            MainWindow.IsSettings = false;
        }

        private void btn_AddSity_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Sity add");
        }

        private void slider_temperature_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
            int value = Convert.ToInt32(slider_temperature.LowerSlider.Value);
            slider_temperature.MinValue.Content = String.Format("Min: {0}°C", value.ToString());
            MinWind = value;

            MaxWind = 70;
            MaxTemperature = 50;
            MinWind = 20;
        }

        public void WriteSettingsDataXML()
        {
            try
            {
                var path = "userSettings.xml";

                XDocument doc = new XDocument();
                XElement settings = new XElement("settings");
                doc.Add(settings);

                XElement location = new XElement("location");

                XElement notification = new XElement("notification");
                XElement notificationIsEnable = new XElement("isenabled");

                //Чек бокс на отправку уведомлений
                if (CbNotifications.IsChecked == true)
                {
                    notificationIsEnable.Value = "true";

                    XElement mintemperature = new XElement("mintemperature");
                    mintemperature.Value = MinTemperature.ToString();

                    XElement maxtemperature = new XElement("maxtemperature");
                    maxtemperature.Value = MaxTemperature.ToString();

                    XElement minwind = new XElement("minwind");
                    minwind.Value = MinWind.ToString();

                    XElement maxwind = new XElement("maxwind");
                    maxwind.Value = MaxWind.ToString();

                    XElement downfall = new XElement("downfall");
                    if (Cbdownfall.IsChecked == true)
                    {
                        downfall.Value = "true";
                    }
                    else
                    {
                        downfall.Value = "false";
                    }


                    XElement email = new XElement("email");
                    email.Value = tbox_email.Text;


                    notification.Add(email, mintemperature, maxtemperature, minwind, maxwind, downfall);


                    XElement city = new XElement("city");
                    city.Value = TboxCityNotification.Text;

                    //XElement country = new XElement("country");
                    //country.Value = tbCountryToDisplay.Text;

                    notification.Add(notificationIsEnable);

                    location.Add(city);

                    doc.Root.Add(location);

                    doc.Root.Add(notification);
                    doc.Save(path);

                    ShowInfo sg = new ShowInfo("Пользовательские данные", "Ваши данные оповещения о изменении погоды было сохраненно");
                    sg.ShowDialog();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btn_savesettings_Click(object sender, RoutedEventArgs e)
        {
            WriteSettingsDataXML();
        }

        private void TbCitySearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            LboxSearchCities.Items.Clear();
            if (TboxCityNotification.Text.Length > 3)
            {
                foreach (var p in Cities.CityList)
                {
                    if (p.name.Contains(TboxCityNotification.Text))
                    {
                        if (!LboxSearchCities.Items.Contains(p.name + ", " + p.country))
                        {
                            LboxSearchCities.Items.Add(p.name + ", " + p.country);
                        }
                    }
                }

                //сities.CityList.FindAll(item => item.name.IndexOf(TbCitySearch.Text, StringComparison.InvariantCultureIgnoreCase) >= 0)
                //    .ForEach(city => LboxSearchCities.Items.Add(city.name + ", " + city.country));
            }
        }

        private void LboxSearchCities_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Weather wtp = new Weather();
                string location = LboxSearchCities.SelectedValue.ToString();

                LboxSearchCities.Items.Clear();
                TboxCityNotification.Clear();
                TboxCityNotification.Text = location;

            }
            catch (Exception ex)
            {
                ShowInfo sg = new ShowInfo("Ошибка", ex.ToString());
                sg.ShowDialog();
            }
        }
    }
}
