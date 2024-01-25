using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Data;
using RunGroupWebApp.Models;
using RunGroupWebApp.Repository.Interfaces;
using RunGroupWebApp.ViewModels;
using System.Runtime.CompilerServices;

namespace RunGroupWebApp.Controllers
{
    public class DashboardController : Controller
    {

        private readonly IDashboardRepository _dashboardRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPhotoService _photoService;

        public DashboardController(IDashboardRepository dashboardRepository, IHttpContextAccessor httpContextAccessor, IPhotoService photoService)
        {
            _dashboardRepository = dashboardRepository;
            _httpContextAccessor = httpContextAccessor;
            _photoService = photoService;
        }

        public async  Task<IActionResult> Index()
        {
            var userRaces = await _dashboardRepository.GetAllUserRaces();
            var userClubs = await _dashboardRepository.GetAllUSerClubs();

            DashboardViewModel dashboardViewModel = new DashboardViewModel()
            {
                Races = userRaces,
                Clubs = userClubs
            };

            return View(dashboardViewModel);
        }

        public async Task<IActionResult> EditUserProfile()
        {
            string? curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            AppUser? user = await _dashboardRepository.GetUserById(curUserId);

            if (user == null) return View("Error");

            EditUserDashboardViewModel editUserDashboardViewModel = new EditUserDashboardViewModel()
            {
                Id = curUserId,
                Pace = user.Pace,
                Milage = user.Mileage,

                URL = user.ProfileImageUrl,
                AddressId = user.AddressId,
                Address = new Address
                {
                    Id = user.Address.Id,
                    City = user.Address.City,
                    Street = user.Address.Street,
                    State = user.Address.State,
                }

                //Street = user.Address.Street,
                //City = user.Address.City,
                //State = user.Address.State,

                //ProfileImageUrl = user.ProfileImageUrl,

            };
            return View(editUserDashboardViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserDashboardViewModel editVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View("EditUserProfile", editVM);


            }

            AppUser? user = await _dashboardRepository.GetByIdAsNoTracking(editVM.Id);

            if(user.ProfileImageUrl == "" || user.ProfileImageUrl == null)
            {
                ImageUploadResult photoResult = await _photoService.AddPhotoAsync(editVM.Image);

                //Optimistic concurrency - "Tracking error"
                // use as no trackinng;

                // it's better to put that in the different function
                user.Pace = editVM.Pace;
                user.Mileage = editVM.Milage;
                user.Address.Street = editVM.Address.Street;
                user.Address.City = editVM.Address.City;
                user.Address.State = editVM.Address.State;

                user.ProfileImageUrl = photoResult.Url.ToString();

                _dashboardRepository.Update(user);
                return RedirectToAction("Index");
            }
            else
            {
                try
                {
                    await _photoService.DeletePhotoAsync(user.ProfileImageUrl);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(editVM);
                }

                ImageUploadResult photoResult = await _photoService.AddPhotoAsync(editVM.Image);

                user.Pace = editVM.Pace;
                user.Mileage = editVM.Milage;
                user.Address.Street = editVM.Address.Street;
                user.Address.City = editVM.Address.City;
                user.Address.State = editVM.Address.State;

                user.ProfileImageUrl = photoResult.Url.ToString();

                _dashboardRepository.Update(user);
                return RedirectToAction("Index");
            }
        }

        
    }
}
