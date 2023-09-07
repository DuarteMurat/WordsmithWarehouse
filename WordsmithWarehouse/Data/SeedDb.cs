﻿using ClassLibrary.Data;
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

            var user = await _userHelper.GetUserByEmailAsync("bf0teste@gmail.com");
            if (user == null)
            { 
                user = new User
                {
                    FirstName = "Admin",
                    LastName = "WordsmithWarehouse",
                    Email = "bf0teste@gmail.com",
                    UserName = "Admin",
                    PhoneNumber = "987654321"
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
            }

            if (!_context.Books.Any())
            {
                AddBook("Book 1");
                AddBook("Book 2");
                AddBook("Book 3");
                AddBook("Book 4");
                await _context.SaveChangesAsync();
            }

            //if (!_context.Tags.Any())
            //{
            //    AddTag("Fantasy", 1);
            //    AddTag("Sci-Fi", 2);
            //    await _context.SaveChangesAsync();
            //}
        }

        private void AddBook(string title)
        {
            _context.Books.Add(new Book
            {
                Title = title,
                ISBN = "0000000000",
                Author = "Ninguém",
            });
        }

        private void AddTag(string title, int id)
        {
            _context.Tags.Add(new Tag
            {
                Name = title,
                isActive = false,
            });
        }
    }
}
