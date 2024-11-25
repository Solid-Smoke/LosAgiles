namespace back_end.Application.Queries
{
    using back_end.Application.Interfaces;
    using back_end.Domain;
    using back_end.Infrastructure.Repositories;
    using System.Collections.Generic;

    public class GetOrdersInProgressQuery
    {
        private readonly IAdminHandler _adminHandler;

        public GetOrdersInProgressQuery(IAdminHandler adminHandler)
        {
            _adminHandler = adminHandler;
        }

        public List<OrderModel> Execute()
        {
            return _adminHandler.GetOrdersInProgress();
        }
    }
}
