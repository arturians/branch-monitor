using BranchMonitor.GitCommands;

namespace BranchMonitor
{
	public class Settings
	{
		public GitSettings GitSettings { get; set; }
		public GoogleSheetSettings GoogleSheetSettings { get; set; }
	}
}