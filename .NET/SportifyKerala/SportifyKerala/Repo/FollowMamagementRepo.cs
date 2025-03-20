using Dapper;
using Org.BouncyCastle.Crypto.Prng;
using SportifyKerala.Context;
using SportifyKerala.Dto;
using SportifyKerala.IRepo;
using SportifyKerala.Models;
using SportifyKerala.Utilitys;

namespace SportifyKerala.Repo
{
    public class FollowMamagementRepo : IFollowManagement
    {
        private readonly DapperContext _context;
        public FollowMamagementRepo(DapperContext context)
        {
            _context = context;
        }


        //to follow a club (he needs to accept it)
        public async Task<APIResponse<bool>> Follow(FollowDto followData, Guid followerId)
        {
            var query = "insert into Follow (ClubId,FollowerId) values(@cid,@fid)";
            var parameters = new DynamicParameters();
            parameters.Add("cid", followData.ClubId);
            parameters.Add("fid",followerId);

            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var rowAffected = await connection.ExecuteAsync(query, parameters);
                connection.Close();
                if (rowAffected == 0)
                {
                    return APIResponse<bool>.Error("Unable to follow");
                }
                return APIResponse<bool>.Success(true, "Followed");
            }
        }

        //for get all the requests
        public async Task<APIResponse<IEnumerable<Follow>>> GetRequests(Guid clubid)
        {
            var query = "select * from Follow where Clubid=@id and Accepeted=0";
            var parameters = new DynamicParameters();
            parameters.Add("id", clubid);
            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var result = await connection.QueryAsync<Follow>(query, parameters);
                connection.Close();
                if (result.Count() == 0)
                {
                    return APIResponse<IEnumerable<Follow>>.Error("No Follow Requests");
                }
                return APIResponse<IEnumerable<Follow>>.Success(result, "Followed");
            }

        }


        //for  accept the follow of a particular user by the Company
        public async Task<APIResponse<bool>> AcceptRequest(Guid userId, Guid clubId)
        {
            var query = "update Follow set Accepeted=1 where ClubId=@cid and FollowerId=@uid";
            var parameters = new DynamicParameters();
            parameters.Add("uid", userId);
            parameters.Add("cid", clubId);
            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var rowAffected = await connection.ExecuteAsync(query, parameters);
                connection.Close();
                if (rowAffected == 0)
                {
                    return APIResponse<bool>.Error("Unable to follow");
                }
                return APIResponse<bool>.Success(true, "Followed");
            }
        }


        //for  Reject the follow of a particular user by the Company
        public async Task<APIResponse<bool>> RejectRequest(Guid userId, Guid clubId)
        {
            var query = "Delete from Follow where ClubId=@cid and FollowerId=@uid";
            var parameters = new DynamicParameters();
            parameters.Add("uid", userId);
            parameters.Add("cid", clubId);
            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var rowAffected = await connection.ExecuteAsync(query, parameters);
                connection.Close();
                if (rowAffected == 0)
                {
                    return APIResponse<bool>.Error("Unable to Reject");
                }
                return APIResponse<bool>.Success(true, "Rejected the Request");
            }
        }


        //nammalk oru aalde post kanumbo avde aale follow cheyyaninfo ellayo, ellenkil aale follow cheyan pattanan poole
        public async Task<APIResponse<bool>> CheckFollowOrNot(Guid clubid,Guid userid)
        {
            var query = "select * from Follow where ClubId=@cid and FollowerId=@uid and Accepeted=1";
            var parameters = new DynamicParameters();
            parameters.Add("uid", userid);
            parameters.Add("cid", clubid);
            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var result = await connection.QueryFirstOrDefaultAsync<Follow>(query, parameters);
                connection.Close();
                if (result == null)
                {
                    return APIResponse<bool>.Error("Not Following");
                }
                return APIResponse<bool>.Success(true, "Following");
            }
        }

        //oru club nu ethra follow reqs ond enn ariyan veendii
        public async Task<APIResponse<int>> FollowRequests(Guid clubid)
        {
            var query = "select count(*) from Follow where ClubId=@id and Accepeted=0";
            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var result = await connection.QueryFirstOrDefaultAsync<int>(query, new {id=clubid});
                connection.Close();
                if (result == null)
                {
                    return APIResponse<int>.Error("No Follow requests");
                }
                return APIResponse<int>.Success(result, "Success");
            }
        }


    }
}
