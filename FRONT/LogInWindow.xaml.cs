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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class LogInWindow : Window
    {
        private const string ERROR_MESSAGE = "Invalid username or password!";
        private readonly UserController _userController;
        public static UserRole userRole { get; set; }
        public static long userId { get; set; }
        public LogInWindow()
        {
            InitializeComponent();
            var app = Application.Current as App;
            _userController = app.UserController;
        }

        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            var userLogIn = new User(
                    tb_userName.Text,
                    pb_password.Password
                );

            var user = _userController.LogIn(userLogIn);
            if (user != null)
            {
                userRole = user.UserRole;
                userId = user.Id;
                MainWindow window = new MainWindow();
                CloseDialog();
                window.Show();
                
            } else {
                ShowError_InvalidCreditentials();
            }
            
        }

        private void CloseDialog() => Close();
        private void ShowError_InvalidCreditentials() => new ErrorMessageWindow(ERROR_MESSAGE, this).ShowDialog();
    }
}
