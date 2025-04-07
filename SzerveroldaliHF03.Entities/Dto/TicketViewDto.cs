namespace SzerveroldaliHF03.Entities.Dto;

public class TicketViewDto
{
    public string Id { get; set; } = "";
    
    public string Email { get; set; } = "";
    
    public string Description { get; set; } = "";
    
    public DateTime CreatedAt { get; set; }
    
    public string UserId { get; set; } = "";
}