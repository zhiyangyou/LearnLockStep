using ZM.ZMAsset;

namespace ZMGC.Battle
{
    /// <summary>
    /// 管理英雄逻辑
    /// </summary>
    public class HeroLogicCtrl : ILogicBehaviour
    {
        #region 属性和字段

        public HeroLogic HeroLogic { get; private set; }

        #endregion

        #region life-cycle

        public void OnCreate()
        {
        }

        public void OnDestroy()
        {
        }

        #endregion

        #region public接口

        public void OnLogicFrameUpdate()
        {
            HeroLogic.OnLogicFrameUpdate();
        }
        
        public void InitHero()
        {
            var goHero = ZMAsset.Instantiate($"{AssetsPathConfig.Game_Hero_Prefabs}/1001.prefab", null);
            var heroRender = goHero.GetComponent<HeroRender>();
            HeroLogic heroLogic = new HeroLogic(1001, heroRender); // TODO 读取配置
            heroRender.SetLogicObject(heroLogic);

            //
            heroLogic.OnCreate();
            heroRender.OnCreate();
            HeroLogic = heroLogic;
        }

        #endregion
    }
}