using SIMS2020.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS2020.Service
{
    public interface IPrescriptionService : IService<Prescription, long>
    {
        IEnumerable<Prescription> SearchPrescription(string searchParam);
    }
}
