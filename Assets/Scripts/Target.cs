using UnityEngine;
// Este script se adjunta a los objetos 3D (targets) interactivos del juego.
public class Target : MonoBehaviour
{
// Componente Rigidbody del objeto (para aplicar físicas si es necesario)
    private Rigidbody targetRb;
    // Referencia al administrador de objetos (ObjectPool)
    private ObjectPool gameManager;
     // Sistema de partículas para explosión (se puede activar visualmente al hacer clic, aunque aquí no se usa aún)
    public ParticleSystem explosionParticle;
    

    public int pointValue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    // Obtiene el Rigidbody del objeto
        targetRb = GetComponent<Rigidbody>();
        // Busca el GameObject llamado "SpawnManager" y accede al script ObjectPool que está en él
        gameManager = GameObject.Find("SpawnManager").GetComponent<ObjectPool>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Se llama cuando el jugador hace clic en el objeto
    private void OnMouseDown()
    {
        if(gameManager.isGameActive)
        {
        // Si el juego está activo, desactiva el objeto y actualiza la puntuación
            gameObject.SetActive(false);
            gameManager.UpdateScore(pointValue);
            gameManager.UpdateActiveObjectCount();
        }
    }
    // Se llama cuando este objeto colisiona con otro trigger
    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
         // Si el objeto no tiene la etiqueta "Bad", se considera que el jugador falló y se acaba el juego
        if(!gameObject.CompareTag("Bad"))
        {
            gameManager.GameOver();
        }
        gameObject.SetActive(false); // en vez de Destroy
        gameManager.UpdateActiveObjectCount(); 
    }
}
