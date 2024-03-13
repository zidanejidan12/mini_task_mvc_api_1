﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyWebFormApp.BLL.DTOs;
using MyWebFormApp.BLL.Interfaces;
using System.Text.Json;

namespace SampleMVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserBLL _userBLL;
        private readonly IRoleBLL _roleBLL;

        public UsersController(IUserBLL userBLL, IRoleBLL roleBLL)
        {
            _userBLL = userBLL;
            _roleBLL = roleBLL;
        }

        public IActionResult ManageUserRoles()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
            }

            var users = _userBLL.GetAll();
            var listUsers = new SelectList(users, "Username", "Username");
            ViewBag.Users = listUsers;

            var roles = _roleBLL.GetAllRoles();
            var listRoles = new SelectList(roles, "RoleID", "RoleName");
            ViewBag.Roles = listRoles;

            var usersWithRoles = _userBLL.GetAllWithRoles();
            return View(usersWithRoles);
        }

        [HttpPost]
        public IActionResult ManageUserRoles(string Username, int RoleID)
        {
            try
            {
                _roleBLL.AddUserToRole(Username, RoleID);
                TempData["Message"] = @"<div class='alert alert-success'><strong>Success!&nbsp;</strong>Role added successfully !</div>";
            }
            catch (Exception ex)
            {
                TempData["Message"] = @"<div class='alert alert-danger'><strong>Error!&nbsp;</strong>" + ex.Message + "</div>";
            }
            return RedirectToAction("ManageUserRoles");
        }

        public IActionResult Login()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                var userDto = _userBLL.LoginMVC(loginDTO);
                //simpan username ke session
                var userDtoSerialize = JsonSerializer.Serialize(userDto);
                HttpContext.Session.SetString("user", userDtoSerialize);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Message = @"<div class='alert alert-danger'><strong>Error!&nbsp;</strong>" + ex.Message + "</div>";
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("user");
            return RedirectToAction("Login");
        }

        //register user baru
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserCreateDTO userCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                _userBLL.Insert(userCreateDto);
                ViewBag.Message = @"<div class='alert alert-success'><strong>Success!&nbsp;</strong>Registration process successfully !</div>";

            }
            catch (Exception ex)
            {
                ViewBag.Message = @"<div class='alert alert-danger'><strong>Error!&nbsp;</strong>" + ex.Message + "</div>";
            }

            return View();
        }

        public IActionResult Profile()
        {
            try
            {
                // Retrieve username from session
                var userDtoSerialize = HttpContext.Session.GetString("user");
                
                if (userDtoSerialize == null)
                {
                    // Redirect to login if session user is not found
                    return RedirectToAction("Login");
                }

                // Deserialize userDto from session
                var userDto = JsonSerializer.Deserialize<UserDTO>(userDtoSerialize);

                // Retrieve user information with roles based on the username
                var userWithRoles = _userBLL.GetUserWithRoles(userDto.Username);

                if (userWithRoles == null)
                {
                    // Handle case where user is not found
                    ViewBag.Message = "<div class='alert alert-danger'><strong>Error!&nbsp;</strong>User not found.</div>";
                    return View();
                }

                // Pass first name and last name to the view
                ViewBag.FirstName = userWithRoles.FirstName;
                ViewBag.LastName = userWithRoles.LastName;

                // Return the profile view
                return View();
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                ViewBag.Message = @"<div class='alert alert-danger'><strong>Error!&nbsp;</strong>" + ex.Message + "</div>";
                return View();
            }
        }
    }
}
