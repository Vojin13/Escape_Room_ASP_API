using Application.Commands.Users;
using Application.DTO.Search;
using Application.DTO.Users;
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
        public IActionResult Get(int id,
                                 [FromServices] IAdminGetUserQuery query)
        {
            var result = _handler.ExecuteQuery(query, id);
            return Ok(result);
        }

        // POST api/<AdminUserController>
        [HttpPost]
        public IActionResult Post([FromBody] CreateUserDTO dto,
                         [FromServices] ICreateUserCommand command)
        {
            _handler.ExecuteCommand(command, dto);
            return StatusCode(201);
        }

        // PUT api/<AdminUserController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id,
                                 [FromBody] UpdateUserDTO dto,
                                 [FromServices] IUpdateUserCommand command)
        {
            dto.Id = id;
            _handler.ExecuteCommand(command, dto);
            return NoContent();
        }

        // DELETE api/<AdminUserController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id,
                                    [FromServices] IDeleteUserCommand command)
        {
            _handler.ExecuteCommand(command, id);
            return NoContent();
        }
    }
}
