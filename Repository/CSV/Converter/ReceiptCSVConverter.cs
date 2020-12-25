using SIMS2020.Model;
using SIMS2020.Model.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS2020.Repository.CSV.Converter
{
    class ReceiptCSVConverter : ICSVConverter<Receipt>
    {
        private readonly string _delimiter;

        public ReceiptCSVConverter(string delimiter)
        {
            _delimiter = delimiter;
        }
        public int i = 0;


        public string ConvertEntityToCSVFormat(Receipt receipt)
        {
            string medicines = "";
            foreach (MedicineAmount medicine in receipt.Medicines)
            {
                medicines += string.Join(_delimiter,
                   medicine.Amount.ToString(),
                   medicine.Medicine.Id,
                   medicine.Medicine.Code,
                   medicine.Medicine.Name,
                   medicine.Medicine.Manufacturer,
                   medicine.Medicine.Prescribed,
                   medicine.Medicine.Price,
                   medicine.Medicine.Deleted
                   );
                medicines += ';';
            }
            string result = "";
            result = string.Join(_delimiter,
           receipt.Id,
           receipt.PharmacistId,
           receipt.DateTime,
           receipt.TotalPrice);

            return result + "#" + medicines;

        }
        public Receipt ConvertCSVFormatToEntity(string receiptCSVFormat)
        {
            
            string[] tokensForMedicineDictionary = receiptCSVFormat.Split('#');
            string[] tokens = tokensForMedicineDictionary[0].Split(_delimiter.ToCharArray());
            Receipt receipt = new Receipt();
            receipt.Id = long.Parse(tokens[0]);
            receipt.PharmacistId = long.Parse(tokens[1]);
            receipt.DateTime = tokens[2];
            receipt.TotalPrice = float.Parse(tokens[3]);

            List<MedicineAmount> medicines = new List<MedicineAmount>();
            MedicineAmount medicineAmount = new MedicineAmount();
            Medicine medicine = new Medicine();

            

            string[] tokensForMedicine = tokensForMedicineDictionary[1].Split(';');
            i = 0;
            while (!tokensForMedicine[i].Equals(""))
            {
                string[] tempTokens = tokensForMedicine[i].Split(',');
                medicineAmount = new MedicineAmount();
                medicineAmount.Amount = int.Parse(tempTokens[0]);
                medicine = new Medicine(long.Parse(tempTokens[1]),
                tempTokens[2],
                tempTokens[3],
                tempTokens[4],
                bool.Parse(tempTokens[5]),
                float.Parse(tempTokens[6]),
                bool.Parse(tempTokens[7]));


                medicineAmount.Medicine = medicine;
                medicines.Insert(i, medicineAmount);
                i++;

            }


            receipt.Medicines = medicines;
            return receipt;

        }
    }
}
