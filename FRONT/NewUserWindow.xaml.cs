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
using SIMS2020.Controller;
using SIMS2020.Model;

namespace SIMS2020.FRONT
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class NewUserWindow : Window
    {
        private const string ERROR_MESSAGE_NOTFILLED = "All fields are mandatory, please fill them!";
        private const string ERROR_MESSAGE_NOTUNIQUE = "There is already user with the same username!";

        private readonly UserController _userController;

        public NewUserWindow()
        {
            InitializeComponent();
            var app = Application.Current as App;
            _userController = app.UserController;

            cb_userRole.ItemsSource = Enum.GetNames(typeof(UserRole)).Where(role => !role.Equals("Admin")).ToList();
        }
        #region REGISTER USER
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (AreAllFieldsFilled() && CheckIfUserNameUnique())
            {
                RegisterUser(sender);
                MainWindow window = new MainWindow();
                Close();
                window.Show();
            } else if (CheckIfUserNameUnique())
                {
                    ShowError_NotUnique();
                }
            else 
            {
            ShowError_NotFilled();
            }
            
           
        }
        private bool AreAllFieldsFilled()
        {
            if( tb_firstName!=null &&
                tb_lastName != null &&
                tb_userName != null &&
                tb_password != null &&
                cb_userRole.SelectedItem != null)
            {
                return true;
            } return false;
        }
        private bool CheckIfUserNameUnique()
        {
            string userName = tb_userName.Text;
           if (_userController.CheckIfUserNameUnique(userName))
                return true;
            return false;
        }       
        private User RegisterUser(object sender)
        {
            var newUser = new User(
                    tb_firstName.Text,
                    tb_lastName.Text,
                    tb_userName.Text,
                    tb_password.Text,
                    (UserRole)Enum.Parse(typeof(UserRole), cb_userRole.SelectedItem.ToString())
                );
            return _userController.Create(newUser);

            
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
        #endregion
        #region ERROR MESSAGES
        private void ShowError_NotFilled() => new ErrorMessageWindow(ERROR_MESSAGE_NOTFILLED, this).ShowDialog();
        private void ShowError_NotUnique() => new ErrorMessageWindow(ERROR_MESSAGE_NOTUNIQUE, this).ShowDialog();
        #endregion
    }
}
