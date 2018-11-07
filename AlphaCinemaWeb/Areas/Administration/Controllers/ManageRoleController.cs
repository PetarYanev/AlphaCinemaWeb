﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaCinemaServices.Contracts;
using AlphaCinemaWeb.Areas.Administration.Models.ManageRoleViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AlphaCinemaWeb.Areas.Administration.Controllers
{
	public class ManageRoleController : Controller
	{
		private readonly IUserService userService;

		public ManageRoleController(IUserService userService)
		{
			this.userService = userService;
		}
		// GET: /<controller>/
		[Area("Administration")]
		[Authorize(Roles = "Administrator")]
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var users = await this.userService.GetAllUsers();
			var userViewModels = users.Select(u => new UserViewModel(u)).ToList();
			foreach (var user in userViewModels)
			{
				if (await userService.IsUserAdmin(user.Id, "Administrator"))
				{
					user.IsAdmin = true;
				}
			}
			var model = new UsersListViewModel(userViewModels.Select(u => u));
			
			return View(model);
		}

		[Area("Administration")]
		[Authorize(Roles = "Administrator")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SetAdmin(string userId)
		{
			var user = await this.userService.GetUser(userId);
			if (user == null)
			{
				this.TempData["Error-Message"] = $"User does not exist!";
				return this.RedirectToAction("ManageRole", "Index");
			}
			await this.userService.SetRole(user.Id, "Administrator");

			this.TempData["Success-Message"] = $"You successfully made [{user.UserName}] administrator!";

			return this.RedirectToAction("Index", "ManageRole");
		}

		[Area("Administration")]
		[Authorize(Roles = "Administrator")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> RemoveAdmin(string userId)
		{
			var user = await this.userService.GetUser(userId);
			if (user == null)
			{
				this.TempData["Error-Message"] = $"User does not exist!";
				return this.RedirectToAction("ManageRole", "Index");
			}
			
			await this.userService.RemoveRole(user.Id, "Administrator");

			this.TempData["Success-Message"] = $"You successfully removed [{user.UserName}] from administrator!";

			return this.RedirectToAction("Index", "ManageRole");
		}
	}
}
