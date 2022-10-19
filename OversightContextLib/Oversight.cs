namespace Mzeey.Shared;
using Microsoft.EntityFrameworkCore;

public class Oversight : DbContext
{
    public Oversight(DbContextOptions<Oversight> options): base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder){
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasOne(user => user.ResidentStatus)
            .WithMany(residentialStatus => residentialStatus.Users)
            .HasForeignKey(user => user.ResidentialStatusId);

        modelBuilder.Entity<UsersSetting>()
            .HasKey(us => new {us.UserId, us.SettingId});

        modelBuilder.Entity<UsersSetting>()
            .HasOne<User>(us => us.User)
            .WithMany(user => user.UsersSettings)
            .HasForeignKey(us => us.UserId);
        
        modelBuilder.Entity<UsersSetting>()
            .HasOne<Setting>(us => us.Setting)
            .WithMany(setting => setting.UsersSettings)
            .HasForeignKey (us => us.SettingId);
        
        modelBuilder.Entity<TwoFA>()
            .HasOne(twoFA => twoFA.User)
            .WithMany(user => user.TwoFAs)
            .HasForeignKey(twoFA => twoFA.UserId);
        
        modelBuilder.Entity<Subscription>()
            .HasOne(subscription => subscription.User)
            .WithMany(user => user.Subscriptions)
            .HasForeignKey(subscription => subscription.UserId);

        modelBuilder.Entity<Subscription>()
            .HasOne(subscription=> subscription.Fee)
            .WithMany(fee => fee.Subscriptions)
            .HasForeignKey(subscription => subscription.FeeId);

        //One-to-One relationship mapping
        modelBuilder.Entity<ResidentialStatus>()
            .HasOne<ResidentialStatusesFee>(rs => rs.ResidentialStatusFee)
            .WithOne(rsf => rsf.ResidentialStatus)
            .HasForeignKey<ResidentialStatusesFee>(rsf => rsf.ResidentialStatusId);
        
        modelBuilder.Entity<ResidentialStatusesFee>()
            .HasOne(rsf => rsf.Fee)
            .WithMany(fee => fee.ResidentStatusesFees)
            .HasForeignKey(rsf => rsf.FeeId);
        
        modelBuilder.Entity<PaymentMethod>()
            .HasOne(pm => pm.User)
            .WithMany(user => user.PaymentMethods)
            .HasForeignKey(pm => pm.UserId);
        
        modelBuilder.Entity<OtpCode>()
            .HasOne(otpcode => otpcode.User)
            .WithMany(user => user.OtpCodes)
            .HasForeignKey(otpcode => otpcode.UserId);

        modelBuilder.Entity<Invoice>()
            .HasOne(invoice => invoice.User)
            .WithMany(user => user.Invoices)
            .HasForeignKey(invoice => invoice.UserId);
    }

    public DbSet<User>? Users {get;set;}
    public DbSet<UsersSetting>? UsersSettings {get;set;}
    public DbSet<TwoFA>? TwoFAs {get;set;}
    public DbSet<Subscription>? Subscriptions {get;set;}
    public DbSet<Setting>? Settings {get;set;}
    public DbSet<ResidentialStatusesFee>? ResidentialStatusesFees {get;set;}
    public DbSet<ResidentialStatus>? ResidentialStatuses {get;set;}
    public DbSet<PaymentMethod>? PaymentMethods {get;set;}
    public DbSet<OtpCode>? OtpCodes {get;set;}
    public DbSet<Invoice>? Invoices {get;set;}
    public DbSet<Fee>? Fees {get;set;}
    public DbSet<EstateAddress>? EstateAddresses {get;set;}
}
