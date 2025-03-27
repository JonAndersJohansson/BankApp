using AutoMapper;
using BankAppProject.ViewModels;
using DataAccessLayer.DTO;

namespace BankAppProject.Profiles
{
    public class CustomerDetailsProfile : Profile
    {
        public CustomerDetailsProfile()
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

            // Mappa AccountInCustomerDetailsDto till AccountInCustomerDetailsViewModel
            CreateMap<AccountInCustomerDetailsDto, AccountInCustomerDetailsViewModel>();

            // Mappa CustomerDetailsDto till EditCustomerViewModel och vice versa
            CreateMap<CustomerDetailsDto, EditCustomerViewModel>();
            CreateMap<EditCustomerViewModel, CustomerDetailsDto>();
        }
    }

}
