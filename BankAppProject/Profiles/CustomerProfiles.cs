using AutoMapper;
using BankAppProject.ViewModels;
using DataAccessLayer.DTO;
using Services.Customer;
using Services.Enums;

namespace BankAppProject.Profiles
{
    public class CustomerProfiles : Profile
    {
        public CustomerProfiles()
        {
            CreateMap<CustomerDetailsDto, CustomerDetailsViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.Givenname} {src.Surname}"))
                .ForMember(dest => dest.ZipcodeCity, opt => opt.MapFrom(src => $"{src.Zipcode} {src.City}"))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src =>
                    src.Birthday.HasValue
                        ? (DateTime.Today.Year - src.Birthday.Value.Year -
                           (DateTime.Today < src.Birthday.Value.ToDateTime(TimeOnly.MinValue).AddYears(DateTime.Today.Year - src.Birthday.Value.Year) ? 1 : 0))
                        : 0))
                .ForMember(dest => dest.Phonenumber, opt => opt.MapFrom(src =>
                    !string.IsNullOrEmpty(src.Telephonecountrycode) && !string.IsNullOrEmpty(src.Telephonenumber)
                        ? $"{src.Telephonecountrycode} {src.Telephonenumber}"
                        : src.Telephonenumber));


            CreateMap<AccountInCustomerDetailsDto, AccountInCustomerDetailsViewModel>();

            CreateMap<CustomerDetailsDto, EditCustomerViewModel>()
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => ParseEnumOrDefault(src.Country, Country.Choose)))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => ParseEnumOrDefault(src.Gender, Gender.Choose)));

            CreateMap<EditCustomerViewModel, CustomerDetailsDto>()
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country.ToString()))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()));

            CreateMap<CustomerIndexDto, CustomerIndexViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Givenname + " " + src.Surname));

            CreateMap<TopCustomerDto, TopCustomerViewModel>();
        }


        private static TEnum ParseEnumOrDefault<TEnum>(string? value, TEnum defaultValue) where TEnum : struct
        {
            return Enum.TryParse<TEnum>(value, true, out var result) ? result : defaultValue;
        }

    }

}
