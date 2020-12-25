using SIMS2020.Controller;
using SIMS2020.Model;
using SIMS2020.Repository;
using SIMS2020.Repository.CSV.Converter;
using SIMS2020.Repository.CSV.Stream;
using SIMS2020.Repository.Sequencer;
using SIMS2020.Service;
using System.Windows;

namespace SIMS2020
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string CSV_DELIMITER = ",";
        //private const string CSV_DICTIONARY_DELIMITER = "#";
        private const string USER_FILE = "../../Resources/Data/users.csv";
        private const string MEDICINE_FILE = "../../Resources/Data/medicines.csv";
        private const string PRESCRIPTION_FILE = "../../Resources/Data/prescriptions.csv";
        private const string RECEIPT_FILE = "../../Resources/Data/receipts.csv";
        private const string DATETIME_FORMAT = "dd.MM.yyyy.";

        
        public App()
        {
            var userRepository = new UserRepository(
                    new CSVStream<User>(USER_FILE, new UserCSVConverter(CSV_DELIMITER)),
                    new LongSequencer()
                );
            var medicineRepository = new MedicineRepository(
                    new CSVStream<Medicine>(MEDICINE_FILE, new MedicineCSVConverter(CSV_DELIMITER)),
                    new LongSequencer()
                );
            var prescriptionRepository = new PrescriptionRepository(
                    new CSVStream<Prescription>(PRESCRIPTION_FILE, new PrescriptionCSVConverter(CSV_DELIMITER)),
                    new LongSequencer()
                );
            var receiptRepository = new ReceiptRepository(
                new CSVStream<Receipt>(RECEIPT_FILE, new ReceiptCSVConverter(CSV_DELIMITER)),
                    new LongSequencer()
                );

            var userService = new UserService(userRepository);
            var medicineService = new MedicineService(medicineRepository);
            var prescriptionService = new PrescriptionService(prescriptionRepository);
            var receiptService = new ReceiptService(receiptRepository);


            UserController = new UserController(userService);
            MedicineController = new MedicineController(medicineService);
            PrescriptionController = new PrescriptionController(prescriptionService);
            ReceiptController = new ReceiptController(receiptService);

        }

        public UserController UserController { get; private set; }
        public MedicineController MedicineController { get; private set; }
        public PrescriptionController PrescriptionController { get; private set; }
        public ReceiptController ReceiptController { get; private set; }
    }
}
