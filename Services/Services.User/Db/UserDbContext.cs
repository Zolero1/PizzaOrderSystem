using Infrastructur.Messaging.Outbox;
using Microsoft.EntityFrameworkCore;
using Services.User.Domain;

namespace Services.User.Db;

public class UserDbContext(DbContextOptions<UserDbContext> options) : OutboxDbContext(options)
{
    public DbSet<OrderRequest> OrderRequests { get; set; }
}