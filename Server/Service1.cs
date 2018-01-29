using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Server.Model.Context;
using Server.Model.Entities;

namespace Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Service1 : IService1
    {

        private static Dictionary<Guid,List<IServerCallBack>> observers= new Dictionary<Guid, List<IServerCallBack>>();
        private static object locker = new object();

        public void AddMeasurement(Measurement measurement)
        {
            using (var context = new DRUSContext())
            {
                 context.Measurements.Add(measurement);
                 context.SaveChanges();
            }
            /*
            if (observers.ContainsKey(measurement.Measurer.Id))
                foreach (var observerForClient in observers[measurement.Measurer.Id])
                {
               
                        try
                        {
                            observerForClient.BroadcastToObserver(measurement);
                        }
                        catch (Exception ex)
                        {
                            observers[measurement.Measurer.Id].Remove(observerForClient);
                        }
                }*/
        }

        public List<Measurer> GetClients()
        {
            using (var context = new DRUSContext())
            {
                return context.Measurers.ToList();
            }
        }

        public List<Measurement> GetLocationDate(Guid LocationId, DateTime fromm, DateTime to)
        {
            using (var context = new DRUSContext())
            {
                var res =
                     from m in context.Measurements
                     where m.Measurer.LocationId == LocationId && m.Time < to && m.Time > fromm
                     select m;

                return res.ToList();
            }
        }

        public List<Location> GetLocations()
        {
            using (var context = new DRUSContext())
            {
                return context.Locations.ToList();
            }
        }

        public List<Measurement> GetLocationValue(Guid LocationId, bool graterThen, double value)
        {
            using (var context = new DRUSContext())
            {
                
                if (graterThen == true)
                {
                    var res =
                    from m in context.Measurements
                    where m.Measurer.LocationId == LocationId && (m.Temperature>value || m.Humidity>value)
                    select m;

                    return res.ToList();

                }
                else {
                    var res =
                     from m in context.Measurements
                     where m.Measurer.LocationId == LocationId && (m.Temperature < value || m.Humidity < value)
                     select m;

                    return res.ToList();
                }

                
            }
        }

        public List<Measurement> GetMeasurementDate(Guid MeasurerId, DateTime fromm, DateTime to)
        {
            using (var context = new DRUSContext())
            {
                var res =
                     from m in context.Measurements
                     where m.Measurer.Id == MeasurerId && m.Time < to && m.Time > fromm
                     select m;

                return res.ToList();
            }
        }

        public List<Measurement> GetMeasurementValue(Guid MeasurerId, bool greaterThen, double value)
        {
            using (var context = new DRUSContext())
            {

                if (greaterThen == true)
                {
                    var res =
                    from m in context.Measurements
                    where m.Measurer.Id == MeasurerId && (m.Temperature > value || m.Humidity > value)
                    select m;

                    return res.ToList();

                }
                else
                {
                    var res =
                     from m in context.Measurements
                     where m.Measurer.Id == MeasurerId && (m.Temperature < value || (m.Humidity < value && m.Humidity >0))
                     select m;

                    return res.ToList();
                }


            }
        }

        public Measurer RegisterMeasurer()
        {
            Measurer measurer = new Measurer();
            Random rand = new Random();
            using (var context = new DRUSContext())
            {

                // Dodeli mu random lokaciju
                int loc = rand.Next(context.Locations.Count());
                var locations = context.Locations.ToArray();
                measurer.LocationId = locations.ElementAt(loc).Id;
                measurer.Name = rand.Next().ToString();
                Measurer m = context.Measurers.Add(measurer);
                context.SaveChanges();
                return m;
            }

        }

        public void RegisterObserver(Guid clientId)
        {
            try
            {
                IServerCallBack callback = OperationContext.Current.GetCallbackChannel<IServerCallBack>();
                lock (locker)
                {

                    if (observers.Keys.Contains(clientId))
                    {
                        List<IServerCallBack> forClient = observers[clientId];
                        forClient.Add(callback);
                    }
                    else
                    {
                        List<IServerCallBack> forClient = new List<IServerCallBack>();
                        forClient.Add(callback);
                        observers.Add(clientId, forClient);

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }

       
    
}
