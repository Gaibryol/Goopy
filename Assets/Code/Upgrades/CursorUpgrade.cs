using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Cursor Upgrade")]
public class CursorUpgrade : Upgrade
{
    [SerializeField] private int increaseFactor; 
    public override IEnumerator Apply()
    {
        for (int i = 0; i < increaseFactor; i++)
        {
            CursorController.Instance.IncrementCursorSize();
        }
        yield return 0;
    }
}
