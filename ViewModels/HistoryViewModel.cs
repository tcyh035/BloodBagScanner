using Stylet;
using System.Collections.Generic;
using System.Linq;
using BloodBagScanner.Models;
using System.Threading.Tasks;
using BloodBagScanner.Services;
using System;
using System.Data;
using HandyControl.Controls;

namespace BloodBagScanner.ViewModels
{
    public class HistoryViewModel : Screen
    {
        private readonly DatabaseService _db;

        public string Barcode { get; set; }

        public BindableCollection<ScanRecord> ScanRecords { get; set; }

        private DateTime? _selectedDate;

        public DateTime? SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                QueryDatabaseByDate(_selectedDate.Value);
                NotifyOfPropertyChange(() => SelectedDate);
            }
        }

        private void QueryDatabaseByDate(DateTime date)
        {
            var records = _db.QueryRecordsByDate(date);
            ScanRecords.Clear();
            foreach (var record in records)
            {
                ScanRecords.Add(record);
            }
        }

        public HistoryViewModel(DatabaseService db)
        {
            DisplayName = "核对历史";
            _db = db;
            ScanRecords = new BindableCollection<ScanRecord>();
            _selectedDate = DateTime.Now.Date;
        }

        protected override void OnActivate()
        {
            Barcode = string.Empty;
            base.OnActivate();
            ShowAll();
        }

        public void Search()
        {
            if (!string.IsNullOrEmpty(Barcode))
            {
                var records = _db.GetScanRecordsByBarcode(Barcode);
                ScanRecords.Clear();
                foreach (var record in records)
                {
                    ScanRecords.Add(record);
                }
            }
            else
            {
                // 如果没有输入条码，查询所有记录
                ShowAll();
            }
        }

        public void ShowAll()
        {
            var allRecords = _db.GetAllRecords();

            // 只取最近 50 条（按时间倒序）
            var latest = allRecords
                .Take(50)
                .ToList();

            ScanRecords.Clear();
            foreach (var record in latest)
                ScanRecords.Add(record);
        }
    }
}
