using Cs.Unicam.TrashHunter.Services.DTOs;

namespace Cs.Unicam.TrashHunter.MVC.Models.ViewModels
{
    public class PostEditViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public AttachmentViewModel[] Base64Images { get; set; }
        public string City { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        
        public PostEditViewModel(PostDTO post)
        {
            Title = post.Title;
            Description = post.Description;
            Base64Images = post.Attachments.Select(a => new AttachmentViewModel(a)).ToArray();
            City = post.City;
            CreationDate = post.CreationDate;
            LastModifiedDate = post.LastModifiedDate;
        }
    }
}
