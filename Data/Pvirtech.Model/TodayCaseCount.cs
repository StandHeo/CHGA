using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pvirtech.Model
{
    public class TodayCaseCount
    {
        private string pname;

        public string Pname
        {
            get
            {
                return pname;
            }

            set
            {
                pname = value;
            }
        }

        public string Pno
        {
            get
            {
                return pno;
            }

            set
            {
                pno = value;
            }
        }

        public int TypeCount
        {
            get
            {
                return typeCount;
            }

            set
            {
                typeCount = value;
            }
        }

        public string TypeNo
        {
            get
            {
                return typeNo;
            }

            set
            {
                typeNo = value;
            }
        }

        public string TypeName
        {
            get
            {
                return typeName;
            }

            set
            {
                typeName = value;
            }
        }

        private string pno;

        private int typeCount;

        private string typeNo;

        private string typeName;

        //private int _Count;

        //public int Count
        //{
        //    get { return _Count; }
        //    set
        //    {
        //        _Count = value;
        //    }
        //}

        //private string _PoliceName;

        //public string PoliceName
        //{
        //    get { return _PoliceName; }
        //    set
        //    {
        //        _PoliceName = value;
        //    }
        //}
        //private string _PoliceNo;

        //public string PoliceNo
        //{
        //    get { return _PoliceNo; }
        //    set
        //    {
        //        _PoliceNo = value;
        //    }
        //}
        //private IList<PopeTrendData> _PopeTrendData;

        //public IList<PopeTrendData> PopeTrendData
        //{
        //    get { return _PopeTrendData; }
        //    set
        //    {
        //        _PopeTrendData = value;
        //    }
        //}

        //private int _XingShiNum;

        ///// <summary>
        ///// 刑事案件
        ///// </summary>
        //public int XingShiNum
        //{
        //    get { return _XingShiNum; }
        //    set
        //    {
        //        _XingShiNum = value;
        //    }
        //}

        //private int _DaoQieNum;

        ///// <summary>
        ///// 盗窃
        ///// </summary>
        //public int DaoQieNum
        //{
        //    get { return _DaoQieNum; }
        //    set
        //    {
        //        _DaoQieNum = value;
        //    }
        //}

        //private int _XingZhengNum;

        ///// <summary>
        /////行政
        ///// </summary>
        //public int XingZhengNum
        //{
        //    get { return _XingZhengNum; }
        //    set
        //    {
        //        _XingZhengNum = value;
        //    }
        //}

        //private int _QiTaNum;

        ///// <summary>
        ///// 其他
        ///// </summary>
        //public int QiTaNum
        //{
        //    get { return _QiTaNum; }
        //    set
        //    {
        //        _QiTaNum = value;
        //    }
        //}
    }
    /// <summary>
    /// 类别（行政,求助等）对应数量
    /// </summary>
    //public class PopeTrendData
    //{
    //    public string Name { get; set; }
    //    public int Num { get; set; }
    //    public int TypeNo { get; set; }
    //}
}
