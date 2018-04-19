using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum gameStatus
{
    next, play, gameover, win
}

public class GameManager : Singleton<GameManager> {

    //Serialized Fields
    [SerializeField]
    private int totalWaves = 10;
    [SerializeField]
    private Text totalMoneyLbl;
    [SerializeField]
    private Text currentWaveLbl;
    [SerializeField]
    private GameObject spawnPoint;
    [SerializeField]
    private GameObject[] enemies;
    [SerializeField]
    private int totalEnemies = 5;
    [SerializeField]
    private int enemiesToSpawn;
    [SerializeField]
    private Text playBtnLbl;
    [SerializeField]
    private Button playBtn;
    [SerializeField]
    private Text totalEscapedLbl;

    private int waveNumber;
    private int totalMoney = 10;
    private int totalEscaped = 0;
    private int roundEscaped = 0;
    private int totalKilled = 0;
    private int whichEnemiestoSpawn = 0;
    private gameStatus currentState = gameStatus.play;

    //NonSerialized Variables
    const float spawnDelay = 0.5f;

    //List Instantiation
    public List<Enemy> EnemyList = new List<Enemy>();

    //Getter/Setters
    public int TotalMoney
    {
        get
        {
            return totalMoney;
        }
        set
        {
            totalMoney = value;
            totalMoneyLbl.text = totalMoney.ToString();
        }
    }

    public int TotalEscaped
    {
        get
        {
            return totalEscaped;
        }
        set
        {
            totalEscaped = value;
        }
    }

    public int RoundEscaped
    {
        get
        {
            return roundEscaped;
        }
        set
        {
            roundEscaped = value;
        }
    }

    public int TotalKilled
    {
        get
        {
            return totalKilled;
        }
        set
        {
            totalKilled = value;
        }
    }



	// Use this for initialization
	void Start () {
        //StartCoroutine(spawn());
        playBtn.gameObject.SetActive(false);
        showMenu();
	}

    void Update()
    {
        handleEscape();
    }

    //IEnumerators
    IEnumerator spawn()
    {
        if (enemiesToSpawn > 0 && EnemyList.Count < totalEnemies)
        {
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                if (EnemyList.Count < totalEnemies)
                {
                    GameObject newEnemy = Instantiate(enemies[0]) as GameObject;
                    newEnemy.transform.position = spawnPoint.transform.position;                }
            }
            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(spawn());
        }
    }

    //Void Functions
    public void RegisterEnemy(Enemy enemy)
    {
        EnemyList.Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy)
    {
        EnemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    public void DestroyAllEnemies()
    {
        foreach(Enemy enemy in EnemyList)
        {
            Destroy(enemy.gameObject);
        }
        EnemyList.Clear();
    }

    public void addMoney(int amount)
    {
        TotalMoney += amount;
    }

    public void subtractMoney(int amount)
    {
        TotalMoney -= amount;
    }

    public void isWaveOver()
    {
        totalEscapedLbl.text = "Escaped: " + TotalEscaped + "/10";
        if((RoundEscaped + TotalKilled) == totalEnemies)
        {

            setCurrentGameState();
            showMenu();
        }
    }

    public void setCurrentGameState()
    {
        if (TotalEscaped >= 10)
        {
            currentState = gameStatus.gameover;
        } else if(waveNumber == 0 && (TotalKilled + RoundEscaped) == 0){
            currentState = gameStatus.play;
        } else if(waveNumber >= totalWaves)
        {
            currentState = gameStatus.win;
        }
        else
        {
            currentState = gameStatus.next;
        }
    }

    public void showMenu()
    {
        switch (currentState)
        {
            case gameStatus.gameover:
                playBtnLbl.text = "Play Again?";
                break;

            case gameStatus.next:
                playBtnLbl.text = "Next Wave";
                break;
            case gameStatus.play:
                playBtnLbl.text = "Play";
                break;
            case gameStatus.win:
                playBtnLbl.text = "Play";
                break;

        }
        playBtn.gameObject.SetActive(true);
    }

    public void playBtnPressed()
    {
        switch (currentState)
        {
            case gameStatus.next:
                waveNumber += 1;
                totalEnemies += waveNumber;
                break;
            default:
                totalEnemies = 3;
                //DEFAULT number of enemies to Spawn: Changeable.
                TotalEscaped = 0;
                TotalMoney = 10;
                TowerManager.Instance.DestroyAllTower();
                TowerManager.Instance.RenameTagsBuildSite();
                totalMoneyLbl.text = TotalMoney.ToString();
                totalEscapedLbl.text = "Escaped: " + TotalEscaped + "/10";
                break;
        }

        DestroyAllEnemies();
        TotalKilled = 0;
        RoundEscaped = 0;
        currentWaveLbl.text = "Wave: " + (waveNumber + 1);
        StartCoroutine(spawn());
        playBtn.gameObject.SetActive(false);
    }

    private void handleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TowerManager.Instance.disableSpriteDrag();
            TowerManager.Instance.towerButtonPressed = null;
        }
    }

}
