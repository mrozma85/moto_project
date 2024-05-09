using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Moto_API.Models;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace Moto_API.Data
{
    public class MotoDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string
    ,
    IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>,
    IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public MotoDbContext(DbContextOptions<MotoDbContext> options) : base(options)
        {

        }

        public DbSet<Ad> Ads{ get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Category> Categories{ get; set; }
        public DbSet<AdType> AdTypes { get; set; }
        public DbSet<LocalUser> LocalUsers { get; set; }
        public DbSet<Image> VehicleImagesURL { get; set; }
        public DbSet<VehicleImages> VehicleImagesDATA { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<MainPageDetials> MainPageDetail { get; set; }
        public DbSet<Colors> Colors { get; set; }
        public DbSet<MainPageImages> MainPageImage { get; set; }
        public DbSet<Address> Addresses { get; set; }



        //override migration
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<ApplicationUser>(user =>
            //{
            //    user.HasMany(ur => ur.UserRoles)
            //        .WithOne(r => r.User)
            //        .HasForeignKey(ur => ur.UserId)
            //        .IsRequired();
            //});

            //builder.Entity<ApplicationRole>(user =>
            //{
            //    user.HasMany(ur => ur.UserRoles)
            //        .WithOne(r => r.Role)
            //        .HasForeignKey(ur => ur.RoleId)
            //    .IsRequired();
            //});

            builder.Entity<ApplicationUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId);

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId);
            });
        }
    }
}
