using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Models;
using RunGroupWebApp.Repository.Interfaces;

namespace RunGroupWebApp.Repository
{
    public class RaceRepository : IRaceRepository
    {
        private readonly ApplicationDbContext _context;

        public RaceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(Race race)
        {
            _context.Add(race);
            return Save();
        }

        public bool Delete(Race race)
        {
            _context.Remove(race);
            return Save();
        }

        public async Task<IEnumerable<Race>> GetAll()
        {
            List<Race> races = await _context.Races
                .Include(r => r.Address)
                .ToListAsync();

            return races;
        }

        public async  Task<IEnumerable<Race>> GetAllRacesByCity(string city)
        {
            List<Race> races = await _context.Races
                .Include(a => a.Address)
                .Where(a => a.Address.City == city)
                .ToListAsync();

            return races;

            //if upper doesn't work let's try this:
            //return await _context.Races.Where(c => c.Address.City.Contains(city)).ToListAsync();

        }

        public async Task<Race> GetByIdAsync(int id)
        {
            Race? race = await _context.Races
                .Include(a => a.Address)
                .FirstOrDefaultAsync(r => r.Id == id);

            return race;
        }

        public async Task<Race> GetByIdAsyncNoTracking(int id)
        {
            Race? race = await _context.Races
                .Include(a => a.Address)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);

            return race;
        }

        public bool Save()
        {
            int saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Race race)
        {
            _context.Races.Update(race);
            return Save();
        }
    }
}
