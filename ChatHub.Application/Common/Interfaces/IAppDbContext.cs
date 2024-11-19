using ChatHub.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatHub.Domain.Entity.setup;
using Microsoft.EntityFrameworkCore.Storage;
using ChatHub.Domain.Entity.Chat;

namespace ChatHub.Application.Common.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<AspNetRole> AspNetRoles { get; }
        DbSet<AspNetUser> AspNetUsers { get; }
        DbSet<AspNetUserClaim> AspNetUserClaims { get; }
        DbSet<AspNetUserLogin> AspNetUserLogins { get; }
        DbSet<User> Users { get; }
        DbSet<ChatHistory> ChatHistories { get; }
        IDbContextTransaction BeginTransaction();
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
