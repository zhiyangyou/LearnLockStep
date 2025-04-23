using ZM.ZMAsset;

namespace ZMGC.Battle
{
    /// <summary>
    /// 管理英雄逻辑
    /// </summary>
    public class HeroLogicCtrl : ILogicBehaviour
    {
        #region life-cycle

        public void OnCreate()
        {
        }

        public void OnDestroy()
        {
        }

        #endregion

        #region public接口

        public void InitHero()
        {
            var goHero = ZMAsset.Instantiate($"{AssetsPathConfig.Game_Hero_Prefabs}/1000.prefab", null);
            
        }

        #endregion
    }
}