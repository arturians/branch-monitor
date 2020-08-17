using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using BranchMonitor.GitCommands;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Http;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using MoreLinq;

namespace BranchMonitor.GoogleSpreadSheetAdapter
{
	public class SpreadSheetAdapter
	{
		private readonly string spreadSheetId;

		public SpreadSheetAdapter(GoogleSheetSettings googleSheetSettings)
		{
			spreadSheetId = googleSheetSettings.SpreadSheetId;
		}

		public void Update(BranchInfo[] branches)
		{
			var sheetId = (int) DateTimeOffset.UtcNow.ToUnixTimeSeconds();

			var batchUpdateSpreadsheetRequest = new BatchUpdateSpreadsheetRequest
			{
				Requests = new List<Request>
				{
					new Request { AddSheet = GetAddSheetRequest(sheetId, branches.Length) },
					new Request { UpdateCells = GetUpdateCellsRequest(sheetId, branches) },
				}
			};
			GetUpdateDimensionPropertiesRequest(sheetId)
				.ForEach(r => batchUpdateSpreadsheetRequest.Requests.Add(new Request {UpdateDimensionProperties = r}));

			var credential = LoadCredentials();
			using (var service = new SheetsService(GetInitializer(credential)))
			{
				var batchUpdateRequest = service.Spreadsheets.BatchUpdate(batchUpdateSpreadsheetRequest, spreadSheetId);
				batchUpdateRequest.Execute();
			}
		}

		private static AddSheetRequest GetAddSheetRequest(int sheetId, int branchesCount)
		{
			var addSheetRequest = new AddSheetRequest
			{
				Properties = new SheetProperties
				{
					Title = DateTime.Now.ToString("dd.MM.yyyy_hh:mm:ss"),
					SheetId = sheetId,
					Index = 0,
					GridProperties = new GridProperties
					{
						ColumnCount = 5, RowCount = branchesCount + 1, FrozenRowCount = 1 //note: +1 row for header /as
					} 
				}
			};
			return addSheetRequest;
		}

		private static UpdateCellsRequest GetUpdateCellsRequest(int sheetId, IEnumerable<BranchInfo> branches)
		{
			var rows = GetSheetHeader().Concat(branches.Select(GetRowData)).ToList();

			return new UpdateCellsRequest
			{
				Start = new GridCoordinate {SheetId = sheetId, ColumnIndex = 0, RowIndex = 0}, //note: cell A1 //as
				Fields = "userEnteredValue, userEnteredFormat",
				Rows = rows
			};
		}

		private static List<RowData> GetSheetHeader()
		{
			var userEnteredFormat = new CellFormat
			{
				TextFormat = new TextFormat {Bold = true, FontSize = 12}
			};
			return new List<RowData>
			{
				new RowData
				{
					Values = new List<CellData>
					{
						new CellData { UserEnteredFormat = userEnteredFormat, UserEnteredValue = new ExtendedValue {StringValue = "Ответственный"} },
						new CellData { UserEnteredFormat = userEnteredFormat, UserEnteredValue = new ExtendedValue {StringValue = "Ветка"} },
						new CellData { UserEnteredFormat = userEnteredFormat, UserEnteredValue = new ExtendedValue {StringValue = "Action"} },
						new CellData { UserEnteredFormat = userEnteredFormat, UserEnteredValue = new ExtendedValue {StringValue = "Дата"} },
						new CellData { UserEnteredFormat = userEnteredFormat, UserEnteredValue = new ExtendedValue {StringValue = "Как давно"}, }
					}
				}
			};
		}

		private static RowData GetRowData(BranchInfo b)
		{
			var cellFormat = new CellFormat {BackgroundColor = GetColorByDate(b.CommitterDate)};

			return new RowData
			{
				Values = new List<CellData>
				{
					new CellData { UserEnteredFormat = cellFormat, UserEnteredValue = new ExtendedValue {StringValue = b.AuthorName} },
					new CellData { UserEnteredFormat = cellFormat, UserEnteredValue = new ExtendedValue {StringValue = b.BranchName} },
					new CellData { UserEnteredFormat = cellFormat, UserEnteredValue = new ExtendedValue {StringValue = "no action"} },
					new CellData { UserEnteredFormat = cellFormat, UserEnteredValue = new ExtendedValue {StringValue = b.CommitterDate.ToString("R")} },
					new CellData { UserEnteredFormat = cellFormat, UserEnteredValue = new ExtendedValue {StringValue = b.CommitterDateRelative} }
				}
			};
		}

		private static Color GetColorByDate(DateTime updateTime)
		{
			var timeSpan = DateTime.Now - updateTime;
			if (timeSpan > TimeSpan.FromDays(61))
				return GetColor(186, 37, 37);
			if (timeSpan > TimeSpan.FromDays(30))
				return GetColor(224, 102, 102);
			if (timeSpan > TimeSpan.FromDays(14))
				return GetColor(244, 204, 204);
			return null;
		}

		private static Color GetColor(int red, int green, int blue)
		{
			return new Color {Alpha = 1.0f, Red = red / 255f, Green = green / 255f, Blue = blue / 255f};
		}

		private static IEnumerable<UpdateDimensionPropertiesRequest> GetUpdateDimensionPropertiesRequest(int sheetId)
		{
			return new[] {220, 310, 650, 270, 130}.Select((v, i) => new UpdateDimensionPropertiesRequest
			{
				Range = new DimensionRange
				{
					SheetId = sheetId,
					Dimension = "COLUMNS",
					StartIndex = i,
					EndIndex = i + 1
				},
				Properties = new DimensionProperties {PixelSize = v},
				Fields = "pixelSize"
			});
		}

		private UserCredential LoadCredentials()
		{
			using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
			{
				const string credPath = "token.json";
				return GoogleWebAuthorizationBroker.AuthorizeAsync(
					GoogleClientSecrets.Load(stream).Secrets,
					new[] {SheetsService.Scope.Spreadsheets},
					"user",
					CancellationToken.None,
					new FileDataStore(credPath, true)).Result;
			}
		}

		private static BaseClientService.Initializer GetInitializer(IConfigurableHttpClientInitializer credential)
		{
			return new BaseClientService.Initializer
			{
				HttpClientInitializer = credential,
				ApplicationName = AppDomain.CurrentDomain.FriendlyName
			};
		}
	}
}