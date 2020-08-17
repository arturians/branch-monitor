using System.Diagnostics;

namespace BranchMonitor.GitCommands
{
	public class GitExternalProcessRunner
	{
		private readonly GitSettings gitSettings;

		public GitExternalProcessRunner(GitSettings gitSettings)
		{
			this.gitSettings = gitSettings;
		}

		public string Run(string arguments)
		{
			string output;
			using (var process = new Process())
			{
				process.StartInfo.FileName = gitSettings.GitExePath;
				process.StartInfo.Arguments = arguments;
				process.StartInfo.WorkingDirectory = gitSettings.RepositoryPath;
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.RedirectStandardOutput = true;
				process.Start();

				output = process.StandardOutput.ReadToEnd();

				process.WaitForExit();
				process.Close();
			}

			return output;
		}
	}
}