using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Models;
using RunGroupWebApp.Repository.Interfaces;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;

        public ClubController(ApplicationDbContext context, 
            IClubRepository clubRepository, 
            IPhotoService photoService)
        {
            _clubRepository = clubRepository;
            _photoService = photoService;
        }


        public async Task<IActionResult> Index()
        {
            IEnumerable<Club> clubs = await _clubRepository.GetAll();
            return View(clubs);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Club? club = await _clubRepository.GetByIdAsync(id);
            return View(club);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubVM)
        {
            if (ModelState.IsValid)
            {
                ImageUploadResult? result = await _photoService.AddPhotoAsync(clubVM.Image);

                Club? club = new Club
                {
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = result.Url.ToString(),
                    Address = new Address
                    {
                        Street = clubVM.Address.Street,
                        City = clubVM.Address.City,
                        State = clubVM.Address.State
                    }
                };

                _clubRepository.Add(club);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo Upload failed");
                return View(clubVM);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            Club? club = await _clubRepository.GetByIdAsync(id);
            if (club is null) return View("Error");

            var clubVM = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                URL = club.Image,
                ClubCategory = club.ClubCategory,
                AddressId = club.AddressId,
                Address = new Address
                {
                    Id = club.Address.Id,
                    Street = club.Address.Street,
                    City = club.Address.City,
                    State = club.Address.State
                }
            };
            return View(clubVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", clubVM);
            }

            var userClub = await _clubRepository.GetByIdAsyncNoTracking(id);

            if (userClub != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(userClub.Image);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(clubVM);
                }


                var photoResult = await _photoService.AddPhotoAsync(clubVM.Image);

                var club = new Club
                {
                    Id = id,
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = photoResult.Url.ToString(),
                    ClubCategory = clubVM.ClubCategory,
                    AddressId = clubVM.AddressId,
                    Address = new Address
                    {
                        Id = clubVM.AddressId,
                        Street = clubVM.Address.Street,
                        City = clubVM.Address.City,
                        State = clubVM.Address.State,
                    }
                };

                _clubRepository.Update(club);
                return RedirectToAction("Index");
            }
            else
            {
                return View(clubVM);
            }

        }

        public async Task<IActionResult> Delete(int id)
        {
            var club = await _clubRepository.GetByIdAsync(id);
            if (club is null)
                return View("Error");

            return View(club);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var clubDetails = await _clubRepository.GetByIdAsync(id);
            if (clubDetails is null) return View("Error");

            _clubRepository.Delete(clubDetails);

            return RedirectToAction("Index");
        }

    }
}
