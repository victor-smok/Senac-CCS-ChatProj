using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using DemoChat.Models;

namespace DemoChat.Controllers
{
    public class AccountController : Controller
    {

        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        // **************************************
        // URL: /Account/LogOn
        // **************************************

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                // Utilizando o Modo LoOnModel com a camada do Entity
                usersEntities context = new usersEntities();
                var usuario = context.usuarios.Where(c => c.userName == model.UserName && c.password == model.Password).FirstOrDefault();

                if (usuario != null)
                {
                    Session["Usuario"] = usuario.userName;

                    return RedirectToAction("Index", "Home");
                }
                else
                {

                    ModelState.AddModelError(string.Empty, "Usuario ou senha incorreto.");
                    //return RedirectToAction("LogOn", "Account");
                    //throw new Exception("Usuario ou senha incorreto");
                }
            }

            return View(model);
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************

        public ActionResult LogOff()
        {
            FormsService.SignOut();

            return RedirectToAction("Index", "Home");
        }

        // **************************************
        // URL: /Account/Register
        // **************************************

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Registrar usuários
                usersEntities context = new usersEntities();
                var usuario = context.usuarios.Where(c => c.userName == model.UserName).FirstOrDefault();
                
                // Se o usuário a ser cadastrado for diferente
                if (usuario == null)
                {                   
                    usuarios cadUser = new usuarios();
                    cadUser.userName = model.UserName;
                    cadUser.password = model.Password;
                    cadUser.email = model.Email;
                    cadUser.dataCadastro = DateTime.Now;
                    context.AddTousuarios(cadUser);
                    context.SaveChanges();
                    
                    return RedirectToAction("LogOn", "Account");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Usuário já existe");
                    //return RedirectToAction("Register", "Account");
                }
            }
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePassword
        // **************************************

        [Authorize]
        public ActionResult ChangePassword()
        {
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePasswordSuccess
        // **************************************

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

    }
}
