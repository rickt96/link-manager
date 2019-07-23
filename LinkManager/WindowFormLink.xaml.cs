using LinkManager.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace LinkManager
{
    /// <summary>
    /// Form di inserimento e modifica di un link
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


        void CheckSave()
        {
            bool textOk, categoryOk = false;

            if (Uri.IsWellFormedUriString(txtUrl.Text, UriKind.Absolute))
            {
                //txtUrl.Background = Brushes.LightGreen;
                textOk = true;
            }
            else
            {
                //txtUrl.Background = Brushes.Orange;
                textOk = false;
            }

            if (cmbCategoria.SelectedIndex == -1)
                categoryOk = false;
            else
                categoryOk = true;

            if (categoryOk == true && textOk == true)
                btnSalva.IsEnabled = true;
            else
                btnSalva.IsEnabled = false;
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
                //MessageBox.Show("Link modificato", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                linkService.Add(_link);
                //MessageBox.Show("Link aggiunto", "", MessageBoxButton.OK, MessageBoxImage.Information);
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
            txtTitolo.Text = (_link.IdLink > 0) ? _link.Titolo : "";
            txtDescrizione.Text = (_link.IdLink > 0) ? _link.Descrizione : "";
            txtUrl.Text = (_link.IdLink > 0) ? _link.URL : "";
            foreach (Categoria c in cmbCategoria.Items)
                if (c.IdCategoria == _link.IdCategoria)
                    cmbCategoria.SelectedItem = c;

            //preload clipboard
            if (_link.IdLink == 0 && Uri.IsWellFormedUriString(Clipboard.GetText(), UriKind.RelativeOrAbsolute))
            {
                txtUrl.Text = Clipboard.GetText();
            }
        }


        private void TxtUrl_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.IsLoaded == false)
                return;

            CheckSave();
        }

        private void cmbCategoria_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckSave();
        }
    }
}
