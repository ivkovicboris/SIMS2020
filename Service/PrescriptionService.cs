using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS2020.Model;
using SIMS2020.Model.Util;
using SIMS2020.Repository.Abstract;

namespace SIMS2020.Service
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IPrescriptionRepository _prescriptionRepository;

        public PrescriptionService(IPrescriptionRepository prescriptionRepository)
        {
            _prescriptionRepository = prescriptionRepository;
        }

        public Prescription Create(Prescription prescription)
        {
           return _prescriptionRepository.Create(prescription);
        }
        public void Delete(Prescription prescription)
        {
            _prescriptionRepository.Delete(prescription);
        }
        public Prescription Get(long id)
        {
            return _prescriptionRepository.Get(id);
        }
        public IEnumerable<Prescription> GetAll()
        {
            return _prescriptionRepository.GetAll();
        }

        public IEnumerable<Prescription> SearchPrescription(string searchParam)
        {
            
                var prescriptions = _prescriptionRepository.GetAll();
                List<Prescription> result = new List<Prescription>();
                foreach (Prescription prescription in prescriptions)
                {
                    if (prescription.Id.Equals(long.Parse(searchParam))) result.Add(prescription);
                    else if (prescription.DoctorId.ToUpper().Contains(searchParam)) result.Add(prescription);
                    else if (prescription.PatientJMBG.ToUpper().Contains(searchParam)) result.Add(prescription);
                    foreach (MedicineAmount medicine in prescription.Medicines)
                    {
                    if (medicine.Medicine.Name.ToUpper().Contains(searchParam)) result.Add(prescription);
                    }
                }
                return result.Distinct();
            
        }

        public void Update(Prescription prescription)
        {
            _prescriptionRepository.Update(prescription);
        }
    }
}
