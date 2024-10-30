using Business.DataAccess.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserBusiness _userBusiness;
        public AccountController(IUserBusiness userBusiness)
        {
            _userBusiness = userBusiness;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var loginResult = await _userBusiness.LoginAsync(email, password);
            if (loginResult.IsSuccess)
            {
                HttpContext.Response.Cookies.Append("UserLogin", JsonSerializer.Serialize(loginResult.Result), new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddMinutes(30),
                    HttpOnly = true,
                });
                return RedirectToAction("MyFavorite");
            }
            return View(loginResult);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string email, string password)
        {
            var registerResult = await _userBusiness.RegisterAsync(email, password);
            if (registerResult.IsSuccess)
            {
                HttpContext.Response.Cookies.Append("UserLogin", JsonSerializer.Serialize(registerResult.Result), new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddMinutes(30),
                    HttpOnly = true,
                });

                return RedirectToAction("MyFavorite");
            }

            return View(registerResult);
        }


        public async Task<IActionResult> MyFavorite()
        {
            if (HttpContext.Request.Cookies.TryGetValue("UserLogin", out var userInfoJson))
            {
                var userInfo = JsonSerializer.Deserialize<UserModel>(userInfoJson);
                var myFavoriteResult = await _userBusiness.GetFavoriteCities(userInfo.Id);
                return View(myFavoriteResult);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public async Task<JsonResult> AddFavorite(int cityId)
        {
            if (HttpContext.Request.Cookies.TryGetValue("UserLogin", out var userInfoJson))
            {
                var userInfo = JsonSerializer.Deserialize<UserModel>(userInfoJson);
                var addFavoriteResult = await _userBusiness.AddFavoriteCity(userInfo.Id, cityId);
                return Json(new { Success = addFavoriteResult.IsSuccess });
            }
            else
            {
                return Json(new { Success = false, Message = "Oturum açınız" });
            }
        }

        [HttpPost]
        public async Task<JsonResult> RemoveFavorite(int cityId)
        {
            if (HttpContext.Request.Cookies.TryGetValue("UserLogin", out var userInfoJson))
            {
                var userInfo = JsonSerializer.Deserialize<UserModel>(userInfoJson);
                var addFavoriteResult = await _userBusiness.RemoveFavoriteCity(userInfo.Id, cityId);
                return Json(new { Success = addFavoriteResult.IsSuccess });
            }
            else
            {
                return Json(new { Success = false, Message = "Oturum açınız" });
            }
        }
    }
}
