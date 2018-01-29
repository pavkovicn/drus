using Observer.ServiceReference1;
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

namespace Observer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private static Service1Client service;
        public List<Location> Locations { get; set; }
        public List<Measurer> Measurers { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            service = new Service1Client();
            Measurers = service.GetClients().ToList();
            Locations = service.GetLocations().ToList();
            cbLocations.ItemsSource = Locations;
            cbLocations.DisplayMemberPath = "Name";
            cbClients.ItemsSource = Measurers;
            cbClients.DisplayMemberPath = "Name";
        }

        private void SubToClient(object sender, RoutedEventArgs e)
        {
            Measurer m = (Measurer)cbClients.SelectedItem;
            service.RegisterObserver(m.Id);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            DateTime from = new DateTime();
            DateTime to = new DateTime();
            Measurer m=null;
            Location l = null;
            double value =  0;
            List<Measurement> result = null;
            bool errorDate = false;
            bool errorValue = false;

            string resultText = "";

            try
            {
                from = Convert.ToDateTime(tbTimeFrom.Text);
                to = Convert.ToDateTime(tbTimeTo.Text);
            }catch(Exception exc){
                errorDate = true;
            }

            try
            {
                value = Convert.ToDouble(tbValue.Text);
            }
            catch (Exception exc)
            {
                errorValue = true;
            }

            if (rbMeasurer.IsChecked == true) {
                m = (Measurer)cbClients.SelectedItem;
            } else {
                l = (Location)cbLocations.SelectedItem;
            }

            if (rbAll.IsChecked == true || rbAverage.IsChecked == true)
            {
                if (errorDate == false) {
                    if (rbMeasurer.IsChecked == true && m != null)
                    {
                        result = service.GetMeasurementDate(m.Id, from, to).ToList();
                    }
                    else if ( l != null)
                    {
                        result = service.GetLocationDate(l.Id, from, to).ToList();
                    }
                }
                if (rbAll.IsChecked == true)
                {
                    foreach (Measurement mes in result)
                    {
                        resultText += mes.Time.ToString() + " " + mes.Humidity + "% " + mes.Temperature + "C \n";
                    }
                }
                else {
                    double temp = 0;
                    double humid = 0;
                    int humidCount = 0;

                    foreach (Measurement mes in result)
                    {
                        temp += mes.Temperature;
                        if (mes.Humidity > 0) { 

                            humidCount++;
                            humid += mes.Humidity;
                        }
                    }

                    temp /= result.Count;
                    humid /= humidCount;
                    if (rbHumidity.IsChecked == true)
                    {
                        resultText = humid.ToString();
                    }
                    else
                    {
                        resultText = temp.ToString();
                    }
                }


            }
 
            else if(errorValue == false)
            {

                bool greaterThen = false;
                if (rbMore.IsChecked == true) {
                    greaterThen = true;
                }

                if (rbMeasurer.IsChecked == true && m != null)
                {
                    result = service.GetMeasurementValue(m.Id, greaterThen, value).ToList();
                }
                else if (l != null)
                {
                    result = service.GetLocationValue(l.Id, greaterThen, value).ToList();
                }

                foreach (Measurement mes in result) {
                    if (greaterThen)
                    {
                        if (rbHumidity.IsChecked == true)
                        {
                            if (mes.Humidity > value)
                                resultText += mes.Time.ToString() + " " + mes.Humidity + "% \n";
                        }
                        else
                        {
                            if (mes.Temperature > value)
                                resultText += mes.Time.ToString() + " " + mes.Temperature + "C \n";
                        }
                    }
                    else {
                        if (rbHumidity.IsChecked == true)
                        {
                            if (mes.Humidity < value && mes.Humidity >0 )
                                resultText += mes.Time.ToString() + " " + mes.Humidity + "% \n";
                        }
                        else
                        {
                            if (mes.Temperature < value)
                                resultText += mes.Time.ToString() + " " + mes.Temperature + "C \n";
                        }

                    }
                }
            }

            tbText.Text = resultText;

        }

       
    }

}
