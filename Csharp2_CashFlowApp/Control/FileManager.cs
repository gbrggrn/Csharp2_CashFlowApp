using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Linq.Expressions;

namespace Csharp2_CashFlowApp.Control
{
    internal class FileManager
    {
        public void Serialize(string filePath, AccountManager accountManager)
        {
            try
            {
                using (StreamWriter write = new(filePath))
                {
                    string jsonString = JsonSerializer.Serialize(accountManager);

                    write.Write(jsonString);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Something went wrong - File not saved\n" +
                    $"Errormessage:\n" +
                    $"{ex.Message}");
            }
        }

        public AccountManager Deserialize(string filePath)
        {
            AccountManager accountManager = new();
            try
            {
                using (StreamReader read = new(filePath))
                {
                    string jsonString = read.ReadToEnd();
                    accountManager = JsonSerializer.Deserialize<AccountManager>(jsonString)!;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Something went wrong - File not loaded\n" +
                    $"Errormessage:\n" +
                    $"{ex.Message}");
            }

            return accountManager;
        }
    }
}
