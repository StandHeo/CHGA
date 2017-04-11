using Prism.Commands;
using Prism.Mvvm;
using Pvirtech.Framework.Common;
using Pvirtech.Model;
using Pvirtech.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Pvirtech.Modules.NormalAlarm.ViewModels.PopUp
{
    public class UCAlarmTypeViewModel: BindableBase
    {
        public AddTicklingControlViewModel.GetSeletecdWord getSeletecdWord;
       
        public UCAlarmTypeViewModel() 
        {
            Words = new ObservableCollection<string>();
            this.okJieAn = new DelegateCommand<object>(OkCommandClick);
        }

        #region 方法

        /// <summary>
        /// 根据选择的值更新回复集合
        /// </summary>
        public string UpWordsByNo(string bjlb,string bjlx,string bjxl)
        {
            string word = "";
            Words.Clear();
            foreach (var item in GetAlarmReply.GetWordByNo(bjlb, bjlx, bjxl))
            {
                Words.Add(item);
            }
            if (Words.Count > 0)
            {
                SelectWord = Words[0];
                OnPropertyChanged("Words");
                word = words[0];
            }
            return word;
        }

        private void OkCommandClick(object obj)
        {
            if (obj == null || string.IsNullOrEmpty(obj.ToString()))
            {
                return;
            }
            SelectWord = obj.ToString();
        }

        /// <summary>
        /// 清空报警细类
        /// </summary>
        public void BjxlDictClear()
        {
            if (BjxlDict != null)
                BjxlDict = new ObservableCollection<DictItem>();
        }

        /// <summary>
        /// 获取报警类型
        /// </summary>
        /// <param name="obj"></param>
        private void GetBjlxByBjlbId(DictItem obj)
        {
            #region 参数验证

            if (obj == null)
            {
                EventCenter.PublishError("参数不能为空");
                return;
            }

            #endregion

            BjlxDict.Clear();
            List<DictItem> GetBjlxs = LocalDataCenter.GetBjlx().Where(M => M.superCode == obj.code).ToList();
            if (GetBjlxs != null && GetBjlxs.Count > 0)
            {
                DictItem addItem;
                foreach (var item in GetBjlxs)
                {
                    addItem = new DictItem();
                    UtilsHelper.CopyEntity(addItem, item);
                    BjlxDict.Add(addItem);
                }
            }
            DictItem d = new DictItem();
            d.code = "-1";
            d.note = "-请选择-";
            BjlxDict.Insert(0, d);
            SelectBjlx = BjlxDict[0];
        }


        /// <summary>
        /// 匹配快捷语句
        /// </summary>
        /// <param name="obj"></param>
        public void GetKjhfWord(DictItem obj)
        {
            #region 参数验证

            if (obj == null)
            {
                EventCenter.PublishError("参数不能为空");
                return;
            }
           
            #endregion

            Words.Clear();
            foreach (var item in GetAlarmReply.GetWordByNo(obj.code))
            {
                Words.Add(item);
            }
            SelectWord = Words[0];
        }


        #endregion


        #region 字段/参数


        private ObservableCollection<string> words;

        /// <summary>
        /// 快捷回复集合
        /// </summary>
        public ObservableCollection<string> Words
        {
            get { return words; }
            set 
            { 
                words = value;
                OnPropertyChanged(() => Words);
            }
        }

        #region 当前选中的对象

        private string selectWord;

        public string SelectWord
        {
            get { return selectWord; }
            set
            {
                selectWord = value;
                OnPropertyChanged("SelectWord");
            }
        }


        private DictItem _SelectBjlb;

        /// <summary>
        /// 当前选中的类别
        /// </summary>
        public DictItem SelectBjlb
        {
            get { return _SelectBjlb; }
            set
            {
                _SelectBjlb = value;
                OnPropertyChanged(() => SelectBjlb);
                GetBjlxByBjlbId(SelectBjlb);
                GetKjhfWord(SelectBjlb);
                BjxlDictClear();
            }
        }

        private DictItem _SelectBjlx;

        /// <summary>
        /// 当前选中的类型
        /// </summary>
        public DictItem SelectBjlx
        {
            get { return _SelectBjlx; }
            set
            {
                if (value != null)
                {
                    _SelectBjlx = value;
                    BjxlDictClear();
                    if (value.code != "-1")
                    {
                        GetBjxlByBjlbId(SelectBjlx);
                        GetKjhfWord(SelectBjlx);
                    }
                    OnPropertyChanged(() => SelectBjlx);
                }
            }
        }

        /// <summary>
        /// 加载报警细类
        /// </summary>
        /// <param name="bjlxNumber"></param>
        public void GetBjxlByBjlbId(DictItem obj)
        {
            #region 参数验证

            if (obj == null)
            {
                EventCenter.PublishError("参数不能为空");
                return;
            }

            #endregion

            BjxlDict.Clear();
            List<DictItem> GetBjxls = LocalDataCenter.GetBjxl().Where(M => M.superCode == obj.code).ToList();
            if (GetBjxls != null && GetBjxls.Count > 0)
            {
                DictItem addItem;
                foreach (var item in GetBjxls)
                {
                    addItem = new DictItem();
                    UtilsHelper.CopyEntity(addItem, item);
                    BjxlDict.Add(addItem);
                }
               
            }
            DictItem d = new DictItem();
                d.code = "-1";
                d.note = "-请选择-";
                BjxlDict.Insert(0, d);
                SelectBjxl = BjxlDict[0];
        }

        private DictItem _SelectBjxl;

        /// <summary>
        /// 当前选中的细类
        /// </summary>
        public DictItem SelectBjxl
        {
            get { return _SelectBjxl; }
            set
            {
                if (value != null)
                {
                    _SelectBjxl = value;
                    if (value.code != null && value.code != "-1")
                        GetKjhfWord(SelectBjlx);
                    else
                        OnPropertyChanged(() => SelectBjxl);
                }
            }
        }

        #endregion


        /// <summary>
        /// 报警类别
        /// </summary>
        private ObservableCollection<DictItem> bjlbDict;
       
        public ObservableCollection<DictItem> BjlbDict
        {
            get { return bjlbDict; }
            set
            { 
                bjlbDict = value;
                OnPropertyChanged(() => BjlbDict);
            }
        }


        private ObservableCollection<DictItem> _BjlxDict;

        /// <summary>
        /// 报警类型
        /// </summary>
        public ObservableCollection<DictItem> BjlxDict
        {
            get { return _BjlxDict; }
            set
            {
                if (_BjlxDict != value)
                {
                    _BjlxDict = value;
                    OnPropertyChanged(() => BjlxDict);
                }
               // _BjlxDict = value;
            }
        }

        private ObservableCollection<DictItem> _BjxlDict;

        /// <summary>
        /// 报警细类
        /// </summary>
        public ObservableCollection<DictItem> BjxlDict
        {
            get { return _BjxlDict; }
            set
            {
                _BjxlDict = value;
                OnPropertyChanged(() => BjxlDict);
            }
        }

        private ICommand okJieAn;
        public ICommand OkJieAn
        {
            get { return okJieAn; }
        }

        #endregion

    }
}
