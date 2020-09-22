using AuthorizationMicroservice.Models;
using AuthorizationMicroservice.Repositories;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationMicroservice.Providers
{
    public class MedicalRepresentativeProvider
    {
        ILog logger = LogManager.GetLogger(typeof(MedicalRepresentativeProvider));
        private readonly MedicalRepresentativeRepository repository;
        public MedicalRepresentativeProvider(MedicalRepresentativeRepository repo)
        {
            repository = repo;
        }
        public bool Validate(MedicalRepresentative representative)
        {
            if (string.IsNullOrEmpty(representative.Email) || string.IsNullOrEmpty(representative.Password))
                return false;
            try
            {
                IEnumerable<MedicalRepresentative> representativesList = repository.GetMedicalRepresentatives();
                MedicalRepresentative authRepresentative = representativesList.Where(r => r.Email == representative.Email).FirstOrDefault();
                if (authRepresentative != null && authRepresentative.Password == representative.Password)
                {
                    logger.Info("Successfully logged in "+authRepresentative.Name);
                    return true;
                }
                logger.Info("Invalid Credentials");
                return false;
            }
            catch(Exception e)
            {
                logger.Error("Exception arised\n" + e.Message);
                return false;
            }
        }
    }
}
