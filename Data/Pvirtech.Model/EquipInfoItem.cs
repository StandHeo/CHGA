using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pvirtech.Model
{
    public class EquipInfoItem
    {

        /**装备类型*/
        private string equipType;

        public string EquipType
        {
            get { return equipType; }
            set { equipType = value; }
        }

        /**装备编号*/
        private string equipNo;

        public string EquipNo
        {
            get { return equipNo; }
            set { equipNo = value; }
        }

        /**装备名字*/
        private string equipName;

        public string EquipName
        {
            get { return equipName; }
            set { equipName = value; }
        }

        /**装备数量*/
        private string equipNum;

        public string EquipNum
        {
            get { return equipNum; }
            set { equipNum = value; }
        }
        /**子弹数量*/
        public string Bullent { get; set; }

        private string holderNo;
        /// <summary>
        /// 持有人编号
        /// </summary>
        public string HolderNo
        {
            get { return holderNo; }
            set { holderNo = value; }
        }

        
        private string holderName;
        /// <summary>
        /// 持有人名字(暂时没用)
        /// </summary>
        public string HolderName
        {
            get { return holderName; }
            set { holderName = value; }
        }

        
        private string equipDesc;
        /// <summary>
        /// /**装备描述*/
        /// </summary>
        public string EquipDesc
        {
            get { return equipDesc; }
            set { equipDesc = value; }
        }

    }
}
