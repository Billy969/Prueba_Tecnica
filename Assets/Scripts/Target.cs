using UnityEngine;

public class Target : MonoBehaviour
{
    private Rigidbody targetRb;
    private ObjectPool gameManager;
    public ParticleSystem explosionParticle;
    

    public int pointValue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetRb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("SpawnManager").GetComponent<ObjectPool>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if(gameManager.isGameActive)
        {
            gameObject.SetActive(false);
            gameManager.UpdateScore(pointValue);
            gameManager.UpdateActiveObjectCount();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
        if(!gameObject.CompareTag("Bad"))
        {
            gameManager.GameOver();
        }
        gameObject.SetActive(false); // en vez de Destroy
        gameManager.UpdateActiveObjectCount(); 
    }
}
