using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetManager.Models;

namespace BudgetManager.Services
{
    public interface IEditorService
    {
        bool EditTransaction(Transaction transaction);
    }
}
