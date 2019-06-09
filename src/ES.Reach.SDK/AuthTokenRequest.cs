namespace Epicalsoft.Reach.Api.Client.Net
{
    internal class AuthTokenRequest
    {
        public string Grant_Type { get; set; }
        public string Refresh_Token { get; set; }
        public string Client_Id { get; set; }
        public string Client_Secret { get; set; }
        public string User_Key { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}