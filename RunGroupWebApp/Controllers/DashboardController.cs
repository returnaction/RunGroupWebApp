﻿using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Data;
using RunGroupWebApp.Repository.Interfaces;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class DashboardController : Controller
    {

        private readonly IDashboardRepository _dashboardRepository;

        public DashboardController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
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
    }
}
