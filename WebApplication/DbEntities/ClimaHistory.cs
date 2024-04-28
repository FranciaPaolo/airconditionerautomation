using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HomeDev.DbEntities
{
    public class ClimaHistory
    {
        [Key]
        public string id { get; set; }
        public string date { get; set; }
        public int room_id { get; set; }
        public decimal temperature { get; set; }
        public decimal humidity { get; set; }

        public static ClimaHistory Create(int room_id, DateTime dateTime, decimal temperature, decimal humidity)
        {
            ClimaHistory climaHistory = new ClimaHistory
            {
                id = CreateKey(room_id, dateTime),
                room_id = room_id,
                date = DbUtility.GetStrDateTime(dateTime),
                temperature = temperature,
                humidity = humidity
            };
            return climaHistory;
        }

        private static string CreateKey(int room_id, string date)
        {
            return room_id + "_" + date;
        }
        private static string CreateKey(int room_id, DateTime dateTime)
        {
            return room_id + "_" + DbUtility.GetStrDateTime(dateTime);
        }

        internal void Update(ClimaHistory climaHistory)
        {
            this.temperature = climaHistory.temperature;
            this.humidity = climaHistory.humidity;
        }

        internal void Rehidrate_FromIot()
        {
            this.date = DbUtility.GetStrDateTime(DateTime.Now);
            this.room_id = 1;
            this.id = CreateKey(this.room_id, this.date);
        }
    }
}