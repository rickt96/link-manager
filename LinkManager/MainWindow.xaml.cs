using LinkManager.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using System.IO;

namespace LinkManager
{
    public partial class MainWindow : Window
    {
        CategorieService categoriaService;
        LinksService linksService;

        bool _isDbLoaded = false;

        public MainWindow()
        {
            InitializeComponent(); 
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ConfigManager cfg = new ConfigManager();

            //check database
            if (!File.Exists(cfg.GetKey("FileName")))
            {
                _isDbLoaded = false;
                MessageBox.Show("Impossibile caricare il file " + cfg.GetKey("FileName") + ". Selezionare un altro file");
                ToggleMenu(false);
            }
            else
            {
                _isDbLoaded = true;
                ToggleMenu(true);

                Init();
            }
        }


        void Init()
        {
            // init services
            categoriaService = new CategorieService();
            linksService = new LinksService();

            // load elementi
            LoadCategorie(categoriaService.GetAll());

            //ui
            this.Title = "Gestione link - " + new ConfigManager().GetKey("FileName");
            // TODO
            //txbCount.Text = "";
        }

        void ToggleMenu(bool state=false)
        {
            foreach(MenuItem mi in meMain.Items)
            {
                if (mi.Header.ToString() != "_File")
                {
                    mi.IsEnabled = state;
                }
            }
        }


        /// <summary>
        /// carica la lista di categorie, contenenti i relativi link 
        /// e le mostra nella listbox
        /// </summary>
        void LoadCategorie(List<Categoria> list)
        {
            // https://stackoverflow.com/questions/27348796/wpf-adding-an-object-to-listbox-with-existing-itemssource
            lbxCategorie.ItemsSource = null;
            dgLinks.ItemsSource = null;
            // utilizzando l'observable sono libero di inserire elementi manualmente nella lista senza essere bloccata da ItemsSource
            var source = new ObservableCollection<Categoria>(list);
            lbxCategorie.ItemsSource = source;
            //source.Add(new Categoria() { Nome = "*Senza categoria", IdCategoria = -1 });
            //source.Add(new Categoria() { Nome = "*Tutti", IdCategoria=0 });
            lbxCategorie.DisplayMemberPath = "Nome";
        }


        /// <summary>
        /// carica nel datagrid una lista di link
        /// </summary>
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
            Categoria c = GetSelectedCategoria();
            int idCat = (c != null) ? c.IdCategoria.Value : 0;

            new WindowFormLink(idCat).ShowDialog();
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

        private void miFileApri_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "file sqlite (*.sqlite)|*.sqlite";
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

        private void miFileNuovo_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "file sqlite (*.sqlite)|*.sqlite";
            if (sfd.ShowDialog() == true)
            {
                ConfigManager cfg = new ConfigManager();
                cfg.SetKey("FileName", sfd.FileName);

                categoriaService = new CategorieService();
                linksService = new LinksService();

                LoadCategorie(categoriaService.GetAll());
                this.Title = "Gestione link - " + sfd.FileName;
            }
        }

        private void LbxCategorie_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MiCategorieModifica_Click(sender, e);
        }

        private void MiLinkCerca_Click(object sender, RoutedEventArgs e)
        {
            string pattern = Interaction.InputBox("cerca");
            LoadLinks(linksService.Search(pattern));
        }
    }
}
