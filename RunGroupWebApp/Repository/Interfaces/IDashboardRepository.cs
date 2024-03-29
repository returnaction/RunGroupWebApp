﻿using RunGroupWebApp.Models;

namespace RunGroupWebApp.Repository.Interfaces
{
    public interface IDashboardRepository
    {
        Task<List<Race>> GetAllUserRaces();
        Task<List<Club>> GetAllUSerClubs();
        Task<AppUser> GetUserById(string id);
        Task<AppUser> GetByIdAsNoTracking(string id);

        bool Update(AppUser user);
        bool Save();
    }
}
