using System;
using System.Linq;

namespace BranchMonitor.GitCommands
{
	public class ParserForCommandToGetBranchInfo
	{
		private const string Format = "ddd dd MMM yyyy hh:mm:ss zzz";

		public static BranchInfo GetInfo(string output, string separator)
		{
			var infos = output
				.Split(new []{separator}, StringSplitOptions.RemoveEmptyEntries)
				.ToArray();

			return new BranchInfo
			{
				AuthorName = infos[0],
				AuthorEmail = infos[1],
				BranchName = infos[2],
				CommitterDateRelative = infos[3],
				CommitterDateUnixTimestamp = int.Parse(infos[4])
			};
		}
	}
}