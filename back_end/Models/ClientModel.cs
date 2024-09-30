namespace back_end.Models
{
    public class ClientModel
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string LastNames { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string UserPassword { get; set; }
        public string AccountState { get; set; }
        public string Rol { get; set; }
    }
}
