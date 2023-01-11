using IOC.Constants;
using IOC.CreateModels;
using IOC.DataBase;
using IOC.Exceptions;
using IOC.Models;
using IOC.RequestModels;
using IOC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

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

        public async Task<List<Availability>> GetAvailabilitiesByType(int type)
        {
            
            return await _context.Availabilities.Where(a=>(int)a.Type==type).ToListAsync();

        }
        public async Task<int> AddAvailability(CreateAvailability createAvailability)
        {
            User user = await _context.Users.FindAsync(createAvailability.IdUser);

            if (user is null)
            {
                throw new UserNotFoundException();
            }

            Availability availability = new Availability
            {
                IdUser = createAvailability.IdUser,
                StartDate = createAvailability.StartDate,
                Type = AvailabilityType.Free
            };

            _context.Add(availability);
            await _context.SaveChangesAsync();
            return availability.IdAvailability;
        }
        public async Task<int> AddTeaTime(CreateTeaTime createAvailability)
        {
            User user = await _context.Users.FindAsync(createAvailability.IdUser);

            if (user is null)
            {
                throw new UserNotFoundException();
            }

            Availability teaTime = new Availability
            {
                IdUser = createAvailability.IdUser,
                StartDate = createAvailability.StartDate,
                Type = AvailabilityType.TeaTime,
                Location = createAvailability.Location,
                IdParticipant = createAvailability.IdParticipant
            };
        
            _context.Add(teaTime);
            await _context.SaveChangesAsync();
            return teaTime.IdAvailability;
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

        public async Task<List<Availability>> GetAllAvailabilitiesByUser(int idUser)
        {

            return await _context.Availabilities.Where(a => a.IdUser == idUser).ToListAsync();

        }
        public async Task<Availability> EditAvailability(Availability a)
        {
            var result = await CheckIfAvailabilityExists(a.IdAvailability);

                result.IdAvailability = a.IdAvailability;
                result.IdUser = a.IdUser;
                result.IdParticipant = a.IdParticipant;
                result.Location = a.Location;
                result.StartDate = a.StartDate;
                _context.Availabilities.Update(result);
                await (_context.SaveChangesAsync());
                return result;
            
        }

        public async Task<Availability> CheckIfAvailabilityExists(int? id)
        {
            var a = await _context.Availabilities.FirstOrDefaultAsync(a => a.IdAvailability == id);
            if (a == null)
                return null;
            else
                return a;
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


