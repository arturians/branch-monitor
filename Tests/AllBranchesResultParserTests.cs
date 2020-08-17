using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using BranchMonitor.GitCommands;

namespace Tests
{
	[TestFixture]
	public class AllBranchesResultParserTests
	{
		private const string InputFileName = "branch-rv_result.txt";
		private const string ExpectedFileName = "branch-rv_expected.txt";

		[OneTimeSetUp]
		public void SetupClass()
		{
			Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
		}

		[Test]
		public void Test()
		{
			var commandOutput = File.ReadAllText($@"TestData\{InputFileName}");
			var expectedBranches = File.ReadAllLines($@"TestData\{ExpectedFileName}");

			var branches = ParserForCommandToGetAllRemoteBranches.GetBranches(commandOutput).ToArray();

			Assert.AreEqual(expectedBranches, branches);
		}
	}
}