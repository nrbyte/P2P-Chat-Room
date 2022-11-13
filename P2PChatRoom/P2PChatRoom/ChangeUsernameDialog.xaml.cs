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
using System.Windows.Shapes;

namespace P2PChatRoom
{
    /// <summary>
    /// Interaction logic for ChangeUsernameDialog.xaml
    /// </summary>
    public partial class ChangeUsernameDialog : Window
    {
        public ChangeUsernameDialog()
        {
            InitializeComponent();
        }
        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            newUsername.SelectAll();
            newUsername.Focus();
        }
    }
}
