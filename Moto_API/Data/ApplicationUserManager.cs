//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.Extensions.Options;
//using Moto_API.Models;

//namespace Moto_API.Data
//{
//    public class ApplicationUserManager : UserManager<IdentityUser>
//    {
//        private readonly UserStore<ApplicationUser, IdentityRole, MotoDbContext, string, IdentityUserClaim<string>,
//            IdentityUserRole<string>, IdentityUserLogin<string>, IdentityUserToken<string>, IdentityRoleClaim<string>> _store;

//        public ApplicationUserManager(
//            IUserStore<IdentityUser> store,
//            IOptions<IdentityOptions> optionsAccessor,
//            IPasswordHasher<IdentityUser> passwordHasher,
//            IEnumerable<IUserValidator<IdentityUser>> userValidators,
//            IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators,
//            ILookupNormalizer keyNormalizer,
//            IdentityErrorDescriber errors,
//            IServiceProvider services,
//            ILogger<UserManager<IdentityUser>> logger)
//            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
//        {
//            _store = (UserStore<ApplicationUser, IdentityRole, MotoDbContext, string, IdentityUserClaim<string>,
//                IdentityUserRole<string>, IdentityUserLogin<string>, IdentityUserToken<string>, IdentityRoleClaim<string>>)store;
//        }

//        public virtual async Task<IdentityResult> AddToRoleByRoleIdAsync(IdentityUser user, string roleId)
//        {
//            ThrowIfDisposed();

//            if (user == null)
//                throw new ArgumentNullException(nameof(user));

//            if (string.IsNullOrWhiteSpace(roleId))
//                throw new ArgumentNullException(nameof(roleId));

//            if (await IsInRoleByRoleIdAsync(user, roleId, CancellationToken))
//                return IdentityResult.Failed(ErrorDescriber.UserAlreadyInRole(roleId));

//            _store.Context.Set<IdentityUserRole<string>>().Add(new IdentityUserRole<string> { RoleId = roleId, UserId = user.Id });

//            return await UpdateUserAsync(user);
//        }

//        public async Task<bool> IsInRoleByRoleIdAsync(IdentityUser user, string roleId, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            ThrowIfDisposed();

//            if (user == null)
//                throw new ArgumentNullException(nameof(user));

//            if (string.IsNullOrWhiteSpace(roleId))
//                throw new ArgumentNullException(nameof(roleId));

//            var role = await _store.Context.Set<IdentityRole>().FindAsync(roleId);
//            if (role == null)
//                return false;

//            var userRole = await _store.Context.Set<IdentityUserRole<string>>().FindAsync(new object[] { user.Id, roleId }, cancellationToken);
//            return userRole != null;
//        }
//    }
//}