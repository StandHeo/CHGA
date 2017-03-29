using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pvirtech.Model
{
    public class CarInfoItem
    {
        private string carNo;
        /// <summary>
        /// 车辆编号
        /// </summary>
        public string CarNo
        {
            get { return carNo; }
            set { carNo = value; }
        }

        /**车辆类型*/
        private string carType;

        public string CarType
        {
            get { return carType; }
            set { carType = value; }
        }

        /**车牌*/
        private string carPlate;

        public string CarPlate
        {
            get { return carPlate; }
            set { carPlate = value; }
        }

        /**车辆名字(暂时没用)*/
        private string carName;

        public string CarName
        {
            get { return carName; }
            set { carName = value; }
        }

        /**车辆呼号*/
        private string carCallNo;

        public string CarCallNo
        {
            get { return carCallNo; }
            set { carCallNo = value; }
        }

        /**车辆负责人编号*/
        private string carHolderNo;

        public string CarHolderNo
        {
            get { return carHolderNo; }
            set { carHolderNo = value; }
        }

        /**车辆负责人名字*/
        private string carHolderName;

        public string CarHolderName
        {
            get { return carHolderName; }
            set { carHolderName = value; }
        }
        /**车辆状况（1：可用  0：不可用）*/
        public string Clzk { get; set; }

        /**视频状况（1：可用  0：不可用）*/
        public string Spzk { get; set; }

        /**GPS状况（1：可用  0：不可用）*/
        public string Gpszk { get; set; }

        /**车载电台状况（1：可用  0：不可用）*/
        public string Czdtzk { get; set; }
        /**
         * 车辆备注信息 
         */
        private string carRemark;

        public string CarRemark
        {
            get { return carRemark; }
            set { carRemark = value; }
        }

    }
}
