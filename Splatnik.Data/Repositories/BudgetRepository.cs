﻿using Microsoft.EntityFrameworkCore;
using Splatnik.Data.Database;
using Splatnik.Data.Database.DbModels;
using Splatnik.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Splatnik.Data.Repositories
{
	public class BudgetRepository : IBudgetRepository
	{
		private readonly DataContext _dataContext;

		public BudgetRepository(DataContext dataContext)
		{
			_dataContext = dataContext;
		}

		public async Task<Budget> CreateBudgetAsync(Budget budget)
		{

			_dataContext.Budgets.Add(budget);
			await _dataContext.SaveChangesAsync();

			return budget;
		}

        public async Task<Budget> GetBudgetAsync(int budgetId)
		{
			return await _dataContext.Budgets
				.Include(x => x.Periods)
				.ThenInclude(x=>x.Incomes)
				.Include(x => x.Periods)
				.ThenInclude(x => x.Expenses)
				.FirstOrDefaultAsync(x => x.Id == budgetId);
		}

		public async Task<IList<Budget>> GetUserBudgets(string userId)
		{
			return await _dataContext.Budgets.Where(x => x.UserId == userId).ToListAsync();
		}

        public async Task<Period> CreatePeriodAsync(Period period)
        {
			_dataContext.Periods.Add(period);
			await _dataContext.SaveChangesAsync();

			return period;
        }

		public async Task<Period> GetPeriodAsync(int budgetId, int periodId) 
		{
			return await _dataContext.Periods.FirstOrDefaultAsync(x => x.Id == periodId && x.BudgetId == budgetId);
		}

		public async Task<IList<Period>> GetBudgetPeriodsAsync(int budgetId)
        {
			return await _dataContext.Periods.Where(x => x.BudgetId == budgetId).ToListAsync();
        }

        public async Task<Period> GetCurrentPeriodAsync(int budgetId, DateTime today)
        {
			return await _dataContext.Periods.FirstOrDefaultAsync(x => x.BudgetId == budgetId && x.FirstDay >= today && x.LastDay <= today);
        }

        public async Task<Expense> CreateExpenseAsync(Expense expense)
        {
			_dataContext.Expenses.Add(expense);
			await _dataContext.SaveChangesAsync();

			return expense;
        }

        public async Task<Expense> GetExpenseAsync(int periodId, int expenseId)
        {
			return await _dataContext.Expenses.FirstOrDefaultAsync(x => x.Id == expenseId && x.PeriodId == periodId);
        }

        public async Task<Income> CreateIncomeAsync(Income income)
        {
			_dataContext.Incomes.Add(income);
			await _dataContext.SaveChangesAsync();

			return income;
        }

        public async Task<Income> GetIncomeAsync(int periodId, int incomeId)
        {
			return await _dataContext.Incomes.FirstOrDefaultAsync(x => x.Id == incomeId && x.PeriodId == periodId);
        }
    }
}
