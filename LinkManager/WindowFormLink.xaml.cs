using LinkManager.Services;
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

namespace LinkManager
{
    /// <summary>
    /// Interaction logic for WindowFormLink.xaml
    /// </summary>
    public partial class WindowFormLink : Window
    {
        LinksService linkService;
        CategorieService categorieService;
        Link _link;

        public WindowFormLink(int defaultIdCategoria=0)
        {
            InitializeComponent();
            linkService = new LinksService();
            categorieService = new CategorieService();

            _link = new Link();
            _link.IdCategoria = defaultIdCategoria;
        }

        public WindowFormLink(Link l)
        {
            InitializeComponent();
            linkService = new LinksService();
            categorieService = new CategorieService();

            _link = l;
        }


        private void BtnSalva_Click(object sender, RoutedEventArgs e)
        {
            _link.Titolo = txtTitolo.Text;
            _link.Descrizione = txtDescrizione.Text;
            _link.URL = txtUrl.Text;
            _link.IdCategoria = ((Categoria)cmbCategoria.SelectedItem).IdCategoria;

            if(_link.IdLink > 0)
            {
                linkService.Edit(_link);
                MessageBox.Show("Link modificato", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                linkService.Add(_link);
                MessageBox.Show("Link aggiunto", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            this.DialogResult = true;
        }

        private void BtnAnnulla_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //load categorie
            cmbCategoria.ItemsSource = categorieService.GetAll();
            cmbCategoria.DisplayMemberPath = "Nome";

            //caricamento elementi form
            txtTitolo.Text = (_link.IdLink > 0) ? _link.Titolo : "nuovo titolo";
            txtDescrizione.Text = (_link.IdLink > 0) ? _link.Descrizione : "descrizione";
            txtUrl.Text = (_link.IdLink > 0) ? _link.URL : "";
            foreach (Categoria c in cmbCategoria.Items)
                if (c.IdCategoria == _link.IdCategoria)
                    cmbCategoria.SelectedItem = c;


        }

        private void TxtUrl_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.IsLoaded == false)
                return;

            if (Uri.IsWellFormedUriString(txtUrl.Text, UriKind.Absolute))
            {
                txtUrl.Background = Brushes.LightGreen;
                btnSalva.IsEnabled = true;
            }
            else
            {
                txtUrl.Background = Brushes.Orange;
                btnSalva.IsEnabled = false;
            }
        }
    }
}
