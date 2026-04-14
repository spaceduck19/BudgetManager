using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetManager.Models;

namespace BudgetManager.Services
{
    public class EditorService : IEditorService
    {
        public bool EditTransaction(Transaction transaction)
        {
            EditorWindow window = new EditorWindow(transaction);
            return window.ShowDialog() == true;
        }
    }
}
