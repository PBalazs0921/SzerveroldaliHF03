using AutoMapper;
using SzerveroldaliHF03.Entities.Dto;
using SzerveroldaliHF03.Entities.Entity;

namespace SzerveroldaliHF03.Logic.Dto;

public class DtoProvider
{
    public Mapper mapper {
        get;
    }
    
    public DtoProvider()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Ticket, TicketViewDto>();
            cfg.CreateMap<TicketCUDDto, Ticket>();
        });
        mapper = new Mapper(config);
    }
}