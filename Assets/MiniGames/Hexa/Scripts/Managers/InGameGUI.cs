using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// Script attached to gui manager which manages the in game gui
/// </summary>
namespace MadFireOn
{
    public class InGameGUI : MonoBehaviour
    {
        public static InGameGUI instance;

        //public string iOSURL = "";
        //public string ANDROIDURL = "";
        public Button pauseBtn, resumeBtn, homeBtn, soundBtn;//, aboutUsBtn, rateBtn;
        [Header("Game Over Panel")]//game over panel buttons
        public Button homeBtn2;
        public Button retryBtn;//, rewardAdsBtn;
        public Text gameOverScore, gameOverBestScore;
        public Image soundImage;
        public Sprite[] soundSprite; // 1 is on and 0 is off
        public string mainMenu;
        public GameObject pausePanel, gameOverPanel;
        public Text score, bestScore, points;
        [HideInInspector]
        public bool gamePause = false;

        private AudioSource sound;

        void Awake()
        {
            if (instance == null)
                instance = this;
        }

        // Use this for initialization
        void Start()
        {
            //this line of code make the default button of android  invisible
            //*Important google feature requirement
            //make full screen in game scene
            Screen.fullScreen = true;

            sound = GetComponent<AudioSource>();
            score.text = "0"; //at start the score must be zero
            bestScore.text = "" + GameManager.instance.bestScore;
            points.text = "" + GameManager.instance.points;
            //at start of game scene we want game over false
            GameManager.instance.gameOver = false;

            if (GameManager.instance.isMusicOn == true)
            {
                AudioListener.volume = 1;
                soundImage.sprite = soundSprite[1];
            }
            else
            {
                AudioListener.volume = 0;
                soundImage.sprite = soundSprite[0];
            }

            pauseBtn.GetComponent<Button>().onClick.AddListener(() => { PauseBtn(); });     //pause
            resumeBtn.GetComponent<Button>().onClick.AddListener(() => { ResumeBtn(); });   //resume
            homeBtn.GetComponent<Button>().onClick.AddListener(() => { HomeBtn(); });       //home
            soundBtn.GetComponent<Button>().onClick.AddListener(() => { SoundBtn(); });     //sound
            //aboutUsBtn.GetComponent<Button>().onClick.AddListener(() => { AboutUsBtn(); }); //about us
            //rateBtn.GetComponent<Button>().onClick.AddListener(() => { RateBtn(); });       //rate
            //game over panel
            retryBtn.GetComponent<Button>().onClick.AddListener(() => { RetryBtn(); });   //retry
            homeBtn2.GetComponent<Button>().onClick.AddListener(() => { HomeBtn(); });       //home
            //rewardAdsBtn.GetComponent<Button>().onClick.AddListener(() => { RewardAdsBtn(); }); //reward ads

        }

        // Update is called once per frame
        void Update()
        {
            score.text = "" + GameManager.instance.currentScore;
            points.text = "" + GameManager.instance.points;

            if (GameManager.instance.bestScore <= GameManager.instance.currentScore)
            {
                bestScore.text = "" + GameManager.instance.bestScore;
                GameManager.instance.bestScore = GameManager.instance.currentScore;
                GameManager.instance.Save();
            }

            if (GameManager.instance.gameOver)
            {
                gameOverBestScore.text = "" + GameManager.instance.bestScore;
                gameOverScore.text = "" + GameManager.instance.currentScore;

                GameManager.instance.lastScore = GameManager.instance.currentScore;
                GameManager.instance.Save();
                StartCoroutine(ActivateGameOverPanel());

            }

            //this is for the android default back button *Important google feature requirement
            if (Input.GetKeyDown(KeyCode.Escape) && !gamePause && !GameManager.instance.gameOver)
            {
                gamePause = true;
                pausePanel.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && gamePause && !GameManager.instance.gameOver)
            {
                gamePause = false;
                pausePanel.SetActive(false);
            }

        }

        IEnumerator ActivateGameOverPanel()
        {
            yield return new WaitForSeconds(1f);
            gameOverPanel.SetActive(true);
        }

        void PauseBtn()
        {
            sound.Play();
            pausePanel.SetActive(true);
            Time.timeScale = 0;
            gamePause = true;
        }

        void ResumeBtn()
        {
            sound.Play();
            pausePanel.SetActive(false);
            Time.timeScale = 1;
            gamePause = false;
        }

        void HomeBtn()
        {
            sound.Play();
            Time.timeScale = 1;
            SceneManager.LoadScene(mainMenu);
            gamePause = false;
        }

        void SoundBtn()
        {
            sound.Play();
            if (GameManager.instance.isMusicOn == true)
            {
                GameManager.instance.isMusicOn = false;
                AudioListener.volume = 0;
                soundImage.sprite = soundSprite[0];
                GameManager.instance.Save();
            }
            else
            {
                GameManager.instance.isMusicOn = true;
                AudioListener.volume = 1;
                soundImage.sprite = soundSprite[1];
                GameManager.instance.Save();
            }
        }

        void AboutUsBtn()
        {
            sound.Play();
        }

        //game over panel

        void RetryBtn()
        {

            sound.Play();
            string sceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(sceneName);
            GameManager.instance.currentScore = 0;
        }

        void RewardAdsBtn()
        {
            sound.Play();
            //UnityAds.instance.ShowRewardedAd();         //uncomment after adding respective sdk
            //AdsManager.instance.ShowRewardBasedVideo(); //choos any one to show ad
        }

//        void RateBtn()
//        {
//            sound.Play();
//#if UNITY_IPHONE
//		Application.OpenURL(iOSURL);
//#endif

//#if UNITY_ANDROID
//            Application.OpenURL(ANDROIDURL);
//#endif
        //}

    }
}//namespace