using Microsoft.AspNetCore.Mvc;
using Serene.Entity;

namespace Serene.Repository.Interface
{
    public interface IAppointmentRepository
    {
        Task<List<Appointment>> GetAllAppointments();
        Task<Appointment> GetAppointmentById(int id);
        Task<Appointment> CreateAppointment(Appointment appointment);
        Task<Appointment> ConfirmAppointment(int id);
        Task<Appointment> RejectAppointment(int id);
        //Task<bool> DeleteAppointment(int id);

        Task<List<Appointment>> GetALLPendingAppointment();
        Task<List<Appointment>> GetALLConfirmAppointment();
        Task<List<Appointment>> GetALLRejectionAppointment();
        Task<List<Appointment>> BookingHistoryAppointment();
    }
}