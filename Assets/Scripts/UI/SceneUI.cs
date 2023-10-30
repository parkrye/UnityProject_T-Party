public abstract class SceneUI : BaseUI
{
    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    public virtual void Initialize()
    {

    }
}