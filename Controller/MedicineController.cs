using SIMS2020.Model;
using SIMS2020.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS2020.Controller
{
    public class MedicineController
    {
        private readonly IMedicineService _medicineService;

        public MedicineController(IMedicineService medicineService)
        {
            _medicineService = medicineService;
        }
        public IEnumerable<Medicine> GetAll() => _medicineService.GetAll();
        public Medicine Get(long id) => _medicineService.Get(id);
        public Medicine Create(Medicine medicine) => _medicineService.Create(medicine);
        public void Update(Medicine medicine) => _medicineService.Update(medicine);
        public void Delete(Medicine medicine) => _medicineService.Delete(medicine);
        public bool CheckIfCodeNameUnique(string code)
        {
            if (_medicineService.CheckIfCodeNameUnique(code)) 
                return true;
            return false;
        }
        public IEnumerable<Medicine> SearchMedicine(string searchParam)
        {
            return _medicineService.SearchMedicine(searchParam.ToUpper());
        }
        public IEnumerable<Medicine> SearchByRange(float minPrice, float maxPrice)
        {
            return _medicineService.SearchByRange(minPrice, maxPrice);
        }

        internal float GetMaxPrice()
        {
            return _medicineService.GetMaxPrice();
        }

        internal void SoftDelete(string code)
        {
            _medicineService.SoftDelete(code);
        }
    }
}
