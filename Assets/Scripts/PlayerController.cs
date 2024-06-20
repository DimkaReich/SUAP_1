using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Класс предназначен для управления движением игрока и установкой бомб.
/// Обрабатывает ввод с клавиатуры и изменяет состояние игрока в соответствии с ним.
/// </summary>
public class PlayerController : MonoBehaviour
{
    // Кнопки, отвечающие за движение вверх
    [SerializeField] public KeyCode inputUp;

    // Кнопки, отвечающие за движение вниз
    [SerializeField] public KeyCode inputDown;

    // Кнопки, отвечающие за движение влево
    [SerializeField] public KeyCode inputLeft;

    // Кнопки, отвечающие за движение вправо
    [SerializeField] public KeyCode inputRight;

    // Кнопка, отвечающая за установку бомбы
    [SerializeField] public KeyCode inputPlaceBomb;

    // Скорость игрока
    [SerializeField] public float speed;

    // Физическая часть игрока
    private Rigidbody2D rigidbody;

    // Позиция и поворот игрока
    private Transform playerTransform;

    // Направление, куда идет игрок
    private Vector2 currentMovementDirection;

    // Контроллер, отвечающий за установку бомб
    private BombController playerBombController;

    /// <summary>
    /// Метод Start вызывается при инициализации объекта.
    /// Инициализирует компоненты Rigidbody2D, Transform и BombController.
    /// </summary>
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        playerTransform = GetComponent<Transform>();
        playerBombController = GetComponent<BombController>();
    }

    /// <summary>
    /// Метод Update вызывается на каждом кадре.
    /// Обрабатывает ввод с клавиатуры для управления движением и установкой бомб.
    /// </summary>
    void Update()
    {
        float xInput = 0f;
        float yInput = 0f;

        // Движение игрока
        if (Input.GetKey(inputUp))
        {
            yInput += 1;
        }
        if (Input.GetKey(inputDown))
        {
            yInput -= 1;
        }
        if (Input.GetKey(inputRight))
        {
            xInput += 1;
        }
        if (Input.GetKey(inputLeft))
        {
            xInput -= 1;
        }
        if (Input.GetKeyDown(inputPlaceBomb))
        {
            playerBombController.PlaceBomb();
        }
        playerBombController.PlaceBombIndicator();

        currentMovementDirection = new Vector2(xInput, yInput);
        SetSpriteDirection(xInput, yInput);
    }

    /// <summary>
    /// Расчет поворота спрайта персонажа в зависимости от направления движения.
    /// </summary>
    /// <param name="xInput">Горизонтальное направление движения.</param>
    /// <param name="yInput">Вертикальное направление движения.</param>
    private void SetSpriteDirection(float xInput, float yInput)
    {
        if (xInput == 0 && yInput == 0)
        {
            return;
        }

        float angleByHorizontalInput = 90 * (-xInput);
        float angleByVerticalInput = 90 + 90 * (-yInput);

        if (yInput == 0)
        {
            playerTransform.rotation = Quaternion.Euler(playerTransform.rotation.x, playerTransform.rotation.y, angleByHorizontalInput);
            return;
        }
        if (xInput == 0)
        {
            playerTransform.rotation = Quaternion.Euler(playerTransform.rotation.x, playerTransform.rotation.y, angleByVerticalInput);
            return;
        }

        int tempCoefficient = 0;
        if (xInput > 0 && yInput < 0)
        {
            tempCoefficient = 1;
        }

        playerTransform.rotation = Quaternion.Euler(playerTransform.rotation.x, playerTransform.rotation.y, (angleByHorizontalInput + angleByVerticalInput) / 2 - 180 * tempCoefficient);
    }

    /// <summary>
    /// Метод FixedUpdate вызывается на каждом фиксированном кадре.
    /// Обновляет скорость игрока в зависимости от текущего направления движения.
    /// </summary>
    private void FixedUpdate()
    {
        rigidbody.velocity = currentMovementDirection * speed * Time.deltaTime;
    }
}