using System;
using BranchMonitor.GitCommands;
using BranchMonitor.GoogleSpreadSheetAdapter;

namespace BranchMonitor
{
	class Program
	{
		static void Main(string[] args)
		{
			var settings = SettingsLoader.Load(args[0]);

			var staleBranches = new StaleBranchesProvider(settings.GitSettings).GetBranches();

			//var staleBranches = GetFakeBranches();

			new SpreadSheetAdapter(settings.GoogleSheetSettings).Update(staleBranches);
		}

		private static BranchInfo[] GetFakeBranches()
		{
			var branchInfos = new[]
			{
				new BranchInfo
				{
					BranchName = "origin/some1", AuthorName = "as", CommitterDateRelative = "8 weeks ago",
					CommitterDateUnixTimestamp = (int) ((DateTimeOffset) DateTime.Now.AddDays(-70)).ToUnixTimeSeconds()
				},
				new BranchInfo
				{
					BranchName = "origin/some2", AuthorName = "as", CommitterDateRelative = "5 weeks ago",
					CommitterDateUnixTimestamp = (int) ((DateTimeOffset) DateTime.Now.AddDays(-35)).ToUnixTimeSeconds()
				},
				new BranchInfo
				{
					BranchName = "origin/some3", AuthorName = "as", CommitterDateRelative = "2 weeks ago",
					CommitterDateUnixTimestamp = (int) ((DateTimeOffset) DateTime.Now.AddDays(-15)).ToUnixTimeSeconds()
				},
				new BranchInfo
				{
					BranchName = "origin/some4", AuthorName = "as", CommitterDateRelative = "1 week ago",
					CommitterDateUnixTimestamp = (int) ((DateTimeOffset) DateTime.Now.AddDays(-7)).ToUnixTimeSeconds()
				}
			};
			return branchInfos;
		}
	}
}
