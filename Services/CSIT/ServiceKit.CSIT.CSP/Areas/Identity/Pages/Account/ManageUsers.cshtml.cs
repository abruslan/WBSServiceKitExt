using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using ServiceKit.CSIT.CSP.Common;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ServiceKit.CSIT.CSP.Pages.Account
{
    [Authorize(Roles = BuiltinRoles.Administrator)]
    public class ManageUsersModel : PageModel
    {
        public readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IQueryable<IdentityUser> Users;

        public ManageUsersModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            Users = _userManager.Users;
            _roleManager = roleManager;
        }

        public bool UserIsAdminAsync(IdentityUser user)
        {
            Task<bool> task = Task.Run<bool>(async () => await _userManager.IsInRoleAsync(user, BuiltinRoles.Administrator));
            return task.Result;
        }

        public async Task<IActionResult> OnPostAsync(string user, string action, int value)
        {
            var evalUser = await _userManager.FindByEmailAsync(user);
            if (evalUser == null)
            {
                return NotFound($"Невозможно загрузить пользователя с ID '{user}'.");
            }
            switch(action)
            {
                case "emailconfirm":
                    evalUser.EmailConfirmed = value == 1;
                    await _userManager.UpdateAsync(evalUser);
                    break;
                case "setadmin":
                    if (! await _roleManager.RoleExistsAsync(BuiltinRoles.Administrator))
                        await _roleManager.CreateAsync(new IdentityRole(BuiltinRoles.Administrator));

                    if (value == 1)
                        await _userManager.AddToRoleAsync(evalUser, BuiltinRoles.Administrator);
                    else
                        await _userManager.RemoveFromRoleAsync(evalUser, BuiltinRoles.Administrator);
                    break;
                case "delete":
                    await _userManager.DeleteAsync(evalUser);
                    break;
            }
            return RedirectToPage();
        }

    }
}
