using Infrastructur.Messaging.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Services.Payment.Db;

public class PaymentDbContext(DbContextOptions<PaymentDbContext> options) : OutboxDbContext(options) {
    public DbSet<Domain.Payment> Payments { get; set; }
    
}