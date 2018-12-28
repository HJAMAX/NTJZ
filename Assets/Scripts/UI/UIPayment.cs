using UnityEngine;

public class UIPayment : MonoBehaviour
{
    private UILabel paymentLabel;

    private int price;

    void OnEnable()
    {
        paymentLabel = GetComponent<UILabel>();
    }

    public void Reset()
    {
        paymentLabel.text = "0";
    }

    /// <summary>
    /// 设置单价
    /// </summary>
    /// <param name="p"></param>
    public void SetPrice(string p)
    {
        price = int.Parse(p);
    }

    /// <summary>
    /// 总价不能大于上限
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="upLimit"></param>
    /// <returns></returns>
    public bool SetTotalPrice(int amount, int upLimit)
    {
        int totalPrice = price * amount;

        if (totalPrice <= upLimit)
        {
            paymentLabel.text = (price * amount).ToString();
            return true;
        }

        return false;
    }
}
