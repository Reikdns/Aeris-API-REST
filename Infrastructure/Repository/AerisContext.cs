using Domain.Entities;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class AerisContext : DbContext
{
	private readonly AppSettings _appSettings;

	public AerisContext(DbContextOptions<AerisContext> options, AppSettings appSettings) : base(options)
	{
		_appSettings = appSettings;
	}

	public async Task CommitAsync()
	{
		await SaveChangesAsync().ConfigureAwait(false);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema(_appSettings.SchemaName);
	}
}