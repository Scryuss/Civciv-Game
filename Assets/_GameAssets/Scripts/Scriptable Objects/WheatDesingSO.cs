using UnityEngine;

[CreateAssetMenu(fileName = "WheatDesingSO", menuName = "ScriptableObjects/WheatDesingSO", order = 1)]
public class WheatDesingSO : ScriptableObject
{
   [SerializeField] public float IncreaseDecreaseMultiplier;
   [SerializeField] public float ResetBoostDuration;
}
