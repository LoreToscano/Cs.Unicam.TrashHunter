using Cs.Unicam.TrashHunter.Models.Abstractions.Repositorys;
using Cs.Unicam.TrashHunter.Models.DB;
using Cs.Unicam.TrashHunter.Models.Entities;
using Cs.Unicam.TrashHunter.Models.Repositorys.Models;
using Cs.Unicam.TrashHunter.Services.Abstractions.Services;
using Cs.Unicam.TrashHunter.Services.DTOs;
using Cs.Unicam.TrashHunter.Services.Services.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs.Unicam.TrashHunter.Services.Services
{
    public class PostService : IPostService
    {
        private readonly IRepository<Post>  _postRepository;
        private readonly IAttachmentService _attachmentService;
        public PostService(IRepository<Post> postRepository, IAttachmentService attachment)
        {
            _postRepository = postRepository;
            _attachmentService = attachment;
        }
        public async Task<PostDTO> CreatePostAsync(PostDTO postDTO)
        {
            UserDTO user = postDTO.User;
            var post = postDTO.ToEntity();
            _postRepository.Add(post);
            await _postRepository.Save();
           
            return await CreatePostDTOAsync(post, user);
        }

        public async Task<PostDTO> DeletePostAsync(int id)
        {
            var post = await _postRepository.Find(id);
            if (post == null)
            {
                throw new KeyNotFoundException("Post not found");
            }

            _postRepository.Delete(post);
            await _attachmentService.DeleteRange(post.Attachments.Select(a => a.FileName).ToArray());
            await _postRepository.Save();
            return new PostDTO(post, null);
        }

        public async Task<PostDTO> GetPostAsync(int id)
        {
            var post = await _postRepository.Find(id);
            if (post == null)
            {
                throw new KeyNotFoundException("Post not found");
            }
            return await CreatePostDTOAsync(post, null);
        }

        public async Task<PagingResult<PostDTO>> GetPostsAsync(PostFilters filters, int page, int pageSize)
        {
            return (await _postRepository.GetFiltered(filters.Filter(), (page - 1) * pageSize, pageSize))
                .Select(CreatePostDTO);
        }

        public async Task<PostDTO> UpdatePostAsync(PostDTO postDTO)
        {
            if (!(await _postRepository.Exist(postDTO.PostId)))
            {
                throw new KeyNotFoundException("Post not found");
            }
            var post = postDTO.ToEntity();
            _postRepository.Update(post);
            await _postRepository.Save();
            return await CreatePostDTOAsync(await _postRepository.Find(postDTO.PostId), null);
        }

        public async Task<bool> ConnectToCompany(int postId, string companyCode)
        {
            var post = await _postRepository.Find(postId);
            if (post == null)
            {
                throw new KeyNotFoundException("Post not found");
            }
            post.SetCompany(companyCode);
            _postRepository.Update(post);
            await _postRepository.Save();
            return true;
        }

        public async Task<bool> Approve(int postId, bool approved)
        {
            var post = await _postRepository.Find(postId);
            if (post == null)
            {
                throw new KeyNotFoundException("Post not found");
            }
            if (approved)
                post.Approve();
            else
                post.Reject();

            _postRepository.Update(post);
            await _postRepository.Save();
            return true;
        }

        public async Task<bool> Complete(int postId, string userCode)
        {
            var post = await _postRepository.Find(postId);
            if (post == null)
            {
                throw new KeyNotFoundException("Post not found");
            }
            post.Complete(userCode);
            _postRepository.Update(post);
            await _postRepository.Save();
            return true;
        }

        private PostDTO CreatePostDTO(Post post) => CreatePostDTOAsync(post, null).Result;
        private async Task<PostDTO> CreatePostDTOAsync(Post post, UserDTO? user)
        {
            var attachments = new List<AttachmentDTO>();
            foreach (var attachment in post.Attachments)
            {
                attachments.Add(await _attachmentService.GetAttachment(attachment.FileName));
            }
            if (user == null)
            {
                user = new UserDTO(post.User);
            }
            return new PostDTO(post, user, attachments);
        }

    }
}
