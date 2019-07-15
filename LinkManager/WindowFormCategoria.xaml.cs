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
    /// Interaction logic for WindowFormCategoria.xaml
    /// </summary>
    public partial class WindowFormCategoria : Window
    {
        CategorieService _service;
        Categoria _categoria;
        public WindowFormCategoria()
        {
            InitializeComponent();
            _service = new CategorieService();

            _categoria = new Categoria();
        }

        public WindowFormCategoria(Categoria c)
        {
            InitializeComponent();
            _service = new CategorieService();

            _categoria = c;
        }

        private void BtnSalva_Click(object sender, RoutedEventArgs e)
        {
            _categoria.Nome = txtNome.Text;
            _categoria.Descrizione = txtDescrizione.Text;

            Categoria res = null;
            //update
            if(_categoria.IdCategoria > 0)
            {
                _service.Edit(_categoria);
                MessageBox.Show("Categoria modificata", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                _service.Add(_categoria);
                MessageBox.Show("Categoria inserita", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            this.DialogResult = true;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //assegnazione valori categoria al form
            txtNome.Text = _categoria.Nome;
            txtDescrizione.Text = _categoria.Descrizione;
        }

        private void BtnAnnulla_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
