using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using WowAirwaysLambda.API.Models;

namespace WowAirwaysLambda.API.Repository
{
    public class DynamoDbRepository
    {
        private readonly DynamoDBContext _context;

        public DynamoDbRepository(IAmazonDynamoDB dynamoDbClient)
        {
            _context = new DynamoDBContext(dynamoDbClient);
        }

        public async Task<string> AddItem(Attendee attendee)
        {
            var uuid = Guid.NewGuid().ToString();

            attendee.Id = Guid.NewGuid().ToString();
            attendee.CreateDate = DateTime.Now;

            await _context.SaveAsync(attendee);

            return uuid;
        }

        public async Task<IEnumerable<Attendee>> GetItems()
        {
            return await _context.ScanAsync<Attendee>(new List<ScanCondition>()).GetRemainingAsync();
        }
    }
}