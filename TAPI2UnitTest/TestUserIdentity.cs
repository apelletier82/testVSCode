using TAPI2.Services.Abstract;

namespace TestWebApi2
{
    public class TestUserIdentity : IUserIdentity
    {
        public string Username => "UnitTestTAPI2";
    }
}