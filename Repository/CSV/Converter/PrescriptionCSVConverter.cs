using SIMS2020.Model;
using SIMS2020.Model.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS2020.Repository.CSV.Converter
{
    class PrescriptionCSVConverter : ICSVConverter<Prescription>
    {
        private readonly string _delimiter;

        public PrescriptionCSVConverter(string delimiter)
        {
            _delimiter = delimiter;
        }
        public int i = 0;
        

        public string ConvertEntityToCSVFormat(Prescription prescription)
        {
            string medicines = "";
            foreach (MedicineAmount medicine in prescription.Medicines)
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
           prescription.Id,
           prescription.DoctorId,
           prescription.PatientJMBG,
           prescription.DateTime);
           
           return result + "#" + medicines;

        }
        public Prescription ConvertCSVFormatToEntity(string prescriptionCSVFormat)
        {
            string[] tokens = prescriptionCSVFormat.Split(_delimiter.ToCharArray());           
           
            Prescription prescription = new Prescription();
            prescription.Id = long.Parse(tokens[0]);
            prescription.DoctorId = tokens[1];
            prescription.PatientJMBG = tokens[2];
            prescription.DateTime = tokens[3];
            
            List<MedicineAmount> medicines = new List<MedicineAmount>();
            MedicineAmount medicineAmount = new MedicineAmount();
            Medicine medicine = new Medicine();

            string[] tokensForMedicineDictionary = prescriptionCSVFormat.Split('#');

            string[] tokensForMedicine = tokensForMedicineDictionary[1].Split(';');
            i = 0;
            while(!tokensForMedicine[i].Equals("")) 
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
                medicines.Insert(i,medicineAmount);
                i++;

            }


            prescription.Medicines = medicines;
            return prescription;
             
        }
    }
}
