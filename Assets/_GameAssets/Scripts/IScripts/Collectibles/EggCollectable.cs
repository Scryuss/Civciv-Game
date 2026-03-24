using System;
using UnityEngine;

public class EggCollectable : MonoBehaviour
{
    // Yumurtanın toplandığını diğer scriptlere duyuracak bir Event (Yayın) oluşturuyoruz
    public static event Action OnEggCollected; 

    private bool _isCollected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_isCollected) return; 

        if (other.CompareTag("Player"))
        {
            _isCollected = true; 
            
            // "Ben toplandım!" anonsunu yap (Dinleyen varsa haberi olur)
            OnEggCollected?.Invoke(); 

            Destroy(gameObject);
        }
    }
}