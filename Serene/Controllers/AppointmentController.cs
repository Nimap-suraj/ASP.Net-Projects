using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serene.Entity;
using Serene.Repository.Interface;
using Serene.Repository.Services;

namespace Serene.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentRepository _service;
        public AppointmentController(IAppointmentRepository service)
        {
            _service = service;
        }
        [HttpGet("booking-history")]
        public async Task<IActionResult> BookingHistory()
        {
            var appointments = await _service.BookingHistoryAppointment(); 
            return Ok(appointments);
            
        }
        [HttpPost("BookAppointment")]
        public async Task<IActionResult> createAppointment(Appointment appointment)
        {
            try
            {
                var newAppointment =  await _service.CreateAppointment(appointment);
                return CreatedAtAction(nameof(GetAppointmentById),
                  new { id = newAppointment.Id },  // Use newAppointment.Id here
                  newAppointment);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            try
            {
                var appointment = await _service.GetAppointmentById(id);
                if(appointment == null)
                {
                    return NotFound();
                }
                return Ok(appointment);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("{id}/confirm")]
        public async Task<IActionResult> ConfirmAppointment(int id)
        {
            try
            {
                var confirmAppointment = await  _service.ConfirmAppointment(id);
                return Ok(confirmAppointment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("{id}/reject")]
        public async Task<IActionResult> RejectAppointment(int id)
        {
            try
            {
                var RejectAppointment = await _service.RejectAppointment(id);
                return Ok(RejectAppointment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("getALLpending")]
        public async Task<IActionResult> GetPendingAppointment()
        {
            try
            {
                var pendingAppointment = await _service.GetALLPendingAppointment();
                return Ok(pendingAppointment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("getAllConfirm")]
        public async Task<IActionResult> GetConfirmAppointment()
        {
            try
            {
                var confirmAppointment = await _service.GetALLConfirmAppointment();
                return Ok(confirmAppointment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("getAllRejection")]
        public async Task<IActionResult> RejectAppointment()
        {
            try
            {
                var RejectionAppointment = await _service.GetALLRejectionAppointment();
                return Ok(RejectionAppointment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
