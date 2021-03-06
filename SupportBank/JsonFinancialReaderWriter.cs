﻿using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

namespace SupportBank
{
    public class JsonFinancialReaderWriter : IFinancialRecordReaderWriter
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public List<Transaction> Read(string file)
        {
            LoggerInitializer.InitializeLogger(Directory.GetCurrentDirectory());

            var jsonDoc = JArray.Parse(File.ReadAllText(file));

            var transactions = new List<Transaction>();

            var index = 1;
            foreach (var jToken in jsonDoc)
            {
                try
                {
                    transactions.Add(jToken.ToObject<Transaction>());
                }
                catch
                {
                    logger.Warn($"Bad data found in {file}: entry {index}");
                    Console.WriteLine($"Bad data found in {file}. Please fix entry #{index}");
                }
            }

            return transactions;
        }
        
        public void Write(List<Transaction> transactions, string file)
        {
            File.WriteAllText(file,
                JsonConvert.SerializeObject(transactions, Newtonsoft.Json.Formatting.Indented));
        }
    }
}