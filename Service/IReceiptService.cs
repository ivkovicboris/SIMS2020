using SIMS2020.Model;
using SIMS2020.Model.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS2020.Service
{
    public interface IReceiptService : IService<Receipt, long>
    {
        IEnumerable<object> GetAllSoldMedicineByManufacturer(string manufacturer);
        IEnumerable<object> GetAllSoldMedicineByPharmacist(long pharmacistId);
        IEnumerable<object> GetAllSoldMedicines();
    }
}
