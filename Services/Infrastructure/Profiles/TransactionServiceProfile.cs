using AutoMapper;
using DataAccessLayer.Models;
using Services.DTOs;

namespace Services.Infrastructure.Profiles
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<Transaction, TransactionInAccountDetailsDto>();
        }
    }
}
