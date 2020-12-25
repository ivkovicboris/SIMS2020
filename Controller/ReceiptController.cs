using SIMS2020.Model;
using SIMS2020.Model.Util;
using SIMS2020.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS2020.Controller
{
    public class ReceiptController
    {
        private readonly IReceiptService _receiptService;

        public ReceiptController (IReceiptService receiptService)
        {
            _receiptService = receiptService;
        }

        public IEnumerable<Receipt> GetAll() => _receiptService.GetAll();
        public Receipt Get(long id) => _receiptService.Get(id);
        public Receipt Create(Receipt receipt) => _receiptService.Create(receipt);

        public IEnumerable<object> GetAllSoldMedicineByManufacturer(string manufacturer)
        {
           return _receiptService.GetAllSoldMedicineByManufacturer(manufacturer);
        }
        public IEnumerable<object> GetAllSoldMedicineByPharmacist(long pharmacistId)
        {
            return _receiptService.GetAllSoldMedicineByPharmacist(pharmacistId);
        }
        public IEnumerable<object> GetAllSoldMedicines()
        {
            return _receiptService.GetAllSoldMedicines();
        }
    }
}
