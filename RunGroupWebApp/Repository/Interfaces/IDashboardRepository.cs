using RunGroupWebApp.Models;

namespace RunGroupWebApp.Repository.Interfaces
{
    public interface IDashboardRepository
    {
        Task<List<Race>> GetAllUserRaces();
        Task<List<Club>> GetAllUSerClubs();

    }
}
