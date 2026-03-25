using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JailExtensionPurchase : PurchaseTargetBase
{
    [SerializeField] private GameObject jailExtension;

    public override bool CanPurchase()
    {
        if (!base.CanPurchase())
            return false;
        if (jailExtension == null)
            return false;

        return true;
    }

    protected override void ApplyPurchase()
    {
        if (!CanPurchase())
            return;

        jailExtension.SetActive(true);
    }
}
