namespace SportifyKerala.Models
{
    public class Follow
    {
        public Guid FollowId {  get; set; }
        public Guid ClubId { get; set; }
        public Guid FollowerId { get; set; }
        public bool IsFollowing { get; set; }
        public bool Accepted { get; set; }
    }
}
