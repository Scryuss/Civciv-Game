using UnityEngine;

[CreateAssetMenu(fileName = "WheatDesingSO", menuName = "ScriptableObjects/WheatDesingSO", order = 1)]
public class WheatDesingSO : ScriptableObject
{
   [SerializeField] public float IncreaseDecreaseMultiplier;
   [SerializeField] public float ResetBoostDuration;
   [SerializeField] private Sprite _activeSprite;
   [SerializeField] private Sprite _passiveSprite;
   [SerializeField] private Sprite _activeWheatSprite;
   [SerializeField] private Sprite _passiveWheatSprite;

   public Sprite ActiveSprite => _activeSprite;
   public Sprite PassiveSprite => _passiveSprite;
   public Sprite ActiveWheatSprite => _activeWheatSprite;
   public Sprite PassiveWheatSprite => _passiveWheatSprite;
}
