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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ShoppingApp_DS_VK_JMK_SNA.ViewModels;

namespace ShoppingApp_DS_VK_JMK_SNA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private ProductViewModel productViewModel;
        public MainWindow()
        {
            InitializeComponent();
            DataContext =  new ProductViewModel();

        }
    }
}
