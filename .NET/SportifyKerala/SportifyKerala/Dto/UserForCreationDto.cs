using System.ComponentModel.DataAnnotations;

namespace SportifyKerala.Dto
{
    public class UserForCreationDto
    {
        [Required (ErrorMessage ="Name is required")]
        public string nameOfUser { get; set; }

        [Required (ErrorMessage = "Email is requied")]
        [EmailAddress (ErrorMessage ="Invalid Email Address")]
        public string emailOfUser { get; set; }

        [Required (ErrorMessage ="Phone is Required")]
        public string phoneOfUser { get; set; }

        public string addressOfUser { get; set; }

        [Required(ErrorMessage = "District is Required")]
        public Guid idOfDistrict { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        public string passwordOfUser { get; set; }
    }
}
