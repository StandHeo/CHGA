using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pvirtech.Model
{
    /// <summary>
    /// 巡组类库
    /// </summary>
    public class Patrol
    {
        /**实时表字段*/
        public string Id{get;set;}

        /**实时表字段*/
        public string TempId{get;set;}

        /**报备单位编号*/
        public string GroupNo{get;set;}

        /**报备单位名字*/
        public string GroupName{get;set;}

        /**班次序号*/
        public string ClassNum{get;set;}

        /** 班次名字*/
        public string ClassName{get;set;}

        /**电台呼号*/
        public string CallNo{get;set;}

        /**单兵设备编号*/
        public string AppNo{get;set;}

        /**单兵是否在线0：未在线  1：在线*/
        public int AppStatus{get;set;}
        
        //车辆在线
        public int CarStatus { get; set; }

        /**值班人数*/
        public int PoliceNum{get;set;}

        /**备勤人数*/
        public int PrePoliceNum{get;set;}

        /**总警力*/
        public int TotalPoliceNum{get;set;}

        /**固定执勤组数量*/
        public int DutyGroupNum{get;set;}

        /**备勤组数量*/
        public int PreGroupNum{get;set;}

        /**装备数量*/
        public int EquipNum{get;set;}

        /**巡逻区域编号*/
        public string PatrolAreaNo{get;set;}

        /**巡逻区域名字*/
        public string PatrolAreaName{get;set;}

        /**车辆数量*/
        public string CarNum{get;set;}
        //车牌号
        public string CarPlate { get; set; }
        

        /**值班领导名字*/
        public string LeaderName{get;set;}

        /**值班人员名字*/
        public string MemberName{get;set;}

        /**报备类型 1：实时报备  2：预报备*/
        public int ReportType{get;set;}

        /**报备方式 1：PC  2：单兵*/
        public int ReportWay{get;set;}

        /**巡组在线状态:0：未在线  1：在线，2：闲，3：忙*/
        public int GroupStatus{get;set;}

        /**人员列表*/
        public List<UserInfoItem> UserList{get;set;}

        /**车辆详情列表*/
        public List<CarInfoItem> CarList{get;set;}

        /**枪械详情列表*/
        public List<EquipInfoItem> GunList{get;set;}
        public int GunNum { get; set; }

        /**枪械详情列表*/
        public List<EquipInfoItem> OtherDeviceList{get;set;}

        /**开始时间*/
        public string BeginTime{get;set;}
        /**结束时间*/
        public string EndTime{get;set;}

        /**报备组织类型
         *  1001：分局指挥中心
            1002：科室
            1003：业务大队
            1004：派出所
            2001：执勤组
         */
        public int GroupType{get;set;}

        /**0:不属于执勤组   1：巡组    2：快反   3：机动  */
        public int PoliceGroupType{get;set;}

        /**上级部门编号*/
        public string ParentNo{get;set;}

        /**上级部门名字*/
        public string ParentName{get;set;}

        /**报备其他内容*/
        public string Other{get;set;}

        /**值班电话*/
        public string DutyCallNum{get;set;}

        /**频道号*/
        public string ChannelNo{get;set;}

        /**报备标志：addGroup:新建巡组   addClass：添加班次     upstringClass:修改班次 */
        public string Flag{get;set;}

        /**当日报备状态  0：未报备   1：已报备*/
        public int Status{get;set;}

        /**报备时间*/
        public string ReportTime{get;set;}

        /**值班领导编号*/
        public string UserLeaderNo{get;set;}

        /**值班领导信息*/
        public string UserLeaderInfo{get;set;}

        public override string ToString()
        {
            return GroupName;
        }
    }
}
