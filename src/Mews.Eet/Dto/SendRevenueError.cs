namespace Mews.Eet.Dto
{
    public class SendRevenueError
    {
        public SendRevenueError(Fault error)
        {
            Error = error;
        }

        public Fault Error { get; }
    }
}
