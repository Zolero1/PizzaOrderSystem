using Infrastructur.Messaging.Outbox.Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructur.Messaging.Outbox;

public class OutboxDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<OutboxMessage> Outbox { get; set; }
}