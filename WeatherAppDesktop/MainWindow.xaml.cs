using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Xml.Linq;

namespace WeatherAppDesktop
{
    public partial class MainWindow : Window
    {
        public SmtpClient client;
        public MailMessage message;
        public MailAddress from;
        public MailAddress to;
        public NetworkCredential credentials;
        public Cities сities = new Cities();
        Weather wr;

        public MainWindow()
        {
            try
            {
                StartWindow StWindow = new StartWindow();
                StWindow.ShowDialog();
                wr = new Weather();
                InitializeComponent();
                InitializeDezign();
                SetWeatherData(wr);
                ReadSettingsDataXML();
                searchflyout.SetMainWnd(this);
            }
            catch (Exception ex)
            {
                ShowInfo sg = new ShowInfo("Ошибка", ex.ToString());
                sg.ShowDialog();
            }
            
        }

        public void CloseWindow ()
        {
            this.Close();
        }

        //private void DownloadListCities  ()
        //{
        //    reader = new StreamReader("city.list.json");
        //    CityList = new List<City>();
        //    while (!reader.EndOfStream)
        //    {
        //        CityList.Add(JsonHelper.JsonDeserialize<City>(reader.ReadLine()));
        //    }
        //}

        public void SetWeatherData (Weather wr)
        {
            LbWindCityName.Content = wr.City + " " + wr.Country;
            DataBox.LbState.Content = String.Format("Состояние: {0} ", wr.Sky);
            DataBox.LbWindSpeed.Content = String.Format("Ветер: {0} м/с", wr.WindSpeed);
            DataBox.LbMinTemperature.Content = String.Format("Tmin: {0} °C", wr.MinTemperature);
            DataBox.LbMaxTemperature.Content = String.Format("Tmax: {0} °C", wr.MaxTemperature);
            DataBox.LbHumidity.Content = String.Format("Влажность: {0} ", wr.Humidity);
            DataBox.LbSunrise.Content = String.Format("Рассвет: {0} ", wr.Sunrise);
            DataBox.LbSunset.Content = String.Format("Закат: {0} ", wr.Sunset);
            DataBox.LbTemperature.Content = String.Format("Температура: {0} °C", wr.Temperature);
            DataBox.LbPressure.Content = String.Format("Давление: {0} ", wr.Pressure);
        }

        public void ReadSettingsDataXML()
        {
            try
            {
                if(System.IO.File.Exists("userSettings.xml"))
                {
                    var path = "userSettings.xml";
                    XDocument doc = XDocument.Load(path);

                    var city = doc.Element("settings").Element("location").Element("city").Value;
                    settingsflyout.TboxCityNotification.Clear();
                    settingsflyout.TboxCityNotification.Text = city;

                    var notificationIsEnabled = doc.Element("settings").Element("notification").Element("isenabled").Value;

                    if (notificationIsEnabled == "true")
                    {
                        settingsflyout.CbNotifications.IsChecked = true;
                        var email = doc.Element("settings").Element("notification").Element("email").Value;
                        settingsflyout.tbox_email.Clear();
                        settingsflyout.tbox_email.Text = email;

                        var maxtemperature = doc.Element("settings").Element("notification").Element("maxtemperature").Value;
                        //numericTemperature.Value = Convert.ToDouble(maxtemperature);

                        var mintemperature = doc.Element("settings").Element("notification").Element("mintemperature").Value;
                        //numericTemperature.Value = Convert.ToDouble(mintemperature);

                        var maxwind = doc.Element("settings").Element("notification").Element("maxwind").Value;
                        //numericHumidity.Value = Convert.ToDouble(maxwind);

                        var minwind = doc.Element("settings").Element("notification").Element("minwind").Value;
                        //numericHumidity.Value = Convert.ToDouble(minwind);

                        var downfall = doc.Element("settings").Element("notification").Element("downfall").Value;
                        if (downfall == "true")
                        {
                            settingsflyout.Cbdownfall.IsChecked = true;
                        }
                        if (downfall == "false")
                        {
                            settingsflyout.Cbdownfall.IsChecked = false;
                        }
                    }
                    else
                        settingsflyout.CbNotifications.IsChecked = false;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void ShowMap(string location)
        {
            string city, country;
            int comaIndex = location.IndexOf(' ');
            country = location.Substring(comaIndex + 1);
            city = location.Remove(comaIndex);

            City cityFound = сities.CityList.FirstOrDefault<City>(c => c.name == city | c.country == country);

            var lat = Convert.ToInt32(cityFound.coord.lat);
            var lon = Convert.ToInt32(cityFound.coord.lon);
            string uri = "http://openweathermap.org/Maps?zoom=12&lat=" + lat.ToString() + "&lon=" + lon.ToString() + "&layers=B0FTTFF";
            webbrawser.brawser.Navigate(uri);
        }

        //Dezign and component window
        #region

        public bool statewindow = true; //Положение окна
        public static bool IsSettings;
        public static bool IsSearch;
        DoubleAnimation da;
        const int widthsetting = 320;
        const int widthsearch = 400;

        private void InitializeDezign ()
        {
            settingsflyout.Width = 0;
            searchflyout.Width = 0;
        }

        //Кнопка закрития окна
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //Перемещение окна
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        //Кнопка полного размера окна
        private void btn_Res_Click(object sender, RoutedEventArgs e)
        {
            if (statewindow)
            {
                this.WindowState = WindowState.Maximized;
                statewindow = false;
            }
            else
            {
                this.WindowState = WindowState.Normal;
                statewindow = true;
            }
        }

        //Кнопка свертивания окна
        private void btn_Hide_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        //Переход на карту
        private void btn_map_Click(object sender, RoutedEventArgs e)
        {
            if (GdBoxInfo.Visibility == Visibility.Visible)
            {
                GdMapInfo.Visibility = Visibility.Visible;
                GdBoxInfo.Visibility = Visibility.Collapsed;
                ShowMap(LbWindCityName.Content.ToString());
            }
            else
            {
                GdBoxInfo.Visibility = Visibility.Visible;
                GdMapInfo.Visibility = Visibility.Collapsed;
            }

        }

        //Кнопка открития flyout настроек
        public void btn_settings_Click(object sender, RoutedEventArgs e)
        {
            da = new DoubleAnimation();
            settingsflyout.Height = Wind.Height;

            if (!IsSettings)
            {
                da.To = widthsetting;
                da.Duration = TimeSpan.FromSeconds(0.2);
                settingsflyout.BeginAnimation(Border.WidthProperty, da);
                IsSettings = true;
                settingsflyout.IsEnabled = true;
            }
            else
            {
                da.To = 0;
                da.Duration = TimeSpan.FromSeconds(0.2);
                settingsflyout.BeginAnimation(Border.WidthProperty, da);
                IsSettings = false;
                settingsflyout.IsEnabled = false;
            }
        }

        //Кнопка открития flyout поиска
        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            da = new DoubleAnimation();
            if (!IsSearch)
            {
                da.To = widthsearch;
                da.Duration = TimeSpan.FromSeconds(0.2);
                searchflyout.BeginAnimation(Border.WidthProperty, da);
                IsSearch = true;
            }
            else
            {
                da.To = 0;
                da.Duration = TimeSpan.FromSeconds(0.2);
                searchflyout.BeginAnimation(Border.WidthProperty, da);
                IsSearch = false;
            }
        }
        #endregion
    }
}
