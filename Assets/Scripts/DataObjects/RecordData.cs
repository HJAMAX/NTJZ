using System;

[Serializable]
public struct StealWineRecord
{
    public string exchange_time;

    public string wine_get;

    public string coins_get;

    public string coins_left;

    public string payment;

    public string loser_name;

    public string comment;
}


[Serializable]
public struct GoldDetailRecord
{
    public string exchange_time;

    public string cause_of_change;

    public string add_num;

    public string reduce_num;

    public string remaining_sum;
}

