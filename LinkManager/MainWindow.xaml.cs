using LinkManager.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Microsoft.VisualBasic;
using Microsoft.Win32;

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


        void LoadCategorie(List<Categoria> list)
        {
            //https://stackoverflow.com/questions/27348796/wpf-adding-an-object-to-listbox-with-existing-itemssource
            lbxCategorie.ItemsSource = null;
            dgLinks.ItemsSource = null;

            var source = new ObservableCollection<Categoria>(list);
            lbxCategorie.ItemsSource = source;
            //source.Add(new Categoria() { Nome = "*Senza categoria", IdCategoria = -1 });
            //source.Add(new Categoria() { Nome = "*Tutti", IdCategoria=0 });

            //(new Categoria() { Nome = "prova" });
            lbxCategorie.DisplayMemberPath = "Nome";
        }


        void LoadLinks(List<Link> source)
        {
            dgLinks.ItemsSource = null;
            dgLinks.ItemsSource = source;
        }

        /// <summary>
        /// ottiene l'oggetto categoria selezionato dalla listbox
        /// </summary>
        Categoria GetSelectedCategoria()
        {
            if (lbxCategorie.SelectedIndex == -1)
                return null;
            else
                return (Categoria)lbxCategorie.SelectedItem;
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
            LoadCategorie(categoriaService.GetAll());
            this.Title = "Gestione link - " + new ConfigManager().GetKey("FileName");
        }


        #region categorie

        private void LbxCategorie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Categoria c = GetSelectedCategoria();
            if(c != null)
                LoadLinks(c.Links.ToList());
        }


        private void MiCategorieInserisci_Click(object sender, RoutedEventArgs e)
        {
            bool result = new WindowFormCategoria().ShowDialog().Value;

            if (result)
                LoadCategorie(categoriaService.GetAll());
        }


        private void MiCategorieModifica_Click(object sender, RoutedEventArgs e)
        {
            if (lbxCategorie.SelectedIndex == -1)
                return;

            Categoria l = (Categoria)lbxCategorie.SelectedItem;
            bool result = new WindowFormCategoria(l).ShowDialog().Value;

            if (result)
                LoadCategorie(categoriaService.GetAll());
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
                LoadCategorie(categoriaService.GetAll());
                //LoadLinks();
            }
        }

        #endregion


        #region link

        private void MiLinkInserisci_Click(object sender, RoutedEventArgs e)
        {
            new WindowFormLink().ShowDialog();
            LoadCategorie(categoriaService.GetAll());
        }


        private void MiLinkModifica_Click(object sender, RoutedEventArgs e)
        {
            if (dgLinks.SelectedIndex == -1)
                return;

            Link l = (Link)dgLinks.SelectedItem;
            new WindowFormLink(l).ShowDialog();

            LoadCategorie(categoriaService.GetAll());
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
                LoadCategorie(categoriaService.GetAll());
            }
        }

        private void DgLinks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgLinks.SelectedIndex == -1)
                return;

            Link l = (Link)dgLinks.SelectedItem;
            new WindowFormLink(l).ShowDialog();
        }






        #endregion

        private void miCategorieCerca_Click(object sender, RoutedEventArgs e)
        {
            string pattern = Interaction.InputBox("cerca");
            LoadCategorie(categoriaService.Search(pattern));
        }

        private void dgLinksUrl_Click(object sender, RoutedEventArgs e)
        {
            //https://stackoverflow.com/questions/5764951/using-wpf-datagridhyperlinkcolumn-items-to-open-windows-explorer-and-open-files
            Hyperlink link = (Hyperlink)e.OriginalSource;
            Process.Start(link.NavigateUri.AbsoluteUri);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void miFileApri_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                ConfigManager cfg = new ConfigManager();
                cfg.SetKey("FileName", ofd.FileName);

                categoriaService = new CategorieService();
                linksService = new LinksService();

                LoadCategorie(categoriaService.GetAll());
                this.Title = "Gestione link - " + ofd.FileName;
            }
        }
    }
}
