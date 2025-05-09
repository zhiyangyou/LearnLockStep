﻿using System;
using UnityEngine;
using FixMath;
namespace FixIntPhysics
{
 
    public class FixIntBoxCollider :ColliderBehaviour
    {
        private bool mIsFloowTarget;
        //public FixIntVector3 Size;
        /// <summary>
        /// Eidotr Mode 绘制碰撞体范围工具
        /// </summary>
        public  BoxColliderGizmo boxDraw;

        public FixIntBoxCollider(FixIntVector3 size, FixIntVector3 conter)
        {
            this.Size = size;
            this.Conter = conter;
            ColliderType = ColliderType.Box;
        }
        public FixIntBoxCollider(Vector3 size, Vector3 conter)
        {
            this.Size =new FixIntVector3(size);
            this.Conter =new FixIntVector3(conter);
            ColliderType = ColliderType.Box;
        }

        /// <summary>
        /// 更新碰撞体信息
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="size"></param>
        /// <param name="radius"></param>
        public override void UpdateColliderInfo(FixIntVector3 pos, FixIntVector3 size = default, FixInt radius = default)
        {
            // Debug.LogError($"{Time.frameCount}: 更新碰撞盒子位置");
            base.UpdateColliderInfo(pos, size, radius);
            this.Size = size;

            if (this.boxDraw!=null)
            {
                this.boxDraw.transform.position = pos.ToVector3();
            }
        }
        /// <summary>
        /// 更新碰撞体信息
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="size"></param>
        /// <param name="radius"></param>
        public override void UpdateColliderInfo(Vector3 pos, Vector3 size = default, float radius = default)
        {
            this.LogicPosition = new FixIntVector3(pos);
            this.Size = new FixIntVector3(size);

            if (this.boxDraw!=null)
            {
                this.boxDraw.transform.position = pos;
            }
        }

        public override void SetBoxData(FixIntVector3 conter, FixIntVector3 size, bool isFloowTarget = false)
        {
            if (boxDraw == null) {
                var obj = CreateGo();
                obj.name = $"boxCollider:{conter}";
                boxDraw = obj.AddComponent<BoxColliderGizmo>();
                boxDraw.SetBoxData(conter.ToVector3(), size.ToVector3(), mIsFloowTarget);
            }

            mIsFloowTarget = isFloowTarget;
            this.Conter = conter;
            this.Size = size;

            //boxDraw.transform.localScale = this.Conter.x >= 0 ? Vector3.one : new Vector3(-1,1,1);
            boxDraw?.SetBoxData(conter.ToVector3(), size.ToVector3(), mIsFloowTarget);
        }
        
        /// <summary>
        /// 设置碰撞体数据
        /// </summary>
        /// <param name="conter">中心点</param>
        /// <param name="size">宽度</param>
        /// <param name="isFloowTarget">碰撞体绘制是否跟随</param>
        public override void SetBoxData(Vector3 conter, Vector3 size, bool isFloowTarget = false)
        {
            SetBoxData(new FixIntVector3(conter), new FixIntVector3(size), isFloowTarget);
        }
        public override void OnRelease()
        {
            if (boxDraw != null && boxDraw.gameObject != null)
            {
                GameObject.DestroyImmediate(boxDraw.gameObject);
            }
            boxDraw = null;
        }
 
    }
}

