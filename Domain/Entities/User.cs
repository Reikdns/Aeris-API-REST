using Domain.Entities.Base;

namespace Domain.Entities;

public class User : EntityBase<Guid>
{
	public string Username { get; set; }

	public string Password { get; set; }

	public string Rol { get; set; }
}