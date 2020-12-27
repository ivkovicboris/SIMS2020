using SIMS2020.Controller;
using SIMS2020.Model;
using SIMS2020.Model.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
    /// Interaction logic for Window3.xaml
    /// </summary>
    public partial class CartWindow : Window
    {
        private const int MAXIMUM_MEDICINE = 50;
        private const string ERROR_MESSAGE_NOTFILLED = "Please fill in Patinet JMBG and Date of the Presription!";
        private const string ERROR_MESSAGE_AMOUNT = "Please select amount of the medication!";
        private const string ERROR_MESSAGE_MedicineNotAdded = "No medicine added to the prescription";
        private const string ERROR_MESSAGE_MedicineNotSelected = "Please select Medicine first!";
        private const string ERROR_MESSAGE_PrescriptionNotSelected = "Please select prescription you want to add first!";
        private const string ERROR_MESSAGE_MedicineIsPrescribed = "This medicine cannot be added withouth prescription!";
        private List<int> AVAILABLE_AMOUNT_OF_MEDICATION = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }; 

        private static ObservableCollection<Medicine> medicineList { get; set; }
        private static BindingList<MedicineAmount> medicinesInCartList { get; set; }
        private static ObservableCollection<Prescription> prescriptionList { get; set; }
        public string MedicineName { get; set; }

        private PrescriptionController _prescriptionController;
        private MedicineController _medicineController;
        private UserController _userController;
        private ReceiptController _receiptController;

        public UserRole userRole = LogInWindow.userRole;
        public long userId = LogInWindow.userId;

        public CartWindow()
        {
            InitializeComponent();
            var app = Application.Current as App;
            _prescriptionController = app.PrescriptionController;
            _medicineController = app.MedicineController;
            _userController = app.UserController;
            _receiptController = app.ReceiptController;
            cb_Amount.ItemsSource = AVAILABLE_AMOUNT_OF_MEDICATION;
            //GetAllMedicines();
            //GetAllPrescriptions();

            medicinesInCartList = new BindingList<MedicineAmount>();
            totalPrice.Text = "0";
            
        }

        #region GET ALL AND SEARCH 
        public void GetAllMedicines()
        {
            medicineList = new ObservableCollection<Medicine>(_medicineController.GetAll().ToList());
            if (medicineList.Count() != 0)
                MedicinesTable.ItemsSource = HideDeletedMedicineIfNotAdminLoggedIn(medicineList);
        }
        private IEnumerable<Medicine> HideDeletedMedicineIfNotAdminLoggedIn(IEnumerable<Medicine> medicineList)
        {
            if (!userRole.Equals(UserRole.Admin))
            {
                return medicineList.Where(medicine => medicine.Deleted.Equals(false)).ToList();
            }
            else return medicineList;
        }
        public void GetAllPrescriptions()
        {
            prescriptionList = new ObservableCollection<Prescription>(_prescriptionController.GetAll().ToList());
            foreach (Prescription prescription in prescriptionList)
            {
                prescription.DoctorId = GetDoctorNameById(prescription.DoctorId);
                foreach (MedicineAmount medicine in prescription.Medicines)
                {
                    MedicineName = medicine.Medicine.Name;
                }
            }
            PrescriptionTable.ItemsSource = prescriptionList;
        }
        public string GetDoctorNameById(string idString)
        {
            long id = Convert.ToInt64(idString);
            User doctor = _userController.Get(id);
            string result = doctor.FirstName + " " + doctor.LastName;
            return result;
        }
        private void SearchMedicineByParam(object sender, TextChangedEventArgs e)
        {
            var searchParam = tb_SearchParamMedicine.Text.Trim();
            BindingList<Medicine> searchMedicineList = new BindingList<Medicine>();
            searchMedicineList = new BindingList<Medicine>(_medicineController.SearchMedicine(searchParam).ToList());
            MedicinesTable.ItemsSource = HideDeletedMedicineIfNotAdminLoggedIn(searchMedicineList);
        }
        private void SearchPrescriptionByParam(object sender, TextChangedEventArgs e)
        {
            var searchParam = tb_SearchParamPrescription.Text.Trim();
            BindingList<Prescription> searchPrescriptionList = new BindingList<Prescription>();
            searchPrescriptionList = new BindingList<Prescription>(_prescriptionController.SearchPrescription(searchParam).ToList());
            foreach(Prescription prescription in searchPrescriptionList)
            {
                string medicinesShow = "";
                prescription.DoctorId = GetDoctorNameById(prescription.DoctorId);
                foreach (MedicineAmount medicine in prescription.Medicines)
                {
                    medicinesShow += medicine.Medicine.Name.ToString() + " - " + medicine.Amount.ToString() + "\n";
                }
                prescription.DateTime = medicinesShow;
            }
            PrescriptionTable.ItemsSource = searchPrescriptionList;
        }
        #endregion

        #region ADD SINGLE MEDICINE TO CART
        private void Button_Click_AddMedicineToCart(object sender, RoutedEventArgs e)
        {
            if (IsAmountSelected() && IsMedicineSelected())
            {
                if (IsMedicinePrescribed())
                {
                    ShowError_MedicineIsPrescribed();
                } else
                {
                    AddMedicineToCart(sender);
                    ResetMedicineList();
                }
            }
            else if (!IsMedicineSelected())
            {
                ShowError_MedicineNotSeleted();
            }
            else
            {
                ShowError_AmountNotSelected();
            }

        }
        private bool IsAmountSelected()
        {
            if (cb_Amount.SelectedItem == null)
                return false;
            return true;
        }
        private bool IsMedicineSelected()
        {
            if ((Medicine)MedicinesTable.SelectedItem == null)
                return false;
            return true;
        }
        private bool IsMedicinePrescribed()
        {
            Medicine medicineToCheck = new Medicine();
            medicineToCheck = (Medicine)MedicinesTable.SelectedItem;
            if (medicineToCheck.Prescribed) 
                return true;
            return false;
        }
        private void AddMedicineToCart(object sender) 
        {
            MedicineAmount newItem = new MedicineAmount();
            newItem.Amount = (int)cb_Amount.SelectedItem;
            newItem.Medicine = (Medicine)MedicinesTable.SelectedItem;
            medicinesInCartList.Add(newItem);
            CartTable.ItemsSource = medicinesInCartList;

            CalculateTotalPrice();
        }
        private void ResetMedicineList()
        {
            MedicinesTable.ItemsSource = null;
        }
        #endregion
        
        #region ADD PRESCRIPTION TO CART
        private void Button_Click_AddPrescriptionToCart(object sender, RoutedEventArgs e)
        {
            if (IsPrescriptionSelected())
            {
                AddPrescriptionToCart(sender);
                ResetPrescriptionList();
            }
            else ShowError_PrescriptionNotSeleted();
        }
        private bool IsPrescriptionSelected()
        {
            if ((Prescription)PrescriptionTable.SelectedItem == null)
                return false;
            return true;
        }
        private void AddPrescriptionToCart(object sender) 
        {
            Prescription prescription = (Prescription)PrescriptionTable.SelectedItem;
            foreach ( MedicineAmount medicineAmount in prescription.Medicines)
            {
               medicinesInCartList.Add(medicineAmount);
               CartTable.ItemsSource = medicinesInCartList;
            }
            CalculateTotalPrice();
        }
        private void ResetPrescriptionList()
        {
            PrescriptionTable.ItemsSource = null;
        }
        #endregion

        private void Button_Click_RemoveMedicineFromCart(object sender, RoutedEventArgs e)
        {

            medicinesInCartList.Remove((MedicineAmount)CartTable.SelectedItem);
            CartTable.ItemsSource = medicinesInCartList;
            CalculateTotalPrice();
        }      
        private void Button_Click_CheckOut(object sender, RoutedEventArgs e)
        {
            var newReceipt = new Receipt();
            newReceipt.PharmacistId = userId;
            newReceipt.DateTime = new DateTime().ToString();
            newReceipt.Medicines = new List<MedicineAmount>();
            foreach (MedicineAmount medicineAmount in medicinesInCartList)
            {
                newReceipt.Medicines.Add(medicineAmount);
            }
            _receiptController.Create(newReceipt);
            Close();
        }   
        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Cancel New Shopping", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                MainWindow window = new MainWindow();
                window.Show();
                Close();
            }
        }
        private void CalculateTotalPrice()
        {
            float.Parse(totalPrice.Text.Replace("rsd",""));
            float totalPriceTemp = 0;
            foreach(MedicineAmount medicineAmount in medicinesInCartList)
            {
                totalPriceTemp += (medicineAmount.Amount * medicineAmount.Medicine.Price);
            }
            totalPrice.Text = totalPriceTemp.ToString() + " rsd";
        }

        #region ERROR MESSAGES
        private void ShowError_PatientJMBGAndDateNotFilled() => new ErrorMessageWindow(ERROR_MESSAGE_NOTFILLED, this).ShowDialog();
        private void ShowError_AmountNotSelected() => new ErrorMessageWindow(ERROR_MESSAGE_AMOUNT, this).ShowDialog();
        private void ShowError_MedicineNotSeleted() => new ErrorMessageWindow(ERROR_MESSAGE_MedicineNotSelected, this).ShowDialog();
        private void ShowError_MedicineIsPrescribed() => new ErrorMessageWindow(ERROR_MESSAGE_MedicineIsPrescribed, this).ShowDialog();
        private void ShowError_MedicinesNotAdded() => new ErrorMessageWindow(ERROR_MESSAGE_MedicineNotAdded, this).ShowDialog();
        private void ShowError_PrescriptionNotSeleted() => new ErrorMessageWindow(ERROR_MESSAGE_PrescriptionNotSelected, this).ShowDialog();
        #endregion
    }
}
