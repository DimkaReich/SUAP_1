using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ����� ������������ ��� ���������� ��������� ������ � ���������� ����.
/// ������������ ���� � ���������� � �������� ��������� ������ � ������������ � ���.
/// </summary>
public class PlayerController : MonoBehaviour
{
    // ������, ���������� �� �������� �����
    [SerializeField] public KeyCode inputUp;

    // ������, ���������� �� �������� ����
    [SerializeField] public KeyCode inputDown;

    // ������, ���������� �� �������� �����
    [SerializeField] public KeyCode inputLeft;

    // ������, ���������� �� �������� ������
    [SerializeField] public KeyCode inputRight;

    // ������, ���������� �� ��������� �����
    [SerializeField] public KeyCode inputPlaceBomb;

    // �������� ������
    [SerializeField] public float speed;

    // ���������� ����� ������
    private Rigidbody2D rigidbody;

    // ������� � ������� ������
    private Transform playerTransform;

    // �����������, ���� ���� �����
    private Vector2 currentMovementDirection;

    // ����������, ���������� �� ��������� ����
    private BombController playerBombController;

    /// <summary>
    /// ����� Start ���������� ��� ������������� �������.
    /// �������������� ���������� Rigidbody2D, Transform � BombController.
    /// </summary>
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        playerTransform = GetComponent<Transform>();
        playerBombController = GetComponent<BombController>();
    }

    /// <summary>
    /// ����� Update ���������� �� ������ �����.
    /// ������������ ���� � ���������� ��� ���������� ��������� � ���������� ����.
    /// </summary>
    void Update()
    {
        float xInput = 0f;
        float yInput = 0f;

        // �������� ������
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
    /// ������ �������� ������� ��������� � ����������� �� ����������� ��������.
    /// </summary>
    /// <param name="xInput">�������������� ����������� ��������.</param>
    /// <param name="yInput">������������ ����������� ��������.</param>
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
    /// ����� FixedUpdate ���������� �� ������ ������������� �����.
    /// ��������� �������� ������ � ����������� �� �������� ����������� ��������.
    /// </summary>
    private void FixedUpdate()
    {
        rigidbody.velocity = currentMovementDirection * speed * Time.deltaTime;
    }
}