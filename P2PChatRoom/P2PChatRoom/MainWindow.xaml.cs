﻿using System;
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
        NetworkManager networkManager;
        StackPanel msgSP;
        string deviceName
        {
            get
            {
                return yourDeviceName.Text.PadRight(10);
            }
        }

        static string currentlyViewingContact = "";

        // Constructor for MainWindow
        public MainWindow()
        {
            InitializeComponent();

            this.ResizeMode = ResizeMode.CanMinimize;
            networkManager = new NetworkManager(this);
            msgSP = messageStackPanel;

            // Finds icon according to current directory and binds it to the window icon
            Uri icon = new Uri($"{path}\\images\\P2P.ico", UriKind.Absolute);
            ImageSource iconSource = new BitmapImage(icon);
            this.Icon = iconSource;


        } 

        public void showMessage(string deviceName, string messageReceived)
        {
            TextBox currentMessage = new TextBox();
            currentMessage.Text = $"{deviceName}:\n{messageReceived}";
            messageStackPanel.Children.Add(currentMessage);
        }

        public void addButton(StackPanel sp, string content)
        {
            Button btn = new Button();
            btn.Content = content;
            btn.Click += DMButtonClick;
            sp.Children.Add(btn);
        }

        private void addConnection_Click(object sender, RoutedEventArgs e)
        {
            NewConnection newConPopup = new NewConnection();
            newConPopup.ShowDialog();

            if (newConPopup.DialogResult == true)
            {
                networkManager.AddDirectMessage(new DirectMessage(this, newConPopup.deviceName.Text.PadRight(10), newConPopup.IPAddress.Text));
                addButton(devices, newConPopup.deviceName.Text);
            }
        }

        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            string msg = inputMessage.Text;
            Console.WriteLine($"sending a message: {yourDeviceName.Text};");
            networkManager.SendMessageOutward(currentlyViewingContact.PadRight(10), yourDeviceName.Text, msg);

            //showMessage(deviceName, inputMessage.Text);
            //networkManager.SendMessage("Bob's PC", deviceName, inputMessage.Text);
            //inputMessage.Text = "";
        }

        private void DMButtonClick(object sender, RoutedEventArgs e)
        {
            currentlyViewingContact = (sender as Button).Content.ToString();
        }
    }
}
