using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Models;
using RunGroupWebApp.Repository.Interfaces;

namespace RunGroupWebApp.Repository
{
    public class ClubRepository : IClubRepository
    {
        private readonly ApplicationDbContext _context;

        public ClubRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(Club club)
        {
            _context.AddAsync(club);
            return Save();
        }

        public bool Delete(Club club)
        {
            _context.Remove(club);
            return Save();
        }

        public async Task<IEnumerable<Club>> GetAll()
        {
            List<Club> clubs = await _context.Clubs
                .Include(a=>a.Address)
                .ToListAsync();
                
            return clubs;
        }

        public async Task<Club> GetByIdAsync(int id)
        {
            Club? club = await _context.Clubs
                .Include(a=>a.Address)
                .FirstOrDefaultAsync(c => c.Id == id);

            return club;
            
        }

        public async Task<IEnumerable<Club>> GetClubByCity(string city)
        {
            List<Club> clubs = await _context.Clubs
                .Include(c => c.Address)
                .Where(c => c.Address.City == city)
                .ToListAsync();

            return clubs;

            //if upper doesn't work let's try this:
            //return await _context.Clubs.Where(c => c.Address.City.Contains(city)).ToListAsync();
        }

        public bool Save()
        {
            int saved =  _context.SaveChanges();
            return saved > 0;
        }

        public bool Update(Club club)
        {
             _context.Clubs.Update(club);
            return Save();
        }
    }
}
