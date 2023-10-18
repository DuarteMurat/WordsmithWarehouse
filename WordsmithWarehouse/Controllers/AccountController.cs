using ClassLibrary.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WordsmithWarehouse.Helpers.Classes;
using WordsmithWarehouse.Helpers.Interfaces;
using WordsmithWarehouse.Models;

namespace WordsmithWarehouse.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;
        private readonly IConfiguration _configuration;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;

        public AccountController(IUserHelper userHelper,
            IImageHelper imageHelper,
            IConfiguration configuration,
            IMailHelper mailHelper,
            IConverterHelper converterHelper)
        {
            _userHelper = userHelper;
            _imageHelper = imageHelper;
            _configuration = configuration;
            _mailHelper = mailHelper;
            _converterHelper = converterHelper;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByUsernameAsync(model.Username);
                if (user == null)
                {
                    this.ModelState.AddModelError(string.Empty, "Failed to login!");
                    return View(model);
                }

                if (await _userHelper.IsUserInRoleAsync(user, "Deactivated"))
                {
                    this.ModelState.AddModelError(string.Empty, "Failed to login!");
                    return View(model);
                }
                var result = await _userHelper.CheckPasswordAsync(user, model.Password);
                if (result)
                {
                    if (model.Username == "Admin" || model.Username == "admin")
                    {
                        var loginstatus = await _userHelper.LoginAsync(model);

                        if (loginstatus.Succeeded)
                        {
                            if (this.Request.Query.Keys.Contains("ReturnUrl"))
                            {
                                return Redirect(this.Request.Query["ReturnUrl"].First());
                            }

                            return this.RedirectToAction("Index", "Home");
                        }
                    }
                    if (string.IsNullOrEmpty(model.Twofa))
                    {
                        var random = new Random();

                        var twofa = random.Next(10000, 99999);

                        await _mailHelper.SendEmail(user.Email, "Your Two-Factor Authentication (2FA) Code for Login",
                        $"Dear {model.Username}, <br/>" +
                        $"You are receiving this email because you recently requested to log in to your <b>WordsmithWarehouse</b> account. To ensure the security of your account, we have implemented Two-Factor Authentication (2FA), and here is your unique login code:<br/>" +
                        $"<b>{twofa}</b>, <br/>" +
                        $"If you did not initiate this login request or suspect any unauthorized access to your account, please contact our support team immediately at <a>wordsmithwarehouse@outlook.pt</a>.<br/>" +
                        $"Thank you for choosing <b>WordsmithWarehouse</b>. We appreciate your trust in us.<br/>" +
                        $"Best regards,<br/><br/>" +
                        $"WordsmithWarehouse.");

                        await _userHelper.UpdateUserTwofa(user, twofa.ToString());
                        model.IsTwofa = true;

                        return View(model);
                    }
                    else
                    {
                        if (model.Twofa != user.Twofa)
                        {
                            this.ModelState.AddModelError(string.Empty, "Incorrect 2fa code!");

                            model.Twofa = string.Empty;
                            model.IsTwofa = true;
                            return View(model);
                        }
                        var loginstatus = await _userHelper.LoginAsync(model);

                        if (loginstatus.Succeeded)
                        {
                            if (this.Request.Query.Keys.Contains("ReturnUrl"))
                            {
                                return Redirect(this.Request.Query["ReturnUrl"].First());
                            }

                            return this.RedirectToAction("Index", "Home");
                        }
                    }
                }
            }
            this.ModelState.AddModelError(string.Empty, "Failed to login!");

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    var path = string.Empty;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "Users");
                    else
                    {
                        path = "/images/Users/notfound.png";
                    };

                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserName = model.Username,
                        ImageURL = path,
                        Address = model.Address,
                        PhoneNumber = model.PhoneNumber,
                        Email = model.Email,
                    };

                    var result = await _userHelper.AddUserAsync(user, model.Password);
                    await _userHelper.AddUserToRoleAsync(user, "Customer");
                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The user couldn't be created.");
                        return View(model);
                    }

                    string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                    string tokenLink = Url.Action("ConfirmEmail", "Account", new
                    {
                        userid = user.Id,
                        token = myToken
                    }, protocol: HttpContext.Request.Scheme);

                    Response response = await _mailHelper.SendEmail(model.Email, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                       $"To allow the user, " +
                       $"plase click in this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>");

                    if (response.IsSuccess)
                    {
                        ViewBag.Message = "The instructions to allow you user has been sent to email";
                        return View(model);
                    }


                    ModelState.AddModelError(string.Empty, "The user couldn't be registered.");
                    return View(model);
                }
            }
            return View(model);
        }

        [Authorize(Roles = "Admin, Employee, Customer")]
        public async Task<IActionResult> ChangeUser()
        {
            var user = await _userHelper.GetUserByUsernameAsync(this.User.Identity.Name);
            if (user == null)
            {
                return View();
            }
            var model = new ChangeUserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName,
                ImageURL = user.ImageURL,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            var path = string.Empty;
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByUsernameAsync(this.User.Identity.Name);
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.UserName = model.Username;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Address = model.Address;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "Users");
                        user.ImageURL = path;
                    }


                    var response = await _userHelper.UpdateUserAsync(user);
                    if (response.Succeeded)
                    {
                        ViewBag.UserMessage = "User updated!";
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin, Employee, Customer")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return this.RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "User not found.");
                }
            }

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByUsernameAsync(model.Username);
                if (user != null)
                {
                    var result = await _userHelper.ValidatePasswordAsync(
                        user,
                        model.Password);

                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Tokens:Issuer"],
                            _configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(15),
                            signingCredentials: credentials);
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return this.Created(string.Empty, results);

                    }
                }
            }

            return BadRequest();
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {

            }

            return View();

        }

        public IActionResult NotAuthorized()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _userHelper.GetAllAsync();

            var usersConverted = await _converterHelper.BulkConvertToManageUserViewModel(users);

            return View(usersConverted);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreateUsers()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUsers(RegisterNewUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "Books");
                }

                else
                {
                    path = "/images/Users/notfound.png";
                };

                var user = _converterHelper.ConvertToUser(model, path, false);

                user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.Username,
                    Email = model.Email,
                    ImageURL = path,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                };

                var result = await _userHelper.AddUserAsync(user, model.Password);
                await _userHelper.AddUserToRoleAsync(user, "Employee");
                if (result != IdentityResult.Success)
                {
                    ModelState.AddModelError(string.Empty, "The user couldn't be created.");
                    return View(model);
                }

                string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                string tokenLink = Url.Action("ConfirmEmail", "Account", new
                {
                    userid = user.Id,
                    token = myToken
                }, protocol: HttpContext.Request.Scheme);

                Response response = await _mailHelper.SendEmail(model.Email, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                   $"Hi \"{model.FirstName}\", congrats on joining our team, here is your information to login in into our platform <br/><br/>" +
                   $"Username:\"{model.Username}\" <br/><br/>" +
                   $"Password:\"{model.Password}\" <br/><br/>" +
                   $"plase click in this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>");

                if (response.IsSuccess)
                {
                    ViewBag.Message = "The instructions to allow you user has been sent to email";
                    return View(model);
                }


                ModelState.AddModelError(string.Empty, "The user couldn't be registered.");
                return View(model);
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string username)
        {
            if (username == null)
            {
                return View();
            }
            var user = await _userHelper.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return new NotFoundViewResult("BookNotFound");
            }

            var convertedUser = _converterHelper.ConvertToManageUserViewModel(user);

            return View(convertedUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string username, string roleName)
        {
            var user = await _userHelper.GetUserByUsernameAsync(username);

            if (await _userHelper.IsUserInRoleAsync(user, "Customer"))
            {
                await _userHelper.DeactivateUserAsync(user, "Customer");
                await _userHelper.AddUserToRoleAsync(user, "Deactivated");
            }

            return RedirectToAction(nameof(ManageUsers));
        }

        public IActionResult RecoverPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "The email doesn't correspont to a registered user.");
                    return View(model);
                }

                var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);

                var link = this.Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token = myToken }, protocol: HttpContext.Request.Scheme);

                Response response = await _mailHelper.SendEmail(model.Email, "Shop Password Reset", $"<h1>Shop Password Reset</h1>" +
                $"To reset the password click in this link:</br></br>" +
                $"<a href = \"{link}\">Reset Password</a>");

                if (response.IsSuccess)
                {
                    this.ViewBag.Message = "The instructions to recover your password has been sent to email.";
                }

                return this.View();

            }

            return this.View(model);
        }

        [Authorize(Roles = "Admin, Employee, Customer")]
        public async Task<IActionResult> UserDetails(string Username)
        {
            if (Username == null)
            {
                return new NotFoundViewResult("BookNotFound");
            }

            var user = await _userHelper.GetUserByUsernameAsync(Username);

            var model = _converterHelper.ConvertToManageUserViewModel(user);
            model.Role = await _userHelper.GetUserRole(user);


            return View(model);

        }

        [Authorize(Roles = "Admin, Employee, Customer")]
        public IActionResult ResetPassword(string token)
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userHelper.GetUserByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    this.ViewBag.Message = "Password reset successful.";
                    return View();
                }

                this.ViewBag.Message = "Error while resetting the password.";
                return View(model);
            }

            this.ViewBag.Message = "User not found.";
            return View(model);
        }

        public async Task<ActionResult> GetImage()
        {
            string path = string.Empty;
            if (this.User.Identity.IsAuthenticated)
            {
                var user = await _userHelper.GetUserByUsernameAsync(this.User.Identity.Name);
                if (user != null)
                {
                    path = user.ImageURL;
                }
                return base.File(path, "image/jpeg");
            }
            return Content(path);
        }
    }
}