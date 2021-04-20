using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ShoppingApp_DS_VK_JMK_SNA.Entities;
using ShoppingApp_DS_VK_JMK_SNA.ViewModels;

namespace ShoppingApp_DS_VK_JMK_SNA
{
    /// <summary>
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        public ManagerWindow()
        {
            InitializeComponent();
            DataContext = new ProductViewModel();
        }

       
    }
}
