using AuthorizationMicroservice.Interfaces;
using AuthorizationMicroservice.Models;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AuthorizationMicroservice.Repositories
{
    public class MedicalRepresentativeRepository:IMedicalRepresentative
    {
        ILog logger = LogManager.GetLogger(typeof(MedicalRepresentativeRepository));
        Uri baseAddress = new Uri("https://localhost:44334/api/MedicalRepresentative");
        HttpClient client;
        public MedicalRepresentativeRepository()
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;
            logger.Info("Constructor Initialized");
        }
        public IEnumerable<MedicalRepresentative> GetMedicalRepresentatives()
        {
            try
            {
                HttpResponseMessage response = client.GetAsync(client.BaseAddress).Result;
                if (response.IsSuccessStatusCode)
                {
                    List<MedicalRepresentative> representativesList;
                    string data = response.Content.ReadAsStringAsync().Result;
                    representativesList = JsonConvert.DeserializeObject<List<MedicalRepresentative>>(data);
                    logger.Info("List successfully fetched from Microservice");
                    return representativesList;
                }
                logger.Error("(In MedicalRepresentativeRepository.cs)Server error " + response.StatusCode);
                return null;
            }
            catch
            {
                logger.Error("RepSchedule Microservice not responding");
                return null;
            }
        }
    }
}
