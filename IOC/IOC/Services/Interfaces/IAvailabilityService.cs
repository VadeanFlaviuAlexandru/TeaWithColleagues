using IOC.CreateModels;
using IOC.Models;
using IOC.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace IOC.Services.Interfaces
{
    public interface IAvailabilityService
    {
        Task<List<Availability>> GetAllAvailabilities();

        Task<Availability> GetAvailabilityById(int id);
        Task<List<Availability>> GetAvailabilitiesByDateAndTime(DateTime dateTime);

        Task<int> AddAvailability(CreateAvailability createAvailabilityRequest);

        Task<bool> DeleteAvailability(int id);

        Task<bool> RescheduleAvailability(int id, DateTime newDate);

    }
}
