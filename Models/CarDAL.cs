using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DealershipClient.Models
{
    public class CarDAL
    {
        public string GetData()
        {
            string url = @"https://localhost:44390/api/cars";

            HttpWebRequest request = WebRequest.CreateHttp(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader rd = new StreamReader(response.GetResponseStream());
            string json = rd.ReadToEnd();

            return json;
        }

        public string GetData(string option, string search)
        {
            string url = $@"https://localhost:44390/api/cars/{option}/{search}";

            HttpWebRequest request = WebRequest.CreateHttp(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader rd = new StreamReader(response.GetResponseStream());
            string json = rd.ReadToEnd();

            return json;
        }

        public List<Car> GetInventory()
        {
            string json = GetData();

            List<Car> inv = JsonConvert.DeserializeObject<List<Car>>(json);

            return inv;
        }

        public List<Car> GetInventory(string option, string search)
        {
            string json = GetData(option, search);

            List<Car> inv = JsonConvert.DeserializeObject<List<Car>>(json);

            return inv;
        }
    }
}
