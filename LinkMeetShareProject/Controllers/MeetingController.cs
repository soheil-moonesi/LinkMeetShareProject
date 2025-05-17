using LinkMeetShareProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LinkMeetShareProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingController : ControllerBase
    {
        private readonly LinkMeetShareProjectDbContext _context;

        public MeetingController(LinkMeetShareProjectDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IEnumerable<MeetingLink>> Get()
        {
            return await _context.MeetingLink.ToListAsync();
        }
    }
}
