using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Storage;
using Bpmtk.Engine.Utils;

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
            user.Created = Clock.Now;

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

        public virtual Group FindGroupById(string groupId)
            => this.session.Find<Group>(groupId);

        //public virtual async Task<Group> FindGroupByNameAsync(string name)
        //{
        //    var q = this.Groups.Where(x => x.Name == name);

        //    return this.session.QuerySingleAsync(q);
        //}

        public virtual User FindUserById(string userId)
            => this.session.Find<User>(userId);

        //public virtual Task<User> FindUserByNameAsync(string name)
        //{
        //    var query = this.session.Users.Where(x => x.UserName == name);

        //    return this.session.QuerySingleAsync(query);
        //}

        public virtual IList<User> GetUsers(params string[] userIds)
        {
            var query = this.session.Users
                .Where(x => userIds.Contains(x.Id))
                .Distinct();

            return query.ToList(); //this.session.QueryMultipleAsync(query);
        }

        public virtual IList<Group> GetGroups(params string[] groupIds)
        {
            var query = this.session.Groups
                .Where(x => groupIds.Contains(x.Id))
                .Distinct();

            return query.ToList(); //this.session.QueryMultipleAsync(query);
        }

        public Task UpdateGroupAsync(Group group)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public virtual void CreateUser(User user)
        {
            this.session.Save(user);
            this.session.Flush();
        }

        public virtual void CreateGroup(Group group)
        {
            this.session.Save(group);
            this.session.Flush();
        }
    }
}
