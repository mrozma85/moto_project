using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moto_API.Data;
using Moto_API.Models;
using Moto_API.Repository.IRepository;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Http.HttpResults;
using Moto_API.Models.Dto;
using System;
using Microsoft.AspNetCore.Mvc;

namespace Moto_API.Repository
{
    public class ListUsersRepository : Repository<ApplicationUser>, IListUsersRepository
    {
        private readonly MotoDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public ListUsersRepository(MotoDbContext db, UserManager<ApplicationUser> userManager) : base(db)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<List<ApplicationUser>> GetById(string id)
        {
            var user = _db.ApplicationUsers.Where(u => u.Id == id)
                                              .Include(u => u.UserRoles)
                                              .ThenInclude(ur => ur.Role).AsNoTracking().ToList();

            //var users = _userManager.Users.Where(u => u.Id == id)
            //                                  .Include(u => u.UserRoles)
            //                                  .ThenInclude(ur => ur.Role).AsNoTracking();
            return user;
        }
        public async Task<List<ApplicationUser>> UpdateAsync(string roleName, List<ApplicationUser> entity)
        {
            // Ze stronki microsoft "DbUpdateConcurrencyException"

            var saved = false;
            while (!saved)
            {
                try
                {
                    // Attempt to save changes to the database
                    _db.ApplicationUsers.Update(entity[0]);
                    await _db.SaveChangesAsync();
                    saved = true;

                    var user = entity[0].Id;
                    var userId = await _userManager.FindByIdAsync(new Guid(user).ToString());
                    var roles = await _userManager.GetRolesAsync(userId);

                    await _userManager.RemoveFromRolesAsync(userId, roles.ToArray());
                    await _userManager.AddToRoleAsync(userId, roleName);

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is ApplicationUser)
                        {
                            var proposedValues = entry.CurrentValues;
                            var databaseValues = entry.GetDatabaseValues();

                            foreach (var property in proposedValues.Properties)
                            {
                                var proposedValue = proposedValues[property];
                                var databaseValue = databaseValues[property];

                                // TODO: decide which value should be written to database
                                // proposedValues[property] = <value to be saved>;
                            }

                            // Refresh original values to bypass next concurrency check
                            entry.OriginalValues.SetValues(databaseValues);
                        }
                        else
                        {
                            throw new NotSupportedException(
                                "Don't know how to handle concurrency conflicts for "
                                + entry.Metadata.Name);
                        }
                    }
                }

            }
            return entity;
        }
    }
}
