using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.Models
{
    public enum TransactionType { Bevétel, Kiadás }

    public partial class Transaction : ObservableObject
    {
        [ObservableProperty]
        private string title = string.Empty;

        [ObservableProperty]
        private int amount;

        [ObservableProperty]
        private TransactionType type;

        [ObservableProperty]
        private string category = "Általános";

        [ObservableProperty]
        private DateTime date = DateTime.Now;

        public Transaction Clone()
        {
            return new Transaction { Title = this.Title, Amount = this.Amount, Type = this.Type, Category = this.Category, Date = this.Date };
        }

        public void CopyFrom(Transaction other)
        {
            this.Title = other.Title;
            this.Amount = other.Amount;
            this.Type = other.Type;
            this.Category = other.Category;
            this.Date = other.Date;
        }
    }
}
