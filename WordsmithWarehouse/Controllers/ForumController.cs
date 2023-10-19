using ClassLibrary.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Linq;
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

            var users = await _userHelper.GetAllAsync();

            foreach (var f in forums)
            {
                var forumUser =users.FirstOrDefault(u => u.Id == f.UserId);
                forumModel.Add(new ForumViewModel { 
                    Id = f.Id,
                    Title = f.Title,
                    Username = forumUser.UserName,
                    CreateDate = f.CreateDate.ToString("dd/MM/yy H:mm"),
                });
            }

            return View(forumModel);
        }

        public async Task<IActionResult> Topic(int id)
        {
            var forum = await _forumRepository.GetForumById(id);

            var messages = await _messageRepository.GetMessagesByForumId(forum.Id);

            var usertopic = await _userHelper.GetUserByIdAsync(forum.UserId);

            var messageModel = new List<MessageViewModel>();

            foreach(var m in messages)
            {
                var usermessage = await _userHelper.GetUserByIdAsync(m.UserId);
                messageModel.Add(new MessageViewModel
                {
                    Content = m.Content,
                    Username = usermessage.UserName,
                    
                });
            }

            var topicModel = new TopicViewModel
            {
                Id = forum.Id,
                Title = forum.Title,
                Description = forum.Description,
                Messages = messageModel,
                Username= usertopic.UserName,
            };


            return View(topicModel);
        }

        [Authorize(Roles = "Admin,Employee,Customer")]
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
                UserId = user.Id,
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
