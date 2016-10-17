namespace MewsEet.Dto
{
    public class Certificate
    {
        public Certificate(string password, byte[] data)
        {
            Password = password;
            Data = data;
        }

        public string Password { get; }

        public byte[] Data { get; }
    }
}
