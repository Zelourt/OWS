using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
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
    /// Логика взаимодействия для StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        MainWindow Mw;
        public void SetMainWnd(MainWindow wnd)
        {
            Mw = wnd;
        }
        public StartWindow()
        {
            InitializeComponent();

            CheckInternetConnection();

            DownloadListCity();

            checkfilesettings();
        }

        private void checkfilesettings ()
        {
            LbInfo.Content = "Проверка файла настроек";
            if (System.IO.File.Exists("userSettings.xml"))
            {
                LbInfo.Content = "Файл настроек уже создан";
            }
            else
            {
                LbInfo.Content = "Файл настроект отсутствует";
            }
            progressbar.Value = 3;
        }
        private void DownloadListCity ()
        {
            LbInfo.Content = "Загрузка списка городов";
            Cities ct = new Cities();
            progressbar.Value = 2;
        }
        private void CheckInternetConnection()
        {
            LbInfo.Content = "Проверка подключения к интернету";
            try
            {
                IPHostEntry obj = Dns.GetHostByName("www.google.com");
                progressbar.Value = 1;
            }
            catch
            {
                ShowInfo sg = new ShowInfo("Состояние сети", "Подключения к интернету нет.");
                sg.ShowDialog();
                Mw.CloseWindow();
                this.Close();
            }
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btn_CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
