﻿using AutoMapper;
using DaisyStudy.Application.Catalog.Classes;
using DaisyStudy.Application.System.Users;
using DaisyStudy.Models;
using DaisyStudy.Data;
using DaisyStudy.Models.Catalog.Classes;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using DaisyStudy.Models.System.Users;

namespace DaisyStudy.Controllers
{
    public class HomeController : Controller
    {
        private readonly IClassService _classService;
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public HomeController(IClassService classService,
                              ApplicationDbContext context,
                              IUserService userService,
                              SignInManager<ApplicationUser> signInManager,
                              IMapper mapper)
        {
            _classService = classService;
            _context = context;
            _userService = userService;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string keyword,
                                         int pageIndex = 1,
                                         int pageSize = 10)
        {
            if (User.Identity.Name != null)
            {
                var _user = await _userService.GetByName(User.Identity.Name);
                if (_user != null)
                {
                    var user = _mapper.Map<UserViewModel, ApplicationUser>(_user);
                    var userRoles = await _signInManager.UserManager.GetRolesAsync(user);

                    bool isAdmin = false;
                    foreach (string item in userRoles)
                    {
                        if (item == "Administrator") ;
                        isAdmin = true;
                    }
                    if (isAdmin == true)
                    {
                        HttpContext.Session.SetString("RoleName", "Administrator");
                    }
                    else
                    {
                        HttpContext.Session.SetString("RoleName", "User");
                    }

                    HttpContext.Session.SetString("UserId", _user.Id);
                    if (!string.IsNullOrEmpty(_user.Avatar) == true)
                        HttpContext.Session.SetString("Avatar", _user.Avatar);
                    else
                        HttpContext.Session.SetString("Avatar", "");

                    if (!string.IsNullOrEmpty(_user.LastName) == true)
                        HttpContext.Session.SetString("LastName", _user.LastName);
                }
            }


            var request = new ClassPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _classService.GetAllClassPagingHome(request);

            ViewBag.Keyword = keyword;
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }

            return View(data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}