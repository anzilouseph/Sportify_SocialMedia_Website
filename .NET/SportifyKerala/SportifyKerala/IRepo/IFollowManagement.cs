using SportifyKerala.Dto;
using SportifyKerala.Models;
using SportifyKerala.Utilitys;

namespace SportifyKerala.IRepo
{
    public interface IFollowManagement
    {
        public Task<APIResponse<bool>> Follow(FollowDto followData, Guid followerId);  //to follow a club (he needs to accept it)
        public Task<APIResponse<IEnumerable<Follow>>> GetRequests(Guid clubid);  //for get all the requests
        public Task<APIResponse<bool>> AcceptRequest(Guid userId, Guid clubId);   //for  accept the follow of a particular user by the Company
        public Task<APIResponse<bool>> RejectRequest(Guid userId, Guid clubId);   //for  Reject the follow of a particular user by the Company

    }
}
