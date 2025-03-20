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
        public Task<APIResponse<bool>> CheckFollowOrNot(Guid clubid,Guid userid);  //nammalk oru aalde post kanumbo avde aale follow cheyyaninfo ellayo, ellenkil aale follow cheyan pattanan poole
        public Task<APIResponse<int>> FollowRequests(Guid clubid);  //oru club nu ethra follow reqs ond enn ariyan veendii
    }
}
