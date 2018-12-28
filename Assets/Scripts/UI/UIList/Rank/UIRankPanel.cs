using UnityEngine;

public class UIRankPanel : EventHandler
{
    [SerializeField]
    private Transform[] tags;

    public override void HandleEvent(params object[] data)
    {
        if (data.Length > 0 && data[0].GetType() == typeof(string))
        {
            gameObject.SetActive(true);
            JsonWrapperArray<ChateauData> wrapper = JsonWrapperArray<ChateauData>.FromJson<ChateauData>((string)data[0]);
            
            for(int i = 0; i < wrapper.items.Length && i < tags.Length; i++)
            {
                tags[i].Find("Amount").GetComponent<UILabel>().text = wrapper.items[i].total_wine_stealed;
                tags[i].Find("Name").GetComponent<UILabel>().text = wrapper.items[i].nicheng;
                tags[i].Find("Location").GetComponent<UILabel>().text = wrapper.items[i].MapName;
            }
        }
    }
}
