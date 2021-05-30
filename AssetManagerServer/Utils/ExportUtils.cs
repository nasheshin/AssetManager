using System;
using System.Collections.Generic;
using System.IO;

namespace AssetManagerServer.Utils
{
    public static class ExportUtils
    {
        public static Tuple<string, string> CreatePortfolioCsv(IEnumerable<ConvertingSortingUtils.PortfolioElement> portfolio, int userId)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var folderPath = Path.Combine(documentsPath, "AssetManagerExports");
            var fileName = $"portfolio{userId}.csv";
            var filePath = Path.Combine(folderPath, fileName);
            
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            if (!File.Exists(filePath))
                File.Create(filePath).Close();

            using (var csvWriter = new StreamWriter(filePath))
            {
                csvWriter.WriteLine($"Актив,Тикер,Тип,Брокер,Кол-во,Привлекательность покупки,Привлекательность продажи");
                foreach (var element in portfolio)
                {
                    csvWriter.WriteLine(element.ToString());
                }
            }

            return new Tuple<string, string>(filePath, fileName);
        }
        
        public static Tuple<string, string> CreateOperationsCsv(IEnumerable<ConvertingSortingUtils.OperationElement> operations, int userId)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var folderPath = Path.Combine(documentsPath, "AssetManagerExports");
            var fileName = $"operations{userId}.csv";
            var filePath = Path.Combine(folderPath, fileName);
            
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            if (!File.Exists(filePath))
                File.Create(filePath).Close();

            using (var csvWriter = new StreamWriter(filePath))
            {
                csvWriter.WriteLine($"Актив,Тикер,Тип,Дата,Операцтя,Брокер,Цена,Привлекательность покупки,Привлекательность продажи");
                foreach (var element in operations)
                {
                    csvWriter.WriteLine(element.ToString());
                }
            }

            return new Tuple<string, string>(filePath, fileName);
        }
    }
}