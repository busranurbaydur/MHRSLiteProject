﻿using MHRSLiteBusiness.Contracts;
using MHRSLiteBusiness.EmailService;
using MHRSLiteEntity;
using MHRSLiteEntity.Enums;
using MHRSLiteEntity.IdentityModels;
using MHRSLiteEntity.Models;
using MHRSLiteUI.Models;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IUnitOfWork _unitOfWork;
        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager, IEmailSender emailSender, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
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
                var checkUserForUserName = await _userManager.FindByNameAsync(model.TCNumber);

                if (checkUserForUserName != null)
                {
                    ModelState.AddModelError(nameof(model.TCNumber), "Bu kullanıcı adı zaten sistemde kayıt vardır..");
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
                    var roleResult =await _userManager.AddToRoleAsync(newUser, RoleNames.Patient.ToString());
                    //patient tablosuna ekleme yapılmalıdır..
                    
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

                Patient newPatient = new Patient()
                {
                    TCNumber = model.TCNumber,
                    UserId = newUser.Id
                };
                if (_unitOfWork.PatientRepository.Add(newPatient) == false)
                {
                    //sistem yöneticisine email gitsin..


                }
                return RedirectToAction("Login", "Account");

            }
            catch (Exception ex)
            {

                return View();
            }

        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            try
            {
                if (userId==null || code==null)
                {
                    return NotFound("Sayfa Bulunamadı..");
                }
                var user = await _userManager.FindByIdAsync(userId);
                if (user==null)
                {
                    return NotFound("Kullanıcı Bulunamadı..");
                }

                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

                var result =await _userManager.ConfirmEmailAsync(user, code);
                if (result.Succeeded)
                {
                    //user pasif rolde mi
                    if (_userManager.IsInRoleAsync(user,RoleNames.Passive.ToString()).Result)
                    {
                        await _userManager.RemoveFromRoleAsync(user, RoleNames.Passive.ToString());
                        await _userManager.AddToRoleAsync(user, RoleNames.Patient.ToString());
                        
                    }

                    TempData["EmailConfirmedMessage"] = "Hesabınız Aktifleşmiştir..";

                    return RedirectToAction("Login", "Account");

                }
                ViewBag.EmailConfirmedMessage = "Hesap Aktifleştirme İşlemi Başarısızdır!..";

                return View();


            }
            catch (Exception ex)
            {
                ViewBag.EmailConfirmedMessage = "Beklenmedik Bir Hata Oluştu..Tekrar Deneyini...";

                return View();
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Veri Girişleri Düzgün Olmaıdır..");
                    return View();

                }


                var result = await _signInManager.PasswordSignInAsync(model.UserName,model.Password,model.RememberMe,true);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı Adınız veya Şifreniz Hatalıdır...");
                    return View();
                }


            }
            catch (Exception)
            {

                ModelState.AddModelError("", "Beklenmedik Bir Hata Oldu mlsf...");
                return View();
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user==null)
                {
                    ViewBag.ResetPasswordMessage = "Girdiğiniz email bulunamadı";
                    return View();
                }
                else
                {
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callBackUrl=Url.Action("ConfirmResetPassword","Account",new {userId=user.Id,code=code},protocol:Request.Scheme);

                    var emailMessage = new EmailMessage()
                    {
                        Subject="MHRSLITE - Şifremi unuttum hk.",
                        Body=$"Merhaba {user.Name} {user.Surname}, </br>Yeni parola belirlemek için  <a href='{HtmlEncoder.Default.Encode(callBackUrl)}>buraya </a>tıklayınız."
                    };
                    await _emailSender.SendAsync(emailMessage);
                    ViewBag.ResetPasswordMessage = "Emailinize şifre güncelleme yönergesi gönderilmiştir.";
                    return View();
                }

            }
            catch (Exception ex)
            {

                ViewBag.ResetPasswordMessage = "Beklenmedik bir hata oluştu!";
                return View();
            }
        }

        [HttpGet]
        public IActionResult ConfirmResetPassword(string userId, string code)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
            {
                return BadRequest("deneme");
            }

            ViewBag.UserId = userId;
            ViewBag.Code = code;
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> ConfirmResetPassword(ResetPasswordViewModel model)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı");
                    return View(model);
                }

                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));

                var result = await _userManager.ResetPasswordAsync(user, code, model.NewPassword);

                if (result.Succeeded)
                {
                    TempData["ConfirmResetPasswordMessage"] = "Şifreniz başarılı bir şekilde değiştirildi.";

                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Şifreniz değiştirilemedi..");
                    return View(model);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("","Beklenmedik bir hata oluştu..");
                return View(model);
            }
        }

    }
}
