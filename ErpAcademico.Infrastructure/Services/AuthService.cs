using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ErpAcademico.Application.DTOs;
using ErpAcademico.Domain.Entities;
using ErpAcademico.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ErpAcademico.Infrastructure.Services;

public class AuthService
{
    private readonly AppDbContext _context;

    private readonly IConfiguration _configuration;

    public AuthService(
        AppDbContext context,
        IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task Registrar(RegisterDto dto)
    {
        var usuarioExiste =
            await _context.Usuarios
                .AnyAsync(u =>
                    u.Email == dto.Email);

        if (usuarioExiste)
            throw new Exception(
                "E-mail já cadastrado.");

        var senhaHash =
            BCrypt.Net.BCrypt.HashPassword(
                dto.Senha);

        var usuario = new Usuario(
            dto.Nome,
            dto.Email,
            senhaHash);

        await _context.Usuarios
            .AddAsync(usuario);

        await _context.SaveChangesAsync();
    }

    public async Task<string> Login(LoginDto dto)
    {
        var usuario =
            await _context.Usuarios
                .FirstOrDefaultAsync(u =>
                    u.Email == dto.Email);

        if (usuario is null)
            throw new Exception(
                "Usuário ou senha inválidos.");

        var senhaValida =
            BCrypt.Net.BCrypt.Verify(
                dto.Senha,
                usuario.SenhaHash);

        if (!senhaValida)
            throw new Exception(
                "Usuário ou senha inválidos.");

        return GerarToken(usuario);
    }

    private string GerarToken(Usuario usuario)
    {
        var jwtKey =
            _configuration["Jwt:Key"];

        var key =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey!));

        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(
                ClaimTypes.NameIdentifier,
                usuario.Id.ToString()),

            new Claim(
                ClaimTypes.Email,
                usuario.Email),

            new Claim(
                ClaimTypes.Name,
                usuario.Nome)
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}