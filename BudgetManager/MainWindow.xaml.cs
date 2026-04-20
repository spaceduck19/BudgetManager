using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CommunityToolkit.Mvvm.DependencyInjection;
using BudgetManager.ViewModels;

namespace BudgetManager
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = Ioc.Default.GetService<MainViewModel>();
        }

        private void CategoryFilterBar_Click(object sender, RoutedEventArgs e)
        {
            // Megnézzük, melyik gomb "dobta fel" az eseményt a StackPanelnek

            if (e.OriginalSource is Button clickedButton)
            {
                string category = clickedButton.Content.ToString() ?? "Összes";

                // Szólunk a ViewModelnek, hogy szűrjön

                if (this.DataContext is MainViewModel vm)
                {
                    vm.FilterByCategory(category);
                }
            }
        }
    }
}