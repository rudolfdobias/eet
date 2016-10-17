using System;
using System.Collections.Generic;
using System.Linq;
using MewsEet.EetService;

namespace MewsEet.Dto
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
            Warnings = warnings ?? new List<Fault>();
        }

        public static SendRevenueResult Create(OdeslaniTrzbyResponse response)
        {
            if (response.Item is OdpovedPotvrzeniType)
            {
                return CreateSuccess(response);
            }

            return CreateError(response);
        }

        private static SendRevenueResult CreateSuccess(OdeslaniTrzbyResponse response)
        {
            var confirmation = response.Item as OdpovedPotvrzeniType;

            return new SendRevenueResult(
                id: Guid.Parse(response.Hlavicka.uuid_zpravy),
                issued: DateTimeWithTimeZone.CreateCzech(response.Hlavicka.dat_prij), 
                securityCode: response.Hlavicka.bkp,
                success: new SendRevenueSuccess(confirmation.fik), 
                error: null,
                isPlayground: confirmation.testSpecified && confirmation.test,
                warnings: GetWarnings(response.Varovani)
            );
        }

        private static SendRevenueResult CreateError(OdeslaniTrzbyResponse response)
        {
            var rejection = response.Item as OdpovedChybaType;

            return new SendRevenueResult(
                id: Guid.Parse(response.Hlavicka.uuid_zpravy),
                issued: DateTimeWithTimeZone.CreateCzech(response.Hlavicka.dat_prij),
                securityCode: response.Hlavicka.bkp,
                success: null, 
                error: new SendRevenueError(new Fault(
                    code: rejection.kod,
                    message: string.Join("\n", rejection.Text)
                )),
                isPlayground: rejection.testSpecified && rejection.test,
                warnings: GetWarnings(response.Varovani)
            );
        }

        public Guid Id { get; }

        public bool IsPlayground { get; }

        public DateTimeWithTimeZone Issued { get; }

        public string SecurityCode { get; }

        public IEnumerable<Fault> Warnings { get; }

        private static IEnumerable<Fault> GetWarnings(OdpovedVarovaniType[] warnings)
        {
            return warnings.Select(w => new Fault(w.kod_varov, string.Join("\n", w.Text)));
        }
    }
}
