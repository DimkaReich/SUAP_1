using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс предназначен для управления установкой и взрывом бомб.
/// Обрабатывает размещение бомб, их пульсацию и взрыв с разрушением объектов.
/// </summary>
public class BombController : MonoBehaviour
{
    // Префаб бомбы
    [SerializeField] public GameObject bombPrefab;

    // Префаб взрыва
    [SerializeField] public ExplosionController explosionPrefab;

    // Время задержки перед взрывом бомбы
    [SerializeField] public int bombFuseTime;

    // Максимальное количество бомб, которые можно разместить одновременно
    [SerializeField] public int bombNumberLimit;

    // Радиус взрыва бомбы
    [SerializeField] public int bombExplosionRadiuis = 1;

    // Слой, с которым взаимодействует взрыв
    [SerializeField] public LayerMask explosionLayerMask;

    // Скорость пульсации бомбы
    [SerializeField] public float bombPulseSpeed;

    // Амплитуда пульсации бомбы
    [SerializeField] public float bombPulseAmount;

    // Список размещённых бомб
    private List<GameObject> placedBombs = new List<GameObject>();

    // Префаб индикатора бомбы
    [SerializeField] public GameObject bombIndicatorPrefab;

    // Текущий индикатор бомбы
    private GameObject currentBombIndicatorObj = null;

    /// <summary>
    /// Размещает индикатор бомбы в позиции, куда будет установлена бомба.
    /// </summary>
    public void PlaceBombIndicator()
    {
        if (currentBombIndicatorObj != null)
        {
            Destroy(currentBombIndicatorObj);
        }

        Vector2 pos = getBombPosition();

        // Проверка, находится ли индикатор в стене
        if (Physics2D.OverlapBox(pos, Vector2.one / 2, 0f, explosionLayerMask))
        {
            return;
        }

        currentBombIndicatorObj = Instantiate(bombIndicatorPrefab, pos, Quaternion.identity);
    }

    /// <summary>
    /// Размещает бомбу, если не превышен лимит размещённых бомб.
    /// </summary>
    public void PlaceBomb()
    {
        if (placedBombs.Count < bombNumberLimit)
        {
            StartCoroutine(InitBomb());
        }
    }

    /// <summary>
    /// Анимация пульсации бомбы.
    /// Увеличивает и уменьшает размер бомбы.
    /// </summary>
    private IEnumerator Pulse(GameObject bomb)
    {
        while (bomb != null)
        {
            for (float i = 0f; i <= 1f; i += 0.1f)
            {
                bomb.transform.localScale = new Vector3(
                    Mathf.Lerp(bomb.transform.localScale.x, bomb.transform.localScale.x + bombPulseAmount, Mathf.SmoothStep(0f, 1f, i)),
                    Mathf.Lerp(bomb.transform.localScale.y, bomb.transform.localScale.y + bombPulseAmount, Mathf.SmoothStep(0f, 1f, i)),
                    bomb.transform.localScale.z);

                yield return new WaitForSeconds(bombPulseSpeed);
            }

            for (float i = 0f; i <= 1f; i += 0.1f)
            {
                bomb.transform.localScale = new Vector3(
                    Mathf.Lerp(bomb.transform.localScale.x, bomb.transform.localScale.x - bombPulseAmount, Mathf.SmoothStep(0f, 1f, i)),
                    Mathf.Lerp(bomb.transform.localScale.y, bomb.transform.localScale.y - bombPulseAmount, Mathf.SmoothStep(0f, 1f, i)),
                    bomb.transform.localScale.z);

                yield return new WaitForSeconds(bombPulseSpeed);
            }
        }
    }

    /// <summary>
    /// Производит взрыв бомбы и вызывает разрушение объектов в радиусе взрыва.
    /// </summary>
    private void Explode(GameObject bomb)
    {
        ExplosionController explosion = Instantiate(explosionPrefab, bomb.transform.position, Quaternion.identity);
        ExplodeInDirection(explosion.transform.position, Vector2.up, bombExplosionRadiuis);
        ExplodeInDirection(explosion.transform.position, Vector2.down, bombExplosionRadiuis);
        ExplodeInDirection(explosion.transform.position, Vector2.left, bombExplosionRadiuis);
        ExplodeInDirection(explosion.transform.position, Vector2.right, bombExplosionRadiuis);
    }

    /// <summary>
    /// Производит взрыв в указанном направлении, разрушая объекты на пути.
    /// </summary>
    private void ExplodeInDirection(Vector2 startPosition, Vector2 direction, int length)
    {
        ExplosionController explosion = Instantiate(explosionPrefab, startPosition += direction, Quaternion.identity);

        if (Physics2D.OverlapBox(explosion.transform.position, Vector2.one / 2, 0f, explosionLayerMask))
        {
            explosion.ClearDestructable();
            return;
        }
        if (length > 1)
        {
            ExplodeInDirection(explosion.transform.position, direction, length - 1);
        }
    }

    /// <summary>
    /// Получает округлённую позицию для размещения бомбы.
    /// </summary>
    private Vector2 getBombPosition()
    {
        Vector2 pos = transform.position;
        pos.x = Mathf.Round(pos.x);
        pos.y = Mathf.Round(pos.y);
        return pos;
    }

    /// <summary>
    /// Инициализирует бомбу, устанавливая её на сцену и запуская таймер взрыва.
    /// </summary>
    private IEnumerator InitBomb()
    {
        Vector2 pos = getBombPosition();
        if (Physics2D.OverlapBox(pos, Vector2.one / 2, 0f, explosionLayerMask))
        {
            yield break;
        }

        GameObject bomb = Instantiate(bombPrefab, pos, Quaternion.identity);
        placedBombs.Add(bomb);
        StartCoroutine(Pulse(bomb));

        yield return new WaitForSeconds(bombFuseTime);

        placedBombs.Remove(bomb);
        Explode(bomb);
        Destroy(bomb);
    }
}