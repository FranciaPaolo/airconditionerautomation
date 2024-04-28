using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace HomeDev.DbEntities
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<ClimaHistory> Clima_History { get; set; }
        public DbSet<AirConditioner> Last_AC_Status { get; set; }

        public async Task<AirConditioner> GetLastAcStatus(int room_id)
        {
            return await Last_AC_Status.FirstOrDefaultAsync(i => i.room_id == room_id);
        }

        public async Task<IEnumerable<ClimaHistory>> GetClima_LastDays(int room_id, int num_of_days)
        {
            DateTime since = DateTime.Now.AddDays(-1 * num_of_days);
            string since_str = since.ToString("yyyy-MM-dd");

            return await Clima_History.FromSqlRaw("select * from clima_history where date > @p1 order by date asc", new SqliteParameter("@p1", since_str))
                .ToListAsync();
        }

        public async Task Create_ClimaHistory_IfNotExistsAsync(ClimaHistory climaHistory)
        {
            if (await Clima_History.FirstOrDefaultAsync(i => i.id == climaHistory.id) == null)
            {
                await Clima_History.AddAsync(climaHistory);
            }
        }

        public async Task CreateOrUpdate_ClimaHistory(ClimaHistory climaHistory)
        {
            var climaOld= await Clima_History.FirstOrDefaultAsync(i => i.id == climaHistory.id);
            if (climaOld == null)
            {
                await Clima_History.AddAsync(climaHistory);
            }
            else
            {
                climaOld.Update(climaHistory);
            }
        }

        public async Task CreateOrUpdate_AirConditioner(AirConditioner airConditioner)
        {
            var lastAc = await Last_AC_Status.FirstOrDefaultAsync(i => i.room_id == airConditioner.room_id);
            if (lastAc==null)
            {
                await Last_AC_Status.AddAsync(airConditioner);
            }
            else
            {
                lastAc.Update(airConditioner);

                //await Last_AC_Status.ExecuteUpdateAsync();
            }
        }

    }
}