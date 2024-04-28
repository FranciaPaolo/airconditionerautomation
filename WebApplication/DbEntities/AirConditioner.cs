using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HomeDev.DbEntities
{
    public class AirConditioner
    {
        [Key]
        public int room_id { get; set; }
        public string date { get; set; }
        public int power { get; set; }
        public int mode { get; set; }
        public decimal temperature { get; set; }

        internal void Update(AirConditioner airConditioner)
        {
            this.date = airConditioner.date;
            this.power = airConditioner.power;
            this.mode = airConditioner.mode;
            this.temperature = airConditioner.temperature;
        }

        internal void SetDate(DateTime dateTime)
        {
            this.date = DbUtility.GetStrDateTime(dateTime);
        }

        internal void Rehidrate_FromIot()
        {
            this.date = DbUtility.GetStrDateTime(DateTime.Now);
            this.room_id = 1;
        }
    }
}