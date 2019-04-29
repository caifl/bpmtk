using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Storage;

namespace Bpmtk.Engine.Identity
{
    public class IdentityManager : IIdentityManager
    {
        private readonly IDbSession session;

        public IdentityManager(Context context)
        {
            this.session = context.DbSession;
            this.Context = context;
        }

        public virtual Context Context
        {
            get;
        }

        public virtual IQueryable<User> Users => session.Users;

        public virtual IQueryable<Group> Groups => session.Groups;

        public virtual Task CreateGroupAsync(Group group)
        {
            return this.session.SaveAsync(group);
        }

        public virtual Task CreateUserAsync(User user)
        {
            return this.session.SaveAsync(user);
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
            => this.session.FindAsync<Group>(groupId);

        public virtual async Task<Group> FindGroupByNameAsync(string name)
        {
            var q = this.Groups.Where(x => x.Name == name);

            return await this.session.QuerySingleAsync(q);
        }

        public virtual Task<User> FindUserByIdAsync(int userId)
            => this.session.FindAsync<User>(userId);

        public virtual Task<User> FindUserByNameAsync(string name)
        {
            var query = this.session.Users.Where(x => x.UserName == name);

            return this.session.QuerySingleAsync(query);
        }

        public virtual Task<IList<User>> GetUsersAsync(params int[] userIds)
        {
            var query = this.session.Users
                .Where(x => userIds.Contains(x.Id))
                .Distinct();

            return this.session.QueryMultipleAsync(query);
        }

        public virtual Task<IList<Group>> GetGroupsAsync(params int[] groupIds)
        {
            var query = this.session.Groups
                .Where(x => groupIds.Contains(x.Id))
                .Distinct();

            return this.session.QueryMultipleAsync(query);
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
