using IOC.Constants;
using IOC.CreateModels;
using IOC.DataBase;
using IOC.Models;
using IOC.RequestModels;
using IOC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IOC.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly DatabaseContext _context;

        public AvailabilityService(DatabaseContext dataBaseContext)
        {
            _context = dataBaseContext;
        }

        public async Task<List<Availability>> GetAllAvailabilities()
        {
            return await _context.Availabilities.ToListAsync();
        }

        public async Task<Availability> GetAvailabilityById(int id)
        {
            return await _context.Availabilities.FindAsync(id);
        }
        public async Task<List<Availability>> GetAvailabilitiesByDateAndTime(DateTime dateTime)
        {
            return await _context.Availabilities.Where(a=>a.StartDate.Equals(dateTime)).ToListAsync();
        }

        public async Task<int> AddAvailability(CreateAvailability createAvailability)
        {
            Availability availability = new Availability
            {
                IdUser = createAvailability.IdUser,
                StartDate = createAvailability.StartDate,
                Type = AvailabilityType.Free
            };

            if (createAvailability.Location != null)
            {
                availability.Location = createAvailability.Location;
                availability.IdParticipant= createAvailability.IdParticipant;
                availability.Type = AvailabilityType.TeaTime;
            }
            _context.Add(availability);
            await _context.SaveChangesAsync();
            return availability.IdAvailability;
        }

        public async Task<bool> DeleteAvailability(int id)
        {
            Availability availability = await _context.Availabilities.FindAsync(id);
            if (availability == null)
                return false;

            _context.Availabilities.Remove(availability);
            await _context.SaveChangesAsync();
            return true;
        }
    
        public async Task<bool> RescheduleAvailability(int id, DateTime newDate)
        {
            Availability availability = await _context.Availabilities.FindAsync(id);
            if (availability == null)
                return false;

            availability.StartDate= newDate;

            _context.Availabilities.Update(availability);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}


