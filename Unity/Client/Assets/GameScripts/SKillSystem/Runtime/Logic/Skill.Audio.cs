public partial class Skill {
    public void OnLogicFrameUpdate_Audio() {
        var audioList = _skillData.audioList;
        if (audioList != null && audioList.Count > 0) {
            foreach (SkillAudioConfig audioConfig in audioList) {
                if (audioConfig.triggerFrame == _curLogicFrame) {
                    // TODO 播放音效
                }

                // 循环音效达到结束帧
                if (audioConfig.isLoop && audioConfig.endFrame == _curLogicFrame) {
                    // TODO 停止当前音效
                }
            }
        }
    }
}