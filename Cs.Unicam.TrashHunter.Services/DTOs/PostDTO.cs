using Cs.Unicam.TrashHunter.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Cs.Unicam.TrashHunter.Services.DTOs
{
    public class PostDTO
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public UserDTO User { get; set; }
        public List<AttachmentDTO> Attachments { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public UserDTO? CompletedBy { get; set; }
        public State State => !IsApproved.HasValue ?
            State.Created 
            : IsApproved.HasValue && !IsApproved.Value ?
                State.Rejected
                : IsApproved.Value && CompletionDate == null ? 
                    State.Pending 
                    : State.Completed;
        public bool? IsApproved { get; set; }

        public PostDTO(Post post, IList<AttachmentDTO>? attachments)
        {
            PostId = post.PostId;
            Title = post.Title;
            Description = post.Description;
            City = post.City;
            User = new UserDTO(post.User);
            Attachments = attachments?.ToList() ?? [];
            CreationDate = post.CreationDate;
            LastModifiedDate = post.LastModifiedDate;
            CompletionDate = post.CompletionDate;
            CompletedBy = post.CompletedBy != null ? new UserDTO(post.CompletedByUser) : null;
            IsApproved = post.IsApproved;
        }

        public PostDTO(Post post, UserDTO user, IList<AttachmentDTO>? attachments)
        {
            PostId = post.PostId;
            Title = post.Title;
            Description = post.Description;
            City = post.City;
            User = user;
            Attachments = attachments?.ToList() ?? [];
            CreationDate = post.CreationDate;
            LastModifiedDate = post.LastModifiedDate;
            CompletionDate = post.CompletionDate;
            CompletedBy = post.CompletedBy != null ? new UserDTO(post.CompletedByUser) : null;
            IsApproved = post.IsApproved;
        }

        public Post ToEntity()
        {
            return new Post(PostId, Title, Description, City, User.Email, [.. Attachments.Select(a => a.ToEntity())], CreationDate, LastModifiedDate);
        }

    }

    public enum State
    {
        Created,
        Rejected,
        Pending,
        Completed
    }

    public static class StateMethods {
        public static string GetStateString(this State state)
        {
            return state switch
            {
                State.Created => "Creato",
                State.Pending => "In attesa",
                State.Rejected => "Rifiutato",
                State.Completed => "Completato",
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
        }

        public static string GetStateColor(this State state)
        {
            return state switch
            {
                State.Created => "info",
                State.Rejected => "danger",
                State.Pending => "warning",
                State.Completed => "success",
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
        }
    }
}
