using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SzerveroldaliHF03.Entities.Dto;
using SzerveroldaliHF03.Logic;

namespace SzerveroldaliHF03.Controllers;

[ApiController]
[Route("[controller]")]
public class TicketController :ControllerBase
{
    private UserManager<IdentityUser> userManager;
    private TicketLogic logic;

    
    public TicketController(UserManager<IdentityUser> userManager, TicketLogic logic)
    {
        this.userManager = userManager;
        this.logic = logic;
    }

    [HttpPost]
    [Authorize]
    public async Task Post([FromBody]TicketCreateDto dto)
    {
        var user = await userManager.GetUserAsync(User);
        if (user != null)
        {
            logic.CreateTicket(dto,user.Id);
        }
        else
        {
            // Handle the case when the user is not found
            // For example, return an error response or throw an exception
            throw new Exception("User not found");
        }
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IEnumerable<TicketViewDto>> Get()
    {
        var user = await userManager.GetUserAsync(User);
        if (user != null)
        {
            return logic.GetTickets();
        }
        else
        {
            // Handle the case when the user is not found
            // For example, return an error response or throw an exception
            throw new Exception("User not found");
        }
    }

}