using SIMS2020.Model;
using SIMS2020.Model.Util;
using SIMS2020.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS2020.Service
{
    public class ReceiptService : IReceiptService
    {
        private readonly IReceiptRepository _receiptRepository;

        public ReceiptService(IReceiptRepository receiptRepository)
        {
            _receiptRepository = receiptRepository;
        }

        public Receipt Create(Receipt receipt)
        {
            return _receiptRepository.Create(receipt);
        }
        public void Delete(Receipt receipt)
        {
            _receiptRepository.Delete(receipt);
        }
        public Receipt Get(long id)
        {
            return _receiptRepository.Get(id);
        }
        public IEnumerable<Receipt> GetAll()
        {
            return _receiptRepository.GetAll();
        }
        public void Update(Receipt receipt)
        {
            _receiptRepository.Update(receipt);
        }

        public IEnumerable<object> GetAllSoldMedicineByManufacturer(string manufacturer)
        {
            var result = new List<MedicineAmount>();
            IEnumerable<Receipt> allReceipts = GetAll();
            foreach (Receipt receipt in GetAll())
            {
                foreach (MedicineAmount medicineAmount in receipt.Medicines)
                {
                    if (medicineAmount.Medicine.Manufacturer.Equals(manufacturer))
                    {
                        result.Add(medicineAmount);
                    }

                }

            } return GroupMedicines(result);
        }

        public IEnumerable<object> GetAllSoldMedicineByPharmacist(long pharmacistId)
        {
            List<MedicineAmount> result = new List<MedicineAmount>();
            IEnumerable<Receipt> pharmacistReceipts = GetAll().Where(receipt => receipt.PharmacistId.Equals(pharmacistId)).ToList();
            foreach (Receipt receipt in pharmacistReceipts)
            {
                result.AddRange(receipt.Medicines);
            }
            return GroupMedicines(result);
        }
        public IEnumerable<object> GetAllSoldMedicines()
        {
            List<MedicineAmount> result = new List<MedicineAmount>();
            foreach(Receipt receipt in GetAll())
            {
                result.AddRange(receipt.Medicines);
            }
            return  GroupMedicines(result);
         }
    

    public IEnumerable<object> GroupMedicines(List<MedicineAmount> allMedicines)
        {
            var totalSales = from medicineAmount in allMedicines
                             group medicineAmount by medicineAmount.Medicine.Name into medicineGroup
                             select new
                             {
                                 Name = medicineGroup.Key,
                                 TotalAmount = medicineGroup.Sum(x => x.Amount),
                                 TotalPrice = medicineGroup.Sum(x => x.Amount * x.Medicine.Price),
                             };
            return totalSales;

        }
    }
}
