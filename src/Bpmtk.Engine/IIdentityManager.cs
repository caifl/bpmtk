using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine
{
    public interface IIdentityManager
    {
        IQueryable<User> Users
        {
            get;
        }

        IQueryable<Group> Groups
        {
            get;
        }

        Task<IList<User>> GetUsersAsync(params int[] userIds);

        Task<IList<Group>> GetGroupsAsync(params int[] groupIds);

        Task CreateUserAsync(User user);

        Task<User> FindUserByIdAsync(int userId);

        Task<User> FindUserByNameAsync(string name);

        Task CreateGroupAsync(Group group);

        Task<Group> FindGroupByIdAsync(int groupId);

        Task<Group> FindGroupByNameAsync(string name);

        Task UpdateGroupAsync(Group group);

        Task UpdateUserAsync(User user);

        Task DeleteGroupAsync(Group group);

        Task DeleteUserAsync(User user);
    }
}
