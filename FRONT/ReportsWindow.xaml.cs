using SIMS2020.Controller;
using SIMS2020.Model;
using SIMS2020.Model.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ReportsWindow.xaml
    /// </summary>
    public partial class ReportsWindow : Window
    {

        private static ObservableCollection<MedicineAmount> medicineList { get; set; }
        //public List<User> pharmacistsComboBox { get; private set; }

        private readonly UserController _userController;
        private readonly MedicineController _medicineController;
        private readonly ReceiptController _receiptController;
        public ReportsWindow()
        {
            InitializeComponent();
            var app = Application.Current as App;
            _userController = app.UserController;
            _medicineController = app.MedicineController;
            _receiptController = app.ReceiptController;

            pharmacistsComboBox.ItemsSource = GetAllPharmacists();
            manufacturersComboBox.ItemsSource = GetAllManufacturers().Distinct();
        }

        private List<User> GetAllPharmacists()
        {
            List<User> result = new List<User>();
           foreach(User pharmacist in  _userController.GetAll().Where(user => user.UserRole.Equals(UserRole.Pharmacist)).ToList()){
                pharmacist.FirstName = (pharmacist.FirstName + " " + pharmacist.LastName);
                result.Add(pharmacist);
            }
            return result;
        }

        private List<string> GetAllManufacturers()
        {
            List<string> result = new List<string>();
            foreach (Medicine medicine in _medicineController.GetAll())
            {
                result.Add(medicine.Manufacturer);
            }
            return result;
        }
        private void FilterByManufacturer(object sender, SelectionChangedEventArgs e)
        {   
            MedicinesTable.ItemsSource = _receiptController.GetAllSoldMedicineByManufacturer(manufacturersComboBox.SelectedItem.ToString());
        }
        private void FilterByPharmaist(object sender, SelectionChangedEventArgs e)
        {
            User pharmacist = _userController.Get((long)pharmacistsComboBox.SelectedValue);
            MedicinesTable.ItemsSource = _receiptController.GetAllSoldMedicineByPharmacist(pharmacist.Id);
        }
        private void Button_Click_AllSoldMedicine(object sender, RoutedEventArgs e)
        {
            MedicinesTable.ItemsSource = _receiptController.GetAllSoldMedicines();
        }
        private void Button_Click_Home(object sender, RoutedEventArgs e)
        {

            MainWindow window = new MainWindow();
            window.Show();
            Close();
        }

        
    }
}
