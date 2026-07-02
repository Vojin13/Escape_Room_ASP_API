using Application.DTO.Search;
using Application.Queries.Users.Admin;
using Implementation.UseCases;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers.Admin
{
    [Route("api/admin/users")]
    [ApiController]
    public class AdminUsersController : ControllerBase
    {
        private readonly UseCaseHandler _handler;

        public AdminUsersController(UseCaseHandler handler)
        {
            _handler = handler;
        }

        // GET: api/<AdminUserController>
        [HttpGet]
        public IActionResult Get([FromQuery] UserSearchDTO dto,
                                 [FromServices] IAdminGetUsersQuery query)
        {
            var result = _handler.ExecuteQuery(query, dto);
            return Ok(result);
        }

        // GET api/<AdminUserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AdminUserController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AdminUserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AdminUserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
