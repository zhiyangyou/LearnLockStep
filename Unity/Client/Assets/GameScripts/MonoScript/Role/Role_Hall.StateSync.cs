using System;
using System.Numerics;
using Fantasy;
using ServerShareToClient;
using UnityEngine;
using ZMGC.Hall;
using Vector3 = UnityEngine.Vector3;

public partial class Role_Hall {
    #region 属性和字段

    private HallRoleLogicCtrl _hallRoleLogicCtrl = null;

    private int _syncStateCount = 0; // 同步计数

    /// <summary>
    /// 同步的目标位置
    /// </summary>
    private Vector3 _syncTargetPos;

    /// <summary>
    /// 同步的方向
    /// </summary>
    private Vector3 _syncDir;

    private Vector3 _lastInput;


    private long _syncPackID = 0;

    #endregion


    #region private

    private void FixedUpdate() {
        if (!_moveActive) {
            return;
        }
        _syncStateCount++;
        if (_syncStateCount == GameConstConfig.MaxSyncStateCount) {
            _syncStateCount = 0;

            _syncPackID++;

            // 状态没有变化,那么不同步
            bool inputDirHasChanged = !(_lastInput.Equals(Vector3.zero)
                                        && _inputDir.Equals(Vector3.zero));
            var accountID = HallWorld.GetExitsDataMgr<UserDataMgr>().account_id;
            // Debug.LogError($"inputDirHasChanged:{inputDirHasChanged} {_lastInput} {_inputDir}");
            if (inputDirHasChanged) {
                StateSyncData syncData = new StateSyncData() {
                    input_dir = _inputDir.ToCSVector3(),
                    role_id = _roleID,
                    player_id = accountID,
                    
                };
                _hallRoleLogicCtrl.SyncRoleState(syncData, _syncPackID);
                _lastInput = _inputDir;
            }
            else {
                // Debug.LogError("状态没有变化,那么不同步");
            }
        }
    }

    #endregion

    #region public

    public void SyncPosition(CSVector3 pos, CSVector3 dir) {
        _syncTargetPos = pos.ToVector3();
        _syncDir = dir.ToVector3();
        // _inputDir = _syncDir;
    }

    #endregion
}