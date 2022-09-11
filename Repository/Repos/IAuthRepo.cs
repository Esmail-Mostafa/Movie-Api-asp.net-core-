using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Movie.Helper;
using Movie.Models;
using Movie.Repository.InterFace;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Movie.Repository.Repos
{
    public class IAuthRepo : IAuthInterface
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly JWT _jwt;
        public IAuthRepo(UserManager<IdentityUser> userManager , IOptions<JWT> jwt , RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this._jwt = jwt.Value;
        }

       

        public async Task<AuthDto> RegisterAsync(RegisterDto model)
        {
            if (await userManager.FindByEmailAsync(model.Email) is not null)
                return new AuthDto { Message= "Email is already registeted "};

            if (await userManager.FindByNameAsync(model.UserName) is not null)
                return new AuthDto { Message = "UserName is already registeted " };

            var user = new IdentityUser { UserName = model.UserName, Email = model.Email }; 

          var result =  await userManager.CreateAsync(user , model.Password);
            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += error.Description;
                }
                return new AuthDto { Message = errors };

            }
            await userManager.AddToRoleAsync(user, "User");

            //jwt create token
            var jwtsecurtyToken = await CreateJwtToken(user);

            return new AuthDto
            {
                Email = user.Email,
                ExiresOn = jwtsecurtyToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtsecurtyToken),
                UserName = user.UserName,   

            };
        }

        public async Task<string> AddRoleAsync(AddRoleDto model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);

            if (user is null || !await roleManager.RoleExistsAsync(model.Role))
                return "invalid user ID or role";
            if (await userManager.IsInRoleAsync(user, model.Role))
                return "user already in this role";
            var rusilt = await userManager.AddToRoleAsync(user, model.Role);

            return rusilt.Succeeded ? String.Empty : "error semrthing wrong";

        }

        public async Task<AuthDto> GetTokenasync(TkoenRequestDto model)
        {
            var authdto = new AuthDto();
            var user = await userManager.FindByEmailAsync(model.Email);
            if(user is null || !await userManager.CheckPasswordAsync(user , model.Password))
            {
                authdto.Message = "Email or password is incorrect";
                return authdto;
            }
            var jwtsecurtyToken = await CreateJwtToken(user);
            var Roleslist = await userManager.GetRolesAsync(user);
            authdto.IsAuthenticated = true;
            authdto.Token = new JwtSecurityTokenHandler().WriteToken(jwtsecurtyToken);
            authdto.Email = user.Email;
            authdto.UserName = user.UserName;
            authdto.ExiresOn = jwtsecurtyToken.ValidTo;
            authdto.Roles = Roleslist.ToList();

            return authdto;
        }

        private async Task<JwtSecurityToken> CreateJwtToken(IdentityUser user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}
