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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LinkManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
            lbxCategorie.ItemsSource = null;
            lbxCategorie.ItemsSource = categoriaService.GetAll();
            lbxCategorie.DisplayMemberPath = "Nome";
        }


        void LoadLinks()
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCategorie();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            categoriaService.Add(new Categoria() {
                Nome = "test",
                Descrizione = "test"
            });
        }

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
    }
}
