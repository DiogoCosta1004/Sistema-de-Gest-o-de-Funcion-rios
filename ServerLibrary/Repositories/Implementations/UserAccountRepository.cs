using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BaseLibrary.DTOs;
using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServerLibrary.Data;
using ServerLibrary.Helpers;
using ServerLibrary.Repositories.Contracts;
using Constants = ServerLibrary.Helpers.Constants;

namespace ServerLibrary.Repositories.Implementations
{
    public class UserAccountRepository(IOptions<JWTSection> config, AppDbContext appDbContext) : IUserAccount
    {
        public async Task<GeneralResponse> CreateAsync(Registro user)
        {
            if (user is null)
                return new GeneralResponse(false, "Modelo está vazio");

            var checkUser = await FindUserByEmail(user.Email);
            if (checkUser != null)
                return new GeneralResponse(false, "Usuário já cadastrado");

            //salvamento de user
            var applicationUser = await AddToDatabase(new ApplicationUser()
            {
                NomeCompleto = user.NomeCompleto,
                Email = user.Email,
                Senha = BCrypt.Net.BCrypt.HashPassword(user.Senha)
            });

            //Fazer a checagem
            var checkAdminRole = await appDbContext.SystemRoles.FirstOrDefaultAsync(_ => _.Nome.Equals(Constants.Admin));
            if (checkAdminRole is null)
            {
                var createAdminRole = await AddToDatabase(new SystemRole() { Nome = Constants.Admin });
                await AddToDatabase(new UserRole() { RoleId = createAdminRole.Id, UserId = applicationUser.Id });
                return new GeneralResponse(true, "Usuário cadastrado com sucesso");
            }
            var checkUserRole = await appDbContext.SystemRoles.FirstOrDefaultAsync(_ => _.Nome!.Equals(Constants.User));
            SystemRole response = new();
            if (checkUserRole is null)
            {
                response = await AddToDatabase(new SystemRole() { Nome = Constants.User });
                await AddToDatabase(new UserRole() { RoleId = response.Id, UserId = applicationUser.Id });
            }
            else
            {
                await AddToDatabase(new UserRole() { RoleId = checkUserRole.Id, UserId = applicationUser.Id });
            }
            return new GeneralResponse(true, "Conta criada com sucesso");
        }

        public async Task<LoginResponse> SignInAsync(Login user)
        {
            if (user == null)
                return new LoginResponse(false, "Modelo está vazio");

            var applicationUser = await FindUserByEmail(user.Email!);
            if (applicationUser == null)
                return new LoginResponse(false, "Usuário não encontrado");

            //Vericar a senha
            if (!BCrypt.Net.BCrypt.Verify(user.Senha, applicationUser.Senha))
                return new LoginResponse(false, "Email/Senha inválido");

            var getUserRole = await FindUserRole(applicationUser.Id);
            if (getUserRole == null)
                return new LoginResponse(false, "Função de usuário não encontrada");

            var getRoleName = await FindRoleName(getUserRole.RoleId);
            if (getUserRole == null)
                return new LoginResponse(false, "Função de usuário não encontrada");

            string jwtToken = GenerateToken(applicationUser, getRoleName!.Nome!);
            string refreshToken = GenerateRefreshToken();

            //salva o refresh token no banco
            var findUser = await appDbContext.RefreshTokenInfos.FirstOrDefaultAsync(_ => _.UserId == applicationUser.Id);
            if(findUser != null)
            {
                findUser!.Token = refreshToken;
                await appDbContext.SaveChangesAsync();
            }
            else
            {
                await AddToDatabase(new RefreshTokenInfo()
                {
                    Token = refreshToken,
                    UserId = applicationUser.Id
                });
            }
            return new LoginResponse(true, "Login Successfully", jwtToken, refreshToken);
        }

        private string GenerateToken(ApplicationUser user, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Value.Key!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.NomeCompleto!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, role!)
            };
            var token = new JwtSecurityToken(
                issuer: config.Value.Issuer,
                audience: config.Value.Audience,
                claims: userClaims,
                expires: DateTime.Now.AddSeconds(2),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<UserRole> FindUserRole(int userId) => await appDbContext.UserRoles.FirstOrDefaultAsync(_ => _.UserId == userId);  
        private async Task<SystemRole> FindRoleName(int roleId) => await appDbContext.SystemRoles.FirstOrDefaultAsync(_ => _.Id == roleId);  
        private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        private async Task<ApplicationUser> FindUserByEmail(string email) =>
            await appDbContext.ApplicationUsers.FirstOrDefaultAsync(_ => _.Email!.ToLower()!.Equals(email!.ToLower()));

        private async Task<T> AddToDatabase<T>(T model)
        {
            var result = appDbContext.Add(model!);
            await appDbContext.SaveChangesAsync();
            return (T)result.Entity;
        }

        public async Task<LoginResponse> RefreshTokenAsync(RefreshToken token)
        {
            if(token == null)
                return new LoginResponse(false, "Modelo está vazio");

            var findToken = await appDbContext.RefreshTokenInfos.FirstOrDefaultAsync(_ => _.Token!.Equals(token.Token));
            if (findToken == null)
                return new LoginResponse(false, "Token não encontrado");    

            //pegar os detalhes do usuário
            var user = await appDbContext.ApplicationUsers.FirstOrDefaultAsync(_ => _.Id == findToken.UserId);
            if (user == null)
                return new LoginResponse(false, "Usuário não encontrado");

            var userRole = await FindUserRole(user.Id);
            var roleName = await FindRoleName(userRole.RoleId);
            string jwtToken = GenerateToken(user, roleName.Nome!);
            string refreshToken = GenerateRefreshToken();

            //atualizar o token
            var updateRefreshToken = await appDbContext.RefreshTokenInfos.FirstOrDefaultAsync(_ => _.UserId == user.Id);
            if (updateRefreshToken == null)
                return new LoginResponse(false, "O token de atualização não pôde ser gerado porque o usuário não fez login");

            updateRefreshToken.Token = refreshToken;
            await appDbContext.SaveChangesAsync();
            return new LoginResponse(true, "Token atualizado com sucesso", jwtToken, refreshToken);
        }
    }
}
