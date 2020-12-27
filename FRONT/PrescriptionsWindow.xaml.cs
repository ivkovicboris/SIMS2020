using SIMS2020.Controller;
using SIMS2020.Model;
using SIMS2020.Model.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SIMS2020.FRONT
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class PrescriptionWindow : Window
    {
        public UserRole userRole = LogInWindow.userRole;
        private static ObservableCollection<Prescription> prescriptionList { get; set; }
        private static ObservableCollection<MedicineAmount> medicineAmount { get; set; }
        public long DoctorName { get; private set; }
        public int MedicineAmount { get; set; }
        
        public string MedicineName { get;  set; }

        private PrescriptionController _prescriptionController;
        private UserController _userController;
        public PrescriptionWindow()
        {
            InitializeComponent();
            var app = Application.Current as App;
            _prescriptionController = app.PrescriptionController;
            _userController = app.UserController;
            CheckVisibility();
            GetAllPrescriptions();
        }

        private void CheckVisibility()
        {
            IsVisibleNew.Visibility = Visibility.Hidden;

            if (userRole.Equals(UserRole.Doctor))
            {
                IsVisibleNew.Visibility = Visibility.Visible;
            }
        }
        public void GetAllPrescriptions()
        {
            prescriptionList = new ObservableCollection<Prescription>(_prescriptionController.GetAll().ToList());
            medicineAmount = new ObservableCollection<MedicineAmount>();
            foreach(Prescription prescription in prescriptionList)
            {
                string medicinesShow = "";
                prescription.DoctorId = GetDoctorNameById(prescription.DoctorId);
                foreach(MedicineAmount medicine in prescription.Medicines)
                {
                     medicinesShow += medicine.Medicine.Name.ToString() + " - " + medicine.Amount.ToString() + "\n";
                }
                prescription.DateTime = medicinesShow;
            }
            PrescriptionTable.ItemsSource = prescriptionList;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string GetDoctorNameById(string idString)
        {
            long id = Convert.ToInt64(idString);
            User doctor = _userController.Get(id);
            string result = doctor.FirstName + " " + doctor.LastName;
            return result;
        }
        private void tb_SearchParam_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchParam = tb_SearchParam.Text.Trim();
            BindingList<Prescription> searchPrescriptionList = new BindingList<Prescription>();
            if (searchParam.Equals(""))
            {
                searchPrescriptionList = new BindingList<Prescription>(_prescriptionController.GetAll().ToList());
            }
            else
            {
                searchPrescriptionList = new BindingList<Prescription>(_prescriptionController.SearchPrescription(searchParam).ToList());
            }
            PrescriptionTable.ItemsSource = searchPrescriptionList;
        }
        private void Button_Click_NewPrescription(object sender, RoutedEventArgs e)
        {
            NewPrescriptionWindow window = new NewPrescriptionWindow();
            window.Show();

        }
        private void Button_Click_Home(object sender, RoutedEventArgs e)
        {
            
            MainWindow window = new MainWindow();
            window.Show();
            Close();
        }
    }
}
