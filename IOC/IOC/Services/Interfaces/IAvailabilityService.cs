using IOC.Constants;
using IOC.CreateModels;
using IOC.Models;
using IOC.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace IOC.Services.Interfaces
{
    public interface IAvailabilityService
    {
        Task<List<APIAvailability>> GetAllAvailabilities();

        Task<APIAvailability> GetAvailabilityById(int id);
        Task<List<APIAvailability>> GetAvailabilitiesByDateAndTime(DateTime dateTime);

        Task<List<APIAvailability>> GetAvailabilitiesByType(int type);
        Task<int> AddAvailability(CreateAvailability createAvailabilityRequest);

        Task<int> AddTeaTime(CreateTeaTime createAvailability);
        Task<bool> DeleteAvailability(int id);

        public Task<List<APIAvailability>> GetAllAvailabilitiesByUser(int idUser);
        Task<Availability> EditAvailability(Availability @a);
        Task<Availability?> CheckIfAvailabilityExists(int? id);
        Task<bool> RescheduleAvailability(int id, DateTime newDate);
    }
}
