#tool "nuget:?package=NUnit.Runners&version=2.6.4"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var needOutput = Argument("output", "N").Equals("Y", StringComparison.OrdinalIgnoreCase);

var buildDir = MakeAbsolute(Directory("./build")).ToString();
var nugetDir = MakeAbsolute(Directory("./nuget")).ToString();

Setup(context =>
{
    CleanDirectory(buildDir);
    CleanDirectory(nugetDir);
});

Teardown(context =>
{
});

Task("Build").Does(() =>
{
    MSBuild("./NextLevelSeven/NextLevelSeven.csproj", settings => 
    {
        settings.SetConfiguration(configuration).SetPlatformTarget(PlatformTarget.MSIL);
        if (needOutput)
        {
            settings.WithProperty("OutDir", buildDir);
        }
    });
    MSBuild("./NextLevelSeven.Test/NextLevelSeven.Test.csproj", settings => 
    {
        settings.SetConfiguration(configuration).SetPlatformTarget(PlatformTarget.MSIL);
    });
});

Task("Test").IsDependentOn("Build").Does(() =>
{
    var assemblies = GetFiles("./NextLevelSeven.Test/bin/*/*.Test.dll");
    NUnit(assemblies);
});

Task("Package").IsDependentOn("Test").Does(() =>
{
    if (needOutput)
    {
        EnsureDirectoryExists(nugetDir);
        var nugetPackSettings = new NuGetPackSettings {
            Id = "NextLevelSeven",
            Title = "NextLevelSeven",
            Authors = new[] {"SaxxonPike"},
            Owners = new[] {"SaxxonPike"},
            Description = "A class library for parsing and manipulating HL7 v2 messages",
            Summary = "A class library for parsing and manipulating HL7 v2 messages, designed to be fast and easy to use.",
            ProjectUrl = new Uri("https://github.com/SaxxonPike/NextLevelSeven/"),
            OutputDirectory = nugetDir,
            BasePath = buildDir
        };
        NuGetPack("./NextLevelSeven/NextLevelSeven.csproj", nugetPackSettings);
    }
});

Task("Publish").IsDependentOn("Package").Does(() =>
{
    if (needOutput)
    {
        var packages = GetFiles("./nuget/NextLevelSeven.*.nupkg");
        foreach (var package in packages)
        {
            NuGetPush(package, new NuGetPushSettings { 
                Source = "https://www.nuget.org/api/v2/package",
                ApiKey = "3207cbb1-f0ac-4ee5-a0d8-53301ed88e46"
            });
        }
    }
});

Task("Default").IsDependentOn("Test");

RunTarget(target);