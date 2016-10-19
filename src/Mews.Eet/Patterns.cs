namespace Mews.Eet
{
    public class Patterns
    {
        public static string BillNumber
        {
            get { return @"^[0-9a-zA-Z\.,:;/#\-_ ]{1,25}$"; }
        }

        public static string CurrencyValue
        {
            get { return @"^((0|-?[1-9]\d{0,7})\.\d\d|-0\.(0[1-9]|[1-9]\d))$"; }
        }

        public static string RegistryIdentifier
        {
            get { return @"^[0-9a-zA-Z.,:;/#-_]{1,20}$"; }
        }

        public static string SecurityCode
        {
            get { return @"^([0-9a-fA-F]{8}-){4}[0-9a-fA-F]{8}$"; }
        }

        public static string TaxIdentifier
        {
            get { return @"^CZ[0-9]{8,10}$"; }
        }

        public static string UUID
        {
            get { return @"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[1-5][0-9a-fA-F]{3}-[89abAB][0-9a-fAF]{3}-[0-9a-fA-F]{12}$"; }
        }
    }
}
