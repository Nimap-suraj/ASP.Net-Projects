using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Serene.Entity
{
    public class Appointment
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string ServiceType { get; set; } = string.Empty; // head,leg,back,hand
        public int Duration { get; set; }
        public decimal Price { get; set; }
        [JsonConverter(typeof(DateOnlyJsonConverter))] // Custom converter
        public DateTime AppointmentDate {  get; set; }  // 07/08/2025

        [JsonConverter(typeof(TimeOnlyJsonConverter))]
        public TimeOnly AppointmentTime { get; set; }
        public string Special_Requests { get; set; } = string.Empty;
        public StatusEnum Status { get; set; } = StatusEnum.Pending ; // "Pending", "Confirmed", 
    }
}
public enum StatusEnum
{
    Pending,
    Confirm,
    Rejected
}