using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = System.Random;


public class GameLogic : MonoBehaviour
{
    private bool paused = false;

    [SerializeField] private int startwWitchCount = 3;
    [SerializeField] private int startGhostCount = 0;
    [SerializeField] private int startLlaternCount = 10;
    [SerializeField] private int startMonsterCount = 1;

    private int witch;
    private int ghost;
    private int latern;
    private int monster;

    private int deadMonster = 0;
    private int deadGhosts = 0;

    private int level = 1;
    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    /// <summary>
    /// ������� ������� ������� �� ����
    /// </summary>
    private int allLaterns;

    /// <summary>
    /// ������� ������� ���� ������� �� ����
    /// </summary>
    private int allUsedLaterns;

    /// <summary>
    /// ������ ���������� ������, ���� ������� �� �������
    /// </summary>
    private float hunger = 1; 

    [SerializeField] private TextMeshProUGUI resourcesTxt;
    [SerializeField] private TextMeshProUGUI monsterTxt;
    [SerializeField] private TextMeshProUGUI ruleTxtLaternToBy;
    [SerializeField] private TextMeshProUGUI ruleTxtAvtoTimeToBuy;
    [SerializeField] private TextMeshProUGUI ruleTxt_time;


    /// <summary>
    /// ���������� ������
    /// </summary>
    [SerializeField] private TextMeshProUGUI statOfLevelTxt;

    /// <summary>
    /// ����� ����������
    /// </summary>
    [SerializeField] private TextMeshProUGUI statTxt;
    /// <summary>
    /// ����� ������ ������
    /// </summary>
    [SerializeField] private TextMeshProUGUI lvlTxt;

    [SerializeField] private Timer witchTimer;
    [SerializeField] private Timer ghostTimer;
    //[SerializeField] private ImageChanger pausa;

    /// <summary>
    /// ������� ������� ���������� ������
    /// </summary>
    private int witchMadeLatern = 2;

    /// <summary>
    /// ������� ������� ������� ����������
    /// </summary>
    private int laternEatGhosts = 2;

    /// <summary>
    /// ��������� ����������
    /// </summary>
    private int laternToBuyGhost = 3;

    /// <summary>
    /// ��������� ������
    /// </summary>
    private int laternToBuyWitch = 1;

    /// <summary>
    /// �������� �������
    /// </summary>
    [SerializeField] private GameObject errorPanel;

    /// <summary>
    /// ������ ������ ������
    /// </summary>
    [SerializeField] private GameObject winLevelPanel;

    /// <summary>
    /// ������ ������ ����
    /// </summary>
    [SerializeField] private GameObject winPanel;

    /// <summary>
    /// ������ ���������
    /// </summary>
    [SerializeField] private GameObject losePanel;

    /// <summary>
    /// �������� �������
    /// </summary>
    [SerializeField] private GameObject errorTimePanel;

    /// <summary>
    /// ����� ��������� ������� �������
    /// </summary>
    [SerializeField] private TextMeshProUGUI time;

    /// <summary>
    /// ������������ ������
    /// </summary>
    [SerializeField] private float timeStart = 20;


    public void Start()
    {
        PauseGame();
        witch = startwWitchCount;
        ghost = startGhostCount;
        latern = startLlaternCount;
        monster = startMonsterCount;
        UpdateTxt();
        ruleTxtLaternToBy.text = $"���������� ���� ������: {laternEatGhosts} ��. � ���. \n������ ������� ������: {witchMadeLatern} ��.� ���";
        ruleTxtAvtoTimeToBuy.text = $"������ ����������: {ghostTimer.MaxTime} ���. \n������ ������: {witchTimer.MaxTime} ���.";
        ruleTxt_time.text = $"������� ��� ����� \n{timeStart} ��������.";
    }

    /// <summary>
    /// ����� ��� ���������� ���� � ��������
    /// </summary>
    public void UpdateTxt()
    {
        resourcesTxt.text = $"�����: {witch.ToString()} \n����������: {ghost.ToString()} \n�������: {latern.ToString()}";
        monsterTxt.text = "�����  � ��������� \n������: " + monster.ToString();
        lvlTxt.text = level.ToString();

        statTxt.text = $"����� ��������: {deadMonster} \n������� �������: {allLaterns} \n������� ���������: {allUsedLaterns}\n \n���� ������������ ����������: {deadGhosts} \n\n����� ��������: {level - 1} \n\n�� �������� �������� \n�����: {13 - level}";
    }


    private void Update()
    {        
        timeStart -= Time.deltaTime;
        if (timeStart <= 0)
        {
            EndLevel();
            War();
            PauseGame();
            timeStart = 20;
            //timeStart = 10 + 20*(level/4);
            UpdateTxt();
        }
        time.text = Mathf.Round(timeStart).ToString();

        if (level == 13)
        {
            winPanel.SetActive(true);
            this.enabled = false;
        }            
    }

    /// <summary>
    /// ���� ����������
    /// </summary>
    public void BuyGhost()
    {
        if ((latern - laternToBuyGhost) < 0)
        {
            ghostTimer.Stop();
            PauseGame();
            errorPanel.SetActive(true);
        }
        else if ((latern - laternToBuyGhost) >= 0 && timeStart > ghostTimer.MaxTime)
        {
            ghostTimer.Restart();
            allUsedLaterns += laternToBuyGhost;
            latern -= laternToBuyGhost;
            UpdateTxt();
        }
        else 
        {
            ghostTimer.Stop();
            PauseGame();
            errorTimePanel.SetActive(true);
        }
    }

    public void GetGhost()
    {
        ghost += 1;
        UpdateTxt();
        ghostTimer.Stop();
    }

    /// <summary>
    /// ���� �����
    /// </summary>
    public void BuyWitch()
    {
        if ((latern - laternToBuyWitch) < 0)
        {
            witchTimer.Stop();
            PauseGame();
            errorPanel.SetActive(true);
        }
        else if ((latern - laternToBuyWitch) >= 0 && timeStart > witchTimer.MaxTime)
        {
            witchTimer.Restart();
            allUsedLaterns += laternToBuyWitch;
            latern -= laternToBuyWitch;
            UpdateTxt();
        }
        else 
        {
            PauseGame();
            witchTimer.Stop();
            errorTimePanel.SetActive(true);
        }
    }

    public void GetWitch()
    {
        witch += 1;
        UpdateTxt();
        witchTimer.Stop();
    }

    /// <summary>
    /// ������� ������ ������ � �������� ������� ����� �������
    /// </summary>
    public void EndLevel()
    {
        ghostTimer.Stop();
        witchTimer.Stop();
        allLaterns += witchMadeLatern * witch;
        allUsedLaterns += laternEatGhosts * ghost;
        latern = latern + (witchMadeLatern * witch) - (laternEatGhosts * ghost);
        statOfLevelTxt.text = $"�� ������ �������� ������������ {12 - level} �. \n�� ������� ���: \n����� ��������: {monster} \n���������� ��������� �������: {laternEatGhosts * ghost}. \n������ ������� �������: {witchMadeLatern * witch}"; 
    }

   
    /// <summary>
    /// ����� ���������� �� �����
    /// </summary>
    public void War()
    {
        ghost = (int)((ghost * hunger) - monster);

        if (ghost >= 0)
        {
            deadGhosts += monster;
            deadMonster += monster;
            UpdateTxt();
            winLevelPanel.SetActive(true);
            level += 1;
            lvlTxt.text = level.ToString();
            monster = NextMonster();
            monsterTxt.text = "���� ��������: " + monster.ToString();
        }
        else
        {
            deadGhosts = deadGhosts + Math.Abs(ghost);
            losePanel.SetActive(true);
            UpdateTxt();
        }

        if (latern > 0)
        {
            hunger = 1;
        }
        else
        { hunger = 0.5f; 
        }
    }

    /// <summary>
    /// �����, ������� ������ ���� �� �����
    /// </summary>
    public void PauseGame()
    {
        if (paused)
        {
            Time.timeScale = 1;
            //pausa.SpriteChang();
        }
        else
        {
            Time.timeScale = 0;
            //pausa.SpriteChang();
        }
        paused = !paused;
    }

    /// <summary>
    /// ����� ��� �������� �������� � ��������� ������
    /// </summary>
    /// <returns>���������� ��������</returns>
    private int NextMonster()
    {
        var rand = new Random();
        if (level <= 5)
        {
            return rand.Next((int)(level / 2), (int)level);
        }            
        return rand.Next((int)(level / 2), 10);       
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        ghostTimer.Stop();
        witchTimer.Stop();
        witch = startwWitchCount;
        ghost = startGhostCount;
        latern = startLlaternCount;
        monster = startMonsterCount;

        deadMonster = 0;
        deadGhosts = 0;
        level = 1;
        UpdateTxt();
        winLevelPanel.SetActive(false);
        timeStart = 20;
        enabled = true;
    }

}
