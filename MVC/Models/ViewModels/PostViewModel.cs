using Cs.Unicam.TrashHunter.Models.Repositorys.Models;
using Cs.Unicam.TrashHunter.Services.DTOs;

namespace Cs.Unicam.TrashHunter.MVC.Models.ViewModels
{
    public class PostViewModel
    {
        public PagingResult<PostDTO> Post { get; set; }
        
        public PostViewModel(PagingResult<PostDTO> post)
        {
            Post = post;
        }


    }   
}
