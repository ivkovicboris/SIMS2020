using SIMS2020.Controller;

using SIMS2020.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
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
    public partial class UsersWindow : Window
    {

        public static ObservableCollection<User> userList { get; set; }
        public static ObservableCollection<User> searchUsersList { get; set; }
        private readonly UserController _userController;
        // private List<User> allUsers;
        public UserRole userRole;
        public UsersWindow()
        {
            
            InitializeComponent();

            var app = Application.Current as App;
            _userController = app.UserController;
            userList = new ObservableCollection<User>(_userController.GetAll().ToList());
            UsersTable.ItemsSource = userList;
                //new ObservableCollection<User>(UserConverter.ConvertUserListToStringList(_userController.GetAll().ToList()));
        }

        private void Button_Click_NewUser(object sender, RoutedEventArgs e)
        {
            NewUserWindow window = new NewUserWindow();
            window.Show();
        }

        private void tb_SearchParam_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchParam = tb_SearchParam.Text.Trim();
            BindingList<User> searchUsersList = new BindingList<User>();
            if (searchParam.Equals(""))
            {
               searchUsersList = new BindingList<User>(_userController.GetAll().ToList());
            } else
            {
               searchUsersList = new BindingList<User>(_userController.SearchUsers(searchParam).ToList());
            }
            UsersTable.ItemsSource = searchUsersList;
        }

        private void Button_Click_Home(object sender, RoutedEventArgs e)
        {

            MainWindow window = new MainWindow();
            window.Show();
            Close();
        }


    }
}
