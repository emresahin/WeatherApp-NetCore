using Business.DataAccess;
using Business.DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class UserBusinessTest : BaseTest
    {
        private IUserBusiness _userBusiness;
        private RepositoryContextManager _repositoryContextManager;
        [SetUp]
        public void Setup()
        {
            _userBusiness = ServiceProvider.GetRequiredService<IUserBusiness>();
            _repositoryContextManager = ServiceProvider.GetRequiredService<RepositoryContextManager>();
        }


        [Test]
        public void TestMethod_FailLogin()
        {

            var result = _userBusiness.LoginAsync("test@test.com", "111").Result;

            Assert.AreEqual(false, result.IsSuccess);
        }

        [Test]
        public async Task TestMethod_UserRegister()
        {
            var testUser = await _repositoryContextManager.Repository.GetQueryable<UserEntity>(p => p.Email == "testmethod@test.com" && p.IsActive).FirstOrDefaultAsync();
            if (testUser != null)
            {
                await _repositoryContextManager.Repository.DeleteAsync<UserEntity>(testUser);
            }

            var result = await _userBusiness.RegisterAsync("testmethod@test.com", "123");

            Assert.AreEqual(true, result.IsSuccess);
        }


        [Test]
        public async Task TestMethod_SuccessLogin()
        {
            var testUser = await _repositoryContextManager.Repository.GetQueryable<UserEntity>(p => p.Email == "testmethod@test.com" && p.IsActive).FirstOrDefaultAsync();
            if (testUser != null)
            {
                await _repositoryContextManager.Repository.DeleteAsync<UserEntity>(testUser);
            }

            _ = await _userBusiness.RegisterAsync("testmethod@test.com", "123");

            var result = await _userBusiness.LoginAsync("testmethod@test.com", "123");

            Assert.AreEqual(true, result.IsSuccess);
        }

        [Test]
        public void TestMethod_UserRegisterAllready()
        {
            var testUser = _repositoryContextManager.Repository.GetQueryable<UserEntity>(p => p.Email == "testmethod@test.com" && p.IsActive).FirstOrDefault();
            if (testUser == null)
            {
                _ = _userBusiness.RegisterAsync("testmethod@test.com", "123").Result;
            }

            var result = _userBusiness.RegisterAsync("testmethod@test.com", "123").Result;

            Assert.AreEqual(false, result.IsSuccess);
        }

        [Test]
        public void TestMethod_AddFavoriteCity()
        {
            var testUser = _repositoryContextManager.Repository.GetQueryable<UserEntity>(p => p.Email == "testmethod@test.com" && p.IsActive).FirstOrDefault();
            if (testUser == null)
            {
                _ = _userBusiness.RegisterAsync("testmethod@test.com", "123").Result;
                testUser = _repositoryContextManager.Repository.GetQueryable<UserEntity>(p => p.Email == "testmethod@test.com" && p.IsActive).FirstOrDefault();
            }

            var result = _userBusiness.AddFavoriteCity(testUser.Id, 1).Result;

            Assert.AreEqual(true, result.IsSuccess);
        }


        [Test]
        public void TestMethod_RemoveFavoriteCity()
        {
            var testUser = _repositoryContextManager.Repository.GetQueryable<UserEntity>(p => p.Email == "testmethod@test.com" && p.IsActive).FirstOrDefault();
            if (testUser == null)
            {
                _ = _userBusiness.RegisterAsync("testmethod@test.com", "123").Result;
                testUser = _repositoryContextManager.Repository.GetQueryable<UserEntity>(p => p.Email == "testmethod@test.com" && p.IsActive).FirstOrDefault();
            }

            var favoriteCity = _repositoryContextManager.Repository.GetQueryable<UserCityEntity>(p => p.UserId == testUser.Id && p.CityId == 1 && p.IsActive).FirstOrDefault();
            if (favoriteCity == null)
            {
                _ = _userBusiness.AddFavoriteCity(testUser.Id, 1).Result;
            }

            var result = _userBusiness.RemoveFavoriteCity(testUser.Id, 1).Result;

            Assert.AreEqual(true, result.IsSuccess);
        }
    }
}
