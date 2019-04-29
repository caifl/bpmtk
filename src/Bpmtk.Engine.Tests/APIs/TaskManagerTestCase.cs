using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Bpmtk.Engine.Tests.APIs
{
    public class TaskManagerTestCase : BpmtkTestCase
    {
        public TaskManagerTestCase(ITestOutputHelper output) : base(output)
        {
        }

        //[Fact]
        //public async Task IdentityLinks()
        //{
        //    var procInstId = 33L;

        //    var identityLinks = await this.runtimeManager.GetIdentityLinksAsync(procInstId);

        //    Assert.True(identityLinks.Count == 0);

        //    //Add user
        //    await this.runtimeManager.AddUserLinksAsync(procInstId, new int[] { 1 }, "superior");

        //    //
        //    identityLinks = await this.runtimeManager.GetIdentityLinksAsync(procInstId);
        //    Assert.True(identityLinks.Count == 1);
        //    Assert.True(identityLinks[0].User.Id == 1);

        //    //Add group
        //    await this.runtimeManager.AddGroupLinksAsync(procInstId, new int[] { 2 }, "superior");

        //    identityLinks = await this.runtimeManager.GetIdentityLinksAsync(procInstId);
        //    Assert.True(identityLinks.Count == 2);

        //    //remove identity-links.
        //    await this.runtimeManager.RemoveIdentityLinksAsync(procInstId,
        //        identityLinks.Select(x => x.Id).ToArray());

        //    identityLinks = await this.runtimeManager.GetIdentityLinksAsync(procInstId);
        //    Assert.True(identityLinks.Count == 0);
        //}

        //[Fact]
        //public async Task Variables()
        //{
        //    var procInstId = 26L;

        //    var variables = await this.runtimeManager.GetVariablesAsync(procInstId);

        //    //Assert.True(variables.Count == 0);

        //    //Set Variables
        //    var map = new Dictionary<string, object>();
        //    map.Add("var_1", "hi var_1"); // string type.
        //    map.Add("var_2", DateTime.Now); // date type.
        //    map.Add("var_3", 3L); // long type.
        //    map.Add("var_4", 999); // int type.

        //    await this.runtimeManager.SetVariablesAsync(procInstId, map);

        //    //
        //    variables = await this.runtimeManager.GetVariablesAsync(procInstId);
        //    //Assert.True(variables.Count == 4);
        //}

        //[Fact]
        //public async Task SetProcInstAttrs()
        //{
        //    var procInstId = 26L;

        //    var name = "hello word";

        //    await this.runtimeManager.SetNameAsync(procInstId, name);

        //    await this.runtimeManager.SetKeyAsync(procInstId, "234324324324");

        //    var pi = await this.runtimeManager.FindAsync(procInstId);
        //    Assert.True(pi.Key == "234324324324");
        //    Assert.True(pi.Name == name);
        //}

        //[Fact]
        //public async Task Comments()
        //{
        //    var procInstId = 26L;

        //    await this.runtimeManager.AddCommentAsync(procInstId, "hello comment.");

        //    var items = await this.runtimeManager.GetCommentsAsync(procInstId);
        //    Assert.True(items.Count == 1);
        //}
    }
}
