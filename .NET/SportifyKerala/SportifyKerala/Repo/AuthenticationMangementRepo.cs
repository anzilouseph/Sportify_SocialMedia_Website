using Dapper;
using SportifyKerala.Context;
using SportifyKerala.Dto;
using SportifyKerala.IRepo;
using SportifyKerala.Models;

namespace SportifyKerala.Repo
{
    public class AuthenticationMangementRepo : IAuthenticationManagementRepo
    {
        private readonly DapperContext _context;
        public AuthenticationMangementRepo(DapperContext context)
        {
            _context = context;
        }
        //for login

        public async Task<Users> Login(LoginDto login)
        {
            var query = "select * from Users where Email =@email";
            var parameters = new DynamicParameters();
            parameters.Add("email",login.emailOfUser);

            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var result = await connection.QueryFirstOrDefaultAsync<Users>(query, parameters);
                connection.Close();
                return result;
            }
        }

    }
}
