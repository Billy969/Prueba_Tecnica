using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ObjectPool : MonoBehaviour
{
    // Puntos donde los animales pueden aparecer (se asignan desde el editor)
    public Transform[] spawnPoints;

    // Prefab del animal que se va a instanciar
    public GameObject[] ObjectPrefab;


    // Tamaño inicial de la piscina de objetos (cantidad de animales a preinstanciar)
    public int poolSize;

    // Referencia al sistema de entradas de Unity (Input System)
    public InputActionAsset inputs;
    public List<GameObject> targets;
    private int score;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public bool isGameActive;
    private float spawnInterval = 2f;
    public TextMeshProUGUI activeObjectsText;

    // Lista que almacena los animales instanciados (usados y no usados)
    [SerializeField] List<GameObject> pooledObjects = new List<GameObject>();

    void Start()
    {
        isGameActive = true;
        score = 0;

        StartCoroutine(SpawnTarget());
        UpdateScore(0);
        AddToPool(poolSize);
    }


    private void Update()
    {

    }

    // Método que agrega nuevos animales a la piscina
    void AddToPool(int poolSize)
    {
        for (int i = 0; i < poolSize; i++)
        {
            int objectIndex = Random.Range(0, ObjectPrefab.Length);
            // Instancia un nuevo animal y lo desactiva
            GameObject prefabObject;
            prefabObject = Instantiate(ObjectPrefab[objectIndex], Vector3.zero, Quaternion.identity);
            prefabObject.SetActive(false);

            // Lo agrega a la lista de la piscina
            pooledObjects.Add(prefabObject);
        }
    }



    // Devuelve el primer animal que esté desactivado (libre para usar)
    public GameObject FirstDeactivate()
    {
        List<GameObject> inactiveObjects = pooledObjects
        .Where(obj => !obj.activeInHierarchy)
        .ToList();

        if (inactiveObjects.Count > 0)
        {
            int randomIndex = Random.Range(0, inactiveObjects.Count);
            return inactiveObjects[randomIndex];
        }

        // Si todos están en uso, crea uno nuevo y devuélvelo
        AddToPool(1);
        return pooledObjects.Last();
    }
    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnInterval);
            if (spawnPoints.Length > 0)
            {
                int spawnIndex = Random.Range(0, spawnPoints.Length);

                // Obtiene un animal que no esté activo
                GameObject objectSpawn = FirstDeactivate();

                if (objectSpawn != null)
                {
                    // Selecciona un animal aleatorio del array de prefabs
                    int objectPrefabIndex = Random.Range(0, ObjectPrefab.Length);

                    objectSpawn.transform.position = spawnPoints[spawnIndex].position;
                    objectSpawn.transform.rotation = spawnPoints[spawnIndex].rotation;

                    // Activa el animal en la escena
                    objectSpawn.SetActive(true);

                    // Asigna un color aleatorio
                    SetRandomColor(objectSpawn);

                    UpdateActiveObjectCount();
                }
                else
                {
                    Debug.LogWarning("No hay objectos desactivados en la piscina. Aumenta el poolSize o revisa la lógica.");
                }
            }
            else
            {
                Debug.LogError("No hay puntos de aparición asignados. Asegúrate de asignar Spawn Points en el Inspector.");
            }
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }
    public void GameOver()
    {
        restartButton.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
        isGameActive = false;

    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void UpdateActiveObjectCount()
    {
        int activeCount = pooledObjects.Count(obj => obj.activeInHierarchy);
        activeObjectsText.text = "Activos: " + activeCount;
    }
void SetRandomColor(GameObject obj)
{
    Renderer renderer = obj.GetComponent<Renderer>();
    if (renderer != null)
    {
        // Instancia un nuevo material para evitar modificar el material global
        renderer.material = new Material(renderer.material);
        Color randomColor = new Color(Random.value, Random.value, Random.value);
        renderer.material.color = randomColor;
    }
}
}
