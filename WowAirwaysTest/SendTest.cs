using WowAirwaysLambda.Service;

namespace WowAirwaysTest
{
    [TestClass]
    public class SendTest
    {
        private EmailService _service;

        [TestInitialize]
        public void Init()
        {
            _service = new EmailService(new PdfService());
        }

        [TestMethod]
        public void SendTest01()
        {
            try
            {
                _service.Send("gino.martin.ingreso@gmail.com"
                    , "John"
                    , "Doe"
                    , Guid.NewGuid().ToString().Substring(0, 17).ToUpper()
                    , Guid.NewGuid().ToString().Substring(0, 5).ToUpper());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.Fail();
            }
        }
    }
}