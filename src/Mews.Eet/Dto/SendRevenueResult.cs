using System;
using System.Collections.Generic;
using System.Linq;
using Mews.Eet.EetService;

namespace Mews.Eet.Dto
{
    public class SendRevenueResult
    {
        public SendRevenueResult(Guid id, DateTimeWithTimeZone issued, string securityCode, SendRevenueSuccess success, SendRevenueError error, bool isPlayground, IEnumerable<Fault> warnings)
        {
            if ((success != null && error != null) || (success == null && error == null))
            {
                throw new ArgumentException("Either error or success has to be non-null.");
            }

            Id = id;
            IsPlayground = isPlayground;
            Issued = issued;
            SecurityCode = securityCode;
            Warnings = warnings ?? Enumerable.Empty<Fault>();
        }

        internal SendRevenueResult(OdeslaniTrzbyResponse response)
        {
            var confirmation = response.Item as OdpovedPotvrzeniType;
            var rejection = response.Item as OdpovedChybaType;

            var date = confirmation != null ? response.Hlavicka.dat_prij : response.Hlavicka.dat_odmit;

            Id = Guid.Parse(response.Hlavicka.uuid_zpravy);
            Issued = new DateTimeWithTimeZone(date, DateTimeWithTimeZone.CzechTimeZone);
            SecurityCode = response.Hlavicka.bkp;
            Success = confirmation != null ? new SendRevenueSuccess(confirmation.fik) : null;
            Error = new SendRevenueError(new Fault(
                code: rejection.kod,
                message: String.Join("\n", rejection.Text)
            ));
            IsPlayground = confirmation != null ? confirmation.testSpecified && confirmation.test : rejection.testSpecified && rejection.test;
            Warnings = GetWarnings(response.Varovani);
        }

        public Guid Id { get; }

        public bool IsPlayground { get; }

        public DateTimeWithTimeZone Issued { get; }

        public string SecurityCode { get; }

        public SendRevenueSuccess Success { get; }

        public SendRevenueError Error { get; }

        public IEnumerable<Fault> Warnings { get; }

        private static IEnumerable<Fault> GetWarnings(OdpovedVarovaniType[] warnings)
        {
            return warnings.Select(w => new Fault(w.kod_varov, String.Join("\n", w.Text)));
        }
    }
}
