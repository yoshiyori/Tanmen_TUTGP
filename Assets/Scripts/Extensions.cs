using System.Linq;
using UnityEngine;

public static class Extensions
{
    /// <summary>
    /// GetComponentsInChildrenの自分以外だけ版
    /// </summary>
    public static T[] GetComponentsInChildrenWithoutSelf<T>(this GameObject self) where T : Component
    {
        return self.GetComponentsInChildren<T>().Where(c => self != c.gameObject).ToArray();
    }
}