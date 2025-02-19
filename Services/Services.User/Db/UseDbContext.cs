using Infrastructur.Messaging.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Services.User.Db;

public class UseDbContext(DbContextOptions<UseDbContext> options) : OutboxDbContext(options)
{
    
}