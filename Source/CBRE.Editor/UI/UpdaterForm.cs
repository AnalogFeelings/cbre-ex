using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;
using CBRE.Common.Extensions;
using CBRE.Localization;
using CBRE.UI.Native;

namespace CBRE.Editor.UI
{
	public partial class UpdaterForm : Form
	{
		private ReleaseAsset PackageAsset;
		private ReleaseAsset ChecksumAsset;
		private string VersionString;

		public UpdaterForm(Version Version, string Description, ReleaseAsset PackageAsset, ReleaseAsset ChecksumAsset)
		{
			InitializeComponent();

			this.PackageAsset = PackageAsset;
			this.ChecksumAsset = ChecksumAsset;

			// HACK: microsoft's version class is stupid as fuck.
			if (Version.Build == -1) Version = new Version(Version.Major, Version.Minor, 0);
			
			VersionString = Version.ToString(3);

			NativeIcons.SHSTOCKICONINFO StockIconInfo = new NativeIcons.SHSTOCKICONINFO();
			StockIconInfo.cbSize = (UInt32)Marshal.SizeOf(typeof(NativeIcons.SHSTOCKICONINFO));
			NativeIcons.SHGetStockIconInfo(NativeIcons.SHSTOCKICONID.SIID_INFO, NativeIcons.SHGSI.SHGSI_ICON | NativeIcons.SHGSI.SHGSI_SHELLICONSIZE, ref StockIconInfo);

			systemBitmap.Image = Icon.FromHandle(StockIconInfo.hIcon).ToBitmap();

			headerLabel.Text = Local.LocalString("updater.header", VersionString);

			changelogBox.Text = Description.Trim();
		}

		private void noButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private async void yesButton_Click(object sender, EventArgs e)
		{
			string CurrentFilename = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);

			this.changelogBox.Select(0, 0);
			this.ControlBox = false;
			this.noButton.Enabled = false;
			this.yesButton.Enabled = false;

			try
			{
				using (HttpClient Client = new HttpClient())
				{
					Client.DefaultRequestHeaders.Add("User-Agent", "AnalogFeelings/cbre-ex");
					
					string DownloadedChecksum = await Client.GetStringAsync(ChecksumAsset.DownloadUrl);

					if (string.IsNullOrWhiteSpace(DownloadedChecksum)) throw new Exception(Local.LocalString("exception.checksum_empty"));

					Progress<float> fileProgress = new Progress<float>();

					fileProgress.ProgressChanged += (senderObj, progress) =>
					{
						int roundedProgress = (int)Math.Round(progress);

						downloadProgress.Value = roundedProgress;
						statusLabel.Text = Local.LocalString("updater.status.downloading", roundedProgress);
					};

					using (FileStream fileStream = File.Create(PackageAsset.Filename))
					{
						await Client.DownloadDataAsync(PackageAsset.DownloadUrl, fileStream, fileProgress);
					}

					statusLabel.Text = Local.LocalString("updater.status.verifying");

					using (SHA256 Hasher = SHA256.Create())
					{
						using (FileStream Stream = File.OpenRead(PackageAsset.Filename))
						{
							string ConvertedChecksum;

							byte[] Sha256Hash = Hasher.ComputeHash(Stream);
							ConvertedChecksum = BitConverter.ToString(Sha256Hash).Replace("-", "").ToLowerInvariant();

							if (DownloadedChecksum != ConvertedChecksum) throw new Exception(Local.LocalString("exception.update_package_corrupted"));
						}
					}

					ProcessStartInfo updaterProcess = new ProcessStartInfo("CBRE.Updater.exe");
					//Arg 0: New version
					//Arg 1: CBRE-EX process name
					//Arg 2: Package filename
					updaterProcess.Arguments = $"{VersionString} {FixSpaces(CurrentFilename)} {PackageAsset.Filename}";
					updaterProcess.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
					updaterProcess.UseShellExecute = true;
					Process.Start(updaterProcess);

					Editor.Instance.Close();
				}
			}
			catch (Exception ex)
			{
				this.ControlBox = true;
				this.noButton.Enabled = true;
				this.yesButton.Enabled = true;
				this.downloadProgress.Value = 0;
				statusLabel.Text = Local.LocalString("updater.status.idle");

				MessageBox.Show(Local.LocalString("error.package", ex.Message), Local.LocalString("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);

				if (File.Exists(PackageAsset.Filename)) File.Delete(PackageAsset.Filename);
				if (File.Exists(ChecksumAsset.Filename)) File.Delete(ChecksumAsset.Filename);
			}
		}

		private string FixSpaces(string Text)
		{
			if (Text.Contains(" ")) return $"\"{Text}\"";
			else return Text;
		}
	}
}
