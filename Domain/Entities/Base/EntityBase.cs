namespace Domain.Entities.Base;

public class EntityBase<T> : DomainEntity, IEntityBase<T>
{
	public T Id { get; set; } = default!;
	public string CreatedBy { get; set; }= default!;
	public DateTime CreatedOn { get; set; }
	public string UpdatedBy { get; set; } = default!;
	public DateTime UpdatedOn { get; set; }
}