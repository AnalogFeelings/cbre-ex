using System.Diagnostics;
using System.Drawing;
using System.IO.Compression;
using Pastel;

namespace CBRE.Extended.Updater;

public enum LogSeverity
{
	Message,
	Warning,
	Error
}

public class EntryPoint
{
	//Arg 0: New version
	//Arg 1: CBRE-EX process name
	//Arg 2: Package filename
	static void Main(string[] Arguments)
	{
		if (Arguments.Length < 3) return;

		string targetDirectory = AppDomain.CurrentDomain.BaseDirectory;
		string currentFilename = Path.GetFileName(Process.GetCurrentProcess().MainModule!.FileName!);

		string newVersion = Arguments[0];
		string friendlyCbreProcess = Arguments[1].Replace(".exe", "");
		string packageFilename = Arguments[2];

		Console.Title = "CBRE-EX Updater";
		if (Environment.OSVersion.Version.Major < 10) ConsoleExtensions.Disable();

		Log($"Waiting until {"CBRE-EX".Pastel(Color.LimeGreen)} shuts down...", LogSeverity.Message);

		while (true)
		{
			Process[] cbreProcesses = Process.GetProcessesByName(friendlyCbreProcess);

			if (cbreProcesses.Length > 0) Thread.Sleep(100);
			else break;
		}

		Log($"Installing {"CBRE-EX".Pastel(Color.LimeGreen)} {$"v{newVersion}".Pastel(Color.Lime)}", LogSeverity.Message);

		try
		{
			if (!File.Exists(packageFilename)) throw new FileNotFoundException($"The update package was not found. Expected a file called \"{packageFilename}\" in this directory.");

			Log($"Extracting {packageFilename.Pastel(Color.LimeGreen)} to Temp directory...", LogSeverity.Message);
			if (Directory.Exists("Temp")) Directory.Delete("Temp", true);

			ZipFile.ExtractToDirectory(packageFilename, "Temp");

			DirectoryInfo tempDirectory = new DirectoryInfo("Temp");
			DirectoryInfo[] tempSubdirectories = tempDirectory.GetDirectories();

			foreach (DirectoryInfo directory in tempSubdirectories)
			{
				Log($"Copying updated directory \"{directory.Name.Pastel(Color.Lime)}\" and its contents to existing install...", LogSeverity.Message);
				CopyDirectory(directory.FullName, Path.Combine(targetDirectory, directory.Name), true);
			}

			FileInfo[] tempDirectoryFiles = tempDirectory.GetFiles();
			foreach (FileInfo file in tempDirectoryFiles)
			{
				if (file.Name == currentFilename || file.Name == "Pastel.dll" || file.Name == Path.GetFileNameWithoutExtension(currentFilename) + ".pdb") continue;

				Log($"Copying updated file \"{file.Name.Pastel(Color.Lime)}\" to existing install...", LogSeverity.Message);
				file.CopyTo(Path.Combine(targetDirectory, file.Name), true);
			}

			Log($"Cleaning up left over files...", LogSeverity.Message);

			Directory.Delete("Temp", true);
			File.Delete(packageFilename);

			Log($"Done! Starting CBRE-EX...", LogSeverity.Message);

			ProcessStartInfo editorProcess = new ProcessStartInfo(Arguments[1]);
			editorProcess.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
			editorProcess.UseShellExecute = true;
			Process.Start(editorProcess);

			Environment.Exit(0);
		}
		catch (Exception ex)
		{
			Log($"Error! {ex.Message.Pastel(Color.IndianRed)}", LogSeverity.Error);

			Thread.Sleep(Timeout.Infinite);
		}
	}

	static void CopyDirectory(string Source, string Destination, bool Recursive)
	{
		DirectoryInfo directory = new DirectoryInfo(Source);
		DirectoryInfo[] subdirectories = directory.GetDirectories();

		Directory.CreateDirectory(Destination);

		foreach (FileInfo file in directory.GetFiles())
		{
			string targetPath = Path.Combine(Destination, file.Name);
			file.CopyTo(targetPath, true);
		}

		if (Recursive)
		{
			foreach (DirectoryInfo subDirectory in subdirectories)
			{
				string targetPath = Path.Combine(Destination, subDirectory.Name);
				CopyDirectory(subDirectory.FullName, targetPath, true);
			}
		}
	}

	static void Log(string Message, LogSeverity Severity)
	{
		switch (Severity)
		{
			case LogSeverity.Message:
				Console.WriteLine($"[{"MSG".Pastel(Color.CadetBlue)}] {Message}");
				break;
			case LogSeverity.Warning:
				Console.WriteLine($"[{"WRN".Pastel(Color.Yellow)}] {Message}");
				break;
			case LogSeverity.Error:
				Console.WriteLine($"[{"ERR".Pastel(Color.Red)}] {Message}");
				break;
		}
	}
}