using SportifyKerala.Utilitys;
using SportifyKerala.Dto;
using SportifyKerala.Models;


namespace SportifyKerala.IRepo
{
    public interface IPostManagementRepo
    {
        public Task<APIResponse<bool>> AddPost(AddPostDto post,Guid ClubId);  //for Adding a post 
        public Task<APIResponse<IEnumerable<PostToListDto>>> GetAllPost();  //for get al the posts
        public Task<APIResponse<IEnumerable<CategoryToListDto>>> GetAllCategorys(); //for et all categorys
        public Task<APIResponse<dynamic>> GetNameandImage(Guid clubid);  //for get the name and Image of the club(To see who posted the image)

    }
}
