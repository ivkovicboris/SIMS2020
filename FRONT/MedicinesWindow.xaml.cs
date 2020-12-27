using SIMS2020.Controller;
using SIMS2020.Model;
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
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class MedicinesWindow : Window

    {

        public UserRole userRole = LogInWindow.userRole;
        private const string ERROR_MESSAGE_MINMAX = "Maximum value cannot be lower than minimum!";
        private const string ERROR_MESSAGE_MedicineNotSelected = "Please select medicine first!";
        private static ObservableCollection<Medicine> medicineList { get; set; }
        private static ObservableCollection<Medicine> searchMedicineList { get; set; }
        private MedicineController _medicineController;
        //private float _maxPrice;

        public MedicinesWindow()
        {
            InitializeComponent();
            var app = Application.Current as App;
            _medicineController = app.MedicineController;
            medicineList = new ObservableCollection<Medicine>(_medicineController.GetAll().ToList());
            if (medicineList.Count() != 0)
                MedicinesTable.ItemsSource = HideDeletedMedicineIfNotAdminLoggedIn(medicineList); 
            CheckVisibility();
        }
        private void CheckVisibility()
        {
            isVisibleNew.Visibility = Visibility.Hidden;
            isVisibleEdit.Visibility = Visibility.Hidden;
            isVisibleDelete.Visibility = Visibility.Hidden;
            if (userRole.Equals(UserRole.Admin) || userRole.Equals(UserRole.Pharmacist))
            {
                isVisibleNew.Visibility = Visibility.Visible;
                isVisibleEdit.Visibility = Visibility.Visible;
                isVisibleDelete.Visibility = Visibility.Visible;
            }
        }
        private IEnumerable<Medicine> HideDeletedMedicineIfNotAdminLoggedIn(ObservableCollection<Medicine> medicineList)
        {
            if (!userRole.Equals(UserRole.Admin))
            {
                return medicineList.Where(medicine => medicine.Deleted.Equals(false)).ToList();
            } else
            {
                return medicineList;
            }            
        }
       
        #region SEARCH MEDICINE
        private void tb_SearchParam_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchParam = tb_SearchParam.Text.Trim();
            BindingList<Medicine> searchUsersList = new BindingList<Medicine>();
            if (searchParam.Equals(""))
            {
                searchUsersList = new BindingList<Medicine>(_medicineController.GetAll().ToList());
            }
            else
            {
                searchUsersList = new BindingList<Medicine>(_medicineController.SearchMedicine(searchParam).ToList());
            }
            MedicinesTable.ItemsSource = searchUsersList;
        }
        private void tb_SearchRange_TextChanged(object sender, TextChangedEventArgs e)
        {           
            float.TryParse(tb_SearchRangeMin.Text, out float minPrice);
            float.TryParse(tb_SearchRangeMax.Text, out float maxPrice);
            if (maxPrice == 0)
            {
                maxPrice = _medicineController.GetMaxPrice();
            }
            else
            {   //IMPLEMENT DELAY
                //if (maxPrice < minPrice)
                //{
                //    ShowError_MinMax();
                //    return;
                //}
            }
            MedicinesTable.ItemsSource = _medicineController.SearchByRange(minPrice, maxPrice);
        }
        #endregion
        private void Button_Click_NewMedicine(object sender, RoutedEventArgs e)
        {
            NewMedicineWindow window = new NewMedicineWindow();
            window.ShowDialog();
        }
        private void Button_Click_Home(object sender, RoutedEventArgs e)
        {
            
            MainWindow window = new MainWindow();
            window.Show();
            Close();
        }
        private void Button_Click_DeleteMedicine(object sender, RoutedEventArgs e)
        {
            if (IsMedicineSelected())
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    Medicine medicine = (Medicine)MedicinesTable.SelectedItem;
                    _medicineController.SoftDelete(medicine.Code);
                }              
            } else
            {
                ShowError_MedicineNotSelected();
            }
        }
        private void Button_Click_UpdateMedicine(object sender, RoutedEventArgs e)
        {
            if (IsMedicineSelected())
            {
                Medicine medicine = (Medicine)MedicinesTable.SelectedItem;
                UpdateMedicineWindow window = new UpdateMedicineWindow(medicine);
                window.Show();
                MedicinesTable.Items.Refresh();
            } else
            {
                ShowError_MedicineNotSelected();
            }        
        }
        private bool IsMedicineSelected()
        {
            if ((Medicine)MedicinesTable.SelectedItem == null)
                return false;
            return true;
        }

        #region ERROR MESSAGES
        private void ShowError_MinMax() => new ErrorMessageWindow(ERROR_MESSAGE_MINMAX, this).ShowDialog();
        private void ShowError_MedicineNotSelected() => new ErrorMessageWindow(ERROR_MESSAGE_MedicineNotSelected, this).ShowDialog();

        #endregion

    }
}
