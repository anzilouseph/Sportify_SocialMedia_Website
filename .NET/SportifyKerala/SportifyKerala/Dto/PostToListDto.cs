namespace SportifyKerala.Dto
{
    public class PostToListDto
    {
        public Guid idofPost { get; set; }
        public Guid idOfClub { get; set; }
        public Guid DistrictId { get; set; }
        public string descriptionOfPost { get; set; }
        public string dateOfPost { get; set; }
        public string imageName { get; set; }
    }
}
