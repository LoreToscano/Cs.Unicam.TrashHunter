using Cs.Unicam.TrashHunter.Services.DTOs;

namespace Cs.Unicam.TrashHunter.MVC.Models.ViewModels
{
    public class UserViewModel
    {
        public UserDTO User { get; set; }
        public IEnumerable<PostDTO> CreatedPosts { get; set; }
        public IEnumerable<PostDTO> CompletedPosts { get; set; }
        public double UserRating { get; set; }

        public UserViewModel(UserDTO user, IEnumerable<PostDTO> posts)
        {
            User = user;
            CompletedPosts = posts.Where(p => p.CompletedBy != null && p.CompletedBy.Email == user.Email);
            CreatedPosts = posts.Where(p => p.User.Email == user.Email);

            UserRating = posts
                .Where(p => p.User.Email == user.Email && p.IsApproved.HasValue)
                .Select(p => p.IsApproved.Value ? +1.1 : -0.5)
                .Sum()
                + posts.Where(p => p.CompletedBy != null && p.CompletedBy.Email == user.Email)
                .Select(p => +1.5)
                .Sum();
        }
    }
}
