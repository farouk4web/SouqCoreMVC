//using AutoMapper;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Souq.DTOs;
//using Souq.Services.Authantication;
//using Souq.Services.ManageService;
//using Souq.Services.ShoppingCart;
//using Souq.Settings;
//using Souq.ViewModels.Account;
//using System.Security.Claims;

//namespace Souq.Controllers
//{
//    [ApiController]
//    [Route("api/[Controller]")]
//    public class AccountssController : ControllerBase
//    {
//        private readonly IAuthanticationService _authanticationService;
//        private readonly IManageService _manageService;
//        private readonly ICartService _cartService;
//        private readonly IMapper _mapper;
//        public AccountssController(IAuthanticationService authanticationService, IManageService manageService, ICartService cartService, IMapper mapper)
//        {
//            _authanticationService = authanticationService;
//            _manageService = manageService;
//            _cartService = cartService;
//            _mapper = mapper;
//        }

//        [HttpPost("Register")]
//        public async Task<IActionResult> Register(RegisterRequest dto)
//        {
//            AuthanticationResponseDto response = await _authanticationService.RegisterAsync(dto);

//            if (!response.IsAuthanticated)
//                return BadRequest(response.Message);

//            _cartService.CreateCart4CurrentUser(response.Message);
//            response.Message = string.Empty;

//            return Ok(response);
//        }

//        [HttpPost("Login")]
//        public async Task<IActionResult> Login(LoginDto dto)
//        {
//            AuthanticationResponseDto response = await _authanticationService.LoginAsync(dto);

//            if (!response.IsAuthanticated)
//                return BadRequest(response.Message);

//            return Ok(response);
//        }

       

//        [Authorize]
//        [HttpPost("LogOut")]
//        public IActionResult RevokeToken(RefreshTokenDto dto)
//        {
//            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
//            bool result = _authanticationService.RevokeTokenAsync(dto, currentUserId);

//            if (!result)
//                return BadRequest();

//            return Ok();
//        }

//        [Authorize]
//        [HttpPost("LogOutFromAllDevices")]
//        public IActionResult RevokeAllToken()
//        {
//            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
//            bool result = _authanticationService.RevokeAllTokens(currentUserId);

//            if (!result)
//                return BadRequest("some thing went wrong, please try again");

//            return Ok("you are log out from all devices");
//        }

//        [Authorize]
//        [HttpPost("ChangePassword")]
//        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
//        {
//            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
//            ServiceResult result = await _authanticationService.ChangePasswordAsync(dto, currentUserId);

//            if (!result.IsSucceded)
//                return BadRequest(result.Msg);

//            return Ok(result.Msg);
//        }

//        [Authorize]
//        [HttpPost("ConfirmEmail")]
//        public async Task<IActionResult> ConfirmEmail()
//        {
//            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
//            var result = await _authanticationService.ConfirmEmailAsync(currentUserId);

//            if (!result.IsSucceded)
//                return BadRequest(result.Message);

//            return Ok(result.Message);
//        }

//        [Authorize]
//        [HttpPost("ConfirmEmailUsingOTP")]
//        public async Task<IActionResult> ConfirmEmailByOTP(ConfirmEmailRequest dto)
//        {
//            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
//            var result = await _authanticationService.ConfirmEmailByOTPAsync(dto, currentUserId);

//            if (!result.IsSucceded)
//                return BadRequest(result.Msg);

//            return Ok(result.Msg);
//        }


//        [HttpPost("ForgetPassword")]
//        public async Task<IActionResult> ForgetPassword(ForgetPasswordRequest dto)
//        {
//            var result = await _authanticationService.ForgetPasswordAsync(dto);

//            if (!result.IsSucceded)
//                return BadRequest(result.Msg);

//            return Ok(result.Msg);
//        }

//        [HttpPost("ResetPassword")]
//        public async Task<IActionResult> ResetPassword(ResetPasswordRequest dto)
//        {
//            var result = await _authanticationService.ResetPasswordAsync(dto);

//            if (!result.IsSucceded)
//                return BadRequest(result.Msg);

//            return Ok(result.Msg);
//        }




//    }
//}
