/*--------------------------------------------------------------------------------------
* Title: 数据脚本自动生成工具
* Author: 铸梦xy
* Date:2024/12/25 21:53:46
* Description:数据层,主要负责游戏数据的存储、更新和获取
* Modify:
* 注意:以下文件为自动生成，强制再次生成将会覆盖
----------------------------------------------------------------------------------------*/

using System.Collections.Generic;

namespace ZMGC.Battle {
    /// <summary>
    /// 英雄相关的数据
    /// </summary>
    public class HeroDataMgr : IDataBehaviour {
        /// <summary>
        /// 普通攻击技能
        /// </summary>
        public Dictionary<int, int[]> heroNormalSkillCfgDic = new Dictionary<int, int[]>() {
            { HeroIDConfig.HeroID_鬼剑士, new int[] { 1001, 1002, 1003 } }, // 鬼剑士
            { HeroIDConfig.HeroID_神枪手, new int[] { 3001, 3002, 3003, 3004,  } }, // 神枪手
        };

        /// <summary>
        /// 普通技能
        /// </summary>
        public Dictionary<int, int[]> heroSkillCfgDic = new() {
            {
                HeroIDConfig.HeroID_鬼剑士, new int[] {
                    1004, // 上挑
                    1005, // 破军升龙击1段
                    // 1006, // 破军升龙击2段
                    1007, // 猛龙
                    1008, // 幻影剑舞一段
                    1010, // 拔刀-前摇
                    1015, // 觉醒1段
                }
            }, {
                HeroIDConfig.HeroID_神枪手, new int[] {
                    3015
                }
            }
        };

        public void OnCreate() { }

        public void OnDestroy() { }

        /// <summary>
        /// 普通攻击技能列表
        /// </summary>
        /// <param name="heroID"></param>
        /// <returns></returns>
        public int[] GetHeroNormalSkillIDs(int heroID) {
            return heroNormalSkillCfgDic[heroID];
        }


        /// <summary>
        /// 普通技能列表
        /// </summary>
        /// <param name="heroID"></param>
        /// <returns></returns>
        public int[] GetHeroSkillIDs(int heroID) {
            return heroSkillCfgDic[heroID];
        }
    }
}