using Application.School.Http.Dto;
using Domain.Entities;
using Domain.Repository;
using Infrastructure.Common.Response;

namespace Application.Security.Service;

public class UserService
{
	private readonly IGenericRepository<User> _repository;

	public UserService(IGenericRepository<User> repository)
	{
		_repository = repository;
	}

	public async Task<Response<UserDto>> SaveUser(User user)
	{
		if (!Validate(user.Username))
			return new Response<UserDto>("El usuario ya ha sido registrado.");
		try
		{
			await _repository.CreateAsync(user);
			// TODO: realizar devolución de respuesta
			return new Response<UserDto>(user);
		}
		catch (Exception e)
		{
			return new Response<UserDto>(e.Message);
		}
	}

	public bool Validate(string identificacion)
	{
		if (SearchById(identificacion).Error)
		{
			return true;
		}

		return false;
	}

	public SearchByIdResponse SearchById(string identificacion)
	{
		var response = SearchAllUsers();
		foreach (var user in response.Users)
		{
			if (user.Identificacion == identificacion)
			{
				return new SearchByIdResponse(user);
			}
		}

		return new SearchByIdResponse("El usuario no ha sido encontrado.");
	}

	public SearchUsersResponse SearchAllUsers()
	{
		try
		{
			_connection.Open();
			List<User> Users = _repository.SearchAll();
			_connection.Close();
			return new SearchUsersResponse(Users);
		}
		catch (Exception e)
		{
			return new SearchUsersResponse(e.Message);
		}
	}
}