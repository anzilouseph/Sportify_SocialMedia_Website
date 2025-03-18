namespace SportifyKerala.Models
{
    public class Users
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public Guid DistrictId { get; set; }    
        public  bool CommitieVerfied { get; set; }
        public string CreatedDate {  get; set; }
        public string Password { get; set; }
        public string salt { get; set; }
        public string Role { get; set; }

    }
}
