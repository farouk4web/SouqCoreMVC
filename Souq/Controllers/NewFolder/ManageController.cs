//using AutoMapper;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Souq.DTOs;
//using Souq.Services.ManageService;
//using Souq.Settings;
//using Souq.ViewModels.Account;
//using System.Security.Claims;

//namespace Souq.Controllers
//{
//    [Authorize(Roles = RoleName.OwnersAndAdmins)]
//    [ApiController]
//    [Route("api/[controller]")]
//    public class ManageController : ControllerBase
//    {
//        private readonly IManageService _manageService;
//        private readonly IMapper _mapper;
//        public ManageController(IManageService manageService, IMapper mapper)
//        {
//            _manageService = manageService;
//            _mapper = mapper;
//        }

//        [AllowAnonymous]
//        [HttpGet("NumberOfUsers")]
//        public IActionResult NumberOfUsers()
//        {
//            var number = _manageService.GetNumberOfUsers();

//            return Ok(number);
//        }

//        [AllowAnonymous]
//        [HttpGet("LastSignedUsers")]
//        public IActionResult LastSignedUsers(int number = 4)
//        {
//            var users = _manageService.GetLastSignedUsers(number);
//            var dto = _mapper.Map<IEnumerable<UserDto>>(users);

//            return Ok(dto);
//        }

//        [Authorize]
//        [HttpGet("GetUserById/{id}")]
//        public async Task<IActionResult> GetUserById(string id)
//        {
//            var result = _manageService.IsExistUser(id);
//            if (!result)
//                return NotFound("Sorry We Dont Found This User");

//            var userInDb = await _manageService.GetUserById(id);
//            var dto = _mapper.Map<UserDto>(userInDb);

//            return Ok(dto);
//        }



//        [HttpGet("AllUsers")]
//        public async Task<IActionResult> AllUsers(int page = 1)
//        {
//            var users = await _manageService.GetAllUsers(page);
//            var dto = _mapper.Map<IEnumerable<UserDto>>(users);

//            return Ok(dto);
//        }

//        [HttpGet("AllUsersOnRole")]
//        public async Task<IActionResult> AllUsersOnRole(string roleName)
//        {
//            var result = await _manageService.IsExistRole(roleName);
//            if (!result)
//                return NotFound("This Role Is not Exist");

//            var users = await _manageService.GetUsersInRole(roleName);
//            var dto = _mapper.Map<IEnumerable<UserDto>>(users);

//            return Ok(dto);
//        }



//        [HttpPost("AddToRole")]
//        public async Task<IActionResult> AddToRole(RoleRequest dto)
//        {
//            var isExistUser = _manageService.IsExistUser(dto.UserId);
//            if (!isExistUser)
//                return NotFound("Sorry We Dont Found This User");

//            var isExistRole = await _manageService.IsExistRole(dto.Role);
//            if (!isExistRole)
//                return NotFound("This Role Is not Exist");

//            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
//            var isHaveAppility = await _manageService.CheckPremetionAsync(currentUserId, dto.Role);
//            if (!isHaveAppility)
//                return NotFound("Sorry you does not Have any Premations To add Any User To Owners Table");

//            var result = await _manageService.AddToRoleAsync(dto);
//            if (!result.IsSucceded)
//                return BadRequest(result.Msg);

//            return Ok(result.Msg);
//        }

//        [HttpPost("RemoveFromRole")]
//        public async Task<IActionResult> RemoveFromRole(RoleRequest dto)
//        {
//            var isExistUser = _manageService.IsExistUser(dto.UserId);
//            if (!isExistUser)
//                return NotFound("Sorry We Dont Found This User");

//            var isExistRole = await _manageService.IsExistRole(dto.Role);
//            if (!isExistRole)
//                return NotFound("This Role Is not Exist");

//            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
//            var isHaveAppility = await _manageService.CheckPremetionAsync(currentUserId, dto.Role);
//            if (!isHaveAppility)
//                return NotFound("Sorry you does not Have any Premations To add Any User To Owners Table");

//            var result = await _manageService.RemoveFromRoleAsync(dto);
//            if (!result.IsSucceded)
//                return BadRequest(result.Msg);

//            return Ok(result.Msg);
//        }
//    }
//}
