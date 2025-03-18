namespace SportifyKerala.Dto
{
    public class AddPostDto
    {
        public Guid DistrictId { get; set; }
        public Guid CategoryId { get; set; }
        public string descriptionOfPost { get; set; }
        public IFormFile image { get; set; }
    }
}
