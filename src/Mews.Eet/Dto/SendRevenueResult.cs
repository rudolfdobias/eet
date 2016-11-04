using System;
using System.Collections.Generic;
using System.Linq;
using Mews.Eet.Dto.Wsdl;

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

        internal SendRevenueResult(SendRevenueXmlResponse response)
        {
            var confirmation = response.Item as ResponseSuccess;
            var rejection = response.Item as ResponseError;

            var date = confirmation != null ? response.Header.Accepted : response.Header.Rejected;

            Id = String.IsNullOrWhiteSpace(response.Header.MessageUuid) ? (Guid?)null : Guid.Parse(response.Header.MessageUuid);
            Issued = new DateTimeWithTimeZone(date, DateTimeWithTimeZone.CzechTimeZone);
            SecurityCode = response.Header.SecurityCode;
            Success = confirmation != null ? new SendRevenueSuccess(confirmation.FiscalCode) : null;
            Error = new SendRevenueError(new Fault(
                code: rejection.Code,
                message: String.Join("\n", rejection.Text)
            ));
            IsPlayground = confirmation != null ? confirmation.IsPlaygroundSpecified && confirmation.IsPlayground : rejection.IsPlaygroundSpecified && rejection.IsPlayground;
            Warnings = GetWarnings(response.Warning);
        }

        public Guid? Id { get; }

        public bool IsPlayground { get; }

        public DateTimeWithTimeZone Issued { get; }

        public string SecurityCode { get; }

        public SendRevenueSuccess Success { get; }

        public SendRevenueError Error { get; }

        public IEnumerable<Fault> Warnings { get; }

        private static IEnumerable<Fault> GetWarnings(ResponseWarning[] warnings)
        {
            return warnings.Select(w => new Fault(w.Code, String.Join("\n", w.Text)));
        }
    }
}
