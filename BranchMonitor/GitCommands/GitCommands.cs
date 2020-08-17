using System.Collections.Generic;
using System.Text;

namespace BranchMonitor.GitCommands
{
	internal static class GitCommands
	{
		public static string FetchWithPrune => "fetch --prune";
		public static string GetMergedToMasterBranches => "branch -r --merged origin/master";

		public static string RemoveRemoteBranches(IEnumerable<string> branches)
		{
			var commandBuilder = new StringBuilder("push origin");
			foreach (var branch in branches)
			{
				commandBuilder.Append($" :{branch}");
			}

			return commandBuilder.ToString();
		}

		public static string GetAllRemoteBranches => "branch -rv";

		public static string GetLogForLastBranchCommit(string separator, string remoteBranch)
			=> $"log -n 1 --format=\"%an{separator}%ae{separator}%D{separator}%cr{separator}%ct\" {remoteBranch}";

		public static string RemotePruneOrigin => "remote prune origin";
	}
}