using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model.Entities
{

    [DataContract]
    public class Measurement
    {

        [DataMember]
        [Key]
        [Column(Order = 1)]
        public Guid MeasurerId { get; set; }
        [DataMember]
        public Measurer Measurer{ get; set; }
        [DataMember]
        public int Temperature { get; set; }
        [DataMember]
        public int Humidity { get; set; }
        [DataMember]
        [Key]
        [Column(Order = 2)]
        public DateTime Time { get; set; }
    }
}
