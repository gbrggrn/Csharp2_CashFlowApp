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
    /// <summary>
    /// Responsible for file-operations.
    /// </summary>
    internal class FileManager
    {
        /// <summary>
        /// Serializes accountManager to a jsonstring and saves to file.
        /// </summary>
        /// <param name="filePath">Path to be saved to</param>
        /// <param name="accountManager">The accountManager to be serialized</param>
        /// <exception cref="ArgumentException">Throws if file does not save</exception>
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

        /// <summary>
        /// Deserializes an accountManager from a saved file.
        /// </summary>
        /// <param name="filePath">Path of the file to be deserialized</param>
        /// <returns>The deserialized accountManager</returns>
        /// <exception cref="ArgumentException">Throws if everything breaks</exception>
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
