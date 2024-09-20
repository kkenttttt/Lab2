using Lab2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace Lab2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private Model1 model = new Model1();
        private int Skip = 0;
        private int Take = 25;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(LoadData);
        }
        private void LoadData(object sender)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)(() =>
            {
                labelLoad.Visibility = Visibility.Visible;
                wrapPanelWeather.Children.Clear();
            }));
            foreach (Weather weather in model.Weather.ToList().Skip(Skip).Take(Take))
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)(() =>
                {
                    UserControlWether weatherControl = new UserControlWether(weather);
                    wrapPanelWeather.Children.Add(weatherControl);
                }));
            }
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)(() =>
            {
                labelLoad.Visibility = Visibility.Collapsed;

            }));
        }

        private void buttonMinus_Click(object sender, RoutedEventArgs e)
        {
            if (Skip > Take && Skip != 0)
            {
                Skip += Take;
                ThreadPool.QueueUserWorkItem(LoadData);
            }
            else if (Skip != 0)
            {
                Skip = 0;
                ThreadPool.QueueUserWorkItem(LoadData);
            }
        }

        private void buttonPlus_Click(object sender, RoutedEventArgs e)
        {
            int Summ = Skip + Take;
            if (model.Weather.ToList().Count > Summ)
            {
                Skip += Take;
                ThreadPool.QueueUserWorkItem(LoadData);
            }
            else if (model.Weather.ToList().Count < Summ)
            {
                Skip += model.Weather.ToList().Count - Skip;
                ThreadPool.QueueUserWorkItem(LoadData);
            }
        }
    }
}

