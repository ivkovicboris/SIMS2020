using SIMS2020.Model;
using SIMS2020.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS2020.Controller
{
    public class PrescriptionController
    {
        private readonly IPrescriptionService _prescriptionService;

        public PrescriptionController(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }
        public IEnumerable<Prescription> GetAll() => _prescriptionService.GetAll();
        public Prescription Get(long id) => _prescriptionService.Get(id);
        public Prescription Create(Prescription prescription) => _prescriptionService.Create(prescription);
        public void Delete(Prescription prescription) => _prescriptionService.Delete(prescription);

        public IEnumerable<Prescription> SearchPrescription(string searchParam) => _prescriptionService.SearchPrescription(searchParam.ToUpper());
        
    }
}
