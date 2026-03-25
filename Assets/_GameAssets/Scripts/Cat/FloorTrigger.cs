using UnityEngine;

public class FloorTrigger : MonoBehaviour
{
    [Header("Kovalanacak Kediler")]
    public EnemyCatAI[] kediler;

    // Oyuncu zemine değdiği her saniye (her kare) kediyi besler
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (EnemyCatAI kedi in kediler)
            {
                if (kedi != null)
                {
                    // Kedinin içindeki zamanlayıcıyı sürekli sıfırlar
                    kedi.ZemineDokunuldu();
                }
            }
        }
    }
}