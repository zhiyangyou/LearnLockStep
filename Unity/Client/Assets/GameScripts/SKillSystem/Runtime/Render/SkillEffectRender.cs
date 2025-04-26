using UnityEngine;

public class SkillEffectRender : RenderObject
{
    #region override

    protected override void Update()
    {
        base.Update();
    }

    public override void OnRelease()
    {
        base.OnRelease();
        GameObject.Destroy(gameObject);
    }

    #endregion
}