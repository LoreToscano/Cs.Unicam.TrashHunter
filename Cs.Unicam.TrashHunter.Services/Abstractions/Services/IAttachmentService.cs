using Cs.Unicam.TrashHunter.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs.Unicam.TrashHunter.Services.Abstractions.Services
{
    /// <summary>
    /// Interface for Attachment Service operations.
    /// </summary>
    public interface IAttachmentService
    {
        /// <summary>
        /// Attaches a new attachment to a post asynchronously.
        /// </summary>
        /// <param name="attachmentDTO">The attachment data transfer object.</param>
        /// <param name="postId">The identifier of the post.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created AttachmentDTO.</returns>
        public Task<AttachmentDTO> Attach(AttachmentDTO attachmentDTO, int postId);
        /// <summary>
        /// Attaches a company 
        /// </summary>
        /// <param name="attachmentDTO">CompanyCode</param>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        public Task<AttachmentDTO> Attach(AttachmentDTO attachmentDTO, string companyCode, bool isLogo);
        /// <summary>
        /// Retrieves lazy attachments for a post asynchronously.
        /// </summary>
        /// <param name="postId">The identifier of the post.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of LazyAttachmentDTO.</returns>
        public Task<IEnumerable<LazyAttachmentDTO>> GetLazyAttachments(int postId);

        /// <summary>
        /// Retrieves an attachment by its identifier asynchronously.
        /// </summary>
        /// <param name="fileName">The identifier of the attachment.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the AttachmentDTO.</returns>
        public Task<AttachmentDTO> GetAttachment(string fileName);

        /// <summary>
        /// Updates an existing attachment asynchronously.
        /// </summary>
        /// <param name="fileName">Change the description of the filename</param>
        /// <param name="description">The new description. If null, the description will not be changed.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated AttachmentDTO.</returns>
        public Task<AttachmentDTO> UpdateAttachment(string fileName, string description);

        /// <summary>
        /// Deletes an attachment by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The identifier of the attachment.</param>
        public Task DeleteAttachment(string id);

        public Task DeleteRange(string[] fileNames);
    }
}