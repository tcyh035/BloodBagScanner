using System.IO;
using System.Data.SQLite;
using System.Collections.Generic;
using BloodBagScanner.Models;
using System;
using Dapper;
using System.Linq;

namespace BloodBagScanner.Services
{
    public class DatabaseService
    {
        private string _connectionString = "Data Source=scanhistory.db;Version=3;";

        public DatabaseService()
        {
            InitializeDatabase();
        }

        public void InitializeDatabase()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                // 创建表的 SQL 语句
                string createTableSql = @"
                CREATE TABLE IF NOT EXISTS ScanRecords (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Timestamp DATETIME NOT NULL,
                    BagCode1 TEXT NOT NULL,
                    BagCode2 TEXT NOT NULL,
                    IsMatch INTEGER NOT NULL
                );";

                // 执行创建表的 SQL
                connection.Execute(createTableSql);
            }
        }

        public void InsertRecord(ScanRecord record)
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    var sql = "INSERT INTO ScanRecords (Timestamp, BagCode1, BagCode2, IsMatch) VALUES (@Timestamp, @BagCode1, @BagCode2, @IsMatch)";
                    connection.Execute(sql, record);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting record: {ex.Message}");
            }
        }

        public List<ScanRecord> QueryRecordsByDate(DateTime date)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                var sql = "SELECT * FROM ScanRecords WHERE strftime('%Y-%m-%d', Timestamp) = @Date";
                return connection.Query<ScanRecord>(sql, new { Date = date.ToString("yyyy-MM-dd") }).ToList();
            }
        }

        public List<ScanRecord> GetAllRecords()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var sql = "SELECT * FROM ScanRecords ORDER BY Timestamp DESC";  // 按时间降序排列
                var records = connection.Query<ScanRecord>(sql).ToList();
                return records;
            }
        }

        public List<ScanRecord> GetScanRecordsByBarcode(string barcode)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var sql = "SELECT * FROM ScanRecords WHERE BagCode1 LIKE @Barcode OR BagCode2 LIKE @Barcode ORDER BY Timestamp DESC";
                return connection.Query<ScanRecord>(sql, new { Barcode = "%" + barcode + "%" }).AsList();
            }
        }
    }
}
