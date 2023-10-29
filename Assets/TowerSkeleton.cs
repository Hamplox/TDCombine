using System.Collections;
using UnityEngine;

public class TowerSkeleton : MonoBehaviour
{
    public TowerScriptableObject ScriptableObject;

    private Tower FirstTower;
    private Tower SecondTower = null;

    private void Start()
    {
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
        FirstTower.Execute(Time.deltaTime);
    }
    private IEnumerator TowerShot() 
    {
        yield return true;
    }
}
