using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RottenWheat") || other.CompareTag("GoldWheat") || other.CompareTag("HolyWheat"))
        {
            // Collect fonksiyonlarını çağır
            var rottenWheat = other.GetComponent<RottenWheatCollectibles>();
            rottenWheat?.Collect();
            
            var goldWheat = other.GetComponent<GoldWheatCollectibles>();
            goldWheat?.Collect();
            
            var holyWheat = other.GetComponent<HolyWheatCollectibles>();
            holyWheat?.Collect();
            
            // Wheatı yok et
            Destroy(other.gameObject);
        }
    }
    
}
