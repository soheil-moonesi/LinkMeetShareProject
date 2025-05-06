using LinkMeetShareProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LinkMeetShareProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserMapper _mapper;
        private LinkMeetShareProjectDbContext _context;
        public UserController(LinkMeetShareProjectDbContext context, UserMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        // GET: api/<UserController>
        [HttpGet]
        public async Task<IEnumerable<User>> Get()
        {
            return await _context.User.ToListAsync();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<User> Get(int id)
        {
           return await _context.User.FindAsync(id);
        }

        // POST api/<UserController>

        //todo : create Dto for Post 
        [HttpPost]
        public void Post([FromBody] UserAddDto value)
        {

            var UserAdd  = _mapper.UserAddDtoToUser(value);
             _context.User.Add(UserAdd);
            _context.SaveChanges();
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] UserAddDto value)
        {
          var x =  _context.User.Find(id);
          x.Email = value.EmailDto;
          _context.SaveChanges();
        }

        [HttpPut("{id}/all")]
        public void PutAll(int id, [FromBody] UserPutAllDto value)
        {
            // Get the existing user with their current meeting links
            var existingUser = _context.User
                .Include(u => u.UserEnrollLinks)
                .FirstOrDefault(u => u.UserKey == id);

            if (existingUser == null)
            {
                return;
            }

            // Update the email
            existingUser.Email = value.EmailDto;

            // Update the meeting links
            if (value.MeetingLinkUserDto != null)
            {
                // Clear existing links

                // Add new links
                foreach (var link in value.MeetingLinkUserDto)
                {
                    // Ensure the user key is set correctly
                    existingUser.UserEnrollLinks.Add(link);
                }
            }

            _context.SaveChanges();
        }


        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
