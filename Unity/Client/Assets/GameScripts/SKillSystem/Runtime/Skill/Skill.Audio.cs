public partial class Skill {
    public void OnLogicFrameUpdate_Audio() {
        var audioList = _skillConfigSo.audioList;
        if (audioList != null && audioList.Count > 0) {
            foreach (SkillConfig_Audio audioConfig in audioList) {
                if (audioConfig.skillAudio == null) {
                    continue;
                }
                if (audioConfig.triggerFrame == _curLogicFrame) {
                    // TODO 播放音效
                    AudioController.GetInstance().PlaySoundByAudioClip(audioConfig.skillAudio, audioConfig.isLoop, AudioPriorityConfig.Skill_Self_AudioClip);
                }

                // 循环音效达到结束帧
                if (audioConfig.isLoop && audioConfig.endFrame == _curLogicFrame) {
                    // TODO 停止当前音效
                    AudioController.GetInstance().StopSound(audioConfig.skillAudio);
                }
            }
        }
    }


    /// <summary>
    /// 播放击中音效
    /// </summary>
    public void PlayHitAudio() {
        AudioController.GetInstance().PlaySoundByAudioClip(_skillConfigSo.skillCfg.SkillHitAudio, false, AudioPriorityConfig.Skill_Hit_AudioClip);
    }
}