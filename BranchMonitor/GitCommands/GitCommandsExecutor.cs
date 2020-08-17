using System.Collections.Generic;

namespace BranchMonitor.GitCommands
{
	public class GitCommandsExecutor
	{
		private readonly GitExternalProcessRunner runner;

		public GitCommandsExecutor(GitExternalProcessRunner runner)
		{
			this.runner = runner;
		}

		public void FetchWithPrune()
		{
			runner.Run(GitCommands.FetchWithPrune);
		}

		public string[] GetMergedBranches()
		{
			var output = runner.Run(GitCommands.GetMergedToMasterBranches);
			return ParserForCommandToGetMergedBranches.GetBranches(output);
		}

		public void RemoveRemoteBranches(IEnumerable<string> remoteBranches)
		{
			var removeRemoteBranches = GitCommands.RemoveRemoteBranches(remoteBranches);
			runner.Run(removeRemoteBranches);
		}

		public void PruneRemote()
		{
			runner.Run(GitCommands.RemotePruneOrigin);
		}

		public string[] GetAllRemoteBranches()
		{
			var output = runner.Run(GitCommands.GetAllRemoteBranches);
			return ParserForCommandToGetAllRemoteBranches.GetBranches(output);
		}

		public BranchInfo GetBranchInfo(string branch)
		{
			const string separator = "   ";
			var output = runner.Run(GitCommands.GetLogForLastBranchCommit(separator, branch));
			return ParserForCommandToGetBranchInfo.GetInfo(output, separator);
		}
	}
}