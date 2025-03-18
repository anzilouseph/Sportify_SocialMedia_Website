using Dapper;
using System.Linq;
using SportifyKerala.Context;
using SportifyKerala.Dto;
using SportifyKerala.IRepo;
using SportifyKerala.Utilitys;
using SportifyKerala.Models;

namespace SportifyKerala.Repo
{
    public class PostManagementRepo : IPostManagementRepo
    {
        private readonly DapperContext context;
        private readonly IWebHostEnvironment _env;

        public PostManagementRepo(DapperContext context, IWebHostEnvironment env)
        {
            this.context = context;
            _env = env;
        }

        //for Adding a post 
        public async Task<APIResponse<bool>> AddPost(AddPostDto post, Guid ClubId)
        {
            string key = Guid.NewGuid().ToString();
            string filePath = string.Empty;
            string newFileName = string.Empty;
            if (post.image != null)
            {
                var folderPath = Path.Combine(_env.WebRootPath, "Media", "Posts");


                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }


                var fileExtension = Path.GetExtension(post.image.FileName).ToLower();

                var allowedExtensions = new HashSet<string> { ".jpg", ".jpeg", ".png", ".gif" };

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return APIResponse<bool>.Error("Invalid file type. Only JPG, JPEG, PNG, and GIF are allowed.");
                }
                newFileName = $"{key}{fileExtension}";
                filePath = Path.Combine(folderPath, newFileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await post.image.CopyToAsync(stream)

;
                }
                Console.WriteLine(post.image.FileName);
                Console.WriteLine(filePath);
                Console.WriteLine(newFileName);
            }
            var query = "insert into TournamentPost(ClubId,DistrictId,Description,ImageName,CategoryId)values(@ClubId,@DistrictId,@Description,@ImageName,@CategoryId)";

            var parameters = new DynamicParameters();
            parameters.Add("ClubId", ClubId);
            parameters.Add("DistrictId", post.DistrictId);
            parameters.Add("Description", post.descriptionOfPost);
            parameters.Add("ImageName", newFileName);
            parameters.Add("CategoryId", post.CategoryId);



            using (var connection = context.CreateConnection())
            {
                connection.Open();
                var result = await connection.ExecuteAsync(query, parameters);
                connection.Close();
                if (result == 0)
                {
                    return APIResponse<bool>.Error("Unable to add product");
                }
                return APIResponse<bool>.Success(true, "Product Added SuccessFully");

            }
        }

        //for get al the posts
        public async Task<APIResponse<IEnumerable<PostToListDto>>> GetAllPost()
        {
            var query = "select * from TournamentPost";
            using (var connection = context.CreateConnection())
            {
                connection.Open();
                var result = await connection.QueryAsync<TournamentPost>(query);
                connection.Close();
                if (result.Count() == 0)
                {
                    return APIResponse<IEnumerable<PostToListDto>>.Error("No posts to show");
                }
                var masked = result.Select(post => new PostToListDto
                {

                    idofPost = post.PostId,
                    idOfClub = post.ClubId,
                    DistrictId = post.DistrictId,
                    descriptionOfPost = post.Description,
                    dateOfPost = post.PostDate,
                    imageName = post.ImageName,
                });

                return APIResponse<IEnumerable<PostToListDto>>.Success(masked, "Success");
            }
        }

        //for get all categorys
        public async Task<APIResponse<IEnumerable<CategoryToListDto>>> GetAllCategorys()
        {
            var query = "select * from Category";
            using (var connection = context.CreateConnection())
            {
                connection.Open();
                var result = await connection.QueryAsync<Category>(query);
                connection.Close();
                if (result.Count() == 0)
                {
                    return APIResponse<IEnumerable<CategoryToListDto>>.Error("No Categories to show");
                }
                var masked = result.Select(cat => new CategoryToListDto
                {
                    idOfCategory = cat.CategoryId,
                    nameOfCategory= cat.CategoryName,
                });

                return APIResponse<IEnumerable<CategoryToListDto>>.Success(masked, "Success");
            }

        }


        //for get the name and Image of the club(To see who posted the image)
        public async Task<APIResponse<dynamic>> GetNameandImage(Guid clubid)
        {
            var query = "select FullName,ProfileImage from Users where userid=@id";
            var parameters = new DynamicParameters();
            parameters.Add("id", clubid);
            using (var connection = context.CreateConnection())
            {
                connection.Open();
                var result = await connection.QueryFirstOrDefaultAsync<dynamic>(query,parameters);
                connection.Close();
                if (result == null)
                {
                    return APIResponse<dynamic>.Error("No Club in this Id");

                }
                return APIResponse<dynamic>.Success(result, "Success");
            }
        }



    }
}
