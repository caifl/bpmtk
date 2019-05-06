using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine
{
    public interface IIdentityManager
    {
        IList<User> GetUsers(params string[] userIds);

        IList<Group> GetGroups(params string[] groupIds);

        void CreateUser(User user);

        User FindUserById(string userId);

        void CreateGroup(Group group);

        Group FindGroupById(string groupId);

        Task UpdateGroupAsync(Group group);

        Task UpdateUserAsync(User user);

        Task DeleteGroupAsync(Group group);

        Task DeleteUserAsync(User user);
    }
}
