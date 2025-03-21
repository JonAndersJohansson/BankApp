//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Services.Account
//{
//    using AutoMapper;
//    using DataAccessLayer.DTO;
//    using DataAccessLayer.Repositories.AccountRepositories;
//    using Microsoft.EntityFrameworkCore;

//    public class AccountService : IAccountService
//    {
//        private readonly IAccountRepository _accountRepository;
//        private readonly IMapper _mapper;

//        public AccountService(IAccountRepository accountRepository, IMapper mapper)
//        {
//            _accountRepository = accountRepository;
//            _mapper = mapper;
//        }

//        public List<AccountIndexDto> GetAccounts(string sortColumn, string sortOrder, int pageNumber, int pageSize, string q, out int totalAccounts)
//        {
//            var query = _accountRepository.GetAllAccounts();

//            // Search
//            if (!string.IsNullOrWhiteSpace(q))
//            {
//                string searchQuery = q.Trim().ToLower();

//                query = query
//                    .Where(a =>
//                        a.AccountId.ToString().Contains(searchQuery));
//            }


//            // Sortering beroende på valda kolumner
//            query = sortColumn switch
//            {
//                "AccountNumber" => sortOrder == "asc" ? query.OrderBy(a => a.AccountId) : query.OrderByDescending(a => a.AccountId),
//                "Balance" => sortOrder == "asc" ? query.OrderBy(a => a.) : query.OrderByDescending(c => c.NationalId),
//                "Name" => sortOrder == "asc" ? query.OrderBy(c => c.Surname) : query.OrderByDescending(c => c.Surname),
//                "Address" => sortOrder == "asc" ? query.OrderBy(c => c.Streetaddress) : query.OrderByDescending(c => c.Streetaddress),
//                "City" => sortOrder == "asc" ? query.OrderBy(c => c.City) : query.OrderByDescending(c => c.City),
//                _ => query.OrderBy(c => c.CustomerId)  // Standard-sortering om inget annat anges
//            };

//            // Antal konton totalt för pagination
//            totalAccounts = query.Count();

//            // Paginering
//            var customers = query
//                .Skip((pageNumber - 1) * pageSize)
//                .Take(pageSize)
//                .Select(c => new CustomerIndexDto
//                {
//                    Id = c.CustomerId,
//                    NationalId = c.NationalId,
//                    Givenname = c.Givenname,
//                    Surname = c.Surname,
//                    Address = c.Streetaddress,
//                    City = c.City
//                }).ToList();

//            return customers;
//        }
//    }

//}
