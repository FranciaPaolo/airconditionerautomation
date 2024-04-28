using HomeDev.DbEntities;
using HomeDev.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeDev.Controllers;

[ApiController]
[Route("[controller]")]
public class ClimateController : ControllerBase
{
    private readonly ILogger<ClimateController> _logger;
    readonly IConfiguration _configuration;
    private readonly AppDbContext _context;
    private readonly IotDevice iotDevice;

    public ClimateController(
        ILogger<ClimateController> logger, 
        IConfiguration configuration,
        AppDbContext context)
    {
        this._logger = logger;
        this._context = context;
        this._configuration = configuration;

        iotDevice = new IotDevice(this._logger, _configuration.GetValue<string>("IodDevice:Url"));
    }

    [HttpGet("getclimahistory")]
    public async Task<IEnumerable<ClimaHistory>> GetClimaHistory(int roomId)
    {
        return await _context.GetClima_LastDays(roomId, 10);
    }

    [HttpGet("getac")]
    public async Task<ActionResult<AirConditioner>> GetAc(int roomId)
    {
        return await _context.GetLastAcStatus(roomId);
    }

    [HttpGet("getachistory")]
    public async Task<ActionResult<AirConditioner>> GetAcHistory(int roomId)
    {
        return await _context.GetLastAcStatus(roomId);
    }

    [HttpPost("setac")]
    public async Task SetAC(AirConditioner status)
    {
        status.SetDate(DateTime.Now);

        // invio il comando
        await iotDevice.SetAcStatus(status);

        // salvo lo stato
        await _context.CreateOrUpdate_AirConditioner(status);
        _context.SaveChanges();
    }

    // [HttpPost("settestclimadata")]
    // public async Task SetTestData()
    // {
    //     Random rnd = new Random();
    //     rnd.Next();
    //     DateTime data = DateTime.Now;
    //     for (int i = 0; i < 30; i++)
    //     {
    //         data = DbUtility.GetDateTime_floor5Minutes(data);
    //         decimal temp = Convert.ToDecimal(Math.Round(rnd.NextDouble()*40.0,1));
    //         decimal hum = Convert.ToDecimal(Math.Round(rnd.NextDouble()*40.0,1));
    //         ClimaHistory clima = ClimaHistory.Create(
    //             1,
    //             data,
    //             temp,
    //             hum
    //         );
    //         await _context.CreateOrUpdate_ClimaHistory(clima);
    //         _context.SaveChanges();
    //         // vado indietro di 5 min
    //         data = data.AddMinutes(-5);
    //     }
    // }
}
