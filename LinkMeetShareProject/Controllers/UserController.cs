using System.Security.Cryptography.X509Certificates;
using LinkMeetShareProject.Dto;
using LinkMeetShareProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LinkMeetShareProject.Controllers
{
    //todo: how to create back office for test
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserMapper _mapper;
        private LinkMeetShareProjectDbContext _context;
        private readonly UserManager<ApiUser> _userManager; // Add this

        public UserController(LinkMeetShareProjectDbContext context, UserMapper mapper,
            UserManager<ApiUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
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

        //todo : create Dto for Post 
        [HttpPost("AddUser")]
        public async Task<string> AddUser([FromBody] UserAddDto value)
        {
            var UserAdd = _mapper.UserAddDtoToUser(value);
            _context.User.Add(UserAdd);
            _context.SaveChanges();
            return "user added";
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] UserAddDto value)
        {
            var x = _context.User.Find(id);
            x.Email = value.EmailDto;
            _context.SaveChanges();
        }

        //todo:remove User id from Dto
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

                // Add new links
                foreach (var link in value.MeetingLinkUserDto)
                {
                    var newLink = new MeetingLinkUser
                    {
                        MeetingLinkKey_R = link.MeetingLinkKey_R,
                        UserKey_R = id
                    };
                    // Ensure the user key is set correctly
                    existingUser.UserEnrollLinks.Add(newLink);
                    _context.SaveChanges();
                }
            }

        }


        // DELETE api/<UserController>/5
        [HttpDelete("{id}/{linkId}")]
        public void DeleteMeetingLink(int id, int linkId)
        {
            var existingUser = _context.User.Include(q => q.UserEnrollLinks).FirstOrDefault(q => q.UserKey == id);
            foreach (var link in existingUser.UserEnrollLinks)
            {
                if (linkId == link.MeetingLinkKey_R)
                {
                    _context.MeetingLinkUser.Remove(link);
                    _context.SaveChanges();
                }
            }

        }

        [HttpPost("getE")]
        public async Task<string> getEmailAndReturn([FromBody] EmailRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return null;
            }

            // Use UserManager to find the user in AspNetUsers table
            var identityUser = await _userManager.FindByEmailAsync(request.Email);
            if (identityUser == null)
            {
                return null;
            }

            // Return the Identity user's ID
            return identityUser.Id;
        }
    }

    public class EmailRequest
    {
        public string Email { get; set; }
    }
}

