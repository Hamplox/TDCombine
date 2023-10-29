public class BasicTower : Tower
{
    public override void Init()
    {
        UnityEngine.Debug.Log("I am a Basic Tower");
    }
    public override bool Execute(float aDeltaTime)
    {
        // Returns true when the action is done
        return true;
    }
}
