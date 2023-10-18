using ClassLibrary.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WordsmithWarehouse.Helpers.Interfaces;
using WordsmithWarehouse.Models;
using WordsmithWarehouse.Repositories.Interfaces;

namespace WordsmithWarehouse.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForumRepository _forumRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IUserHelper _userHelper;

        public ForumController(IForumRepository forumRepository, IMessageRepository messageRepository, IUserHelper userHelper)
        {
            _forumRepository = forumRepository;
            _messageRepository = messageRepository;
            _userHelper = userHelper;
        }

        public async Task<IActionResult> Index()
        {
            var forums = _forumRepository.GetAll();

            var forumModel = new List<ForumViewModel>();

            await forums.ForEachAsync(f =>
            {
                forumModel.Add(new ForumViewModel { 
                    Id = f.Id,
                    Title = f.Title,
                });
            });

            return View(forumModel);
        }

        public async Task<IActionResult> Topic(int id)
        {
            var forum = await _forumRepository.GetForumById(id);

            var messages = await _messageRepository.GetMessagesByForumId(forum.Id);

            var messageModel = new List<MessageViewModel>();

            messages.ForEach(m =>
            {
                messageModel.Add(new MessageViewModel
                {
                    Content = m.Content,
                });
            });

            var topicModel = new TopicViewModel
            {
                Id = forum.Id,
                Title = forum.Title,
                Description = forum.Description,
                Messages = messageModel,
            };


            return View(topicModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTopicViewModel model)
        {
            var user = await _userHelper.GetUserByUsernameAsync(User.Identity.Name);

            await _forumRepository.CreateAsync(new Forum
            {
                Title = model.Title,
                Description = model.Description,
                CreateDate = DateTime.Now,
            });

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Topic(TopicViewModel model)
        {
            var user = await _userHelper.GetUserByUsernameAsync(User.Identity.Name);

            await _messageRepository.CreateAsync(new Message
            {
                Content = model.NewMessageContent,
                ForumId = model.Id,
                UserId = user.Id,

            });

            return await Topic(model.Id);
        }
    }
}
