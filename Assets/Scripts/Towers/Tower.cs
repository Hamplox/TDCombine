using UnityEngine;

[System.Serializable]
public abstract class Tower
{
    // These Classes will only be used for behaviour of the towers. IE how their projectials will interact with the world
    [SerializeField]
    protected GameObject Target;
    public abstract void Init();
    public abstract bool Execute(float aDeltaTime);
}
