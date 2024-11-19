using ChatHub.Application.Common.Interfaces;
using ChatHub.Domain.Entity;
using ChatHub.Domain.Entity.Chat;
using ChatHub.Domain.Entity.setup;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection.Emit;

namespace ChatHub.Infrastructure;

public class AppDbContext : DbContext, IAppDbContext
{
    private readonly IUser _user;

    public AppDbContext(DbContextOptions<AppDbContext> options, IUser user) : base(options)
    {
        _user = user;
    }

    public DbSet<AspNetRole> AspNetRoles => Set<AspNetRole>();
    public DbSet<AspNetUser> AspNetUsers => Set<AspNetUser>();
    public DbSet<AspNetUserClaim> AspNetUserClaims => Set<AspNetUserClaim>();
    public DbSet<AspNetUserLogin> AspNetUserLogins => Set<AspNetUserLogin>();
    public DbSet<User> Users => Set<User>();
    public DbSet<ChatHistory> ChatHistories => Set<ChatHistory>();

    public IDbContextTransaction BeginTransaction()
    {
        return Database.BeginTransaction();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return await Database.BeginTransactionAsync(cancellationToken);
    }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(builder);

        builder.Entity<AspNetUserLogin>()
        .HasKey(u => new { u.LoginProvider, u.ProviderKey });
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>().ToList())
        {


            var originalValues = entry.OriginalValues;
            var currentValues = entry.CurrentValues;
            var logMessage = new List<string>();

            switch (entry.State)
            {

                case EntityState.Added:
                    entry.Entity.CreateDate = DateTime.UtcNow;
                    entry.Entity.CreateBy = _user.UserName;

                    break;

                case EntityState.Modified:
                    entry.Entity.UpdateDate = DateTime.UtcNow;
                    entry.Entity.UpdateBy = _user.UserName;
                    break;
            }
        }
        return await base.SaveChangesAsync(cancellationToken);
    }

}
