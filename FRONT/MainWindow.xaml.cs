using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SIMS2020.Model;

namespace SIMS2020.FRONT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window

    {
        
        public static MainWindow ins;
        public MainWindow()
        { 
            InitializeComponent();
            CheckVisibility();  
        }
        public static MainWindow instanca
        {
            get { return ins; }
        }
        private void Button_Click_MedicinesWindow(object sender, RoutedEventArgs e)
        {
            MedicinesWindow window = new MedicinesWindow();
            Close();
            window.ShowDialog();
            
        }
        private void Button_Click_PrescriptionsWindow(object sender, RoutedEventArgs e)
        {
            PrescriptionWindow window = new PrescriptionWindow();
            Close();
            window.ShowDialog();
            
        }
        private void Button_Click_ReportWindow(object sender, RoutedEventArgs e)
        {
            ReportsWindow window = new ReportsWindow();
            Close();
            window.ShowDialog();
            
        }
        private void Button_Click_CartWindow(object sender, RoutedEventArgs e)
        {
            
            CartWindow window = new CartWindow();
            Close();
            window.ShowDialog();
            
        }
        private void Button_Click_UsersWindow(object sender, RoutedEventArgs e)
        {
            
            UsersWindow window = new UsersWindow();
            Close();
            window.ShowDialog();
            
        }
        private void Button_Click_LogOut(object sender, RoutedEventArgs e)
        {
            LogInWindow window = new LogInWindow();
            Close();
            window.Show();
        }
        private void CheckVisibility()
        {
            UserRole userRole = LogInWindow.userRole;
            isVisibleUsers.Visibility = Visibility.Hidden;
            isVisibleReports.Visibility = Visibility.Hidden;
            isVisibleCart.Visibility = Visibility.Hidden;
            if (userRole.Equals(UserRole.Admin))
            {
                isVisibleUsers.Visibility = Visibility.Visible;
                isVisibleReports.Visibility = Visibility.Visible;
            }
            else if (userRole.Equals(UserRole.Pharmacist))
            {
                isVisibleCart.Visibility = Visibility.Visible;
            }
        }
      
    }
}
