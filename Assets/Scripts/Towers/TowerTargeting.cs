using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

public struct Target
{
    public UnityEngine.Vector3 targetPos;
    public Test_Enemy enemy;
    public Target(Test_Enemy aEnemy, UnityEngine.Vector3 aTargetPos)
    {
        enemy = aEnemy;
        targetPos = aTargetPos;
    }
}

public class TowerTargeting
{
    public enum TargetingType
    {
        First,
        Last,
        Strongest,
        Weakest,
        Closest
    }    
    
    public Target? GetTarget(TowerSkeleton aTower, List<Target> someTargets)
    {
        if(someTargets.Count == 0) return null;
        switch (aTower.TargetingType)
        {
            case TargetingType.First:
                return SingleTarget(aTower, someTargets);
            case TargetingType.Last:
                return LastTarget(aTower, someTargets);
            case TargetingType.Strongest:
                return StrongestTarget(aTower, someTargets);
            case TargetingType.Weakest:
                return WeakestTarget(aTower, someTargets);
            case TargetingType.Closest:
                return ClosestTarget(aTower, someTargets);
        }
        return null;
    }

    private Target SingleTarget(TowerSkeleton aTower, List<Target> someTargets)
    {
        return someTargets.First();
    }
    private Target LastTarget(TowerSkeleton aTower, List<Target> someTargets)
    {
        return someTargets.Last();
    }
    private Target StrongestTarget(TowerSkeleton aTower, List<Target> someTargets)
    {
        return someTargets.OrderBy(target => target.enemy.Health).Last();
    }
    private Target WeakestTarget(TowerSkeleton aTower, List<Target> someTargets)
    {
        return someTargets.OrderBy(target => target.enemy.Health).First();
    }
    private Target ClosestTarget(TowerSkeleton aTower, List<Target> someTargets)
    {
        // Some Proximity base Bullshit, to lazy for that now -- Peter
        return someTargets[0];
    }

}
