namespace back_end.Domain
{
    public class SuperUserModel {
        public int SuperUserID { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public int CreatedByID { get; set; }
    }
}
