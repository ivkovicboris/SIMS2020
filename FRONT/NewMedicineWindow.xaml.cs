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
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class NewMedicineWindow : Window
    {

        private const string ERROR_MESSAGE_NOTFILLED = "All fields are mandatory, please fill them!";
        private const string ERROR_MESSAGE_NOTUNIQUE = "There is already medcine with the same ID!";

        private readonly MedicineController _medicineController;
        public bool prescribed;
        public NewMedicineWindow()
        {
            
            InitializeComponent();
            var app = Application.Current as App;
            _medicineController = app.MedicineController;

            cb_Prescribed.ItemsSource = new List<string>() { "yes", "no" };
        }

        private void Button_Click_SaveMedicine(object sender, RoutedEventArgs e)
        {
            if (AreAllFieldsFilled() && CheckIfCodeUnique())
            {
                AddMedicine(sender);
                MainWindow window = new MainWindow();
                Close();
                window.Show();
            }
            else if (CheckIfCodeUnique())
            {
                ShowError_NotUnique();
            }
            else
            {
                ShowError_NotFilled();
            }

        }

        private Medicine AddMedicine(object sender)
        {
            float.TryParse(tb_Price.Text, out float price);
            
            if (cb_Prescribed.SelectedItem.ToString().Equals("YES"))
            {
                prescribed = true;
            } else { prescribed = false; }
            var newMedicine = new Medicine(
                tb_Code.Text  ,  
                tb_Name.Text,
                tb_Manufacturer.Text,
                prescribed, 
                price,
                false
                );
            return _medicineController.Create(newMedicine);
        }
        private bool AreAllFieldsFilled()
        {
            if (tb_Code != null &&
                tb_Name != null &&
                tb_Manufacturer != null &&
                tb_Price != null &&
                cb_Prescribed.SelectedItem != null )
            {
                    return true;
            }
            return false;
        }
        private bool CheckIfCodeUnique()
        {
            string code = tb_Code.Text;
            if (_medicineController.CheckIfCodeNameUnique(code))
                return true;
            return false;
        }

        private void CloseDialog() => Close();
        private void ShowError_NotFilled() => new ErrorMessageWindow(ERROR_MESSAGE_NOTFILLED, this).ShowDialog();
        private void ShowError_NotUnique() => new ErrorMessageWindow(ERROR_MESSAGE_NOTUNIQUE, this).ShowDialog();
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
    }
}
