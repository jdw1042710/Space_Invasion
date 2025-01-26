using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    private Unit unit;
    private Slider slider;
    private void Awake()
    {
        unit = GetComponentInParent<Unit>();
        Debug.Assert(unit);
        unit.AddHealthListener(UpdateHealthUI);
        slider = GetComponentInChildren<Slider>();
        Debug.Assert(slider);
        slider.maxValue = unit.MaxHealth;
    }

    private void UpdateHealthUI(float value)
    {
        slider.value = value;
    }
}
