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

        public ObservableCollection<Transaction> AllTransactions { get; } = new();
        public ObservableCollection<Transaction> DisplayedTransactions { get; } = new();

        [ObservableProperty]
        private Transaction? selectedTransaction;

        [ObservableProperty]
        private int currentBalance;

        public MainViewModel(IEditorService editorService)
        {
            this.editorService = editorService;

            // Kezdeti adatok
            AddTransaction(new Transaction { Title = "Ösztöndíj", Amount = 80000, Type = TransactionType.Income, Category = "Salary" });
            AddTransaction(new Transaction { Title = "BKV Bérlet", Amount = 3450, Type = TransactionType.Expense, Category = "Transport" });
            AddTransaction(new Transaction { Title = "Ebéd", Amount = 2500, Type = TransactionType.Expense, Category = "Food" });

            FilterByCategory("All");
        }

        private void AddTransaction(Transaction t)
        {
            AllTransactions.Add(t);
            CalculateBalance();
        }

        public void FilterByCategory(string category)
        {
            DisplayedTransactions.Clear();
            foreach (var t in AllTransactions)
            {
                if (category == "All" || t.Category == category)
                    DisplayedTransactions.Add(t);
            }
        }

        [RelayCommand]
        private void CreateNew()
        {
            Transaction newTrans = new Transaction();
            if (editorService.EditTransaction(newTrans))
            {
                AddTransaction(newTrans);
                FilterByCategory("All"); // Frissítjük a nézetet
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
            int income = AllTransactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
            int expense = AllTransactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);
            CurrentBalance = income - expense;
        }
    }
}
