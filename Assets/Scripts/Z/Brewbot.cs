using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
public class Brewbot : MonoBehaviour
{
    [SerializeField]
    private int number;

    public int Number { get { return number; } }

    private BrewbotState state = BrewbotState.NOT_AVAILABLE;

    public BrewbotState State { get { return state; } }

    private static Dictionary<BrewbotState, string> stateMap;

    private TextMesh timerText;

    public int WineStored { get; set; }

    void Awake()
    {
        if(stateMap == null)
        {
            stateMap = new Dictionary<BrewbotState, string>();
            stateMap[BrewbotState.DONE] = "可取酒";
            stateMap[BrewbotState.AVAILABLE] = "空闲";
            stateMap[BrewbotState.NOT_AVAILABLE] = "不可用";
        }
    }

    void Start()
    {
        timerText = transform.Find("Timer").GetComponent<TextMesh>();
    }

    /// <summary>
    /// 更新状态：可取酒，空闲，不可用
    /// </summary>
    /// <param name="brewbotState"></param>
    public void UpdateState(BrewbotState brewbotState)
    {
        state = brewbotState;
        timerText.text = stateMap[state];
    }

    /// <summary>
    /// 更新状态：正在酿酒
    /// 开始倒计时
    /// </summary>
    /// <param name="brewbotState"></param>
    /// <param name="totalSeconds"></param>
    public void UpdateState(BrewbotState brewbotState, int totalSeconds)
    {
        state = brewbotState;
        timerText.GetComponent<UITimeCountDown>().StartCountDown(totalSeconds);
    }
}

