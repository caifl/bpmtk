using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.Identity
{
    public class IdentityManager : IIdentityManager
    {
        private readonly IDbSession db;

        public IdentityManager(Context context)
        {
            this.db = context.DbSession;
            this.Context = context;
        }

        public virtual Context Context
        {
            get;
        }

        public virtual IQueryable<User> Users => db.Users;

        public virtual IQueryable<Group> Groups => db.Groups;

        public virtual Task CreateGroupAsync(Group group)
        {
            return this.db.SaveAsync(group);
        }

        public virtual Task CreateUserAsync(User user)
        {
            return this.db.SaveAsync(user);
        }

        public virtual Task DeleteGroupAsync(Group group)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public virtual Task<Group> FindGroupByIdAsync(int groupId)
            => this.db.FindAsync<Group>(groupId);

        public virtual async Task<Group> FindGroupByNameAsync(string name)
        {
            var q = this.Groups.Where(x => x.Name == name);

            return await this.db.QuerySingleAsync(q);
        }

        public virtual Task<User> FindUserByIdAsync(int userId)
            => this.db.FindAsync<User>(userId);

        public virtual Task<User> FindUserByNameAsync(string name)
        {
            var query = this.db.Users.Where(x => x.UserName == name);

            return this.db.QuerySingleAsync(query);
        }

        public Task UpdateGroupAsync(Group group)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
