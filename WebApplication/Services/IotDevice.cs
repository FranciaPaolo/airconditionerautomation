using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeDev.DbEntities;
using Newtonsoft.Json;

namespace HomeDev.Services
{
    public class IotDevice
    {
        private readonly ILogger _logger;
        public readonly string _url = "";

        public IotDevice(ILogger logger, string url)
        {
            this._logger = logger;
            this._url = url;
        }

        public async Task<AirConditioner> GetAcStatus()
        {
            string url_method = $"{_url}/airconditioning";
            this._logger.LogInformation($"...sending request to {url_method}");

            AirConditioner obj = null;
            try
            {
                using var client = new HttpClient();
                var result = await client.GetAsync(url_method);

                if (result.IsSuccessStatusCode)
                {
                    string strResult = await result.Content.ReadAsStringAsync();
                    this._logger.LogInformation(strResult);

                    obj = JsonConvert.DeserializeObject<AirConditioner>(strResult);
                    obj.Rehidrate_FromIot();
                }
                else
                {
                    this._logger.LogError($"error reading from {_url}airconditioning");
                    this._logger.LogError(result.StatusCode.ToString());
                    this._logger.LogError(await result.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
            }

            return obj;
        }

        public async Task<ClimaHistory> GetClima()
        {
            string url_method = $"{_url}/dht22";
            this._logger.LogInformation($"...sending request to {url_method}");

            ClimaHistory obj = null;
            try
            {
                using var client = new HttpClient();
                var result = await client.GetAsync(url_method);

                if (result.IsSuccessStatusCode)
                {
                    string strResult = await result.Content.ReadAsStringAsync();
                    this._logger.LogInformation(strResult);

                    obj = JsonConvert.DeserializeObject<ClimaHistory>(strResult);
                    obj.Rehidrate_FromIot();
                }
                else
                {
                    this._logger.LogError($"error reading from {_url}airconditioning");
                    this._logger.LogError(result.StatusCode.ToString());
                    this._logger.LogError(await result.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
            }

            return obj;
        }

        public async Task<AirConditioner> SetAcStatus(AirConditioner airConditioner)
        {
            string url_method = $"{_url}";

            if (airConditioner.power == 1)
                url_method += $"/climateon?temperature={airConditioner.temperature}&mode={airConditioner.mode}";
            else
                url_method += "/climateoff";

            this._logger.LogInformation($"...sending request to {url_method}");

            AirConditioner obj = null;
            try
            {
                using var client = new HttpClient();
                var result = await client.GetAsync(url_method);

                if (result.IsSuccessStatusCode)
                {
                    string strResult = await result.Content.ReadAsStringAsync();
                    this._logger.LogInformation(strResult);

                    obj = JsonConvert.DeserializeObject<AirConditioner>(strResult);
                    obj.Rehidrate_FromIot();
                }
                else
                {
                    this._logger.LogError($"error reading from {_url}airconditioning");
                    this._logger.LogError(result.StatusCode.ToString());
                    this._logger.LogError(await result.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
            }
            return obj;
        }
    }
}