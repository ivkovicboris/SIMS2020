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
    public partial class NewPrescriptionWindow : Window
    {
        private const int MAXIMUM_MEDICINE = 50;
        private const string ERROR_MESSAGE_NOTFILLED = "Please fill in Patinet JMBG and Date of the Presription!" ;
        private const string ERROR_MESSAGE_AMOUNT = "Please select amount of the medication!";
        private const string ERROR_MESSAGE_MedicineNotAdded = "No medicine added to the prescription";
        private const string ERROR_MESSAGE_MedicineNotSelected = "Please select Medicine first!";
        private  List<int> AVAILABLE_AMOUNT_OF_MEDICATION = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        private static ObservableCollection<Medicine> medicineList { get; set; }
        private static BindingList<MedicineAmount> medicinesInPrescriptionList { get; set; }

        private PrescriptionController _prescriptionController;
        private MedicineController _medicineController;

        public UserRole userRole = LogInWindow.userRole;
        public long userId = LogInWindow.userId;

        public NewPrescriptionWindow()
        {
            
            InitializeComponent();
            var app = Application.Current as App;
            _prescriptionController = app.PrescriptionController;
            _medicineController = app.MedicineController;
            cb_Amount.ItemsSource = AVAILABLE_AMOUNT_OF_MEDICATION;
            GetAllMedicines();

            medicinesInPrescriptionList = new BindingList<MedicineAmount>();
        }
        public void GetAllMedicines()
        {
            medicineList = new ObservableCollection<Medicine>(_medicineController.GetAll().ToList());
            if (medicineList.Count() != 0)
                MedicinesTable.ItemsSource = HideDeletedMedicineIfNotAdminLoggedIn(medicineList);
        }
        private void tb_SearchParam_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchParam = tb_SearchParam.Text.Trim();
            BindingList<Medicine> searchUsersList = new BindingList<Medicine>();
            searchUsersList = new BindingList<Medicine>(_medicineController.SearchMedicine(searchParam).ToList());
            MedicinesTable.ItemsSource = HideDeletedMedicineIfNotAdminLoggedIn(searchUsersList);
        }
        private IEnumerable<Medicine> HideDeletedMedicineIfNotAdminLoggedIn(IEnumerable<Medicine> medicineList)
        {
            if (!userRole.Equals(UserRole.Admin))
            {
                return medicineList.Where(medicine => medicine.Deleted.Equals(false)).ToList();
            }
            else return medicineList;         
        }

        #region ADD MEDICINE TO PRESCRIPTION
        private void Button_Click_AddMedicineToPrescription(object sender, RoutedEventArgs e)
        {
            if (IsAmountSelected() && IsMedicineSelected())
            {
                AddMedicineToPrescription(sender);
            } else if  (!IsMedicineSelected())
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
            if(cb_Amount.SelectedItem == null)
                return false;
            return true;
        }
        private bool IsMedicineSelected()
        {
            if ((Medicine)MedicinesTable.SelectedItem == null) 
                return false;
            return true;
        }
        private void AddMedicineToPrescription (object sender)
        {
            MedicineAmount newItem = new MedicineAmount();
            newItem.Amount = (int)cb_Amount.SelectedItem;
            newItem.Medicine = (Medicine)MedicinesTable.SelectedItem;
            medicinesInPrescriptionList.Add(newItem);
            PrescriptionTable.ItemsSource = medicinesInPrescriptionList;
        }
        private void Button_Click_RemoveMedicineFromPrescription(object sender, RoutedEventArgs e)
        {

            medicinesInPrescriptionList.Remove((MedicineAmount)PrescriptionTable.SelectedItem);
            PrescriptionTable.ItemsSource = medicinesInPrescriptionList;
        }
        #endregion
        #region CREATE PRESCRIPTION
        private void Button_Click_CreatePrescriptions(object sender, RoutedEventArgs e)
        {
            if (AreAllFieldsFilled() && AreMedicinesAdded())
            {
                CreatePrescription(sender);
                PrescriptionWindow window = new PrescriptionWindow();
                Close();
                window.Show();
            } else if (!AreMedicinesAdded())
            {
                ShowError_MedicinesNotAdded();
            } else
            {
                ShowError_PatientJMBGAndDateNotFilled();
            }
        }
        private bool AreAllFieldsFilled()
        {
            if (patientJMBG.Text != "" && selectedDate.SelectedDate.ToString() != "")
                return true;
            return false;
        }
        private bool AreMedicinesAdded()
        {
            if (medicinesInPrescriptionList.Count==0) 
                return false;
            return true;
        }
        private void CreatePrescription(object sender)
        {
            var newPrescription = new Prescription();
            newPrescription.PatientJMBG = patientJMBG.Text;
            newPrescription.DoctorId = userId.ToString();
            newPrescription.DateTime = selectedDate.ToString();
            newPrescription.Medicines = new List<MedicineAmount>();
            foreach (MedicineAmount medicineAmount in medicinesInPrescriptionList)
            {
                newPrescription.Medicines.Add(medicineAmount);
            }
            _prescriptionController.Create(newPrescription);
        }
        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion
        #region ERROR MESSAGES
        private void ShowError_PatientJMBGAndDateNotFilled() => new ErrorMessageWindow(ERROR_MESSAGE_NOTFILLED, this).ShowDialog();
        private void ShowError_AmountNotSelected() => new ErrorMessageWindow(ERROR_MESSAGE_AMOUNT, this).ShowDialog();
        private void ShowError_MedicineNotSeleted() => new ErrorMessageWindow(ERROR_MESSAGE_MedicineNotSelected, this).ShowDialog();
        private void ShowError_MedicinesNotAdded() => new ErrorMessageWindow(ERROR_MESSAGE_MedicineNotAdded, this).ShowDialog();
        #endregion
    }
}
