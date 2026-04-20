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

        // Egyetlen, közös lista a kategóriáknak a legördülő menühöz
        public ObservableCollection<string> Categories { get; } = new()
        {
            "Összes", "Fizetés", "Étel", "Utazás", "Szórakozás", "Általános"
        };

        [ObservableProperty]
        private Transaction? selectedTransaction;

        [ObservableProperty]
        private int currentBalance;

        [ObservableProperty]
        private int totalIncome;

        [ObservableProperty]
        private int totalExpense;

        [ObservableProperty]
        private bool isFilterActive;

        [ObservableProperty]
        private string selectedFilterCategory = "Összes";

        public MainViewModel(IEditorService editorService)
        {
            this.editorService = editorService;

            // Kezdeti adatok
            AddTransaction(new Transaction { Title = "Ösztöndíj", Amount = 80000, Type = TransactionType.Bevétel, Category = "Fizetés" });
            AddTransaction(new Transaction { Title = "BKV Bérlet", Amount = 3450, Type = TransactionType.Kiadás, Category = "Utazás" });
            AddTransaction(new Transaction { Title = "Ebéd", Amount = 2500, Type = TransactionType.Kiadás, Category = "Étel" });

            FilterByCategory();
        }

        private void AddTransaction(Transaction t)
        {
            AllTransactions.Add(t);
            UpdateCategories(t.Category);
            CalculateBalance();
        }

        private void FilterByCategory()
        {
            DisplayedTransactions.Clear();
            foreach (var t in AllTransactions)
            {
                if (!IsFilterActive || SelectedFilterCategory == "Összes" || t.Category == SelectedFilterCategory)
                {
                    DisplayedTransactions.Add(t);
                }
            }
        }

        [RelayCommand]
        private void CreateNew()
        {
            Transaction newTrans = new Transaction();
            if (editorService.EditTransaction(newTrans))
            {
                AddTransaction(newTrans);
                FilterByCategory();
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
            TotalIncome = AllTransactions.Where(t => t.Type == TransactionType.Bevétel).Sum(t => t.Amount);
            TotalExpense = AllTransactions.Where(t => t.Type == TransactionType.Kiadás).Sum(t => t.Amount);

            CurrentBalance = TotalIncome - TotalExpense;
        }

        private void UpdateCategories(string newCategory)
        {
            if (!string.IsNullOrWhiteSpace(newCategory) && !Categories.Contains(newCategory))
            {
                Categories.Add(newCategory);
            }
        }

        //Toolkit meghívja, ha pipa vagy kategóriaváltás
        partial void OnIsFilterActiveChanged(bool value) => FilterByCategory();
        partial void OnSelectedFilterCategoryChanged(string value) => FilterByCategory();
    }
}