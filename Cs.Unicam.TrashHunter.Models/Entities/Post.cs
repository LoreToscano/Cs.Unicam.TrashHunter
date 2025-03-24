using Cs.Unicam.TrashHunter.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs.Unicam.TrashHunter.Models.Entities
{
    public class Post
    {
        public int PostId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string UserCode { get; private set; }
        public string City { get; private set; }
        public string CompanyCode { get; private set; }
        public DateTime CreationDate { get; private set; }
        public virtual User User { get; private set; }
        public virtual ICollection<Attachment> Attachments { get; private set; } = new List<Attachment>();
        public virtual Company Company { get; private set; }
        public DateTime LastModifiedDate { get; private set; }
        public string CompletedBy { get; private set; }
        public virtual User CompletedByUser { get; private set; }
        public DateTime? CompletionDate { get; private set; }
        public bool? IsApproved { get; private set; }



        /// <summary>
        /// Empty constructor for EF
        /// </summary>
        public Post() { }
        public Post(string title, string description, string city, string userCode, ICollection<Attachment> attachments)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(userCode))
                throw new UserException("Parametri non validi");
            Title = title;
            Description = description;
            City = city;
            UserCode = userCode;
            Attachments = attachments;
            CreationDate = DateTime.Now;
            LastModifiedDate = DateTime.Now;
            IsApproved = null;
        }

        public Post(string title, string description, string city, User user, ICollection<Attachment> attachments) : this(title, description, city, user.Email, attachments)
        {
            User = user;
        }

        public Post(int id, string title, string description, string city, string userCode, ICollection<Attachment> attachments, DateTime creationDate, DateTime lastEditDate) 
            : this(title, description, city, userCode, attachments)
        {
            this.PostId = id;
            this.CreationDate = creationDate;
            this.LastModifiedDate = lastEditDate;
        }

        public void Approve()
        {
            IsApproved = true;
        }

        public void Reject()
        {
            IsApproved = false;
        }


        public void Edit(string title, string description, string city)
        {
            Title = title;
            Description = description;
            City = city;
            LastModifiedDate = DateTime.Now;
        }

        public void Complete(string completedBy)
        {
            CompletedBy = completedBy;
            CompletionDate = DateTime.Now;
        }

        public void SetCompany(Company company)
        {
            Company = company;
        }
        
        public void SetCompany(string companyCode)
        {
            CompanyCode = companyCode;
        }
    }
}
