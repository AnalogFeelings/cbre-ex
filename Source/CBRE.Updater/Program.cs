using CBRE.Localization;
using Pastel;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Threading;

namespace CBRE.Updater
{
	public enum LogSeverity
	{
		MESSAGE,
		WARNING,
		ERROR
	}

	public class Program
	{
		//Arg 0: New version
		//Arg 1: CBRE-EX process name
		//Arg 2: Package filename
		static void Main(string[] args)
		{
			if (args.Length < 3) return;

			string TargetDirectory = AppDomain.CurrentDomain.BaseDirectory;
			string CurrentFilename = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);

			string NewVersion = args[0];
			string FriendlyCbreProcess = args[1].Replace(".exe", "");
			string PackageFilename = args[2];

			Console.Title = Local.LocalString("updater.title");
			if (Environment.OSVersion.Version.Major < 10) ConsoleExtensions.Disable();

			Log(Local.LocalString("log.updater.wait", "CBRE-EX".Pastel(Color.LimeGreen)), LogSeverity.MESSAGE);

			while (true)
			{
				Process[] CbreProcess = Process.GetProcessesByName(FriendlyCbreProcess);

				if (CbreProcess.Length > 0) Thread.Sleep(100);
				else break;
			}

			Log(Local.LocalString("log.updater.install", "CBRE-EX".Pastel(Color.LimeGreen), NewVersion.Pastel(Color.Lime)), LogSeverity.MESSAGE);

			try
			{
				if (!File.Exists(PackageFilename)) throw new FileNotFoundException(Local.LocalString("error.updater.package_not_found", PackageFilename));

				Log(Local.LocalString("log.updater.extract", PackageFilename.Pastel(Color.LimeGreen)), LogSeverity.MESSAGE);
				if (Directory.Exists("Temp")) Directory.Delete("Temp", true);

				ZipFile.ExtractToDirectory(PackageFilename, "Temp");

				DirectoryInfo TempDir = new DirectoryInfo("Temp");
				DirectoryInfo[] TempSubdirs = TempDir.GetDirectories();

				foreach (DirectoryInfo Dir in TempSubdirs)
				{
					Log(Local.LocalString("log.updater.copy", Dir.Name.Pastel(Color.Lime)), LogSeverity.MESSAGE);
					CopyDirectory(Dir.FullName, Path.Combine(TargetDirectory, Dir.Name), true);
				}

				FileInfo[] TempDirFiles = TempDir.GetFiles();
				foreach (FileInfo File in TempDirFiles)
				{
					if (File.Name == CurrentFilename || File.Name == "Pastel.dll" || File.Name == Path.GetFileNameWithoutExtension(CurrentFilename) + ".pdb") continue;

					Log(Local.LocalString("log.updater.copy.file", File.Name.Pastel(Color.Lime)), LogSeverity.MESSAGE);
					File.CopyTo(Path.Combine(TargetDirectory, File.Name), true);
				}

				Log(Local.LocalString("log.updater.clean"), LogSeverity.MESSAGE);

				Directory.Delete("Temp", true);
				File.Delete(PackageFilename);

				Log(Local.LocalString("log.updater.done"), LogSeverity.MESSAGE);

				ProcessStartInfo editorProcess = new ProcessStartInfo(args[1]);
				editorProcess.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
				editorProcess.UseShellExecute = true;
				Process.Start(editorProcess);

				Environment.Exit(0);
			}
			catch (Exception ex)
			{
				Log(Local.LocalString("error.updater.error", ex.Message.Pastel(Color.IndianRed)), LogSeverity.ERROR);

				Thread.Sleep(Timeout.Infinite);
			}
		}

		static void CopyDirectory(string Source, string Destination, bool Recursive)
		{
			DirectoryInfo Dir = new DirectoryInfo(Source);
			DirectoryInfo[] Subdirs = Dir.GetDirectories();

			Directory.CreateDirectory(Destination);

			foreach (FileInfo File in Dir.GetFiles())
			{
				string TargetPath = Path.Combine(Destination, File.Name);
				File.CopyTo(TargetPath, true);
			}

			if (Recursive)
			{
				foreach (DirectoryInfo Subdir in Subdirs)
				{
					string TargetPath = Path.Combine(Destination, Subdir.Name);
					CopyDirectory(Subdir.FullName, TargetPath, true);
				}
			}
		}

		static void Log(string Message, LogSeverity Severity)
		{
			switch (Severity)
			{
				case LogSeverity.MESSAGE:
					Console.WriteLine($"[{"MSG".Pastel(Color.CadetBlue)}] {Message}");
					break;
				case LogSeverity.WARNING:
					Console.WriteLine($"[{"WRN".Pastel(Color.Yellow)}] {Message}");
					break;
				case LogSeverity.ERROR:
					Console.WriteLine($"[{"ERR".Pastel(Color.Red)}] {Message}");
					break;
			}
		}
	}
}