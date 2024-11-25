namespace back_end.Application.Queries
{
    using back_end.Application.Interfaces;
    using back_end.Domain;
    using back_end.Infrastructure.Repositories;
    using System.Collections.Generic;

    public class GetMonthlyRevenueQuery
    {
        private readonly IAdminHandler _adminHandler;

        public GetMonthlyRevenueQuery(IAdminHandler adminHandler)
        {
            _adminHandler = adminHandler;
        }

        public List<MonthlyDataModel> Execute()
        {
            return _adminHandler.GetMonthlyRevenue();
        }
    }
}
