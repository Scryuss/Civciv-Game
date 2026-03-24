using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 3;

    private int _currentHealth;

    void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void Damage(int damageAmount)
    {
        if (_currentHealth > 0)
        {
            _currentHealth -= damageAmount;
            // UI Animation for damage can be triggered here

            if (_currentHealth <= 0)
            {
                //Die();
            }
        }
    }

        public void Heal(int healAmount)
        {
            if (_currentHealth < _maxHealth)
            {
                _currentHealth = Mathf.Min(_currentHealth + healAmount, _maxHealth);
            }
        }
}
