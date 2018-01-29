using Client.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Client
{
    class Program
    {
        private static Timer timer;
        private static Random rand;
        private static int call;
        private static Service1Client client;
        private static ServiceReference1.Measurer me;
        static void Main(string[] args)
        {
            client = new Service1Client();
            rand = new Random();
            //load me from file, if i dont exist register me
            me = client.RegisterMeasurer();
            //save 'me' to file
            Console.WriteLine("Ja sam {0}.", me.Name);
           
            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += SendMeasurent;
            timer.Enabled = true;
            String s = "";
            while (true) { s = Console.ReadLine();
                if (s.Equals("stop")) break;
            }
            client.Close();
        }

        private static void SendMeasurent(object sender, EventArgs e)
        {
            Measurement m = new Measurement();
            m.MeasurerId = me.Id;
            m.Temperature = rand.Next(70) - 25;
            m.Humidity = -1;
            m.Time = System.DateTime.Now;
            call++;
            if (call == 6)
            {
                call = 0;
                m.Humidity = rand.Next(15) + 85;
            }
            client.AddMeasurement(m);
            Console.WriteLine("Sent temp {0}, hum {1}", m.Temperature, m.Humidity);
        }
    }
}
