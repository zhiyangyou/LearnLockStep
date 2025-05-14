public partial class Skill {
    private void OnLogicFrameUpdate_Buff() {
        var listBuff = _skillConfigSo.buffCfgList;
        if (listBuff != null && listBuff.Count > 0) {
            for (int i = 0; i < listBuff.Count; i++) {
                var buffConfig = listBuff[i];
                if (buffConfig.triggerFrame == _curLogicFrame) {
                    BuffSystem.Instance.AttachBuff(
                        buffConfig.buffID, _skillCreater, _skillCreater, this, null);
                }
            }
        }
    }
}