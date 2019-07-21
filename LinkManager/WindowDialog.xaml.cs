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
using System.Xml;
using Microsoft.Win32;

namespace LinkManager
{
    /// <summary>
    /// Interaction logic for WindowDialog.xaml
    /// </summary>
    public partial class WindowDialog : Window
    {
        ConfigManager cfg;

        public WindowDialog()
        {
            InitializeComponent();
            cfg = new ConfigManager();

            txtPath.Text = cfg.GetKey("FileName");
        }

        // https://www.c-sharpcorner.com/uploadfile/dpatra/access-values-from-app-config-in-wpf/
        // https://www.google.com/search?q=c%23+wpf+configuration+file&oq=c%23+wpf+configuration+file&aqs=chrome..69i57j69i58j0l3.9886j0j4&sourceid=chrome&ie=UTF-8


        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                txtPath.Text = openFileDialog.FileName;
            }
        }


        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (txtPath.Text != cfg.GetKey("FileName"))
            {
                cfg.SetKey("FileName", txtPath.Text);
                MessageBox.Show("nuova config salvata");
                this.DialogResult = true;
            }
            else
            {
                this.DialogResult = false;
            }

            
        }
    }
}
