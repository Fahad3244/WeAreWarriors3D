using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class TroopButton : MonoBehaviour
{
    public Sprite unlockedSprite;
    public Sprite lockedSprite;
    
    [ShowIf("showIntVariable")]
    public bool inShop;
    public int troopNmbr;
    public int meatConsume;
    public int purchasePrise;

    [ShowIf("showIntVariable")]
    public bool isDummy;

    private bool showIntVariable = false;

    [Button(ButtonSizes.Large)]
    private void ClickIfIsOnSHop()
    {
        showIntVariable = !showIntVariable;
    }

    private void Start() 
    {
        OnStart();
    }
    
    public void OnStart()
    {
        if(inShop)
        {
            if (!isDummy)
            {
                GetComponent<Button>().onClick.AddListener(() => PurchaseTroop());
                if(Controller.self.troopsManager.IsTroopUnlocked(troopNmbr))
                {
                    this.gameObject.SetActive(false);
                }
            }

            if(Controller.self.currencyManager.GetCurrentCoin() < purchasePrise)
            {
                GetComponent<Image>().sprite = lockedSprite;
            }
            else
            {
                GetComponent<Image>().sprite = unlockedSprite;
            }
        }
        else
        {
            GetComponent<Button>().onClick.AddListener(() => OnButtonClick());
        }
        
        if(troopNmbr == 1)
        {
            Controller.self.troopsManager.UnlockTroop(1);
        }
    }

    public void OnButtonClick()
    {
        Controller.self.meatManager.CutMeat(meatConsume);
    }

    public void PurchaseTroop()
    {
        if(Controller.self.troopsManager.IsTroopUnlocked(troopNmbr))
        {
            return;
        }
        if(Controller.self.currencyManager.GetCurrentCoin() < purchasePrise) return;
        Controller.self.currencyManager.CutCoin(purchasePrise);
        Controller.self.purchaseManager.PurchaseTroop(troopNmbr);
        Controller.self.uiController.upgradeShopPannelView.RefreshUi();
    }
}
