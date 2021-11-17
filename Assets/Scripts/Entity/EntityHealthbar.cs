using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealthbar : MonoBehaviour
{
    public GameObject Placeholder;
    public GameObject Filling;

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        maxHealth = (maxHealth > 0) ? maxHealth : 1;

        var placeholderLocalScale = Placeholder.transform.localScale;
        var placeholderLocalPosition = Placeholder.transform.localPosition;
        var percentage = currentHealth / maxHealth;

        Filling.transform.localScale = new Vector3(placeholderLocalScale.x * percentage, placeholderLocalScale.y, placeholderLocalScale.z);
        Filling.transform.localPosition = new Vector3(-((placeholderLocalScale.x - Filling.transform.localScale.x) / 2f), placeholderLocalPosition.x, placeholderLocalPosition.z);
    }
}
