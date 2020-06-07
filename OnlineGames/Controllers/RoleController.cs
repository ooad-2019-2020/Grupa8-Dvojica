using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineGames.Models;

namespace OnlineGames.Controllers
{
    public class RoleController : Controller
    {
        private RoleManager<IdentityRole> roleManager;
        private UserManager<IdentityUser> userManager;
        public RoleController(RoleManager<IdentityRole> roleMng, UserManager<IdentityUser> userMng)
        {
            roleManager = roleMng;
            userManager = userMng;
        }
        
        [Authorize(Roles = "Glavni")]
        public ViewResult Index() => View(roleManager.Roles);

        //[Authorize(Roles = "Glavni")]
        //public IActionResult Create() => View();

        //[Authorize(Roles = "Glavni")]
        //[HttpPost]
        //public async Task<IActionResult> Create([Required]string name)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
        //        if (result.Succeeded)
        //            return RedirectToAction("Index");
        //        else
        //            Errors(result);
        //    }
        //    return View(name);
        //}

        //[Authorize(Roles = "Glavni")]
        //[HttpPost]
        //public async Task<IActionResult> Delete(string id)
        //{
        //    IdentityRole role = await roleManager.FindByIdAsync(id);
        //    if (role != null)
        //    {
        //        IdentityResult result = await roleManager.DeleteAsync(role);
        //        if (result.Succeeded)
        //            return RedirectToAction("Index");
        //        else
        //            Errors(result);
        //    }
        //    else
        //        ModelState.AddModelError("", "No role found");
        //    return View("Index", roleManager.Roles);
        //}

        [Authorize(Roles = "Glavni")]
        public async Task<IActionResult> Update(string id, string searchFilter)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            List<IdentityUser> members = new List<IdentityUser>();
            List<IdentityUser> nonMembers = new List<IdentityUser>();
            
            ViewData["Pretraga"] = searchFilter;

            foreach (IdentityUser user in userManager.Users)
            {
                var list = await userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                list.Add(user);
            }

            if (!String.IsNullOrEmpty(searchFilter))
            {
                var saUlogom = new List<IdentityUser>();
                var bezUloge = new List<IdentityUser>();

                foreach (var item in members)
                {
                    if (item.Email.Contains(searchFilter))
                    {
                        saUlogom.Add(item);
                    }
                }

                foreach(var item in nonMembers)
                {
                    if (item.Email.Contains(searchFilter))
                    {
                        bezUloge.Add(item);
                    }
                }

                return View(new RoleEdit
                {
                    Role = role,
                    Members = saUlogom,
                    NonMembers = bezUloge
                }) ;
            }

            return View(new RoleEdit
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [Authorize(Roles = "Glavni")]
        [HttpPost]
        public async Task<IActionResult> Update(RoleModification model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.AddIds ?? new string[] { })
                {
                    IdentityUser user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                            Errors(result);
                    }
                }
                foreach (string userId in model.DeleteIds ?? new string[] { })
                {
                    IdentityUser user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                            Errors(result);
                    }
                }
            }

            if (ModelState.IsValid)
                return RedirectToAction(nameof(Index));
            else
                return await Update(model.RoleId, null);
        }

        [Authorize(Roles = "Glavni")]
        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}