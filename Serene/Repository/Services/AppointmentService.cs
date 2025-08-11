using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Serene.Data;
using Serene.Entity;
using Serene.Repository.Interface;

namespace Serene.Repository.Services
{
    public class AppointmentService : IAppointmentRepository
    {
        private readonly ApplicationDbContext _context;
        public AppointmentService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Appointment> CreateAppointment(Appointment appointment)
        {
            if (!await IsSlotAvailable(appointment.AppointmentDate, appointment.AppointmentTime, appointment.Duration))
            {
                throw new InvalidOperationException("The selected time slot is already booked");
            }
            if (appointment == null)
            {
                throw new ArgumentException("Invalid Time for Booking an Appointment");
            }
            var time = appointment.AppointmentTime;
            if (time < new TimeOnly(9, 0) || time > new TimeOnly(17, 0))
            {
                throw new ArgumentException("Invalid Time for Booking an Appointment");
            }
            var slotTaken = (time >= new TimeOnly(9,0) && time <= new TimeOnly(20,0));
            //_context.Appointments.Add(appointment);
            appointment.Status = StatusEnum.Pending;
            appointment.AppointmentDate = appointment.AppointmentDate.Date;
            appointment.Price = CalculatePrice(appointment.ServiceType, appointment.Duration);
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return appointment;

        }
        public async Task<List<Appointment>> GetALLPendingAppointment()
        {
            var appointment = await _context.Appointments.Where(x => x.Status == StatusEnum.Pending).ToListAsync();
            if (appointment == null)
            {
                return null;
            }
            return appointment;
        }
        private async Task<bool> IsSlotAvailable(DateTime appointmentDate, TimeOnly appointmentTime, int duration)
        {
            var endTime = appointmentTime.AddMinutes(duration);
            var hasOverlap = await _context.Appointments.AsNoTracking().Where(a => a.AppointmentDate.Date == appointmentDate.Date)
                .AnyAsync(a => (appointmentTime < a.AppointmentTime.AddMinutes(a.Duration)) && (endTime > a.AppointmentTime));    
            return !hasOverlap;
        }
        private decimal CalculatePrice(string serviceType, int duration)
        {
            return (serviceType.ToLower(), duration) switch
            {
                ("head", 30) => 100,
                ("head", 60) => 200,
                ("head", 90) => 500,
                ("leg", 30) => 200,
                ("leg", 60) => 500,
                ("leg", 90) => 2000,
                ("hand", 30) => 100,
                ("hand", 60) => 200,
                ("hand", 90) => 500,
                ("back", 30) => 2000,
                ("back", 60) => 3000,
                ("back", 90) => 5000,
                _ => 100 // Default
            };
        }
        public async Task<List<Appointment>> GetAllAppointments()
        {
            var appointments = await _context.Appointments.ToListAsync();
            if (appointments.Count == 0)
            {
                return null;
            }
            return appointments;
        }
        public async Task<Appointment> GetAppointmentById(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            //if (appointment == null)
            //{
            //    throw new KeyNotFoundException($"Appointment with ID {id} not found");
            //}
            return appointment;

        }
        public async Task<List<Appointment>> BookingHistoryAppointment()
        {
            var appointment = await _context.Appointments.ToListAsync();
            if (appointment == null)
            {
                return null;
            }
            return appointment;
        }
        public async Task<Appointment> ConfirmAppointment(int id)
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(x => x.Id == id);
            //if (appointment == null)
            //{
            //    return null;
            //}            
            if (appointment != null)
            {
                appointment.Status = StatusEnum.Confirm;
                await _context.SaveChangesAsync();
            }
            return appointment;
        }
        public  async Task<List<Appointment>> GetALLConfirmAppointment()
        {
            var appointment = await  _context.Appointments.Where(x => x.Status== StatusEnum.Confirm).ToListAsync();
            if(appointment == null)
            {
                return null;
            }
            return appointment;
        }
        public async Task<List<Appointment>> GetALLRejectionAppointment()
        {
            var appointment = await _context.Appointments.Where(x => x.Status == StatusEnum.Rejected).ToListAsync();
            //if (appointment == null)
            //{
            //    return null;
            //}
            return appointment;
        }
        public async Task<Appointment> RejectAppointment(int id)
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(x => x.Id == id);
            if (appointment == null)
            {
                return null;
            }
            appointment.Status = StatusEnum.Rejected;
            await _context.SaveChangesAsync();
            return appointment;
        }
    }
}
