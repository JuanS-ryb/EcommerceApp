using Ecommerce.server.Context;
using Ecommerce.server.Dto;
using Ecommerce.server.Models;
using Ecommerce.server.services.interfaces;
using Ecommerce.server.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Ecommerce.server.services
{
    public class AuthService (AppDbContext context, JwtUtils jwt) : IAuthService
    {
        public async Task<AuthDto?> LoginAsync(UserDto request)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Name == request.Name);
            if (user is null) return null;

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed) return null;


            return new()
            {
                Token = jwt.CreateToken(user),
                Email = user.Email,
                Name = user.Name
            };
        }
        public async Task<User?> RegisterAsync(UserDto request)
        {
            if (await context.Users.AnyAsync(u => u.Name == request.Name)) return null;

            var user = new User { 
                Name = request.Name,
                Email = request.Email,
                CreatedAt = DateTime.Now
            };
            user.PasswordHash = jwt.HashMyPassword(user, request.Password);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return user;
        }
        public async Task<User?> GetMyUser()
        {
            int? myId = jwt.GetIdByToken();

            return myId is null ? throw new Exception("No User in token") : await context.Users.FirstOrDefaultAsync(u => u.Id == myId.Value);
        }
    }
}
