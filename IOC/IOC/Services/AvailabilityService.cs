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
using System.Collections.Generic;

namespace IOC.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly DatabaseContext _context;

        public AvailabilityService(DatabaseContext dataBaseContext)
        {
            _context = dataBaseContext;
        }

        public async Task<List<APIAvailability>> GetAllAvailabilities()
        {
             List < APIAvailability >  availabilities = await _context.Availabilities.Select(a => new APIAvailability
            {

                StartDate = a.StartDate,
                IdAvailability = a.IdAvailability,
                IdUser = a.IdUser,
                IdParticipant = a.IdParticipant,
                Location = a.Location,
                Type = a.Type,
                //APIUser = null //await _context.Users.FindAsync(a.IdUser)

            }).ToListAsync();

            foreach(APIAvailability a in availabilities )
            {
                User user = await _context.Users.FindAsync(a.IdUser);
                a.APIUser = new()
                {
                    IDUser = user.IDUser,
                    Name = user.Name,
                    Surname = user.Surname,
                    PhoneNumber = user.PhoneNumber,
                    MailAddress = user.MailAddress
                };
            }
            return availabilities;
        }

        public async Task<APIAvailability> GetAvailabilityById(int id)
        {
           Availability Availability = _context.Availabilities.Where(a => a.IdAvailability == id).FirstOrDefault();

            User user = await _context.Users.FindAsync(Availability.IdUser);

            APIAvailability apiAvailability =  new()
            {
                StartDate = Availability.StartDate,
                IdAvailability = Availability.IdAvailability,
                IdUser = Availability.IdUser,
                IdParticipant = Availability.IdParticipant,
                Location = Availability.Location,
                Type = Availability.Type,
                APIUser = new() { IDUser=user.IDUser,
                    Name = user.Name,
                    Surname = user.Surname,
                    PhoneNumber = user.PhoneNumber,
                    MailAddress = user.MailAddress
                }
            };
            return apiAvailability;
            //            return await _context.Availabilities.FindAsync(id);
        }
        public async Task<List<APIAvailability>> GetAvailabilitiesByDateAndTime(DateTime dateTime)
        {
            List<APIAvailability> availabilities = await _context.Availabilities.Where(a => a.StartDate.Equals(dateTime)).Select(a => new APIAvailability
            {

                StartDate = a.StartDate,
                IdAvailability = a.IdAvailability,
                IdUser = a.IdUser,
                IdParticipant = a.IdParticipant,
                Location = a.Location,
                Type = a.Type,
                //APIUser = null //await _context.Users.FindAsync(a.IdUser)

            }).ToListAsync();

            foreach (APIAvailability a in availabilities)
            {
                User user = await _context.Users.FindAsync(a.IdUser);
                a.APIUser = new()
                {
                    IDUser = user.IDUser,
                    Name = user.Name,
                    Surname = user.Surname,
                    PhoneNumber = user.PhoneNumber,
                    MailAddress = user.MailAddress
                };
            }
            return availabilities;
            //return await _context.Availabilities.Where(a=>a.StartDate.Equals(dateTime)).ToListAsync();
        }

        public async Task<List<APIAvailability>> GetAvailabilitiesByType(int type)
        {
            List<APIAvailability> availabilities = await _context.Availabilities.Where(a => (int)a.Type==type).Select(a => new APIAvailability
            {

                StartDate = a.StartDate,
                IdAvailability = a.IdAvailability,
                IdUser = a.IdUser,
                IdParticipant = a.IdParticipant,
                Location = a.Location,
                Type = a.Type,
                //APIUser = null //await _context.Users.FindAsync(a.IdUser)

            }).ToListAsync();

            foreach (APIAvailability a in availabilities)
            {
                User user = await _context.Users.FindAsync(a.IdUser);
                a.APIUser = new()
                {
                    IDUser = user.IDUser,
                    Name = user.Name,
                    Surname = user.Surname,
                    PhoneNumber = user.PhoneNumber,
                    MailAddress = user.MailAddress
                };
            }
            return availabilities;
            //return await _context.Availabilities.Where(a=>(int)a.Type==type).ToListAsync();

        }
        public async Task<int> AddAvailability(CreateAvailability createAvailability)
        {
            User user = await _context.Users.FindAsync(createAvailability.IdUser);

            if (user is null)
            {
                throw new UserNotFoundException();
            }

            Availability availability = new()
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

        public async Task<List<APIAvailability>> GetAllAvailabilitiesByUser(int idUser)
        {
            List<APIAvailability> availabilities = await _context.Availabilities.Where(a => a.IdUser==idUser).Select(a => new APIAvailability
            {

                StartDate = a.StartDate,
                IdAvailability = a.IdAvailability,
                IdUser = a.IdUser,
                IdParticipant = a.IdParticipant,
                Location = a.Location,
                Type = a.Type,
                //APIUser = null //await _context.Users.FindAsync(a.IdUser)

            }).ToListAsync();

            foreach (APIAvailability a in availabilities)
            {
                User user = await _context.Users.FindAsync(a.IdUser);
                a.APIUser = new()
                {
                    IDUser = user.IDUser,
                    Name = user.Name,
                    Surname = user.Surname,
                    PhoneNumber = user.PhoneNumber,
                    MailAddress = user.MailAddress
                };
            }
            return availabilities;
            //return await _context.Availabilities.Where(a => a.IdUser == idUser).ToListAsync();

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


