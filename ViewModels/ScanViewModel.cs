using BloodBagScanner.Models;
using BloodBagScanner.Services;
using HandyControl.Controls;
using HandyControl.Tools.Command;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using TextBox = System.Windows.Controls.TextBox;

namespace BloodBagScanner.ViewModels
{
    public class ScanViewModel : Screen
    {
        public ScanViewModel(DatabaseService db)
        {
            _db = db;
            DisplayName = "扫码";
        }

        private string _bagCode1;
        private string _bagCode2;
        private string _verificationResult;
        private TextBox _bagCode1TextBox;
        private TextBox _bagCode2TextBox;

        private readonly DatabaseService _db;

        public void HandleEnter(string textBoxName)
        {
            if (textBoxName == "BagCode1" && !string.IsNullOrEmpty(BagCode1))
            {
                _bagCode2TextBox?.Focus();
            }
            else if (textBoxName == "BagCode2" && !string.IsNullOrEmpty(BagCode2))
            {
                Verify();
            }
        }

        public string BagCode1
        {
            get { return _bagCode1; }
            set
            {
                var cleaned = value?.TrimStart('=').Trim();
                if (_bagCode1 != cleaned)
                {
                    _bagCode1 = cleaned;
                    NotifyOfPropertyChange(() => BagCode1);
                }
            }
        }

        public string BagCode2
        {
            get { return _bagCode2; }
            set
            {
                var cleaned = value?.TrimStart('=').Trim();
                if (_bagCode2 != cleaned)
                {
                    _bagCode2 = cleaned;
                    NotifyOfPropertyChange(() => BagCode2);
                }
            }
        }

        public string VerificationResult
        {
            get { return _verificationResult; }
            set { SetAndNotify(ref _verificationResult, value); }
        }

        // 核对命令
        public void Verify()
        {
            VerificationResult = string.Empty;
            bool isMatch = false;

            if (string.IsNullOrEmpty(BagCode1) || string.IsNullOrEmpty(BagCode2))
            {
                VerificationResult = "请输入所有条码";
            }
            else if (BagCode1.Length < 15 || BagCode2.Length < 15)
            {
                VerificationResult = "条码长度过短";
            }
            else
            {
                var prefix1 = BagCode1.Substring(0, 13);
                var prefix2 = BagCode2.Substring(0, 13);

                isMatch = prefix1 == prefix2;
                VerificationResult = isMatch ? "成功" : "失败";
            }

            _db.InsertRecord(new ScanRecord
            {
                BagCode1 = BagCode1,
                BagCode2 = BagCode2,
                IsMatch = isMatch,
                Timestamp = DateTime.Now
            });
        }

        public void Clear()
        {
            BagCode1 = string.Empty;
            BagCode2 = string.Empty;
            VerificationResult = string.Empty;
            _bagCode1TextBox?.Focus();
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();
            var view = this.View as UserControl;
            if (view != null)
            {
                _bagCode1TextBox = view.FindName("BagCode1TextBox") as TextBox;
                _bagCode2TextBox = view.FindName("BagCode2TextBox") as TextBox;
                _bagCode1TextBox?.Focus();
            }
        }
    }
}