namespace SportifyKerala.Dto
{
    public class UserToListDto
    {
        public Guid idOfUser { get; set; }
        public string nameOfUser { get; set; }
        public string emailOfUser { get; set; }
        public string phoneOfUser { get; set; }
        public string addressOfUser { get; set; }
        public Guid idOfDistrict { get; set; }
        public bool commitie { get; set; }
        public string roleOfUser { get; set; }
    }
}
