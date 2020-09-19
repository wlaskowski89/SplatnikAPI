﻿using AutoMapper;
using Splatnik.API.Dtos;
using Splatnik.API.Services.Interfaces;
using Splatnik.Contracts.V1.Requests;
using Splatnik.Data.Database.DbModels;
using Splatnik.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Splatnik.API.Services
{
	public class BudgetService : IBudgetService
	{
		private readonly IBudgetRepository _budgetRepository;
		private readonly IMapper _mapper;

		public BudgetService(IBudgetRepository budgetRepository, IMapper mapper)
		{
			_budgetRepository = budgetRepository;
			_mapper = mapper;

		}

		public async Task<Budget> NewBudgetAsync(NewBudgetRequest budgetRequest, string userId)
		{

			var budgetDto = new BudgetDto
			{
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow,
				Name = budgetRequest.Name,
				Description = budgetRequest.Description,
				UserId = userId
			};

			var budget = _mapper.Map<Budget>(budgetDto);
			
			var created = await _budgetRepository.CreateBudgetAsync(budget);

			return created;
		}

		public async Task<Budget> GetBudgetAsync(int budgetId)
		{
			return await _budgetRepository.GetBudgetAsync(budgetId);
		}

		public async Task<IList<Budget>> GetUserBudgets(string userId) 
		{
			return await _budgetRepository.GetUserBudgets(userId);
		}

		public async Task<Period> NewPeriodAsync(NewPeriodRequest periodRequest, int budgetId)
        {
			var periodDto = new PeriodDto
			{
				CreatedAt = DateTime.UtcNow,
				DisplayName = periodRequest.DisplayName,
				FirstDay = periodRequest.FirstDay,
				LastDay = periodRequest.LastDay,
				Notes = periodRequest.Notes,
				BudgetId = budgetId
			};

			var period = _mapper.Map<Period>(periodDto);

			var created = await _budgetRepository.CreatePeriodAsync(period);

			return created;

        }

		public async Task<Period> GetPeriodAsync(int budgetId, int periodId)
        {
			return await _budgetRepository.GetPeriodAsync(budgetId, periodId);
        }

		public async Task<Period> GetCurrentPeriodAsync(int budgetId, DateTime today)
        {
			return await _budgetRepository.GetCurrentPeriodAsync(budgetId, today);
        }

		public async Task<IList<Period>> GetBudgetPeriodsAsync(int budgetId)
        {
			return await _budgetRepository.GetBudgetPeriodsAsync(budgetId);
        }

        public async Task<Expense> NewExpenseAsync(int periodId, NewExpenseRequest request)
        {
			var expenseDto = new ExpenseDto
			{
				CreatedAt = DateTime.UtcNow,
				IncomeDate = request.IncomeDate,
				Name = request.Name,
				Description = request.Description,
				ExpanseValue = request.ExpanseValue,
				CurrencyId = request.CurrencyId,
				PeriodId = periodId
			};

			var expense = _mapper.Map<Expense>(expenseDto);

			var created = await _budgetRepository.CreateExpenseAsync(expense);

			return created;
        }

        public async Task<Expense> GetExpenseAsync(int periodId, int expenseId)
        {
			return await _budgetRepository.GetExpenseAsync(periodId, expenseId);
        }

        public async Task<Income> NewIncomeAsync(int periodID, NewIncomeRequest request)
        {
			var incomeDto = new IncomeDto
			{
				CreatedAt = DateTime.UtcNow,
				IncomeDate = request.IncomeDate,
				Name = request.Name,
				Description = request.Description,
				IncomeValue = request.IncomeValue,
				CurrencyId = request.CurrencyId,
				PeriodId = periodID
			};

			var income = _mapper.Map<Income>(incomeDto);

			var created = await _budgetRepository.CreateIncomeAsync(income);

			return created;
        }

		public async Task<Income> GetIncomeAsync(int periodId, int incomeId)
		{
			return await _budgetRepository.GetIncomeAsync(periodId, incomeId);
		}

	}
}
