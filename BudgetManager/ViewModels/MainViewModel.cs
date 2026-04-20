using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BudgetManager.Models;
using BudgetManager.Services;

namespace BudgetManager.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IEditorService editorService;

        // Listákat ebben tároljuk

        public ObservableCollection<Transaction> AllTransactions { get; } = new();
        public ObservableCollection<Transaction> DisplayedTransactions { get; } = new();


        public ObservableCollection<string> Categories { get; } = new() { "Összes", "Fizetés", "Étel", "Utazás", "Szórakozás", "Általános" };

        // CommunityToolkit.Mvvm Source Generator-a a háttérben legenerálja a publikus SelectedTransaction property-t
        // és az INofityPropertyChanged hívásokat

        [ObservableProperty]
        private Transaction? selectedTransaction;

        [ObservableProperty]
        private int currentBalance;

        // A konstruktor IoC-ből (DI) kapja meg az IEditorService-t, így a ViewModel nem példányosít közvetlenül felületi elemeket
        // MVVM-kompatibilis

        public MainViewModel(IEditorService editorService)
        {
            this.editorService = editorService;

            // Kezdeti adatok
            AddTransaction(new Transaction { Title = "Ösztöndíj", Amount = 80000, Type = TransactionType.Bevétel, Category = "Fizetés" });
            AddTransaction(new Transaction { Title = "BKV Bérlet", Amount = 3450, Type = TransactionType.Kiadás, Category = "Utazás" });
            AddTransaction(new Transaction { Title = "Ebéd", Amount = 2500, Type = TransactionType.Kiadás, Category = "Étel" });

            FilterByCategory("Összes");
        }

        private void AddTransaction(Transaction t)
        {
            AllTransactions.Add(t);
            UpdateCategories(t.Category);
            CalculateBalance();
        }

        public void FilterByCategory(string category)
        {
            DisplayedTransactions.Clear();
            foreach (var t in AllTransactions)
            {
                if (category == "Összes" || t.Category == category)
                    DisplayedTransactions.Add(t);
            }
        }

        // RelayCommand automatikusan ICommand típusú parancsot csinál a metódusból,
        // amit a XAML-ből közvetlenül meg tudunk hívni adatkötéssel

        [RelayCommand]
        private void CreateNew()
        {
            Transaction newTrans = new Transaction();
            if (editorService.EditTransaction(newTrans))
            {
                AddTransaction(newTrans);
                FilterByCategory("Összes"); // Frissítjük a nézetet
            }
        }

        [RelayCommand]
        private void EditSelected()
        {
            if (SelectedTransaction != null)
            {
                Transaction copy = SelectedTransaction.Clone();
                if (editorService.EditTransaction(copy))
                {
                    SelectedTransaction.CopyFrom(copy);
                    UpdateCategories(SelectedTransaction.Category);
                    CalculateBalance();
                }
            }
        }

        [RelayCommand]
        private void DeleteSelected()
        {
            if (SelectedTransaction != null)
            {
                AllTransactions.Remove(SelectedTransaction);
                DisplayedTransactions.Remove(SelectedTransaction);
                CalculateBalance();
            }
        }

        private void CalculateBalance()
        {
            int income = AllTransactions.Where(t => t.Type == TransactionType.Bevétel).Sum(t => t.Amount);
            int expense = AllTransactions.Where(t => t.Type == TransactionType.Kiadás).Sum(t => t.Amount);
            CurrentBalance = income - expense;
        }
        private void UpdateCategories(string newCategory)
        {
            if (!string.IsNullOrWhiteSpace(newCategory) && !Categories.Contains(newCategory))
            {
                Categories.Add(newCategory);
            }
        }
    }
}
