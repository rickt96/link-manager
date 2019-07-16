using LinkManager.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace LinkManager
{

    public partial class MainWindow : Window
    {
        CategorieService categoriaService;
        LinksService linksService;


        public MainWindow()
        {
            InitializeComponent();
            categoriaService = new CategorieService();
            linksService = new LinksService();
        }


        void LoadCategorie()
        {
            lbxCategorie.SelectedIndex = -1;

            lbxCategorie.ItemsSource = null;
            lbxCategorie.ItemsSource = categoriaService.GetAll();
            lbxCategorie.DisplayMemberPath = "Nome";
        }


        void LoadLinks()
        {

        }

        void OpenUrl(string url)
        {
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                Process.Start(url);
            }
            else
            {
                MessageBox.Show("Url invalido", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCategorie();
        }


        #region categorie

        private void LbxCategorie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxCategorie.SelectedIndex == -1)
                return;

            Categoria c = (Categoria)lbxCategorie.SelectedItem;
            dgLinks.ItemsSource = linksService.GetAllByIdCategoria(c.IdCategoria.Value);

        }


        private void MiCategorieInserisci_Click(object sender, RoutedEventArgs e)
        {
            bool result = new WindowFormCategoria().ShowDialog().Value;

            if (result)
                LoadCategorie();
        }


        private void MiCategorieModifica_Click(object sender, RoutedEventArgs e)
        {
            if (lbxCategorie.SelectedIndex == -1)
                return;

            Categoria l = (Categoria)lbxCategorie.SelectedItem;
            bool result = new WindowFormCategoria(l).ShowDialog().Value;

            if (result)
                LoadCategorie();
        }


        private void MiCategorieElimina_Click(object sender, RoutedEventArgs e)
        {
            if (lbxCategorie.SelectedIndex == -1)
                return;

            Categoria c = (Categoria)lbxCategorie.SelectedItem;

            MessageBoxResult res = MessageBox.Show("Eliminare la categoria " + c.Nome + "?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                categoriaService.Delete(c.IdCategoria.Value);
                LoadLinks();
            }
        }

        #endregion


        #region link

        private void MiLinkInserisci_Click(object sender, RoutedEventArgs e)
        {
            new WindowFormLink().ShowDialog();
        }


        private void MiLinkModifica_Click(object sender, RoutedEventArgs e)
        {
            if (dgLinks.SelectedIndex == -1)
                return;

            Link l = (Link)dgLinks.SelectedItem;
            new WindowFormLink(l).ShowDialog();
        }


        private void MiLinkElimina_Click(object sender, RoutedEventArgs e)
        {
            if (dgLinks.SelectedIndex == -1)
                return;

            Link l = (Link)dgLinks.SelectedItem;
            MessageBoxResult res = MessageBox.Show("Eliminare il link " + l.Titolo + "?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                linksService.Delete(l.IdLink);
                LoadLinks();
            }
        }

        private void DgLinks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgLinks.SelectedIndex == -1)
                return;

            Link l = (Link)dgLinks.SelectedItem;
            OpenUrl(l.URL);
        }

        #endregion






    }
}
