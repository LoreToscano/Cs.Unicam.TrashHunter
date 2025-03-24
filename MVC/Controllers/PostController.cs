using Cs.Unicam.TrashHunter.Models.Entities;
using Cs.Unicam.TrashHunter.MVC.Extensions;
using Cs.Unicam.TrashHunter.MVC.Models.ViewModels;
using Cs.Unicam.TrashHunter.Services.Abstractions.Services;
using Cs.Unicam.TrashHunter.Services.Services.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cs.Unicam.TrashHunter.MVC.Controllers
{
    public class PostController : BaseController
    {
        private readonly IPostService _postService;
        public PostController(IPostService postService, IServiceScopeFactory factory) : base(factory)
        {
            _postService = postService;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _postService.GetPostsAsync(new PostFilters(), 1, int.MaxValue - 1);
            posts.Items = posts.Items
                .Where(p => (p.IsApproved == true && !User.IsInRole(Role.Checker)) || p.User.Email == User.Identity.Name || (p.IsApproved == null && User.IsInRole(Role.Checker)) || User.IsInRole(Role.Admin))
                .ToList();
            return View(
                new PostViewModel(posts)
            );
        }

        public IActionResult Create(PostViewModel post)
        {
            return null;
        }

        public async Task<IActionResult> Evaluate(int postId, bool approve)
        {
            await _postService.Approve(postId, approve);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Complete(int postId)
        {
            await _postService.Complete(postId, User.Identity.Name);
            return RedirectToAction(nameof(Index));
        }
    }
}
