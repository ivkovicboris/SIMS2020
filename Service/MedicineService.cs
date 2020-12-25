using SIMS2020.Model;
using SIMS2020.Repository;
using SIMS2020.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS2020.Service
{
    public class MedicineService : IMedicineService
    {
        private readonly IMedicineRepository _medicineRepository;

        public MedicineService(IMedicineRepository medicineRepository)
        {
            _medicineRepository = medicineRepository;
        }
        public IEnumerable<Medicine> GetAll()
        {
            var medicines = _medicineRepository.GetAll();
            return medicines;
        }
        public Medicine Get(long id)
        {
            var medicine = _medicineRepository.Get(id);
            return medicine;
        }
        public Medicine Create(Medicine medicine)
        {
            var newMedicine = _medicineRepository.Create(medicine);
            return newMedicine;
        }
        public void Delete(Medicine medicine)
        {
            _medicineRepository.Delete(medicine);
        }
        public void Update(Medicine medicine)
        {
            
            _medicineRepository.Update(medicine);
        }
        public bool CheckIfCodeNameUnique(string code)
        {
            if (_medicineRepository.GetAll().ToList().Find(medicine => medicine.Code.Equals(code)) == null)
            {
                return true;
            }
            return false;
        }
        public IEnumerable<Medicine> SearchMedicine(string searchParam)
        {
            var medicines = _medicineRepository.GetAll();
            List<Medicine> result = new List<Medicine>();
            foreach (Medicine medicine in medicines)
            {
                if (medicine.Code.ToUpper().Contains(searchParam)) result.Add(medicine);
                else if (medicine.Name.ToUpper().Contains(searchParam)) result.Add(medicine);
                else if (medicine.Manufacturer.ToUpper().Contains(searchParam)) result.Add(medicine);
                //TODO PRICE RANGE

            }
            return result.Distinct();
        }
        public IEnumerable<Medicine> SearchByRange(float minPrice, float maxPrice)
        {
            return _medicineRepository.Find(medicine => medicine.Price >= minPrice && medicine.Price <= maxPrice).ToList();
        }
        public float GetMaxPrice()
        {
            float maxPrice = 0;
            foreach (Medicine medicine in _medicineRepository.GetAll())
            {
                if (medicine.Price > maxPrice) maxPrice = medicine.Price;
            }
            return maxPrice;
        }
        public void SoftDelete(string code)
        {
            Medicine medicine = _medicineRepository.Find(med => med.Code.Equals(code)).First();
            medicine.Deleted = true;
            Update(medicine);
        }
    }
}
