using UnityEngine;

public class UIAmount : EventHandler
{
    public int increment;

    [SerializeField]
    private int boostIncrement;

    private int pileUp = 0;

    /// <summary>
    /// 增量按钮
    /// </summary>
    [SerializeField]
    private GameObject icrButton;

    /// <summary>
    /// 减量按钮
    /// </summary>
    [SerializeField]
    private GameObject desButton;

    [SerializeField]
    private UILabel amountLabel;

    [SerializeField]
    private UIPayment paymentLabel;

    [SerializeField]
    private string upLimitName;

    [SerializeField]
    private int upLimit = 50000;

    /// <summary>
    /// 相隔x秒再进行操作
    /// </summary>
    [SerializeField]
    private float timeGap;
    
    private float curTime = 0.0f;

    public override void HandleEvent(params object[] data)
    {
        object upLimitObj = GameData.GetField(upLimitName);
        if (upLimitObj != null)
        {
            upLimit = int.Parse(upLimitObj.ToString());
        }
    }

    void Update()
    {
        //连续增加
        if(Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Stationary &&
                (UICamera.IsPressed(icrButton) || UICamera.IsPressed(desButton)))
            {
                curTime += Time.deltaTime;
                if (curTime > timeGap)
                {
                    pileUp++;
                    int quantity = pileUp > 2 ? boostIncrement : increment;

                    if (UICamera.IsPressed(icrButton))
                        Increase(quantity);
                    else
                        Decrease(quantity);

                    curTime = 0.0f;
                }
            }
            else if(touch.phase == TouchPhase.Ended)
            {
                pileUp = 0;
            }
        }
    }

    public void SetIncrement(string number)
    {
        increment = int.Parse(number);
    }

    public void Reset()
    {
        amountLabel.text = "0";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string GetAmountStr()
    {
        return amountLabel.text;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="quantity"></param>
    public void Increase(int quantity)
    {
        int amount = int.Parse(amountLabel.text) + quantity;
        amount = amount > upLimit ? upLimit : amount;
        amountLabel.text = amount.ToString();

        if (paymentLabel)
        {
            paymentLabel.SetTotalPrice(amount, upLimit);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="quantity"></param>
    public void Decrease(int quantity)
    {
        int amount = int.Parse(amountLabel.text) - quantity;
        amount = amount > 0 ? amount : 0;
        amountLabel.text = amount.ToString();

        if (paymentLabel)
        {
            paymentLabel.SetTotalPrice(amount, upLimit);
        }
    }
}
