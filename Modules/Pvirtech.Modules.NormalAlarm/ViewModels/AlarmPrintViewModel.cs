
using Prism.Commands;
using Prism.Mvvm;
using Pvirtech.Framework.Common;
using Pvirtech.Framework.Interactivity;
using Pvirtech.Model;
using Pvirtech.Modules.NormalAlarm.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;

namespace Pvirtech.Modules.NormalAlarm.ViewModels
{
    #region 反馈/PDA反馈/警情记录对象

    /// <summary>
    /// 全部反馈
    /// </summary>
    public class AllFk
    {
        private string _Text;
        
        public string Text
        {
            get
            {
                return _Text;
            }

            set
            {
                _Text = value;
            }
        }
    }

    /// <summary>
    /// PDA反馈
    /// </summary>
    public class PDAFK
    {
        private string _Text;

        public string Text
        {
            get
            {
                return _Text;
            }

            set
            {
                _Text = value;
            }
        }
    }

    /// <summary>
    /// 警情记录
    /// </summary>
    public class LogText
    {
        private string _Text;

        public string Text
        {
            get
            {
                return _Text;
            }

            set
            {
                _Text = value;
            }
        }
    }

    #endregion

    public class AlarmPrintViewModel : BindableBase, IConfirmation, IInteractionRequestAware
    {
        public string ThisTemplateUrl = "";
        public AlarmPrintViewModel(CompletedAlarmInfoViewModel alarm)
        {
            FlowDocument = new FlowDocument();
            ViewModel = alarm;
            LoadPrintPage();
            IsShowPDAFanKui = true;
            IsShowPDAJieAn = true;
            PrintDate = DateTime.Now.ToString("yyyy年MM月dd日");
            printPage = new DelegateCommand<object>(PrintThisPage);
            PDATickling = Visibility.Visible;
            PDACloseCase = Visibility.Visible;
        }

        private string aa { get; set; }

        /// <summary>
        /// 打印当前页面
        /// </summary>
        /// <param name="obj"></param>
        private void PrintThisPage(object obj)
        {
            MAlarmPrint.PrintPageClick(this,FlowDocument);
        }

        private void LoadPrintPage()
        {
            FormatAlarmLog();
            SelectXAMLAndLoad();
        }

        /// <summary>
        /// 格式化记录/反馈信息
        /// </summary>
        private void FormatAlarmLog()
        {
            if (ViewModel == null)
            {
                return;
            }
            #region 格式化PDA反馈信息/全部反馈

            if (ViewModel.FeedBacks != null && ViewModel.FeedBacks.Count > 0)
            {
                StringBuilder pdafk = new StringBuilder();
                StringBuilder allfk = new StringBuilder();
                foreach (var item in ViewModel.FeedBacks.OrderBy(O => O.Fksj))
                {
                    string sjtext = "-  -";
                    if (!string.IsNullOrEmpty(item.Fksj))
                        sjtext = DateTime.ParseExact(item.Fksj, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture).ToString("yyyy:MM:dd HH:mm:ss");
                    if (item.PcOrPDA == 1)
                        pdafk.AppendFormat("【反馈单位】" + item.Cjdwmz + "【反馈时间】" + sjtext + ":" + item.Fknr + "\r\n");
                    else
                        allfk.AppendFormat("【反馈单位】" + item.Cjdwmz + "【反馈时间】" + sjtext + ":" + item.Fknr + "\r\n");
                }
                PDABackTexts = pdafk.ToString();
                AllFanKuis = allfk.ToString();
            }


            #endregion

            #region 格式化记录

            if (ViewModel.OperRecord != null && ViewModel.OperRecord.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in ViewModel.OperRecord.OrderBy(O => O.StartTime))
                {
                    string sjtext = "- -";
                    if (!string.IsNullOrEmpty(item.StartTime))
                        sjtext = DateTime.ParseExact(item.StartTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture).ToString("yyyy-MM-dd HH:mm:ss");
                    AlarmLogStatus alarmStatus = (AlarmLogStatus)item.Dzlx;
                    sb.AppendFormat("[处警人]" + item.StartName + "[指派单位]" + item.TargetName +"[时间]"+ sjtext + "[内容]:" + alarmStatus.ToString()+ "\r\n");
                }
                LogTexts = sb.ToString();
            }

            #endregion
        }

        /// <summary>
        /// 加载用户选择的XAML文件
        /// </summary>
        public void SelectXAMLAndLoad()
        {
            FlowDocument = null;
            if (IsShowPDAJieAn == false && IsShowPDAFanKui == false)
            {
                ThisTemplateUrl = MAlarmPrint.AlarmXml.Items.FirstOrDefault(M => M.Key == "CaseFlowDocumentInfo").Url;
            }
            else if (IsShowPDAJieAn == true && IsShowPDAFanKui == true)
            {
                ThisTemplateUrl = MAlarmPrint.AlarmXml.Items.FirstOrDefault(M => M.Key == "CaseFlowDocument").Url;
            }
            else if (IsShowPDAJieAn == true && IsShowPDAFanKui == false)
            {
                ThisTemplateUrl = MAlarmPrint.AlarmXml.Items.FirstOrDefault(M => M.Key == "CaseFlowDocumentPDAJieAn").Url;
            }
            else if (IsShowPDAJieAn == false & IsShowPDAFanKui == true)
            {
                ThisTemplateUrl = MAlarmPrint.AlarmXml.Items.FirstOrDefault(M => M.Key == "CaseFlowDocumentPDAFk").Url;
            }
            LoadXaml(ThisTemplateUrl);
            OnPropertyChanged("LogTexts");
            OnPropertyChanged("FlowDocument");
        }

        /// <summary>
        /// 加载XAML
        /// </summary>
        /// <param name="url"></param>
        public void LoadXaml(string url)
        {
            Stream stream = XmlHelper.GetXmlStream(url);
            FlowDocument = XamlReader.Load(stream) as FlowDocument;
            stream.Close();
        }

        private ICommand printPage;
        
        public string Title { get; set; }

        public object Content { get; set; }

        private Visibility pDATickling;
        
        /// <summary>
        /// 巡组反馈信息
        /// </summary>
        public Visibility PDATickling
        {
            get { return pDATickling; }
            set
            {
                pDATickling = value;
                OnPropertyChanged(() => PDATickling);
            }
        }

        private Visibility pDACloseCase;

        /// <summary>
        /// 巡组结案信息
        /// </summary>
        public Visibility PDACloseCase
        {
            get { return pDACloseCase; }
            set
            {
                pDACloseCase = value;
                OnPropertyChanged(() => PDACloseCase);
            }
        }

        public CompletedAlarmInfoViewModel ViewModel { get; set; }

        public INotification Notification
        {
            get;
            set;
        }

        public Action FinishInteraction
        {
            get;
            set;
        }
        public Action Activate { get; set; }
        public bool Confirmed
        {
            get;
            set;
        }
        public bool Topmost { get; set; }
        public System.Windows.ResizeMode ResizeMode { get; set; }
        public WindowState WindowState { get; set; }
        public bool IsModal { get; set; }
        
        private FlowDocument flowDocument;

        private bool isShowPDAFanKui;

        private bool isShowPDAJieAn;

        public FlowDocument FlowDocument
        {
            get
            {
                return flowDocument;
            }

            set
            {
                flowDocument = value;
                OnPropertyChanged("FlowDocument");
            }
        }

        private string pDABackTexts;

        /// <summary>
        /// PDA反馈信息
        /// </summary>
        public string PDABackTexts
        {
            get
            {
                return pDABackTexts;
            }
            set
            {
                pDABackTexts = value;
                OnPropertyChanged(() => PDABackTexts);
            }
        }

        private string _LogTexts;

        public string LogTexts
        {
            get
            {
                return _LogTexts;
            }
            set
            {
                _LogTexts = value;
                OnPropertyChanged("LogTexts");
            }
        }

        private string _AllFanKuis;

        /// <summary>
        /// 全部反馈信息
        /// </summary>
        public string AllFanKuis
        {
            get
            {
                return _AllFanKuis;
            }

            set
            {
                _AllFanKuis = value;
                OnPropertyChanged(() => AllFanKuis);
            }
        }

        private string pDAJieAn;
            
        /// <summary>
        /// PDA结案
        /// </summary>
        public string PDAJieAn
        {
            get
            {
                return pDAJieAn;
            }

            set
            {
                pDAJieAn = value;
            }
        }


        public ICommand PrintPage
        {
            get
            {
                return printPage;
            }

            set
            {
                printPage = value;
            }
        }

        /// <summary>
        /// 打印日期
        /// </summary>
        public string PrintDate
        {
            get
            {
                return _PrintDate;
            }
            set
            {
                _PrintDate = value;
            }
        }
        
        /// <summary>
        /// 是否显示PDA反馈
        /// </summary>
        public bool IsShowPDAFanKui
        {
            get
            {
                return isShowPDAFanKui;
            }

            set
            {
                isShowPDAFanKui = value;
                SelectXAMLAndLoad();
                OnPropertyChanged("IsShowPDAFanKui");
            }
        }

        /// <summary>
        /// 是否显示PDA结案描述
        /// </summary>
        public bool IsShowPDAJieAn
        {
            get
            {
                return isShowPDAJieAn;
            }

            set
            {
                isShowPDAJieAn = value;
                SelectXAMLAndLoad();
                OnPropertyChanged("IsShowPDAJieAn");
            }
        }
       

        private string _PrintDate;
    }
}
