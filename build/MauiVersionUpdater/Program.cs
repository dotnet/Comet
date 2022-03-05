
using System.Text.Json;
using System.Text.Json.Serialization;

var url = "https://aka.ms/dotnet/maui/main.json";
var path = "Directory.Build.props";
if (args.Length > 0)
{
	url = args[0];
}

Console.WriteLine($"Parsing url: {url}");
var json = await new HttpClient().GetStringAsync(url);

var versionInfo = JsonSerializer.Deserialize<RollBackModel>(json);

Console.WriteLine($"MauiVersion {versionInfo.MicrosoftNetSdkMaui}");
Console.WriteLine($"Current Directory {Directory.GetCurrentDirectory()}");

var buildProps = File.ReadAllText(path);
var openString = "<MauiVersion>";
var start = buildProps.IndexOf(openString);
start += openString.Length;
var end = buildProps.IndexOf("</MauiVersion>");
var oldValue = buildProps.Substring(start, end - start);
Console.WriteLine($"Old Value: {oldValue}");
var newProps = buildProps.Replace(oldValue, versionInfo.MicrosoftNetSdkMaui);
File.WriteAllText(path, newProps);

public class RollBackModel
{
	[JsonPropertyName("microsoft.net.sdk.android")]
	public string MicrosoftNetSdkAndroid { get; set; }

	[JsonPropertyName("microsoft.net.sdk.ios")]
	public string MicrosoftNetSdkIos { get; set; }

	[JsonPropertyName("microsoft.net.sdk.maccatalyst")]
	public string MicrosoftNetSdkMaccatalyst { get; set; }

	[JsonPropertyName("microsoft.net.sdk.macos")]
	public string MicrosoftNetSdkMacos { get; set; }

	[JsonPropertyName("microsoft.net.sdk.maui")]
	public string MicrosoftNetSdkMaui { get; set; }

	[JsonPropertyName("microsoft.net.sdk.tvos")]
	public string MicrosoftNetSdkTvos { get; set; }

	[JsonPropertyName("microsoft.net.workload.mono.toolchain")]
	public string MicrosoftNetWorkloadMonoToolchain { get; set; }

	[JsonPropertyName("microsoft.net.workload.emscripten")]
	public string MicrosoftNetWorkloadEmscripten { get; set; }
}
