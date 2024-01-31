using Amazon.DynamoDBv2.DataModel;

namespace WowAirwaysLambda.API.Models
{
    [DynamoDBTable("Attendee")]
    public class Attendee
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Brand { get; set; }
        public string Branch { get; set; }
        public string Position { get; set; }
        public string BookingReference { get; set; }
        public string FlightNo { get; set; }
        public string SeatNo { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? PrintDate { get; set; }
    }
}