<h1>BPMTK</h1>
<p>
BPMTK is a workflow and Business Process Management (BPM) Platform targeted at business people, developers and system admins, based on .NET Platform(.NET Standard 2.0). 
It's open-source and distributed under the Apache license.
</p>

<h3>Visual Studio</h3>
<ul>
    <li>Download Visual Studio 2017 or 2019 (any edition) from https://www.visualstudio.com/downloads/</li>
    <li>Open bpmtk.sln and wait for Visual Studio to restore all Nuget packages</li>
</ul>

<h3>Create database schema</h3>
<p>(ConsoleApp project) </p>
<ul>
    <li>Remove exists migrations: dotnet ef migrations remove</li>
    <li>Create migrations: dotnet ef migrations add {MigrationName}</li>
    <li>Update database: dotnet ef database update</li>
</ul>