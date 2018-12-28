using UnityEngine;
using System.Collections;
/// <summary>
/// Script for the hexa object
/// </summary>
namespace MadFireOn
{
    public class PlayerHexa : MonoBehaviour
    {

        private Rigidbody2D mybody;
        // Use this for initialization
        void Start()
        {
            mybody = GetComponent<Rigidbody2D>();
            FollowHexa.instance.PlayerSettings();
        }

        // Update is called once per frame
        void Update()
        {
            //when the hexa object falls of the blocks and goes beyond limits game is over
            if (transform.position.x <= -2.5 || transform.position.x >= 2.5)
            {
                GameManager.instance.gameOver = true;
                StartCoroutine(DeactivateGravity()); //and after some time its gravity is deactivated

                if (GameManager.instance.sceneName != "Hexa_MainGame")
                {
                    print(GameManager.instance.sceneName);
                    LevelGuiManager.instance.levelFailed = true;
                }

            }

        }

        IEnumerator DeactivateGravity()
        {
            yield return new WaitForSeconds(2f);
            mybody.isKinematic = true;
        }

        //void OnCollisionEnter2D(Collision2D other)
        //{
            
        //    if (other.gameObject.tag == "Base")
        //    {
        //        if (GameManager.instance.sceneName != "Hexa_MainGame")
        //        {
        //            LevelGuiManager.instance.levelComplete = true;
        //        }
        //    }
        //}

    }
}//namespace