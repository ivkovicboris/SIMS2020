using SIMS2020.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS2020.Service
{
    public interface IMedicineService : IService<Medicine, long>
    {
        bool CheckIfCodeNameUnique(string code);
        IEnumerable<Medicine> SearchMedicine(string searchParam);
        IEnumerable<Medicine> SearchByRange(float minPrice, float maxPrice);
        float GetMaxPrice();
        void SoftDelete(string code);
    }
}
