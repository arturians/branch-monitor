using System;
using System.Linq;

namespace BranchMonitor.GitCommands
{
	public class StaleBranchesProvider
	{
		private readonly GitSettings settings;

		public StaleBranchesProvider(GitSettings settings)
		{
			this.settings = settings;
		}

		public BranchInfo[] GetBranches()
		{
			var runner = new GitExternalProcessRunner(settings);
			var gitCommandsExecutor = new GitCommandsExecutor(runner);
			gitCommandsExecutor.FetchWithPrune(); // подтянули все ветки
			var mergedToMasterBranches = gitCommandsExecutor.GetMergedBranches();  // взяли влитые ветки
			gitCommandsExecutor.RemoveRemoteBranches(mergedToMasterBranches); // удалили из мастера влитые ветки

			try
			{
				gitCommandsExecutor.PruneRemote(); // удалить ветки, которые влиты в мастер, но остались локально
			}
			catch (Exception)
			{
				// ignored
			}
			gitCommandsExecutor.FetchWithPrune();
			var branchInfos = gitCommandsExecutor.GetAllRemoteBranches()
				.Select(gitCommandsExecutor.GetBranchInfo)
				.Where(i => DateTime.Now - i.CommitterDate > TimeSpan.FromDays(7))
				.OrderBy(i => i.AuthorEmail)
				.ThenBy(i => i.CommitterDate)
				.ToArray();

			return branchInfos;
		}
	}
}