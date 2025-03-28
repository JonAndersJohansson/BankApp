using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Enums
{
    public enum ValidationResult
    {
        OK,
        BalanceTooLow,
        IncorrectAmount,
        NoAccountFound,
        DateInPast,
        NoReceivingAccountFound,
        MissingGivenName,
        MissingSurname,
        MissingStreetAddress,
        MissingCity,
        MissingZipCode,
        MissingCountry,
        MissingGender,
        MissingBirthday,
        InvalidBirthday,
        MissingNationalId,
        MissingPhone,
        MissingEmail,
        InvalidCountry,
        InvalidTelephoneCountryCode,
        CustomerNotFound
    }
    //public enum Frequency
    //{
    //    Choose = 0,
    //    Daily = 1,
    //    Weekly = 2,
    //    Monthly = 3,
    //    Yearly = 4
    //}
}
