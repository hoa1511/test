using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SS.View;

public class  GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private float slowMotionTime = 0.1f;
    [SerializeField] public int levelToLoad;

    public int currentLevelIndex;
    public int levelDisplayIndex ;
    private bool isPauseGame = false;
    public bool hasWon;
    public bool hasLoose;
    public bool isWaiting;
    public bool hasReachFinalSkin = false;

    public bool hasPlayLooseGameUI;

    private int numberOfLifeShield;
    
    SoundSettings soundSettings;
    CharacterHolder characterHolder;

    [Header ("Save")]
        public bool isFirstPlay;

    private void Start()
    {
        hasPlayLooseGameUI = false;
        hasLoose = false;
        hasWon = false;
        isWaiting = false;

        PlayerPrefs.SetInt("hasPause", 0);

        int saveLevel = PlayerPrefs.GetInt("level");
        int saveLevelDisplayIndex = PlayerPrefs.GetInt("displayLevel");

        currentLevelIndex = saveLevel < 1 ? 1 : saveLevel;
        levelDisplayIndex = saveLevelDisplayIndex < 1 ? 1 : saveLevelDisplayIndex;

        PlayerPrefs.SetInt("level", currentLevelIndex);
        PlayerPrefs.SetInt("displayLevel", levelDisplayIndex);
        
        Manager.SceneFadeOutDuration = 0.3f;
        Manager.SceneFadeInDuration = 0.3f;

        SetUpNumberOfLifeShield(); 
    }

    public void SetUpNumberOfLifeShield()
    {
        numberOfLifeShield = (int)CharacterStatController.Instance.GetCurrentSkinStat(PassiveStats.LifeShield);
    }

    void Awake()
    {
        Application.targetFrameRate = 60;

        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        int saveLevel = PlayerPrefs.GetInt("level");
        int levelDisplayIndex = PlayerPrefs.GetInt("displayLevel");

        currentLevelIndex = saveLevel < 1 ? 1 : saveLevel;
        levelDisplayIndex = levelDisplayIndex < 1 ? 1 : levelDisplayIndex;

        PlayerPrefs.SetInt("level", currentLevelIndex);
        PlayerPrefs.SetInt("displayLevel", levelDisplayIndex);
    }

    public void StartSlowMotion()
    {
        if(isPauseGame == false)
        {
            Time.timeScale = slowMotionTime;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }
    }

    public void StopSlowMotion()
    {
        if(isPauseGame == false)
        {
            Time.timeScale = 1f;
        }
    }

    public void StartTutorialScene()
    {
        PlayerPrefs.SetInt("hasPause", 1);
        Manager.Add(TutorialSceneController.TUTORIALSCENE_SCENE_NAME);
        PauseGame();
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseGame()
    {
        PauseTimeScale();
        isPauseGame = true;
    }

    public void ResumeGame()
    {
        NormalTimeScale();
        isPauseGame = false;
        PlayerPrefs.SetInt("hasPause", 0);
    }

    public IEnumerator DisPlayLooseUI()
    {
        if(hasWon == false)
        {
            yield return new WaitForSeconds(1.5f);
            hasPlayLooseGameUI = true;
            Manager.Add(LoseGameSceneController.LOSEGAMESCENE_SCENE_NAME);
        }
    }

    public IEnumerator DisPlayWinUI(int expInEachLevel)
    {
        if(hasLoose == false)
        {
            NormalTimeScale();
            yield return new WaitForSeconds(0.5f);     
            Manager.Add(EndGameSceneController.ENDGAMESCENE_SCENE_NAME, expInEachLevel);
        }
    }

    public void ReloadLevel()
    {
        hasWon = false;
        hasLoose = false;
        
        hasPlayLooseGameUI = false;
        string level = PlayerPrefs.GetInt("level").ToString();
        ResetCurentcy();
        Manager.Load("L" + level);
    }

    public void ReLoadCheckPoint()
    { 
        hasWon = false;
        hasLoose = false;
        
        hasPlayLooseGameUI = false;
        ResetCurentcy();

        if(numberOfLifeShield > 0)
        {
            numberOfLifeShield -= 1;
        }
        else
        {
            int displayLevel = PlayerPrefs.GetInt("displayLevel");
        
            int visibleLevel = (displayLevel - 1) / 5 * 5 + 1;
            int levelCurrent = (PlayerPrefs.GetInt("level") - 1) / 5 * 5 + 1;

            if (visibleLevel < 6) 
            {
                visibleLevel = PlayerPrefs.GetInt("displayLevel");
                levelCurrent = PlayerPrefs.GetInt("level");
            }

            PlayerPrefs.SetInt("displayLevel", visibleLevel);
            PlayerPrefs.SetInt("level", levelCurrent);
        }  

        Manager.Load("L" + PlayerPrefs.GetInt("level"));
    }

    public void LoadScene(int level)
    {   
        if(hasWon == true)
        {
            LoadNextLevel();
        }
        else
        {
            Manager.Load("L" + level);
        }
    }

    public int GenerateRandomNumber(int min, int max)
    {
        return Random.Range(min, max + 1);
    }


    public void LoadNextLevel()
    {
        QuestManager.Instance.DailyQuestController.DoDailyQuest(QuestType.Clear);

        hasWon = false;
        hasLoose = false;

        currentLevelIndex = PlayerPrefs.GetInt("level");
        levelDisplayIndex = PlayerPrefs.GetInt("displayLevel");

        currentLevelIndex++;

        levelDisplayIndex++;

        QuestManager.Instance.AchievementController.DoAchievementReachLevel(levelDisplayIndex);

        if(currentLevelIndex >= 25)
        {
            PlayerPrefs.SetInt("hasPlay30Round", 1);
            currentLevelIndex = 2;
        }

        ResetCurentcy();

        Manager.Load("L" + currentLevelIndex);
        SetLevel("level", currentLevelIndex);
        SetLevel("displayLevel", levelDisplayIndex);

        NormalTimeScale();
    }

    public void SetLevel(string KeyName, int Value)
    {
        PlayerPrefs.SetInt(KeyName, Value);
    }

    public int GetLevel(string KeyName)
    {
        return PlayerPrefs.GetInt(KeyName);
    }
    public void LoadSettingScene()
    {
        PlayerPrefs.SetInt("hasPause", 1);
        Manager.Add(SettingSceneController.SETTINGSCENE_SCENE_NAME);
        PauseGame();
    }

    public void NormalTimeScale()
    {
        Time.timeScale = 1;
        isPauseGame = false;
    }

    public void PauseTimeScale()
    {
        Time.timeScale = 0;
    }

    private void ResetCurentcy()
    {
        Bank.Instance.NumberOfCoinCollect = 0;
        Bank.Instance.EarnBalance = 0;
        PlayerPrefs.SetInt("earnBalance", 0);
        PlayerPrefs.SetInt("numberOfCoinCollect", 0);
    }

}
