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
    public partial class MainWindow : Window, ChatHandler
    {
        string path = Directory.GetCurrentDirectory();
        public MainWindow()
        {
            // Constructor for MainWindow
            InitializeComponent();

            Uri icon = new Uri($"{path}\\images\\P2P.ico", UriKind.Absolute);
            ImageSource iconSource = new BitmapImage(icon);
            this.Icon = iconSource;

            NetworkManager networkManager = new NetworkManager(this);
            networkManager.AddMessage("OWN DEVICE", "TEST MESSAGE 101");

            
            // Placeholder
            showMessage("Telegraph System, California Outpost", "IMPORTANT ANNOUCEMENT 25TH OCTOBER 1881\n---------------------------------------\n\nThe Imperial Empire of Japan has attached our precious naval islands with their navy.\n\nCongress has approved $100 million in military funding to defeat this upcoming enemy.");
        } 

        public void showMessage(string deviceName, string messageReceived)
        {
            this.deviceName.Text = deviceName;
            this.messageDisplayed.Text = messageReceived;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized) ShowInTaskbar = false;
            else ShowInTaskbar = true;
        }
    }
}
