using SIMS2020.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS2020.Repository.CSV.Converter
{
    public class MedicineCSVConverter :ICSVConverter<Medicine>
    {
        private readonly string _delimiter;

        public MedicineCSVConverter(string delimiter)
        {
            _delimiter = delimiter;
        }

        public string ConvertEntityToCSVFormat(Medicine medicine)
        => string.Join(_delimiter,
            medicine.Id,
            medicine.Code,
            medicine.Name,
            medicine.Manufacturer,
            medicine.Prescribed,
            medicine.Price,
            medicine.Deleted
            );

        public Medicine ConvertCSVFormatToEntity(string medicineCSVFormat)
        {
            string[] tokens = medicineCSVFormat.Split(_delimiter.ToCharArray());

            return new Medicine(
                long.Parse(tokens[0]),
                tokens[1],
                tokens[2],
                tokens[3],
                bool.Parse(tokens[4]),
                float.Parse(tokens[5]),
                bool.Parse(tokens[6]));
                
        }
    }
}
