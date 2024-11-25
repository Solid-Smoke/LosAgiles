namespace back_end.Application.Queries
{
    using back_end.Application.Interfaces;
    using back_end.Domain;
    using back_end.Infrastructure.Repositories;
    using System.Collections.Generic;

    public class GetMonthlyShippingExpensesQuery
    {
        private readonly IAdminHandler _adminHandler;

        public GetMonthlyShippingExpensesQuery(IAdminHandler adminHandler)
        {
            _adminHandler = adminHandler;
        }

        public List<MonthlyDataModel> Execute()
        {
            return _adminHandler.GetMonthlyShippingExpense();
        }
    }
}
