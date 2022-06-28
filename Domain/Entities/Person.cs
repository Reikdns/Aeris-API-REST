using Domain.Entities.Base;

namespace Domain.Entities;

public class Person : EntityBase<Guid>
{
	public string Name { get; set; }

	public string LastName { get; set; }

	public int Age { get; set; }

	public string IdNumber { get; set; }
}