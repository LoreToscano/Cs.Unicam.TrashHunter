using Cs.Unicam.TrashHunter.Models.Repositorys.Models;
using Cs.Unicam.TrashHunter.Services.DTOs;
using Cs.Unicam.TrashHunter.Services.Services.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs.Unicam.TrashHunter.Services.Abstractions.Services
{
    /// <summary>
    /// Interface for Post Service operations.
    /// </summary>
    public interface IPostService
    {
        /// <summary>
        /// Creates a new post asynchronously.
        /// </summary>
        /// <param name="postDTO">The post data transfer object.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created PostDTO.</returns>
        public Task<PostDTO> CreatePostAsync(PostDTO postDTO);

        /// <summary>
        /// Retrieves a post by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The identifier of the post.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the PostDTO.</returns>
        public Task<PostDTO> GetPostAsync(int id);

        /// <summary>
        /// Retrieves all posts asynchronously whit some filters.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of PostDTO.</returns>
        public Task<PagingResult<PostDTO>> GetPostsAsync(PostFilters filters, int page, int pageSize);

        /// <summary>
        /// Updates an existing post asynchronously.
        /// </summary>
        /// <param name="postDTO">The post data transfer object.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated PostDTO.</returns>
        public Task<PostDTO> UpdatePostAsync(PostDTO postDTO);

        /// <summary>
        /// Deletes a post by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The identifier of the post.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the deleted PostDTO.</returns>
        public Task<PostDTO> DeletePostAsync(int id);

        public Task<bool> ConnectToCompany(int postId, string companyCode);

        public Task<bool> Approve(int postId, bool approved);

        public Task<bool> Complete(int postId, string userCode);
    }
}