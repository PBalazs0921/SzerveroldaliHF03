using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SzerveroldaliHF03.Data;
using SzerveroldaliHF03.Entities.Dto;
using SzerveroldaliHF03.Entities.Entity;
using SzerveroldaliHF03.Logic.Dto;

namespace SzerveroldaliHF03.Logic;

public class TicketLogic
{
    private Repository<Ticket> _repository;
    public Mapper mapper;
    private readonly UserManager<IdentityUser> _userManager;

    public TicketLogic(Repository<Ticket> repository, DtoProvider dtoProvider,UserManager<IdentityUser> manager)
    {
        _repository = repository;
        mapper = dtoProvider.mapper;
        _userManager = manager;
    }

    public void CreateTicket(TicketCreateDto dto, string userid)
    {
        _repository.Create(
            new Ticket
            {
                Description = dto.Description,
                CreatedAt = DateTime.Now,
                UserId = userid
            }
            );
    }
    
    public IEnumerable<TicketViewDto> GetTickets()
    {
        var tickets = _repository.GetAll().ToList();
        var ticketDtos = new List<TicketViewDto>();

        foreach (var ticket in tickets)
        {
            var user = _userManager.FindByIdAsync(ticket.UserId).Result;

            ticketDtos.Add(new TicketViewDto
            {
                Id = ticket.Id,
                Description = ticket.Description,
                CreatedAt = ticket.CreatedAt,
                Email = user?.Email ?? "N/A"
            });
        }

        return ticketDtos;
    }

    
}