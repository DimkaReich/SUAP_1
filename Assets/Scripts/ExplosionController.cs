using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

/// <summary>
/// Класс предназначен для управления взрывами в игре.
/// Обрабатывает столкновения с игроками и уничтожаемыми объектами, а также анимацию взрыва.
/// </summary>
public class ExplosionController : MonoBehaviour
{
    // Время жизни взрыва
    [SerializeField] public float explosionTime = 1.5f;

    // Tilemap для уничтожаемых объектов
    private Tilemap desctructables;

    /// <summary>
    /// Метод вызывается при столкновении другого коллайдера с этим объектом.
    /// Уничтожает игрока при столкновении и завершает игру.
    /// </summary>
    /// <param name="other">Коллайдер, столкнувшийся с этим объектом.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(other.gameObject);
            SceneController.FinishGame(other.gameObject);
        }
    }

    /// <summary>
    /// Метод Start вызывается при инициализации объекта.
    /// Запускает корутину анимации взрыва.
    /// </summary>
    void Start()
    {
        StartCoroutine(Pulse());
    }

    /// <summary>
    /// Метод Awake вызывается при инициализации объекта.
    /// Находит Tilemap для уничтожаемых объектов.
    /// </summary>
    private void Awake()
    {
        desctructables = GameObject.FindWithTag("Destructables").GetComponent<Tilemap>();
    }

    /// <summary>
    /// Удаляет уничтожаемый объект в месте взрыва.
    /// </summary>
    public void ClearDestructable()
    {
        desctructables.SetTile(desctructables.WorldToCell(transform.position), null);
    }

    /// <summary>
    /// Анимация пульсации взрыва.
    /// Увеличивает размер объекта, а затем уничтожает его.
    /// </summary>
    private IEnumerator Pulse()
    {
        for (float i = 0f; i <= 1f; i += 0.1f)
        {
            this.transform.localScale = new Vector3(
                Mathf.Lerp(this.transform.localScale.x, this.transform.localScale.x + 0.2f, Mathf.SmoothStep(0f, 1f, i)),
                Mathf.Lerp(this.transform.localScale.y, this.transform.localScale.y + 0.2f, Mathf.SmoothStep(0f, 1f, i)),
                this.transform.localScale.z);

            yield return new WaitForSeconds(explosionTime / 10);
        }
        Destroy(this.gameObject);
    }
}