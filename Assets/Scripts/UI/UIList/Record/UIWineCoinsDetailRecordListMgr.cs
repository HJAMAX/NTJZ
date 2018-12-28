
public class UIWineCoinsDetailRecordListMgr : UIGetListManager<StealWineRecord>
{
    public override void GetList()
    {
        if (postData == null)
        {
            
            postData = new PostData[] { new PostData("user_id", GameData.playerData.user_id) };
        }

        gameObject.SetActive(true);
        base.GetList();
    }
}
