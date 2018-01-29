using Server.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    //[ServiceContract(CallbackContract = typeof(IServerCallBack))]
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        Measurer RegisterMeasurer();

        [OperationContract(IsOneWay = true)]
        void AddMeasurement(Measurement measurement);
        [OperationContract]
        List<Location> GetLocations();
        [OperationContract]
        List<Measurer> GetClients();
        [OperationContract]
        List<Measurement> GetMeasurementDate(Guid MeasurerId, DateTime fromm, DateTime to);
        [OperationContract]
        List<Measurement> GetLocationDate(Guid LocationId, DateTime fromm, DateTime to);
        [OperationContract]
        List<Measurement> GetMeasurementValue(Guid MeasurerId, bool greaterThen, double value);
        [OperationContract]
        List<Measurement> GetLocationValue(Guid LocationId, bool graterThen, double value);



        [OperationContract(IsOneWay = true)]
        void RegisterObserver(Guid clientId);

       
    }
    public interface IServerCallBack
    {
        [OperationContract(IsOneWay = true)]
        void BroadcastToObserver(Measurement data);
    }


}
