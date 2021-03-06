Uhuru Software .Net Extensions to Cloud Foundry
===============================================

Copyright (c) 2011 Uhuru Software, Inc., All Rights Reserved

## Code Status
### Current Build (Unit Tests)

[![Build Status](http://uhurusoftware.com/build_status/status-vcap.png)](http://uhurusoftware.com/build_status/vcap-dotnet.htm)

### Last Nightly Build (Integration and System Tests)

[![Build Status](http://uhurusoftware.com/build_status/status-vcap-nb.png)](http://uhurusoftware.com/build_status/vcap-dotnet-nb.htm)


What are the .NET Extensions to Cloud Foundry?
----------------------------------------------

This project is an effort to extend Cloud Foundry so it runs .Net web applications on a Windows environment.

Cloud Foundry was developed on Linux and Ruby and lacks support for Microsoft Windows Server environments. The .NET Extensions from Uhuru software are built entirely on Windows and .NET. We have ported the Cloud Foundry NATS client message bus, DEA (Droplet Execution Engine) and the Service Node base components to .NET and Windows Server. The Uhuru Software .NET Extensions allow Windows Servers to be full-fledged Cloud Foundry citizens. Windows developers can now benefit from the same Cloud Foundry application deployment advances that Ruby developers already enjoy.

The .NET Extensions also make it possible for the open source developer community to add new Cloud Foundry enabled frameworks and services to Windows Servers.

So far the Uhuru .NET Extensions to Cloud Foundry include:

* A dotNet framework that runs .Net 2.0, 3.5 and 4.0 web applications on IIS 7
* An mssql system service that allows you to provision MS SQL Server 2008 R2 databases using Cloud Foundry

License
-------

This project uses the Apache 2 license.  See LICENSE for details.

Development Tools
-----------------

This is a list of tools used by the Uhuru Software development team:

* Visual Studio 2010 Ultimate
* Style Cop (needed for code analysis)
* Visual Studio Power Tools (makes life easier)
* Ghost Doc (helps with documenting the code)
* PowerCommands for Visual Studio (makes life easier)
* Sandcastle Help File Builder (needed for creating .chm documentation)


Installation Notes
------------------

To run .Net on Cloud Foundry, the first thing you need is a Cloud Foundry installation. Refer to the [http://www.github.com/CloudFoundry/vcap](vcap repo) for details on how to setup that up.

Once you have a Cloud Foundry setup running, you can either:

* get the code and compile it to get the installers (use the vcap-dotnet-installers solution)
* grab the latest release from [http://www.uhurusoftware.com/products.aspx](here)

Install "DEAInstaller.msi" and "MSSqlNodeInstaller.msi" on a box that meets the following prerequisites:

* Windows 7 or Windows 2008 Server R2
* IIS 7
* MS SQL Server 2008 R2

Detailed Install/Run Instructions:
----------------------------------

### Installing the Windows DEA

#### Prerequisites
* IIS 7 (Default features should work. Best, install all features.) 
* .NET 4.0 Framework

You have to run DEAInstaller.msi in order to install the service. There are two ways to do that.

#### Install from from command line using following command and parameters: 

    msiexec /i DEAInstaller.msi [parameterName=parameterValue]

Valid parameters that can be used are: 
	
	baseDir (string)
	localRoute (string)
	filerPort (int) - port for the fileserver
	messageBus (string) - message bus address (nats://user:password@someip:port/)
	multiTenant (boolean)
	maxMemory (integer)
	secure (boolean)
	enforceUlimit (integer)
	heartBeatInterval (integer)
	forceHttpSharing (boolean)

Example: 

    msiexec /i DEAInstaller.msi baseDir=C:\Droplets localRoute=192.168.1.1 filerPort=12345 messageBus=nats://user:password@192.168.1.100:4222/ secure=true maxMemory=4096

#### Install using the UI. 

In this case, you will have to edit all the configuration manually after the installation in {installationDirectory}/uhuru.config.

#### After the installation start the DeaWindowsService in services.msc

### Installing the MS Sql Server Node

#### Prerequisites
* SQL Server 2008 R2
* .NET 4.0 Framework

You have to run  MSSqlNodeInstaller.msi in order to install the service. There are two ways to do that.

#### Install from from command line using following command and parameters: 

     msiexec /i MSSqlNodeInstaller.msi [parameterName=parameterValue]

Valid parameters that can be used are: 

	nodeId (string)
	migrationNfs (string)
	mbus (string) - message bus address (nats://user:password@someip:port/)
	index (integer)
	zInterval (integer)
	maxDbSize (integer)
	maxLongQuery (integer)
	maxLongTx (integer)
	localDb (string)
	baseDir (string)
	localRoute (string)
	availableStorage (integer)
	host (string) - MSSql Server address
	user (string) - MSSql Server user
	password (string) - MSSql Server password
	port (integer) - MSSql Server port

Example: 

    msiexec /i MSSqlNodeInstaller.msi nodeId=0 mbus=nats://user:password@192.168.1.100:4222/ localRoute=192.168.1.1 host=localhost user=admin password=password port=1433

#### Install using the UI. 

In this case, you will have to edit all the configurations manually after the installation in {installationDirectory}/uhuru.config.

#### After the installation start the MssqlNodeService in services.msc

Trying your setup
-----------------


Running the tests
-----------------
All the tests have been implemented using MSTest framework.

### Unit Tests
Unit tests take the smallest piece of testable software in the application, isolate it from the remainder of the code, and determine whether it behaves correctly.
#### Configure
The Unit Tests run out of the box, no additional configuration is needed.
#### Run
* Open Visual studio command prompt
* Build vcap-dotnet solution

        msbuild {cloneDirectory}\src\vcap-dotnet.sln

* Run tests in the "Unit" category using MSTest

        cd {clonePath}\src
        MSTest.exe /testcontainer:{cloneDirectory}\bin\Uhuru.CloudFoundry.Test.dll /category:"Unit"

### Integration Tests
This type of tests ensure that all the functional requirements are met at the component level.
#### Configure
These tests require a working NATS Server deployment.
To edit the NATS Server used for the tests follow the steps:

* Go to Uhuru.CloudFoundtry.Test project

        cd {clonePath}\src\Uhuru.CloudFoundry.Test\

* Edit the App.config file

        notepad App.config

* Set a valid NATS Server for the "nats" key:

    `<add key="nats" value="nats://nats:nats@192.168.1.120:4222"/>`

#### Run
* Open Visual studio command prompt
* Build vcap-dotnet solution

        msbuild {cloneDirectory}\src\vcap-dotnet.sln

* Build CloudTestApp solution

        msbuild {cloneDirectory}\src\Uhuru.CloudFoundry.Test\TestApps\CloudTestApp\App\CloudTestApp.sln

* Run tests in the "Integration" category using MSTest

        cd {clonePath}\src
        MSTest.exe /testcontainer:{cloneDirectory}\bin\Uhuru.CloudFoundry.Test.dll /category:"Integration"

### System Tests
System testing is conducted on the complete, integrated system to evaluate the system's compliance with the specified requirements.
#### Configure
  To run the System Tests you must have a full deployment as described above, in the deployment section. Additional configuration steps are described bellow:

* Go to Uhuru.CloudFoundtry.Test project

        cd {clonePath}\src\Uhuru.CloudFoundry.Test\

* Edit the App.config file

        notepad App.config

* Set a valid NATS Server for the nats key:

    `<add key="nats" value="nats://nats:nats@192.168.1.120:4222"/>`

* Set the target CloudFoundry deployment

    `<add key="target" value="api.uhurucloud.net"/>`

* Set a proxy for CloudFoundry deployment if necessary, else delete it

    `<proxy proxyaddress="http://192.168.1.120:80" bypassonlocal="False"/>`
    
* Set the user name for the deployment

    `<add key="adminUsername" value="continuousintegration@uhurusoftware.com"/>`

* Set the password for the deployment

    `<add key="adminPassword" value="myPassword"/>`

* We use Umbraco as a test app, you can download it from [http://umbraco.codeplex.com/](here)) and edit its web.config file like so:

    `<add key="umbracoDbDSN" value="{mssql-2008#umbracosvc}" />`

* Set the Umbraco root directory

    `<add key="umbracoRootDir" value="C:\PathToUmbraco"/>`

#### Run
* Open Visual studio command prompt
* Build vcap-dotnet solution

        msbuild {cloneDirectory}\src\vcap-dotnet.sln

* Build CloudTestApp solution

        msbuild {cloneDirectory}\src\Uhuru.CloudFoundry.Test\TestApps\CloudTestApp\App\CloudTestApp.sln

* Run tests in the "System" category using MSTest

        cd {clonePath}\src
        MSTest.exe /testcontainer:{cloneDirectory}\bin\Uhuru.CloudFoundry.Test.dll /category:"System"