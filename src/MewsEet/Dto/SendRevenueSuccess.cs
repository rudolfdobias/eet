namespace MewsEet.Dto
{
    public class SendRevenueSuccess
    {
        public SendRevenueSuccess(string fiscalCode)
        {
            FiscalCode = fiscalCode;
        }

        public string FiscalCode { get; }
    }
}
