using System.Globalization;
using WW.Application.Common.Interfaces;
//using WW.Application.TodoLists.Queries.ExportTodos;
//using WW.Infrastructure.Files.Maps;
using CsvHelper;

namespace WW.Infrastructure.Files;

public class CsvFileBuilder : ICsvFileBuilder
{
    /*public byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records)
    {
        using var memoryStream = new MemoryStream();
        using (var streamWriter = new StreamWriter(memoryStream))
        {
            using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

            csvWriter.Configuration.RegisterClassMap<TodoItemRecordMap>();
            csvWriter.WriteRecords(records);
        }

        return memoryStream.ToArray();
    }*/
}
