using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.Framework.CodeGeneration.CodeGenerators
{

    public class ProjectGenerator
    {
        public Dictionary<string, string> GetBussiness(string solutionname, string projName)
        {
            var result = new Dictionary<string, string>();
            var projectGuid = Guid.NewGuid();
            var projectName = string.Format("{0}.{1}", solutionname, projName);
            var assembleGuid = Guid.NewGuid();
            var assemblyCompany = "";
            var assemblyYear = DateTime.Now.Year.ToString();
            
            #region Project
            var str = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Project ToolsVersion=""12.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <Import Project=""$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props"" Condition=""Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')"" />
	<PropertyGroup>
		<Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>
		<Platform Condition="" '$(Platform)' == '' "">AnyCPU</Platform>
		<ProjectGuid>{0}</ProjectGuid>
		<OutputType>Library</OutputType>
		<AppDesignerFolder>Properties</AppDesignerFolder>
		<RootNamespace>{1}</RootNamespace>
		<AssemblyName>{1}</AssemblyName>
		<TargetFrameworkVersion>v4.5</TargetFrameworkVersion>
		<FileAlignment>512</FileAlignment>
	</PropertyGroup>
	<PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' "">
		<DebugSymbols>true</DebugSymbols>
		<DebugType>full</DebugType>
		<Optimize>false</Optimize>
		<OutputPath>bin\Debug\</OutputPath>
		<DefineConstants>DEBUG;TRACE</DefineConstants>
		<ErrorReport>prompt</ErrorReport>
		<WarningLevel>4</WarningLevel>
	</PropertyGroup>
	<PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' "">
		<DebugType>pdbonly</DebugType>
 		<Optimize>true</Optimize>
		<OutputPath>bin\Release\</OutputPath>
		<DefineConstants>TRACE</DefineConstants>
		<ErrorReport>prompt</ErrorReport>
		<WarningLevel>4</WarningLevel>
	</PropertyGroup>
	<ItemGroup>
        <Reference Include=""Microsoft.SqlServer.Types, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91, processorArchitecture=MSIL"">
          <SpecificVersion>False</SpecificVersion>
          <HintPath>Libs\Microsoft.SqlServer.Types.dll</HintPath>
        </Reference>
        <Reference Include=""Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed, processorArchitecture=MSIL"">
          <SpecificVersion>False</SpecificVersion>
          <HintPath>Libs\Newtonsoft.Json.dll</HintPath>
        </Reference>
		<Reference Include=""System""/>
		<Reference Include=""System.Core""/>
		<Reference Include=""System.Xml.Linq""/>
		<Reference Include=""System.Data.DataSetExtensions""/>
		<Reference Include=""Microsoft.CSharp""/>
 		<Reference Include=""System.Data""/>
		<Reference Include=""System.Xml""/>
	</ItemGroup>
	<ItemGroup>
		<Compile Include=""Properties\AssemblyInfo.cs"" />
	</ItemGroup>
    <ItemGroup>
        <Content Include=""Libs\Microsoft.SqlServer.Types.dll"" />
        <Content Include=""Libs\Newtonsoft.Json.dll"" />
    </ItemGroup>
	<Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />
  <!-- To modify your build process, add your task inside one of the targets below and uncomment it. 
       Other similar extension points exist, see Microsoft.Common.targets.
  <Target Name=""BeforeBuild"">
  </Target>
  <Target Name=""AfterBuild"">
  </Target>
  -->
 </Project>";
            var projectFileName = string.Format(@"{0}.csproj", projectName);
            var projectFileContent = string.Format(str, projectGuid, projectName);
            result.Add(projectFileName, projectFileContent);
            #endregion
            #region Assembly

            var str2 = @"using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle(""{0}"")]
[assembly: AssemblyDescription("""")]
[assembly: AssemblyConfiguration("""")]
[assembly: AssemblyCompany(""{1}"")]
[assembly: AssemblyProduct(""{0}"")]
[assembly: AssemblyCopyright(""Copyright © {1} {2}"")]
[assembly: AssemblyTrademark("""")]
[assembly: AssemblyCulture("""")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid(""{3}"")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion(""1.0.*"")]
[assembly: AssemblyVersion(""1.0.0.0"")]
[assembly: AssemblyFileVersion(""1.0.0.0"")]";

            var assemblyFileName = string.Format(@"Properties\assemblyinfo.cs", projectName);
            var assemblyContent = string.Format(str2, projectName, assemblyCompany, assemblyYear, assembleGuid);
            result.Add(assemblyFileName, assemblyContent);
            
            #endregion

            return result;
        }

        public Dictionary<string, string> GetWebServiceProject(string solutionname, string projName)
        {
            var result = new Dictionary<string, string>();
            var projectGuid = Guid.NewGuid();
            var projectName = string.Format("{0}.{1}", solutionname, projName);
            var assembleGuid = Guid.NewGuid();
            var assemblyCompany = "";
            var assemblyYear = DateTime.Now.Year.ToString();

            #region Project
            var str = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Project ToolsVersion=""12.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <Import Project=""$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props"" Condition=""Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')"" />
  <PropertyGroup>
    <Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>
    <Platform Condition="" '$(Platform)' == '' "">AnyCPU</Platform>
    <ProductVersion>
    </ProductVersion>
    <SchemaVersion>2.0</SchemaVersion>
    <ProjectGuid>{{{0}}}</ProjectGuid>
    <ProjectTypeGuids>{{349c5851-65df-11da-9384-00065b846f21}};{{fae04ec0-301f-11d3-bf4b-00c04f79efbc}}</ProjectTypeGuids>
    <OutputType>Library</OutputType>
    <AppDesignerFolder>Properties</AppDesignerFolder>
    <RootNamespace>{1}</RootNamespace>
    <AssemblyName>{1}</AssemblyName>
    <TargetFrameworkVersion>v4.5</TargetFrameworkVersion>
    <UseIISExpress>true</UseIISExpress>
    <IISExpressSSLPort />
    <IISExpressAnonymousAuthentication />
    <IISExpressWindowsAuthentication />
    <IISExpressUseClassicPipelineMode />
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' "">
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>bin\</OutputPath>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' "">
    <DebugType>pdbonly</DebugType>
    <Optimize>true</Optimize>
    <OutputPath>bin\</OutputPath>
    <DefineConstants>TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include=""Microsoft.CSharp"" />
    <Reference Include=""System.Web.DynamicData"" />
    <Reference Include=""System.Web.Entity"" />
    <Reference Include=""System.Web.ApplicationServices"" />
    <Reference Include=""System.ComponentModel.DataAnnotations"" />
    <Reference Include=""System"" />
    <Reference Include=""System.Data"" />
    <Reference Include=""System.Core"" />
    <Reference Include=""System.Data.DataSetExtensions"" />
    <Reference Include=""System.Web.Extensions"" />
    <Reference Include=""System.Xml.Linq"" />
    <Reference Include=""System.Drawing"" />
    <Reference Include=""System.Web"" />
    <Reference Include=""System.Xml"" />
    <Reference Include=""System.Configuration"" />
    <Reference Include=""System.Web.Services"" />
    <Reference Include=""System.EnterpriseServices"" />
  </ItemGroup>
  <ItemGroup>
    <None Include=""Web.Debug.config"">
      <DependentUpon>Web.config</DependentUpon>
    </None>
    <None Include=""Web.Release.config"">
      <DependentUpon>Web.config</DependentUpon>
    </None>
  </ItemGroup>
  <ItemGroup>
    <Content Include=""Web.config"" />
  </ItemGroup>
  <ItemGroup>
    <Compile Include=""Properties\AssemblyInfo.cs"" />
  </ItemGroup>
  <PropertyGroup>
    <VisualStudioVersion Condition=""'$(VisualStudioVersion)' == ''"">10.0</VisualStudioVersion>
    <VSToolsPath Condition=""'$(VSToolsPath)' == ''"">$(MSBuildExtensionsPath32)\Microsoft\VisualStudio\v$(VisualStudioVersion)</VSToolsPath>
  </PropertyGroup>
  <Import Project=""$(MSBuildBinPath)\Microsoft.CSharp.targets"" />
  <Import Project=""$(VSToolsPath)\WebApplications\Microsoft.WebApplication.targets"" Condition=""'$(VSToolsPath)' != ''"" />
  <Import Project=""$(MSBuildExtensionsPath32)\Microsoft\VisualStudio\v10.0\WebApplications\Microsoft.WebApplication.targets"" Condition=""false"" />
  <ProjectExtensions>
    <VisualStudio>
      <FlavorProperties GUID=""{{349c5851-65df-11da-9384-00065b846f21}}"">
        <WebProjectProperties>
          <UseIIS>True</UseIIS>
          <AutoAssignPort>True</AutoAssignPort>
          <DevelopmentServerPort>{2}</DevelopmentServerPort>
          <DevelopmentServerVPath>/</DevelopmentServerVPath>
          <IISUrl>http://localhost:{2}/</IISUrl>
          <NTLMAuthentication>False</NTLMAuthentication>
          <UseCustomServer>False</UseCustomServer>
          <CustomServerUrl>
          </CustomServerUrl>
          <SaveServerSettingsInUserFile>False</SaveServerSettingsInUserFile>
        </WebProjectProperties>
      </FlavorProperties>
    </VisualStudio>
  </ProjectExtensions>
  <!-- To modify your build process, add your task inside one of the targets below and uncomment it. 
       Other similar extension points exist, see Microsoft.Common.targets.
  <Target Name=""BeforeBuild"">
  </Target>
  <Target Name=""AfterBuild"">
  </Target>
  -->
</Project>";
            var random = new Random();
            var port = random.Next(20000, 60000);
            var projectFileName = string.Format(@"{0}.csproj", projectName);
            var projectFileContent = string.Format(str, projectGuid, projectName, port);
            result.Add(projectFileName, projectFileContent);
            #endregion
            #region Assembly

            var str2 = @"using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle(""{0}"")]
[assembly: AssemblyDescription("""")]
[assembly: AssemblyConfiguration("""")]
[assembly: AssemblyCompany(""{1}"")]
[assembly: AssemblyProduct(""{0}"")]
[assembly: AssemblyCopyright(""Copyright © {1} {2}"")]
[assembly: AssemblyTrademark("""")]
[assembly: AssemblyCulture("""")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid(""{3}"")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion(""1.0.*"")]
[assembly: AssemblyVersion(""1.0.0.0"")]
[assembly: AssemblyFileVersion(""1.0.0.0"")]";

            var assemblyFileName = string.Format(@"Properties\assemblyinfo.cs", projectName);
            var assemblyContent = string.Format(str2, projectName, assemblyCompany, assemblyYear, assembleGuid);
            result.Add(assemblyFileName, assemblyContent);

            #endregion
            #region Web.Config

            var str3 = @"<?xml version=""1.0""?>
<!--
  For more information on how to configure your ASP.NET application, please visit
  http://go.microsoft.com/fwlink/?LinkId=169433
  -->
<configuration>
  <configSections>
    <section name=""WebSecurity"" type=""Infoline.Web.SecurityWebConfig""/>
  </configSections>
  <WebSecurity LoginPage=""/security/login"" CookieName=""Mira"" TicketLife=""30"">
    <Securepath>/</Securepath>
    <Publicpath>/Security/Login</Publicpath>
    <Publicpath>/Security/SignUp</Publicpath>
    <Publicpath>/Security/ForgotPassword</Publicpath>
  </WebSecurity>
  <system.serviceModel>
    <serviceHostingEnvironment aspNetCompatibilityEnabled=""true""/>
  </system.serviceModel>
  <!--
    For a description of web.config changes see http://go.microsoft.com/fwlink/?LinkId=235367.

    The following attributes can be set on the <httpRuntime> tag.
      <system.Web>
        <httpRuntime targetFramework=""4.5"" />
      </system.Web>
  -->
  <system.web>
    <httpHandlers>
      <add path=""*.js"" verb=""*"" type=""Infoline.Web.StaticFileHandler,Infoline.Framework""/>
      <add path=""*.css"" verb=""*"" type=""Infoline.Web.StaticFileHandler,Infoline.Framework""/>
    </httpHandlers>
    <customErrors mode=""RemoteOnly"" defaultRedirect=""err.htm"" redirectMode=""ResponseRewrite""/>
    <compilation debug=""true"" targetFramework=""4.5""/>
    <httpModules>
      <add name=""Security"" type=""Infoline.Web.SecurityAuthenticationModule,Infoline.Framework""/>
    </httpModules>
  </system.web>
  <system.webServer>
    <httpProtocol>
      <customHeaders>
        <add name=""Access-Control-Allow-Origin"" value=""*""/>
      </customHeaders>
    </httpProtocol>
    <validation validateIntegratedModeConfiguration=""false""/>
    <modules runAllManagedModulesForAllRequests=""true"">
      <add name=""Security"" type=""Infoline.Web.SecurityAuthenticationModule,Infoline.Framework""/>
    </modules>
    <handlers>
      <add name=""staticjs"" path=""*.js"" verb=""*"" type=""Infoline.Web.StaticFileHandler,Infoline.Framework""/>
      <add name=""staticcss"" path=""*.css"" verb=""*"" type=""Infoline.Web.StaticFileHandler,Infoline.Framework""/>
    </handlers>
  </system.webServer>
</configuration>";

            var webConfigContent = str3;
            result.Add("Web.config", webConfigContent);

            #endregion
            #region Web.Debug.Config
            var webDebugConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>

<!-- For more information on using web.config transformation visit http://go.microsoft.com/fwlink/?LinkId=125889 -->

<configuration xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"">
  <!--
    In the example below, the ""SetAttributes"" transform will change the value of 
    ""connectionString"" to use ""ReleaseSQLServer"" only when the ""Match"" locator 
    finds an attribute ""name"" that has a value of ""MyDB"".
    
    <connectionStrings>
      <add name=""MyDB"" 
        connectionString=""Data Source=ReleaseSQLServer;Initial Catalog=MyReleaseDB;Integrated Security=True"" 
        xdt:Transform=""SetAttributes"" xdt:Locator=""Match(name)""/>
    </connectionStrings>
  -->
  <system.web>
    <!--
      In the example below, the ""Replace"" transform will replace the entire 
      <customErrors> section of your web.config file.
      Note that because there is only one customErrors section under the 
      <system.web> node, there is no need to use the ""xdt:Locator"" attribute.
      
      <customErrors defaultRedirect=""GenericError.htm""
        mode=""RemoteOnly"" xdt:Transform=""Replace"">
        <error statusCode=""500"" redirect=""InternalError.htm""/>
      </customErrors>
    -->
  </system.web>
</configuration>";

            result.Add("Web.Debug.config", webDebugConfig);

            #endregion
            #region Web.Release.Config
            var webReleaseConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>

<!-- For more information on using web.config transformation visit http://go.microsoft.com/fwlink/?LinkId=125889 -->

<configuration xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"">
  <!--
    In the example below, the ""SetAttributes"" transform will change the value of 
    ""connectionString"" to use ""ReleaseSQLServer"" only when the ""Match"" locator 
    finds an attribute ""name"" that has a value of ""MyDB"".
    
    <connectionStrings>
      <add name=""MyDB"" 
        connectionString=""Data Source=ReleaseSQLServer;Initial Catalog=MyReleaseDB;Integrated Security=True"" 
        xdt:Transform=""SetAttributes"" xdt:Locator=""Match(name)""/>
    </connectionStrings>
  -->
  <system.web>
    <compilation xdt:Transform=""RemoveAttributes(debug)"" />
    <!--
      In the example below, the ""Replace"" transform will replace the entire 
      <customErrors> section of your web.config file.
      Note that because there is only one customErrors section under the 
      <system.web> node, there is no need to use the ""xdt:Locator"" attribute.
      
      <customErrors defaultRedirect=""GenericError.htm""
        mode=""RemoteOnly"" xdt:Transform=""Replace"">
        <error statusCode=""500"" redirect=""InternalError.htm""/>
      </customErrors>
    -->
  </system.web>
</configuration>";


            result.Add("Web.Release.config", webReleaseConfig);
            #endregion
            #region packages.Config
//            var packagesConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
//<packages>
//  <package id=""Microsoft.CodeDom.Providers.DotNetCompilerPlatform"" version=""1.0.0"" targetFramework=""net452"" />
//  <package id=""Microsoft.Net.Compilers"" version=""1.0.0"" targetFramework=""net452"" developmentDependency=""true"" />
//</packages>";


//            result.Add("packages.config", packagesConfig);
            #endregion

            return result;
        }


        public Dictionary<string, string> GetTemplates()
        {
            var result = new Dictionary<string, string>();

            #region ConnecionString
            var connectionString = @"<#@ template debug=""false"" hostspecific=""false"" language=""C#"" #>
<#@ assembly name=""System.Core"" #>
<#@ assembly name=""System.Data"" #>
<#@ import namespace=""System.Linq"" #>
<#@ import namespace=""System.Text"" #>
<#@ import namespace=""System.Data.SqlClient"" #>
<#@ import namespace=""System.Collections.Generic"" #>
<#@ output extension="".txt"" #>
<#+
	public class ConnectionStringBase
	{
		public string Server { get; private set; }
		public string DBName { get; private set; }
		public string User   { get; private set; }
		public string Pass   { get; private set; }
		
		public ConnectionStringBase()
		{
			Server = null;
			DBName = null;
			User   = null;
			Pass   = null;
		}

		public string GetConnectionStringBase()
		{
            if(Server == null || DBName == null || User == null || Pass == null)
                return null;
			
			var builder = new SqlConnectionStringBuilder();
			builder.DataSource = Server;
            builder.InitialCatalog = DBName;
            builder.UserID = User;
            builder.Password = Pass;
            return builder.ToString();
		}
	}
#>";
            #endregion
            #region BasicTabels
            var basicTables = @"<#@ template debug=""True"" hostspecific=""True"" language=""C#"" #>
<#@ assembly name=""System.Core"" #>
<#@ assembly name=""EnvDTE""#>
<#@ import namespace=""System.Linq"" #>
<#@ import namespace=""System.Text"" #>
<#@ import namespace=""System.Collections.Generic"" #>
<#@ import namespace=""System.Reflection"" #>
<#@ import namespace=""System.IO""#>
<#@ import namespace=""Microsoft.VisualStudio.TextTemplating""#>
<#@ include file=""1_ConnectionString.tt""#>

<#@ output extension="".txt"" #>
<#

var cnn = new ConnectionStringBase();
var connectionString = cnn.GetConnectionStringBase();


var check_table = ""SELECT count(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{0}'"";
var default_command1 = ""ALTER TABLE [dbo].[{0}] ADD  CONSTRAINT [DF_{0}_id]  DEFAULT (newid()) FOR [id]"";

var sh_pages = @""
CREATE TABLE [dbo].[SH_Pages](
	[id] [uniqueidentifier] NOT NULL,
	[created] [datetime] NULL,
	[changed] [datetime] NULL,
	[createdby] [uniqueidentifier] NULL,
	[changedby] [uniqueidentifier] NULL,
	[Bread] [nvarchar](300) NULL,
	[Description] [nvarchar](300) NULL,
	[Action] [nvarchar](300) NULL,
	[Status] [bit] NULL,
	[Area] [nvarchar](300) NULL,
	[Controller] [nvarchar](300) NULL,
	[Method] [nvarchar](300) NULL,
	[BreadCrumbStatu] [bit] NULL,
	[ReturnParametre] [nvarchar](300) NULL,
	[MethodParametre] [nvarchar](300) NULL,
 CONSTRAINT [PK_SH_Pages_Id] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]"";

var sh_pages1 = ""ALTER TABLE [dbo].[SH_Pages] ADD  DEFAULT ((0)) FOR [BreadCrumbStatu]"";
var sh_pages2 = ""ALTER TABLE [dbo].[SH_Pages] ADD  CONSTRAINT [DF_SH_Pages_created]  DEFAULT (getdate()) FOR [created]"";


var sh_role = @""
CREATE TABLE [dbo].[SH_Role](
	[id] [uniqueidentifier] NOT NULL,
	[created] [datetime] NULL,
	[changed] [datetime] NULL,
	[createdby] [uniqueidentifier] NULL,
	[changedby] [uniqueidentifier] NULL,
	[idcode] [nvarchar](50) NULL,
	[rolname] [nvarchar](100) NULL,
 CONSTRAINT [PK_SH_Role] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]"";
var sh_role1 = ""ALTER TABLE [dbo].[SH_Role] ADD  CONSTRAINT [DF_SH_Role_created]  DEFAULT (getdate()) FOR [created]"";


var sh_ticket = @""
CREATE TABLE [dbo].[SH_Ticket](
	[id] [uniqueidentifier] NOT NULL,
	[userid] [uniqueidentifier] NOT NULL,
	[createtime] [datetime] NOT NULL,
	[endtime] [datetime] NOT NULL,
	[IP] [nvarchar](50) NULL,
 CONSTRAINT [PK_SH_Ticket] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]"";


var sh_user = @""
CREATE TABLE [dbo].[SH_User](
	[id] [uniqueidentifier] NOT NULL,
	[created] [datetime] NULL,
	[changed] [datetime] NULL,
	[createdby] [uniqueidentifier] NULL,
	[changedby] [uniqueidentifier] NULL,
	[idcode] [nvarchar](50) NULL,
	[tckimlikno] [nvarchar](11) NULL,
	[type] [int] NULL,
	[loginname] [nvarchar](50) NULL,
	[firstname] [nvarchar](100) NULL,
	[lastname] [nvarchar](100) NULL,
	[birthday] [datetime] NULL,
	[password] [nvarchar](200) NULL,
	[title] [nvarchar](150) NULL,
	[email] [nvarchar](100) NULL,
	[phone] [nvarchar](50) NULL,
	[cellphone] [nvarchar](50) NULL,
	[address] [nvarchar](max) NULL,
	[status] [bit] NOT NULL,
	[city] [uniqueidentifier] NULL,
	[town] [uniqueidentifier] NULL,
 CONSTRAINT [PK_SH_User] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]"";

var sh_userRole =	@""
CREATE TABLE [dbo].[SH_UserRole](
	[id] [uniqueidentifier] NOT NULL,
	[created] [datetime] NULL,
	[changed] [datetime] NULL,
	[createdby] [uniqueidentifier] NULL,
	[changedby] [uniqueidentifier] NULL,
	[userid] [uniqueidentifier] NULL,
	[roleid] [uniqueidentifier] NULL,
	[status] [bit] NULL,
 CONSTRAINT [PK_SH_UserRole] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]"";

var sh_authorityActionRole = @""
CREATE TABLE [dbo].[SH_AuthorityActionRole](
	[id] [uniqueidentifier] NOT NULL,
	[created] [datetime] NULL,
	[changed] [datetime] NULL,
	[createdby] [uniqueidentifier] NULL,
	[changedby] [uniqueidentifier] NULL,
	[actionid] [uniqueidentifier] NULL,
	[roleid] [uniqueidentifier] NULL,
	[Status] [bit] NULL,
	[Action] [nvarchar](100) NULL,
 CONSTRAINT [PK_AuthorityRole] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]"";

var tables = new Dictionary<string, string[]>();
tables.Add(""SH_Pages"",                  new string[] { sh_pages, sh_pages1, sh_pages2 });
tables.Add(""SH_Role"",                   new string[] { sh_role,  sh_role1             });
tables.Add(""SH_Ticket"",                 new string[] { sh_ticket                      });
tables.Add(""SH_User"",                   new string[] { sh_user                        });
tables.Add(""SH_UserRole"",               new string[] { sh_userRole                    });
tables.Add(""SH_AuthorityActionRole"",    new string[] { sh_authorityActionRole         });


if(connectionString != null)
{
    StringBuilder template = GenerationEnvironment;
    ITextTemplatingEngineHost host = Host;
    
    var solution = ((EnvDTE.DTE)(((IServiceProvider)host).GetService(typeof(EnvDTE.DTE)))).Solution;
    var project = solution.Projects.Cast<EnvDTE.Project>().Where(a => a.Name == ""Infoline.Framework"").FirstOrDefault();
    
    var frameworkDir = Path.GetDirectoryName(project.FullName);
    var frameworkDll = Path.Combine(frameworkDir, ""bin"", ""Debug"", ""Infoline.Framework.dll"");
    var DLL = Assembly.LoadFile(frameworkDll);
    var type = DLL.GetType(""Infoline.Framework.Database.Database""); 
    
    using(var db = (IDisposable)Activator.CreateInstance(type, new object[] { connectionString }))
    {
    	foreach(var table in tables)
    	{
    		MethodInfo method = type.GetMethod(""ExecuteScaler"");
            MethodInfo generic = method.MakeGenericMethod(typeof(int));
    		var count = (int)generic.Invoke(db, new object[] { string.Format(check_table, table.Key), new string[0] });
    		if(count == 0)
    		{
    			for(int i = 0; i < table.Value.Length; i++)
    			    type.InvokeMember(""ExecuteNonQuery"", BindingFlags.InvokeMethod, null, db, new object[] { table.Value[i] });
    			type.InvokeMember(""ExecuteNonQuery"", BindingFlags.InvokeMethod, null, db, new object[] { string.Format(default_command1, table.Key) });
    		}
    	}
    }
}

#>";
            #endregion
            #region DatabaseObjects
            var databaseObjects = @"<#@ template language=""C#"" hostspecific=""True"" debug=""True"" #>
<#@ include file=""General_Manager.tt""#>
<#@ include file=""1_ConnectionString.tt""#>
<#@ import namespace=""System.IO"" #>
<#
	DatabaseObjects.Run(Host, GenerationEnvironment);
#>
<#+
	public class DatabaseObjects
	{
		public static void Run(ITextTemplatingEngineHost host, StringBuilder template)
		{
			template.AppendLine(""DatabaseObjects"");
			template.AppendLine(string.Format(""Son Çalıştırma Tarihi: {0}"", DateTime.Now));

			var cnn = new ConnectionStringBase();
			var manager = Manager.Create(host, template, false); //3.Parametre False alırsa Var Olan Dosyaların Üzerine Yazar.
			
            var connectionString = cnn.GetConnectionStringBase();
            if (connectionString == null)
                return;

			var frameworkDir = Path.GetDirectoryName(manager.GetProjectFilePath(""Infoline.Framework""));
			var frameworkDll = Path.Combine(frameworkDir, ""bin"", ""Debug"", ""Infoline.Framework.dll"");
			var DLL = Assembly.LoadFile(frameworkDll);

			var type = DLL.GetType(""Infoline.Framework.CodeGeneration.CodeGenerators.SQLClassGenerator""); 
			var gen = Activator.CreateInstance(type, null); 
			var code = (Dictionary<string, string>)type.GetMethod(""GenerateMultiFile"").Invoke(gen, new object[] { cnn.GetConnectionStringBase() } );
			

			manager.StartHeader();
			template.AppendLine(""using System;"");
			template.AppendLine(""using Microsoft.SqlServer.Types;"");
			template.AppendLine("""");
			template.AppendLine(""namespace "" + manager.SolutionName +  "".BusinessData"");
			template.AppendLine(""{"");
			manager.EndBlock();
			
			manager.StartFooter();
			template.AppendLine(""}"");
			manager.EndBlock();
			 
			foreach(var kvp in code)
			{	
				string project = manager.SolutionName + "".BusinessData"";
				string directory = @""DatabaseObjects"";
				string fileName = string.Format(""{0}.cs"", kvp.Key);
				manager.StartNewFile(project, directory, fileName);
				template.AppendLine(kvp.Value); 
				manager.EndBlock();
			}
			manager.Process(true);
		}
	}
#>
";
            #endregion
            #region DatabaseFunctions
            var databaseFunctions = @"<#@ template language=""C#"" hostspecific=""True"" debug=""True"" #>
<#@ include file=""General_Manager.tt""#>
<#@ include file=""1_ConnectionString.tt""#>
<#@ import namespace=""System.IO"" #>
<#
	DatabaseFunctions.Run(Host, GenerationEnvironment);
#>
<#+
	public class DatabaseFunctions
	{
		public static void Run(ITextTemplatingEngineHost host, StringBuilder template)
		{
			template.AppendLine(""SQLDatabaseFunctions"");
			template.AppendLine(string.Format(""Son Çalıştırma Tarihi: {0}"", DateTime.Now));

			var cnn = new ConnectionStringBase();
			var manager = Manager.Create(host, template, false); //3.Parametre False alırsa Var Olan Dosyaların Üzerine Yazar.
			
            var connectionString = cnn.GetConnectionStringBase();
            if (connectionString == null)
                return;

			var frameworkDir = Path.GetDirectoryName(manager.GetProjectFilePath(""Infoline.Framework""));
			var frameworkDll = Path.Combine(frameworkDir, ""bin"", ""Debug"", ""Infoline.Framework.dll"");
			var DLL = Assembly.LoadFile(frameworkDll);

			var type = DLL.GetType(""Infoline.Framework.CodeGeneration.CodeGenerators.SQLDatabaseFunctions""); 
			var gen = Activator.CreateInstance(type, null); 
			var code = (Dictionary<string, string>)type.GetMethod(""GenerateMultiFile"").Invoke(gen, new object[] { cnn.GetConnectionStringBase() } );


			manager.StartHeader();
			
			template.AppendLine(""using System;"");
			template.AppendLine(""using System.Collections.Generic;"");
			template.AppendLine(""using System.Linq;"");
			template.AppendLine(""using System.Text;"");
			template.AppendLine(""using System.Threading.Tasks;"");
			template.AppendLine(""using Infoline.Framework.Database;"");
			template.AppendLine(""using "" + manager.SolutionName +  "".BusinessData;"");
			
			template.AppendLine("""");
			template.AppendLine(""namespace "" + manager.SolutionName +  "".BusinessAccess"");
			template.AppendLine(""{"");
			manager.EndBlock();
			
			manager.StartFooter();
			template.AppendLine(""}"");
			manager.EndBlock();
			
			foreach(var kvp in code)
			{	
				string project = manager.SolutionName + "".BusinessAccess"";
				string directory = @""DatabaseFunctions\AutoGenerate"";
				string fileName = string.Format(""DB{0}.cs"", kvp.Key);
				manager.StartNewFile(project, directory, fileName);
				template.AppendLine(kvp.Value); 
				manager.EndBlock();
			}
			manager.Process(true);
		}
	}
#>";
            #endregion
            #region SecurityObjects
            var securityObjects = @"<#@ template language=""C#"" hostspecific=""True"" debug=""True"" #>
<#@ include file=""General_Manager.tt""#>
<#@ include file=""1_ConnectionString.tt""#>
<#@ import namespace=""System.IO"" #>
<#
	CreateSecurityClass.Run(Host, GenerationEnvironment);
#>
<#+
	public class CreateSecurityClass
	{
		public static void Run(ITextTemplatingEngineHost host, StringBuilder template)
		{
			template.AppendLine(""CreateSecurityClass"");
			template.AppendLine(string.Format(""Son Çalıştırma Tarihi: {0}"", DateTime.Now));

			var cnn = new ConnectionStringBase();
			var manager = Manager.Create(host, template, false); 
			var solutionName = manager.SolutionName;

            var connectionString = cnn.GetConnectionStringBase();
            if (connectionString == null)
                return;

			var frameworkDir = Path.GetDirectoryName(manager.GetProjectFilePath(""Infoline.Framework""));
			var frameworkDll = Path.Combine(frameworkDir, ""bin"", ""Debug"", ""Infoline.Framework.dll"");
			var DLL = Assembly.LoadFile(frameworkDll);

			var type = DLL.GetType(""Infoline.Framework.CodeGeneration.CodeGenerators.SecurityClassGenerator""); 
			var gen = Activator.CreateInstance(type, null); 
			var code = (Dictionary<string, string>)type.GetMethod(""GenerateMultiFile"").Invoke(gen, new object[] { solutionName, cnn.DBName } );

			foreach(var kvp in code)
			{	
				string project = manager.SolutionName + "".BusinessAccess"";
				string directory = @""Security"";
				string fileName = string.Format(""{0}.cs"", kvp.Key);
				manager.StartNewFile(project, directory, fileName);
				template.AppendLine(kvp.Value); 
				manager.EndBlock();
			}
			manager.Process(true);
		}
	}
#>
";
            #endregion
            #region Handlers
            var handlers = @"<#@ template language=""C#"" hostspecific=""True"" debug=""True"" #>
<#@ include file=""General_Manager.tt""#>
<#@ include file=""1_ConnectionString.tt""#>
<#@ import namespace=""System.IO"" #>
<#
	DatabaseServices.Run(Host, GenerationEnvironment);
#>
<#+
	public class DatabaseServices
	{
		public static void Run(ITextTemplatingEngineHost host, StringBuilder template)
		{
			
			template.AppendLine(""SQLServiceGenerator"");
			template.AppendLine(string.Format(""Son Çalıştırma Tarihi: {0}"", DateTime.Now));

			
			var cnn = new ConnectionStringBase();
			var manager = Manager.Create(host, template, false); 
			
            var connectionString = cnn.GetConnectionStringBase();
            if (connectionString == null)
                return;

			var frameworkDir = Path.GetDirectoryName(manager.GetProjectFilePath(""Infoline.Framework""));
			var frameworkDll = Path.Combine(frameworkDir, ""bin"", ""Debug"", ""Infoline.Framework.dll"");
			var DLL = Assembly.LoadFile(frameworkDll);

			var type = DLL.GetType(""Infoline.Framework.CodeGeneration.CodeGenerators.SQLServiceGenerator""); 
			var gen = Activator.CreateInstance(type, null); 
			var code = (Dictionary<string, string>)type.GetMethod(""GenerateMultiFile"").Invoke(gen, new object[] { cnn.GetConnectionStringBase() } );
			

			manager.StartHeader();
			template.AppendLine(""using System;"");
			template.AppendLine(string.Format(""using {0}.BusinessData;"", manager.SolutionName));
			template.AppendLine(""using Infoline.Web.SmartHandlers;"");
			template.AppendLine(""using System.ComponentModel.Composition;"");
			template.AppendLine(""using System.Web;"");
			template.AppendLine(""using Infoline.Framework.Database;"");
			template.AppendLine(string.Format(""using {0}.BusinessAccess;"", manager.SolutionName));
			template.AppendLine("""");
			template.AppendLine(""namespace "" + manager.SolutionName + "".WebService"");
			template.AppendLine(""{"");
			manager.EndBlock();
			
			manager.StartFooter();
			template.AppendLine(""}"");
			manager.EndBlock();
			
			foreach(var kvp in code)
			{	
				string project = manager.SolutionName + "".WebService"";
				string directory = @""Handlers"";
				string fileName = string.Format(""{0}.cs"", kvp.Key);
				manager.StartNewFile(project, directory, fileName);
				template.AppendLine(kvp.Value); 
				manager.EndBlock();
			}
			manager.Process(true);
		}
	}
#>
";
            #endregion
            #region MvcController
            var mvcFiles = @"<#@ template language=""C#"" hostspecific=""True"" debug=""True"" #>
<#@ include file=""General_Manager.tt""#>
<#@ include file=""1_ConnectionString.tt""#>
<#@ import namespace=""System.IO"" #>
<#
	DatabaseMVCGenerator.Run(Host, GenerationEnvironment);
#>
<#+
	public class DatabaseMVCGenerator
	{
		public static void Run(ITextTemplatingEngineHost host, StringBuilder template)
		{
		
			var cnn = new ConnectionStringBase();
			var manager = Manager.Create(host, template, false); 

			var frameworkDir = Path.GetDirectoryName(manager.GetProjectFilePath(""Infoline.Framework""));
			var frameworkDll = Path.Combine(frameworkDir, ""bin"", ""Debug"", ""Infoline.Framework.dll"");
			var DLL = Assembly.LoadFile(frameworkDll);

			var pageSetupType = DLL.GetType(""Infoline.Framework.CodeGeneration.CodeGenerators.MVCPageSetup"");
			var generatorType = DLL.GetType(""Infoline.Framework.CodeGeneration.CodeGenerators.MVCGenerator""); 
			var genController = Activator.CreateInstance(generatorType, null); 

			
			var projectName = ""Infoline.Sevice.Test.WebProject"";
			var pages = new object[]
            {
				Activator.CreateInstance(pageSetupType, new object[] { ""Station2"", ""VWST_Istasyon"", ""ST_Istasyon"", true, true, true, true, true }),
				Activator.CreateInstance(pageSetupType, new object[] { ""Station2"", ""VWBK_Bitki"", ""BK_Bitki"", true, true, true, true, true }),
            };


			
			template.AppendLine(""MVCGenerator"");
			template.AppendLine(string.Format(""Son Çalıştırma Tarihi: {0}"", DateTime.Now));

						
			var solutionName = manager.SolutionName;
            var connectionString = cnn.GetConnectionStringBase();
            if (connectionString == null)
                return;

			template.AppendLine(genController == null ? ""null"" : genController.ToString());

			var controllers = (Dictionary<string, string>)generatorType.GetMethod(""GenerateControllers"").Invoke(genController, new object[] { connectionString, solutionName, projectName, pages } );
			var views = (Dictionary<string, string>)generatorType.GetMethod(""GenerateViews"").Invoke(genController, new object[] { connectionString, solutionName, pages } );
		
			
			WriteControllers(manager, template, projectName, controllers);
			WriteViews(host, template, projectName, views);

		}

		private static void WriteControllers(Manager manager, StringBuilder template, string projectName, Dictionary<string, string> controllers)
		{
			manager.StartHeader();
			manager.EndBlock();
			
			manager.StartFooter();
			manager.EndBlock();
			 
			foreach(var controller in controllers)
			{	
				string project = projectName;
				string directory = System.IO.Path.GetDirectoryName(controller.Key);
				string fileName = System.IO.Path.GetFileName(controller.Key);
				template.AppendLine(directory);
				template.AppendLine(fileName);
				manager.StartNewFile(project, directory, fileName);
				template.AppendLine(controller.Value); 
				manager.EndBlock();
			}

			manager.Process(true);
		}

		private static void WriteViews(ITextTemplatingEngineHost host, StringBuilder template, string projectName, Dictionary<string, string> views)
		{
			var manager = Manager.Create(host, template, false); 

			manager.StartHeader();
			manager.EndBlock();
			
			manager.StartFooter();
			manager.EndBlock();
			
			foreach(var view in views)
			{	
				string project = projectName;
				string directory = System.IO.Path.GetDirectoryName(view.Key);
				string fileName = System.IO.Path.GetFileName(view.Key);
				manager.StartNewFile(project, directory, fileName);
				template.AppendLine(view.Value); 
				manager.EndBlock();
			}

			manager.Process(true);
		}

	}
#>";
            #endregion
            #region MvcView // Kapalı
//            var mvcViews = @"<#@ template language=""C#"" hostspecific=""True"" debug=""True"" #>
//<#@ include file=""General_Manager.tt""#>
//<#@ include file=""1_ConnectionString.tt""#>
//<#@ import namespace=""System.IO"" #>
//<#
//	DatabaseMVCController.Run(Host, GenerationEnvironment);
//#>
//<#+
//	public class DatabaseMVCController
//	{
//		public static void Run(ITextTemplatingEngineHost host, StringBuilder template)
//		{
			
//			template.AppendLine(""MVCViewGenerator"");
//			template.AppendLine(string.Format(""Son Çalıştırma Tarihi: {0}"", DateTime.Now));

//			var cnn = new ConnectionStringBase();
//			var manager  = Manager.Create(host, template, false); 
//			var bussinessNameSpace = string.Format(""{0}.Business.DatabaseObjects"", manager.SolutionName);
			
//            var connectionString = cnn.GetConnectionStringBase();
//            if (connectionString == null)
//                return;

//			var frameworkDir = Path.GetDirectoryName(manager.GetProjectFilePath(""Infoline.Framework""));
//			var frameworkDll = Path.Combine(frameworkDir, ""bin"", ""Debug"", ""Infoline.Framework.dll"");
//			var DLL = Assembly.LoadFile(frameworkDll);

//			var tabels = new Dictionary<string, string>();
//			tabels.Add(""Management_Hardware_Processor"", ""Processor"");
//			tabels.Add(""Management_Hardware_DiskDrive"", ""Disk"");

//			var type = DLL.GetType(""Infoline.Framework.CodeGeneration.CodeGenerators.MVCViewGenerator""); 
//			var genView = Activator.CreateInstance(type, null); 
//			var views = (Dictionary<string, string>)type.GetMethod(""GenerateMultiFile"").Invoke(genView, new object[] { cnn.GetConnectionStringBase(), bussinessNameSpace,tabels } );

			
//			manager.StartHeader();
//			manager.EndBlock();
			
//			manager.StartFooter();
//			manager.EndBlock();
			
			

			
			
			
//			foreach(var view in views)
//			{	
//				var dir = view.Key.Split('\\')[0];
//				var file = view.Key.Split('\\')[1];

//				string project = ""Infoline.FrameworkTest.Web"";
//				string directory = string.Format(""Views\\{0}"", dir);
//				string fileName = string.Format(""{0}"", file);
//				manager.StartNewFile(project, directory, fileName);
//				template.AppendLine(view.Value); 
//				manager.EndBlock();
//			}

//			manager.Process(true);
//		}
//	}
//#>";
            #endregion
            #region GeneralManager
            var generalManager = @"<#@ template language=""C#"" hostspecific=""True"" debug=""True"" #>
<#@ assembly name=""System.Core""#>
<#@ assembly name=""System.Data.Linq""#>
<#@ assembly name=""EnvDTE""#>
<#@ assembly name=""VSLangProj""#>
<#@ assembly name=""System.Xml""#>
<#@ assembly name=""System.Xml.Linq""#>
<#@ import namespace=""System""#>
<#@ import namespace=""System.CodeDom""#>
<#@ import namespace=""System.CodeDom.Compiler""#>
<#@ import namespace=""System.Collections.Generic""#>
<#@ import namespace=""System.Data.Linq""#>
<#@ import namespace=""System.Data.Linq.Mapping""#>
<#@ import namespace=""System.IO""#>
<#@ import namespace=""System.Linq""#>
<#@ import namespace=""System.Reflection""#>
<#@ import namespace=""System.Text""#>
<#@ import namespace=""System.Xml.Linq""#>
<#@ import namespace=""Microsoft.VisualStudio.TextTemplating""#>
<#@ output extension=""txt"" #>
<#+
class Manager
{
	public class Block { 
		public String Project;
		public String Directory;
		public String Name; 
		public int Start, Length; 
	}
	private class FolderItem {
		public String Path;
		public EnvDTE.ProjectItems Items;
	}
	private class CreatedFile {
		public String FileName;
		public FolderItem Folder;
	}

	private Block _currentBlock;
	private List<Block> _files = new List<Block>();
	private Block _footer = new Block();
	private Block _header = new Block();
	private ITextTemplatingEngineHost _host;
	private StringBuilder _template;
	private List<CreatedFile> _generatedFileNames = new List<CreatedFile>();
	

	private EnvDTE.ProjectItem _templateProjectItem;
	private EnvDTE.DTE _dte;
	private Action<String> _checkOutAction;
	private Action<IEnumerable<CreatedFile>> _projectSyncAction;
	private bool _dontTouchExistingFile = false;

	public static Manager Create(ITextTemplatingEngineHost host, StringBuilder template, bool dontTouchExistingFile = true){
		return new Manager(host, template, dontTouchExistingFile);
	}

	public String DefaultProjectNamespace{
		get 
		{
			return _templateProjectItem.ContainingProject.Properties.Item(""DefaultNamespace"").Value.ToString();
		}
	}
	public String SolutionName {
		get {
			return Path.GetFileNameWithoutExtension(_dte.Solution.FullName);
		}
	}
	public String SolutionDirectory {
		get {
			return Path.GetDirectoryName(_dte.Solution.FullName);
		}
	}
	public Block CurrentBlock{
		get { return _currentBlock; }
		set {
			if (CurrentBlock != null)
				EndBlock();
			if (value != null)
				value.Start = _template.Length;
			_currentBlock = value;
		}
	}

	private Manager(ITextTemplatingEngineHost host, StringBuilder template, bool dontTouchExistingFile) {

		_host = host;
		_template = template;
		_dontTouchExistingFile = dontTouchExistingFile;

		var hostServiceProvider = (IServiceProvider)host;
		if (hostServiceProvider == null)
			throw new ArgumentNullException(""Could not obtain IServiceProvider"");
		_dte = (EnvDTE.DTE)hostServiceProvider.GetService(typeof(EnvDTE.DTE));
		if (_dte == null)
			throw new ArgumentNullException(""Could not obtain DTE from host"");
		_templateProjectItem = _dte.Solution.FindProjectItem(host.TemplateFile);
		_checkOutAction = (String fileName) => _dte.SourceControl.CheckOutItem(fileName);
		_projectSyncAction = (IEnumerable<CreatedFile> keepFileNames) => ProjectSync(keepFileNames);
	
	}

	public string GetProjectFilePath(string projectPath)
	{
		EnvDTE.Solution sol = _dte.Solution;
		var project = GetProjectByName(sol, projectPath);
		if(project != null)
		{
			return project.FullName;
		}
		return null;
	}

	public void StartNewFile(String project, String directory, String name){
		if (name == null)
			throw new ArgumentNullException(""name"");
		CurrentBlock = new Block { Name = name, Directory = directory, Project = project };
	}
	public void StartFooter(){
		CurrentBlock = _footer;
	}
	public void StartHeader(){
		CurrentBlock = _header;
	}
	public void EndBlock(){
		if (CurrentBlock == null)
			return;
		CurrentBlock.Length = _template.Length - CurrentBlock.Start;
		if (CurrentBlock != _header && CurrentBlock != _footer)
			_files.Add(CurrentBlock);
		_currentBlock = null;
	}
	public void Process(bool split){
		if (_templateProjectItem.ProjectItems == null)
			return;

		if (split)
		{
			EndBlock();
			String headerText = _template.ToString(_header.Start, _header.Length);
			String footerText = _template.ToString(_footer.Start, _footer.Length);
			_files.Reverse();
			foreach (Block block in _files)
			{
				FolderItem outputFolder = GetFolderItem(block.Project, block.Directory, block.Name);
				var outputFile = Path.Combine(outputFolder.Path, block.Name);
				String content = headerText + _template.ToString(block.Start, block.Length) + footerText;
				
				var createdFile = new CreatedFile();
				createdFile.FileName = outputFile;
				createdFile.Folder = outputFolder;
				_generatedFileNames.Add(createdFile);

				CreateFile(outputFile, content);
				_template.Remove(block.Start, block.Length);
			}
			_template.Remove(_footer.Start, _footer.Length);
			_template.Remove(_header.Start, _header.Length);
		}
		_projectSyncAction.EndInvoke(_projectSyncAction.BeginInvoke(_generatedFileNames, null, null));
	}
	public void CreateProject(string projectName, string temp)
	{
		EnvDTE.Solution sol = _dte.Solution;
		var solutionPath = Path.GetDirectoryName(sol.FullName);
		var projectPath = Path.Combine(solutionPath, projectName);
		_template.AppendLine(solutionPath);
		_template.AppendLine(projectPath);
		if(GetProjectByName(sol, projectName) == null)
		{
			Directory.CreateDirectory(projectPath);
			EnvDTE.Project pro = sol.AddFromTemplate(temp, projectPath, projectName, false);
			pro.Save();
		}
	}

	public void AddProject(string path)
	{
		try
		{
		
			EnvDTE.Solution sol = _dte.Solution;
			CheckoutFileIfRequired(sol.FullName);
			var projectPath = Path.GetFileNameWithoutExtension(path);
			if(GetProjectByName(sol, projectPath) == null)
			{
				//_template.AppendLine(sol.FullName);
				//_template.AppendLine(path);
				sol.AddFromFile(path);
			}
		}
		catch(Exception ex)
		{
			_template.AppendLine(ex.Message);
		}

	}

	public void AddFileToProject(string projectPath, string filePath)
	{
		_template.AppendLine(""1"");
		EnvDTE.Solution sol = _dte.Solution;
		_template.AppendLine(""2"");
		var projPath = Path.GetFileNameWithoutExtension(projectPath);
		_template.AppendLine(""3"");
		var project = GetProjectByName(sol, projectPath);
		_template.AppendLine(""4"");
		if(project != null)
		{
			_template.AppendLine(""5"");
			_template.AppendLine(project.FullName);
			_template.AppendLine(filePath);
			var projitems = project.ProjectItems;
			projitems.AddFromFile(filePath);
		}
		_template.AppendLine(""6"");
	}
	

	public bool ProjectExists(string path)
	{
		EnvDTE.Solution sol = _dte.Solution;

		var projName = Path.GetFileNameWithoutExtension(path);
		return GetProjectByName(sol, projName) != null;
	}

	public void AddReferance(string projectName, string referanceName)
	{
		EnvDTE.Solution sol = _dte.Solution;
		
		var project = GetProjectByName(sol, projectName);
		VSLangProj.VSProject vsProject = (VSLangProj.VSProject)project.Object;
		
		vsProject.References.Add(referanceName);
		project.Save();
	}

	public void AddPojectReferance(string projectName, string referanceProjectName)
	{
		EnvDTE.Solution sol = _dte.Solution;
		
		var refProject = GetProjectByName(sol, referanceProjectName);
		var project = GetProjectByName(sol, projectName);

		VSLangProj.VSProject vsProject = (VSLangProj.VSProject)project.Object;
		
		vsProject.References.AddProject(refProject);
		project.Save();
	}

	private FolderItem GetFolderItem(String project, String directory, String fileName){

		EnvDTE.Project proj = GetProjectByName(_dte.Solution, project);
		if (proj == null)
		{
			_template.Append(""Hata: Proje bulunamadı"");
			return null;
		}
		String projectPath = Path.GetDirectoryName(proj.FileName);
		var fullName = Path.Combine(projectPath, directory);
		var result = new FolderItem();

		if(directory == null || directory == """")
		{
			result.Path = projectPath;
			result.Items = proj.ProjectItems;
			return result;
		}

		AddFolderByPath(proj, directory);
		result.Path = fullName;
		result.Items = GetFolderByPath(proj, fullName).ProjectItems;
		return result;
	}
	private void CreateFile(String outputFile, String content){
		
		if(_dontTouchExistingFile && File.Exists(outputFile))
			return;

		//if (IsFileContentDifferent(outputFile, content))
		{
			CheckoutFileIfRequired(outputFile);
			File.WriteAllText(outputFile, content, Encoding.UTF8);
		}
	}
	private void InsertBOM()
	{

	}
	private bool IsFileContentDifferent(String fileName, String newContent){
		return !(File.Exists(fileName) && File.ReadAllText(fileName) == newContent);
	}
	private void ProjectSync(IEnumerable<CreatedFile> keepFileNames){
		
		foreach (var createdFile in keepFileNames) {
			
			createdFile.Folder.Items.AddFromFile(createdFile.FileName);
		}
		
		//var keepFileNameSet = new HashSet<String>(keepFileNames);
		//var projectFiles = new Dictionary<String, EnvDTE.ProjectItem>();
		//var originalFilePrefix = Path.GetFileNameWithoutExtension(templateProjectItem.get_FileNames(0)) + ""."";
		//
		//foreach (EnvDTE.ProjectItem projectItem in templateProjectItem.ProjectItems)
		//	projectFiles.Add(projectItem.get_FileNames(0), projectItem);
		//
		//// Remove unused items from the project
		//foreach (var pair in projectFiles)
		//	if (!keepFileNames.Contains(pair.Key) && !(Path.GetFileNameWithoutExtension(pair.Key) + ""."").StartsWith(originalFilePrefix))
		//		pair.Value.Delete();
		//
		//// Add missing files to the project
		//foreach (String fileName in keepFileNameSet)
		//	if (!projectFiles.ContainsKey(fileName))
		//		templateProjectItem.ProjectItems.AddFromFile(fileName);
	}
	private void CheckoutFileIfRequired(String fileName){
		var sc = _dte.SourceControl;
		if (sc != null && sc.IsItemUnderSCC(fileName) && !sc.IsItemCheckedOut(fileName))
			_checkOutAction.EndInvoke(_checkOutAction.BeginInvoke(fileName, null, null));
	}
	private EnvDTE.Project GetProjectByName(EnvDTE.Solution solution, String name){
		EnvDTE.Projects projects = solution.Projects;
		foreach(EnvDTE.Project projectItem in projects)
		{
		_template.AppendLine(projectItem.Name);
			if (projectItem.Name == name)
				return projectItem;
		}
		return null;
	}
	private EnvDTE.ProjectItem GetFolderByPath(EnvDTE.Project project, string path){
		
		String projectPath = Path.GetDirectoryName(project.FullName);
		path = path.Replace(projectPath, """");
		String[] folders = path.Split(new char[]{ '\\' }, StringSplitOptions.RemoveEmptyEntries);
		
		EnvDTE.ProjectItems projItems = project.ProjectItems;
		EnvDTE.ProjectItem projItem = null;
		for(int i = 0; i < folders.Length; i++){
			foreach(EnvDTE.ProjectItem projectItem in projItems) {
				if(projectItem.Name == folders[i])
				{
					projItems = projectItem.ProjectItems;
					if (i == folders.Length - 1) projItem = projectItem;
					break;
				}
			}
		}
		return projItem;
	}
	private void AddFolderByPath(EnvDTE.Project project, string path){
		
		String projectPath = Path.GetDirectoryName(project.FullName);
		path = path.Replace(projectPath, """");
		string fullPath = Path.Combine(projectPath, path);
		var item = GetFolderByPath(project, fullPath);
		if(item != null)
			return;
		
		Directory.CreateDirectory(fullPath);
		String[] folders = path.Split(new char[]{ '\\' }, StringSplitOptions.RemoveEmptyEntries);
		
		EnvDTE.ProjectItems projItems = project.ProjectItems;
		EnvDTE.ProjectItem projItem = null;
		String tempPath = projectPath;
		for(int i = 0; i < folders.Length; i++){
			bool folderExists = false;
			tempPath = Path.Combine(tempPath, folders[i]);
			foreach(EnvDTE.ProjectItem projectItem in projItems) {
				if(projectItem.Name == folders[i])
				{
					projItems = projectItem.ProjectItems;
					if (i == folders.Length - 1) projItem = projectItem;
					folderExists = true;
					break;
				}
			}
			if (!folderExists) {
				projItems.AddFromDirectory(tempPath);
				return;
			}
		}
		return;
	}
}
#>";
            #endregion

            //result.Add("8_CreateMvcViews.tt", mvcViews);
            result.Add("7_CreateMvc.tt", mvcFiles);
            result.Add("6_CreateHandlers.tt", handlers);
            result.Add("5_CreateSecurityObjects.tt", securityObjects);
            result.Add("4_CreateDatabaseFunctions.tt", databaseFunctions);
            result.Add("3_CreateDatabaseObjects.tt", databaseObjects);
            result.Add("2_CreateBasicTables.tt", basicTables);
            result.Add("1_ConnectionString.tt", connectionString);
            result.Add("General_Manager.tt", generalManager);

            return result;
        }
        

    }
}
