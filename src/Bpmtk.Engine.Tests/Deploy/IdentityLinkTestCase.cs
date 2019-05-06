using Bpmtk.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Bpmtk.Engine.Tests.Deploy
{
    public class IdentityLinkTestCase : BpmtkTestCase
    {
        public IdentityLinkTestCase(ITestOutputHelper output) : base(output)
        {
        }

      
        [Fact] public async Task Execute()
        {
            //var user = await this.identityManager.FindUserByNameAsync("felix");
            //var group = await this.identityManager.FindGroupByNameAsync("tests");

            var identityLinks = new IdentityLink[]
            {
                new IdentityLink() { UserId = "test", Type = "starter" },
                new IdentityLink() { GroupId = "tests", Type = "tests" }
            };

            var procDefId = 1;

            //add
            await this.deploymentManager.AddIdentityLinksAsync(procDefId, identityLinks);

            var items = await this.deploymentManager.GetIdentityLinksAsync(procDefId);
            Assert.True(items.Count >= identityLinks.Length);

            //Remove
            var array = items.Select(x => x.Id).ToArray();
            await this.deploymentManager.RemoveIdentityLinksAsync(array);

            items = await this.deploymentManager.GetIdentityLinksAsync(procDefId);
            Assert.True(items.Count == 0);

            this.Commit();
        }
    }
}
