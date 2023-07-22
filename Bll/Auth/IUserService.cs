using AutoMapper;
using Bll.Exception;
using Dal;
using Dal.Entities;
using Microsoft.EntityFrameworkCore;
using WorldBuilderBLL.Auth.Dto;

namespace Bll.Auth;

public interface IUserService
{
    Task<UserIdentityDto> Register(RegisterDto registerDto);
}