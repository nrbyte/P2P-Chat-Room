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
using System.IO;

namespace P2PChatRoom
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string path = Directory.GetCurrentDirectory();
        public MainWindow()
        {
            InitializeComponent();
            Uri icon = new Uri($"{path}\\images\\P2P.ico", UriKind.Absolute);
            ImageSource iconSource = new BitmapImage(icon);
            this.Icon = iconSource;
            MainGrid.MouseUp += new MouseButtonEventHandler(MainGrid_MouseUp);
            MainGrid.MouseDown += new MouseButtonEventHandler(MainGrid_MouseDown);
        }

        private void MainGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("You clicked at " + e.GetPosition(this).ToString(), "Position Alert!");
        }

        private void MainGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine();
        }
    }
}
