using System;

namespace BranchMonitor.GitCommands
{
	public class BranchInfo
	{
		public string AuthorName { get; set; }				// %an
		public string AuthorEmail { get; set; }				// %ae
		public string BranchName { get; set; }				// %D
		public string CommitterDateRelative { get; set; }	// %cr
		public int CommitterDateUnixTimestamp { get; set; }	// %ct
		public DateTime CommitterDate => DateTimeOffset.FromUnixTimeSeconds(CommitterDateUnixTimestamp).UtcDateTime;
	}
}