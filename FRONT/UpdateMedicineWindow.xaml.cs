using SIMS2020.Controller;
using SIMS2020.Model;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for UpdateMedicineWindow.xaml
    /// </summary>
    public partial class UpdateMedicineWindow : Window
    {
        
        private readonly MedicineController _medicineController;
        public bool prescribed;
        private Medicine medicine;
        private string medicineName;
        private string medicineManufacturer;
        private float medicinePrice;

        public UpdateMedicineWindow(Medicine medicine)
        {

            InitializeComponent();
            var app = Application.Current as App;
            _medicineController = app.MedicineController;
            this.medicine = medicine;

            cb_Prescribed.ItemsSource = new List<string>() { "yes", "no" };
        }

        private void Button_Click_SaveMedicine(object sender, RoutedEventArgs e)
        {
            if (ConfirmUpdate())
            {
                UpdateMedicine(sender);
                MedicinesWindow window = new MedicinesWindow();
                Close();            
            }               
        }

        private void UpdateMedicine(object sender)
        {
            float.TryParse(tb_Price.Text, out float price);
            if (cb_Prescribed.SelectedItem == null)
            {
                prescribed = this.medicine.Prescribed;
            } else
            {
                if (cb_Prescribed.SelectedItem.ToString().Equals("YES"))
                {
                    prescribed = true;
                }
                else { prescribed = false; }
            }
            if (tb_Name.Text == "") medicineName = this.medicine.Name; else medicineName = tb_Name.Text;
            if (tb_Manufacturer.Text == "") medicineManufacturer = this.medicine.Manufacturer; else medicineManufacturer = tb_Manufacturer.Text;
            if (price == 0) medicinePrice = this.medicine.Price; else medicinePrice = price;
            
            var newMedicine = new Medicine(
                this.medicine.Id,
                this.medicine.Code,
                medicineName,
                medicineManufacturer,
                prescribed,
                medicinePrice,
                this.medicine.Deleted
                );
            _medicineController.Update(newMedicine);
        }
        
        private bool ConfirmUpdate()
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Update Medicine", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.No)
            {
                Close();
                return false;
            }
            Close();
            return true;
        }

         private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Cancel New Shopping", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Close();
            }
        }
    }
}

