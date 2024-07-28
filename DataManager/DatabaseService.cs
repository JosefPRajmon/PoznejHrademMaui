using PoznejHrademMaui.Components;
using SQLite;

namespace PoznejHrademMaui.DataManager
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            try
            {
                var databasePath = Path.Combine(FileSystem.AppDataDirectory, "ScannedCodes.db");
                _database = new SQLiteAsyncConnection(databasePath);
                _database.CreateTableAsync<ScannedCode>().Wait();
                Task.Run(async () =>
                {
                    List<ScannedCode> resultDB = await GetScannedCodesAsync();
                    if ((resultDB).Count ==0)
                    {
                        //List<string> list = new List<string>() { };
                        for (global::System.Int32 i = 1; i < (Int16)Application.Current.Resources["keysCount"]; i++)
                        {
                            SaveScannedCodeAsync(new ScannedCode()
                            {
                                Id=i,
                                QRCodeText = (string)Application.Current.Resources[$"key{i}"],

                            });
                        }

                    }
                });
            }
            catch (Exception e)
            {

                throw;
            }


        }

        public Task<List<ScannedCode>> GetScannedCodesAsync()
        {
            return _database.Table<ScannedCode>().ToListAsync();
        }

        public Task<ScannedCode> GetScannedCodesAsync(string qrCodeText)
        {
            return _database.Table<ScannedCode>().Where(c => c.QRCodeText == qrCodeText).FirstOrDefaultAsync();
        }

        public Task<int> SaveScannedCodeAsync(ScannedCode code)
        {
            return _database.InsertAsync(code);
        }

        public Task<int> UpdateScannedCodeAsync(ScannedCode code)
        {
            code.IsScanned = true;
            return _database.UpdateAsync(code);
        }
    }
}
