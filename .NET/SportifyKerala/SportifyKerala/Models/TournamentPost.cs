namespace SportifyKerala.Models
{
    public class TournamentPost
    {
        public Guid PostId {  get; set; }
        public Guid ClubId { get; set; }
        public Guid DistrictId { get; set; }
        public string Description { get; set; }
        public string PostDate {  get; set; }
        public string ImageName {  get; set; }
        public Guid CategorId { get; set; }

    }
}
