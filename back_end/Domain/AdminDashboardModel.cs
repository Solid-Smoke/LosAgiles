namespace back_end.Domain
{
    public class AdminDashboardModel
    {
        public List<MonthlyDataModel> Ganancias { get; set; }
        public List<MonthlyDataModel> GastoEnvios { get; set; }
        public List<OrderModel> OrdersInProgress { get; set; }
    }

    public class MonthlyDataModel
    {
        public string Month { get; set; }
        public decimal Total { get; set; }
    }
}
