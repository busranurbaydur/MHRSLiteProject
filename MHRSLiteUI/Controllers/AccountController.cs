﻿using MHRSLiteBusiness.EmailService;
using MHRSLiteEntity;
using MHRSLiteEntity.Enums;
using MHRSLiteEntity.IdentityModels;
using MHRSLiteUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace MHRSLiteUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IEmailSender _emailSender;
        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            CheckRoles();
        }

        //ilk rolleri vermek için burayı yazdık.
        private void CheckRoles()
        {
            
            var allRoles = Enum.GetNames(typeof(RoleNames));
            foreach (var item in allRoles)
            {
                if (!_roleManager.RoleExistsAsync(item).Result)
                {
                    var result = _roleManager.CreateAsync(new AppRole()
                    {
                        Name = item,
                        Description = item
                    }).Result;

                }
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                var checkUserForUserName = await _userManager.FindByNameAsync(model.UserName);

                if (checkUserForUserName != null)
                {
                    ModelState.AddModelError(nameof(model.UserName), "Bu kullanıcı adı zaten sistemde kayıt vardır..");
                    return View(model);
                }

                var checkUserForEmail = await _userManager.FindByEmailAsync(model.Email);

                if (checkUserForEmail != null)
                {
                    ModelState.AddModelError(nameof(model.UserName), "Bu email sistemde kayıtlıdır....");
                    return View(model);
                }

                //ifleri geçtiyse yeni kullanıcıyı oluşturalım

                AppUser newUser = new AppUser()
                {
                    Email = model.Email,
                    Name = model.Name,
                    Surname = model.Surname,
                    UserName = model.UserName,
                    //TODO : birthdate
                    Gender=model.Gender
                };


                //rol atadık yeni kişiye..hasta rolüü
                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)
                {
                    var roleResult = _userManager.AddToRoleAsync(newUser, RoleNames.Patient.ToString());
                }
                else
                {
                    ModelState.AddModelError("", "Beklenmedik Bir Hata Oluştu!");

                    return View(model);
                }

                //email gönderilecekk

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var callBackUrl = Url.Action("ConfirmEmail", "Account", new { userId = newUser.Id, code = code }, protocol: Request.Scheme);

                //email service ve appsetting işlemleri yapıldı geri dönüldü buraya

                var emailMessage = new EmailMessage()
                {
                    Contacts = new string[] { newUser.Email },
                    Subject ="MHRSLITE - Email Aktivasyonu",
                    Body=$"Merhaba {newUser.Name} {newUser.Surname}, <br/> Hesabınızı Aktifleştirmek İçin <a href='{HtmlEncoder.Default.Encode(callBackUrl)}>buraya</a> tıklayınız.."
                };

               await _emailSender.SendAsync(emailMessage);
                return RedirectToAction("Login", "Account");

            }
            catch (Exception ex)
            {

                return View();
            }

        }

    }
}