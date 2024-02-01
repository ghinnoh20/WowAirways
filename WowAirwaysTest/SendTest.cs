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
            _service = new EmailService();
        }

        [TestMethod]
        public void SendTest01()
        {
            try
            {
                //_service.Send("geingreso@shakeys.biz");
                _service.Send("gino.martin.ingreso@gmail.com");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.Fail();
            }
        }
    }
}