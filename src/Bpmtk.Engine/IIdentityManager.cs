using System;
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
