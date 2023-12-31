﻿using ClassLibrary.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordsmithWarehouse.Data;
using WordsmithWarehouse.Helpers.Interfaces;
using WordsmithWarehouse.Models;

namespace WordsmithWarehouse.Helpers.Classes
{
    public class UserHelper : IUserHelper
    {
        public readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserHelper(UserManager<User> userManager, 
            SignInManager<User> signInManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task UpdateUserTwofa(User user, string twofa)
        {
            user.Twofa = twofa;
            
            await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user,
            string oldPassword,
            string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName,
                });
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                model.RememberMe,
                false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                return await _userManager.FindByNameAsync(username);
            }

            return null;
        }

        public async Task<SignInResult> ValidatePasswordAsync(User user, string password)
        {
            return await _signInManager.CheckPasswordSignInAsync(
                user,
                password,
                false);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task DeactivateUserAsync (User user, string roleremoved)
        {
            await _userManager.RemoveFromRoleAsync(user, roleremoved);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
        {
            return await _userManager.ResetPasswordAsync(user, token, password);
        }

        public async Task<string> GetUserRole(User user)
        {
            if (await IsUserInRoleAsync(user, "customer"))
                return "Customer";
            if (await IsUserInRoleAsync(user, "employee"))
                return "Employee";
            if (await IsUserInRoleAsync(user, "admin"))
                return "Admin";
            if (await IsUserInRoleAsync(user, "deactivated"))
                return "Deactivated";

            return "No Role";
        }
    }
}
