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
    /// сколько сделано фонарей за игру
    /// </summary>
    private int allLaterns;

    /// <summary>
    /// сколько фонарей было съедено за игру
    /// </summary>
    private int allUsedLaterns;

    /// <summary>
    /// делает приведения слабей, если пшеницы не хватает
    /// </summary>
    private float hunger = 1; 

    [SerializeField] private TextMeshProUGUI resourcesTxt;
    [SerializeField] private TextMeshProUGUI monsterTxt;
    [SerializeField] private TextMeshProUGUI ruleTxtLaternToBy;
    [SerializeField] private TextMeshProUGUI ruleTxtAvtoTimeToBuy;
    [SerializeField] private TextMeshProUGUI ruleTxt_time;


    /// <summary>
    /// статистика уровня
    /// </summary>
    [SerializeField] private TextMeshProUGUI statOfLevelTxt;

    /// <summary>
    /// общая статистика
    /// </summary>
    [SerializeField] private TextMeshProUGUI statTxt;
    /// <summary>
    /// вывод номера уровня
    /// </summary>
    [SerializeField] private TextMeshProUGUI lvlTxt;

    [SerializeField] private Timer witchTimer;
    [SerializeField] private Timer ghostTimer;
    //[SerializeField] private ImageChanger pausa;

    /// <summary>
    /// сколько фонарей производит ведьма
    /// </summary>
    private int witchMadeLatern = 2;

    /// <summary>
    /// сколько фонарей съедает приведение
    /// </summary>
    private int laternEatGhosts = 2;

    /// <summary>
    /// стоимость приведения
    /// </summary>
    private int laternToBuyGhost = 3;

    /// <summary>
    /// стоимость ведьмы
    /// </summary>
    private int laternToBuyWitch = 1;

    /// <summary>
    /// нехватка фонарей
    /// </summary>
    [SerializeField] private GameObject errorPanel;

    /// <summary>
    /// панель победы уровня
    /// </summary>
    [SerializeField] private GameObject winLevelPanel;

    /// <summary>
    /// панель победы игры
    /// </summary>
    [SerializeField] private GameObject winPanel;

    /// <summary>
    /// панель проигрыша
    /// </summary>
    [SerializeField] private GameObject losePanel;

    /// <summary>
    /// нехватка времени
    /// </summary>
    [SerializeField] private GameObject errorTimePanel;

    /// <summary>
    /// вывод обратного отсчета времени
    /// </summary>
    [SerializeField] private TextMeshProUGUI time;

    /// <summary>
    /// длительность уровня
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
        ruleTxtLaternToBy.text = $"Приведения едят фонари: {laternEatGhosts} шт. в час. \nВедьмы создают фонари: {witchMadeLatern} шт.в час";
        ruleTxtAvtoTimeToBuy.text = $"Призыв приведения: {ghostTimer.MaxTime} сек. \nПризыв ведьмы: {witchTimer.MaxTime} сек.";
        ruleTxt_time.text = $"Игровой час равен \n{timeStart} секундам.";
    }

    /// <summary>
    /// метод для обновления инфы о ресурсах
    /// </summary>
    public void UpdateTxt()
    {
        resourcesTxt.text = $"Ведьм: {witch.ToString()} \nПриведений: {ghost.ToString()} \nФонарей: {latern.ToString()}";
        monsterTxt.text = "Детей  в следующем \nраунде: " + monster.ToString();
        lvlTxt.text = level.ToString();

        statTxt.text = $"Детей напугано: {deadMonster} \nФонарей создано: {allLaterns} \nФонарей потрачено: {allUsedLaterns}\n \nДети развоплотили приведений: {deadGhosts} \n\nЧасов пережито: {level - 1} \n\nДо рассвета осталось \nчасов: {13 - level}";
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
    /// найм приведений
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
    /// найм ведьм
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
    /// подсчет итогов уровня и остатков фонарей перед набегом
    /// </summary>
    public void EndLevel()
    {
        ghostTimer.Stop();
        witchTimer.Stop();
        allLaterns += witchMadeLatern * witch;
        allUsedLaterns += laternEatGhosts * ghost;
        latern = latern + (witchMadeLatern * witch) - (laternEatGhosts * ghost);
        statOfLevelTxt.text = $"До заката осталось продержаться {12 - level} ч. \nЗа прошлый час: \nДетей напугано: {monster} \nПриведения поглотили фонарей: {laternEatGhosts * ghost}. \nВедьмы создали фонарей: {witchMadeLatern * witch}"; 
    }

   
    /// <summary>
    /// метод отвечающий за набег
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
            monsterTxt.text = "Ждем монстров: " + monster.ToString();
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
    /// метод, который ставит игру на паузу
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
    /// меотд для рассчета монстров в следующем раунде
    /// </summary>
    /// <returns>количество монстров</returns>
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
