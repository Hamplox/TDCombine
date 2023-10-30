using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSkeleton : MonoBehaviour
{
    public TowerScriptableObject ScriptableObject;
    public TowerTargeting.TargetingType TargetingType;


    private Tower FirstTower;
    private Tower SecondTower = null;
    private List<Target> TargetList = new List<Target>(); // Will be Enemies main Inheirited Variable instead of Target


    private Target? Target; // Question mark means nullable (allowed to be null). Avoid if possible because it takes X + 8 bits size. Does it matter, Probably not
    private TowerTargeting TowerTargeting;

    private void Start()
    {
        TowerTargeting = new TowerTargeting();
        TargetList = new List<Target>()
        {
            new Target(new Test_Enemy(), new Vector3()),
            new Target(new Test_Enemy(), new Vector3(0,0,1)),
            new Target(new Test_Enemy(), new Vector3(0,0,2)),
            new Target(new Test_Enemy(), new Vector3(0,0,3)),
            new Target(new Test_Enemy(), new Vector3(0,0,4))
        };
        switch (ScriptableObject.TowerType)
        {
            case TowerType.Basic:
                FirstTower = new BasicTower();
                break;
            case TowerType.Cannon:
                FirstTower = new CannonTower();
                break;
            case TowerType.Multi:
                FirstTower = new MultiTower();
                break;
            default:
                break;
        }

        FirstTower.Init();
    }

    private void Update()
    {
        Target = TowerTargeting.GetTarget(this, TargetList);
        FirstTower.Execute(Time.deltaTime);
    }
    private IEnumerator TowerShot() 
    {
        yield return true;
    }
}
