namespace Domain.Entities.Base;

public interface IEntityBase<T>
{
	T Id { get; set; }
	string CreatedBy { get; set; }
	DateTime CreatedOn { get; set; }
	string UpdatedBy { get; set; }
	DateTime UpdatedOn { get; set; }
}