using Cs.Unicam.TrashHunter.Models.Abstractions.Repositorys;
using Cs.Unicam.TrashHunter.Models.DB;
using Cs.Unicam.TrashHunter.Models.Entities;
using Cs.Unicam.TrashHunter.Services.Abstractions.Services;
using Cs.Unicam.TrashHunter.Services.DTOs;
using Cs.Unicam.TrashHunter.Services.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs.Unicam.TrashHunter.Services.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IRepository<Attachment> _attachmentRepository;
        private readonly IRepository<Post> _postRepository;
        private readonly AttachmentOptions _options;

        public AttachmentService(IRepository<Attachment> attachmentRepository, IRepository<Post> postRepository, IOptions<AttachmentOptions> options)
        {
            _attachmentRepository = attachmentRepository;
            _postRepository = postRepository;
            _options = options.Value;
            if (!Directory.Exists(_options.RootPath))
            {
                Directory.CreateDirectory(_options.RootPath);
            }
        }

        public async Task<AttachmentDTO> Attach(AttachmentDTO attachmentDTO, int postId)
        {
            return await AttachInternal(attachmentDTO, postId, null, false);
        }

        public async Task<AttachmentDTO> Attach(AttachmentDTO attachmentDTO, string companyCode, bool isLogo)
        {
            return await AttachInternal(attachmentDTO, null, companyCode, isLogo);
        }

        public async Task DeleteAttachment(string fileName)
        {
            (await DeleteAttachmentNoSave(fileName)).Invoke();
            await _attachmentRepository.Save();
        }

        public async Task<AttachmentDTO> GetAttachment(string fileName)
        {
            var attachment = await _attachmentRepository.Find(fileName);
            if (attachment == null)
            {
                throw new KeyNotFoundException("Attachment not found");
            }

            return new AttachmentDTO(attachment, await File.ReadAllBytesAsync(attachment.Path));
        }


        public async Task<IEnumerable<LazyAttachmentDTO>> GetLazyAttachments(int postId)
        {
            var post = await _postRepository.Find(postId);
            return post.Attachments
                .Select(a => new LazyAttachmentDTO(a));
        }

        public async Task<AttachmentDTO> UpdateAttachment(string fileName, string description)
        {
            var attachment = await _attachmentRepository.Find(fileName);
            if (attachment == null)
            {
                throw new KeyNotFoundException("Attachment not found");
            }

            if (!string.IsNullOrEmpty(description))
            {
                attachment.Edit(description);
            }

            _attachmentRepository.Update(attachment);
            await _attachmentRepository.Save();

            return new AttachmentDTO(attachment, await File.ReadAllBytesAsync(attachment.Path));
        }

        public async Task DeleteRange(string[] fileNames)
        {
            var actions = new List<Action>();
            foreach (var fileName in fileNames)
            {
                actions.Add(await DeleteAttachmentNoSave(fileName));
            }
            actions.ForEach(a => a.Invoke());
            await _attachmentRepository.Save();
        }

        private async Task<Action> DeleteAttachmentNoSave(string fileName)
        {
            var attachment = await _attachmentRepository.Find(fileName);
            if (attachment == null)
            {
                throw new KeyNotFoundException("Attachment not found");
            }
            _attachmentRepository.Delete(attachment);
            
            if (File.Exists(attachment.Path))
            {
                return () => File.Delete(attachment.Path);
            }
            return () => { };
        }
        private async Task<AttachmentDTO> AttachInternal(AttachmentDTO attachmentDTO, int? postId, string? companyCode, bool isLogo)
        {
            var filePath = string.IsNullOrWhiteSpace(attachmentDTO.Path)
                ? Path.Combine(_options.RootPath, attachmentDTO.FileName)
                : Path.Combine(_options.RootPath, attachmentDTO.Path, attachmentDTO.FileName);

            await File.WriteAllBytesAsync(filePath, attachmentDTO.File);

            Attachment attachment;
            if (postId.HasValue)
            {
                attachment = new Attachment(attachmentDTO.FileName, attachmentDTO.Description, filePath, postId.Value);
            }
            else
            {
                attachment = new Attachment(attachmentDTO.FileName, attachmentDTO.Description, filePath, companyCode, isLogo);
            }

            _attachmentRepository.Add(attachment);
            await _attachmentRepository.Save();

            attachmentDTO.Path = filePath;
            attachmentDTO.File = await File.ReadAllBytesAsync(filePath);
            return attachmentDTO;
        }
    }
}