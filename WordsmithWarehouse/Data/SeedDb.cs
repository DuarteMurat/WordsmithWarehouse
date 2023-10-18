using ClassLibrary.Data;
using ClassLibrary.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using WordsmithWarehouse.Helpers.Interfaces;

namespace WordsmithWarehouse.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Customer");
            await _userHelper.CheckRoleAsync("Employee");
            await _userHelper.CheckRoleAsync("Deactivated");

            var user = await _userHelper.GetUserByEmailAsync("bf0teste@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Admin",
                    LastName = "WordsmithWarehouse",
                    Email = "bf0teste@gmail.com",
                    UserName = "Admin",
                    PhoneNumber = "987654321",
                    Address = "Rua de Almada",
                    ImageURL= "/images/Users/notfound.png",
                    EmailConfirmed = true,
                };

                var result = await _userHelper.AddUserAsync(user, "123456");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");
            if (!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            if (!_context.Authors.Any())
            {
                AddAuthor("Duarte Marques");

                await _context.SaveChangesAsync();
            }

            if (!_context.Books.Any())
            {
                AddBook("Book 1");
                AddBook("Book 2");
                AddBook("Book 3");

                await _context.SaveChangesAsync();
            }

            if (!_context.Tags.Any())
            {
                AddTag("New Release");

                await _context.SaveChangesAsync();
            }

            if (!_context.Forums.Any())
            {
                await _context.Forums.AddAsync(new Forum
                {
                    Title = "Teste",
                    Description = "This is a test site",
                    UserId = user.Id,
                });

                await _context.SaveChangesAsync();
            }

            if (!_context.Messages.Any())
            {
                await _context.Messages.AddAsync(new Message
                {
                    ForumId = 1,
                    Content = "This is a test site",
                    UserId = user.Id,
                });

                await _context.SaveChangesAsync();
            }
        }

        private void AddBook(string title)
        {
            _context.Books.Add(new Book
            {
                Title = title,
                ISBN = "0000000000",
                AuthorId = 1,
                ImageURL = "/images/Books/notfound.png",
            });
        }

        private void AddAuthor(string Name) 
        {
            _context.Authors.Add(new Author
            {
                Name = Name,
                Description = "Olá",
                ImageURL = "/images/Authors/unknown.png",
            });
        }

        private void AddTag(string Name)
        {
            _context.Tags.Add(new Tag 
            { 
                Name = Name,
                isActive = false,
                isAdmin = true,
            });
        }
    }
}

