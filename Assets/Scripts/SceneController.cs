using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Класс предназначен для управления сценой игры.
/// Обрабатывает логику паузы игры, перезапуска и подсчета побед каждого игрока.
/// </summary>
public class SceneController : MonoBehaviour
{
    // Флаг, указывающий, приостановлена ли игра
    public static bool isGamePaused = false;

    // Количество побед игрока 1
    private static int player1Wins = 0;

    // Количество побед игрока 2
    private static int player2Wins = 0;

    // UI элемент для отображения побед игрока 1
    public TextMeshProUGUI player1WinsUIText;

    // UI элемент для отображения побед игрока 2
    public TextMeshProUGUI player2WinsUIText;

    // UI элемент для отображения подсказки о перезапуске
    public GameObject restartHintUIText;

    /// <summary>
    /// Завершает игру, определяя победителя и обновляя счет побед.
    /// Деактивирует всех игроков и устанавливает флаг паузы.
    /// </summary>
    /// <param name="playerObjToLose">Объект игрока, который проиграл.</param>
    public static void FinishGame(GameObject playerObjToLose)
    {
        List<GameObject> players = GameObject.FindGameObjectsWithTag("Player").ToList();
        foreach (GameObject player in players)
        {
            player.SetActive(false);
        }
        isGamePaused = true;

        if (playerObjToLose.name == "Player_1")
        {
            player2Wins++;
        }
        else
        {
            player1Wins++;
        }
    }

    /// <summary>
    /// Перезапускает текущую сцену, начиная игру заново.
    /// Сбрасывает флаг паузы.
    /// </summary>
    public static void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        isGamePaused = false;
    }

    /// <summary>
    /// Метод Awake вызывается при инициализации объекта.
    /// Устанавливает текст UI элементов, отображающих количество побед каждого игрока.
    /// </summary>
    private void Awake()
    {
        player1WinsUIText.text = "Wins: " + player1Wins;
        player2WinsUIText.text = "Wins: " + player2Wins;
    }

    /// <summary>
    /// Метод Update вызывается на каждом кадре.
    /// Обновляет состояние подсказки о перезапуске и обрабатывает ввод для перезапуска игры.
    /// </summary>
    private void Update()
    {
        restartHintUIText.SetActive(isGamePaused);
        if (isGamePaused && Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
    }
}