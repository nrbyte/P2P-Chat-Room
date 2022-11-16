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
    public partial class MainWindow : Window, ChatHandler, NewDMHandler
    {
        string path = Directory.GetCurrentDirectory();
        NetworkManager networkManager;
        DockPanel msgDP;
        Button? currentButton;
        string yourDeviceName;

        static string currentContact = "";

        // Constructor for MainWindow
        public MainWindow()
        {
            InitializeComponent();

            this.ResizeMode = ResizeMode.CanMinimize;
            networkManager = new NetworkManager(this, this);
            msgDP = messageDockPanel;

            // Finds icon according to current directory and binds it to the window icon
            Uri icon = new Uri($"{path}\\images\\P2P.ico", UriKind.Absolute);
            ImageSource iconSource = new BitmapImage(icon);
            this.Icon = iconSource;

            // yourDeviceName and selfContact Button initialisation.
            yourDeviceName = this.FindResource("username").ToString();
            DirectMessage contact = new DirectMessage(this, yourDeviceName, networkManager.cs.GetLocalIPAddress());
            networkManager.AddDirectMessage(contact);
            currentContact = yourDeviceName;
            currentButton = selfContact;
        } 

        // Displays given message with deviceName on the stackPanel
        public void showMessage(string deviceName, string messageReceived)
        {
            TextBox currentMessage = new TextBox();
            currentMessage.Text = $"{deviceName}:\n{messageReceived}";
            messageDockPanel.Children.Insert(0, currentMessage);
            messageSV.ScrollToBottom();
        }

        // Adds a DM button with the given content to the given stack panel
        public void addDMButton(StackPanel sp, string content)
        {
            Button btn = new Button();
            btn.Content = content;
            btn.Click += DMButtonClick;
            sp.Children.Add(btn);
        }   

        // It shows the dialog, and the inputted IP Address and deviceName are used to make a new DM contact
        private void addConnection_Click(object sender, RoutedEventArgs e)
        {
            NewConnection newConPopup = new NewConnection();
            newConPopup.ShowDialog();

            string IPAddress = newConPopup.IPAddress.Text;
            string contactName = newConPopup.deviceName.Text;

            if (newConPopup.DialogResult == true)
            {
                DirectMessage contact = new DirectMessage(this, contactName, IPAddress);
                networkManager.AddDirectMessage(contact);
                addDMButton(deviceSP, newConPopup.deviceName.Text);
            }
        }

        // Sends message to recipient given, and shows it to sender as well
        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            string msg = inputMessage.Text;
            if (networkManager.SendMessageOutward(currentContact, yourDeviceName, msg))
            {
                showMessage(yourDeviceName, inputMessage.Text);
                inputMessage.Text = "";
            }
        }

        // Changes current contact
        private void DMButtonClick(object sender, RoutedEventArgs e)
        {
            currentButton.Background = Brushes.Gainsboro;
            Button btnClicked = (Button)sender;

            // Ensures that an DM Button with no content can't be selected
            if (string.IsNullOrEmpty( btnClicked.Content.ToString() )) {
                MessageBox.Show("The selected contact isn't labeled!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else {
                currentContact = btnClicked.Content.ToString();
                btnClicked.Background = Brushes.White;
                currentButton = btnClicked;
                messageDockPanel.Children.Clear();
            }
        }

        // Prevents window from being closed with X button. AKA the classic skype fakeout
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
            e.Cancel = true;
        }

        // Changes the username in the App.xaml resources, reassigns yourDeviceName
        private void changeUsername(string newUsername)
        {
            networkManager.directMessages.Where(x => x.contactName == yourDeviceName).First().contactName = newUsername;
            this.Resources["username"] = newUsername;
            yourDeviceName = newUsername;
        }

        // Handles the contextMenu click to change the username
        private void ChangeUsername_Click(object sender, RoutedEventArgs e)
        {
            ChangeUsernameDialog changeDialog = new ChangeUsernameDialog();
            changeDialog.ShowDialog();

            string newUsername = changeDialog.newUsername.Text;
            if (changeDialog.DialogResult == true)
            {
                changeUsername(newUsername);
            }
        }

        public void createNewDM(string senderIP)
        {
            DirectMessage contact = new DirectMessage(this, senderIP, senderIP);
            networkManager.AddDirectMessage(contact);
            addDMButton(deviceSP, senderIP);
        }
    }
}
