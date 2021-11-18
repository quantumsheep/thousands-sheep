using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EntityIdentity : MonoBehaviour
{
    public TextMeshPro Name;
    public TextMeshPro Title;

    public void UpdateIdentity(int level, string name, string title)
    {
        Name.text = $"[{level}] {name}";
        Title.text = title;

        Name.gameObject.SetActive(!string.IsNullOrEmpty(name));
        Title.gameObject.SetActive(!string.IsNullOrEmpty(title));

        if (string.IsNullOrEmpty(title))
        {
            Name.transform.localPosition = new Vector3(0f, 0f, 0f);
        }
        else
        {
            Name.transform.localPosition = new Vector3(0f, 0.3f, 0f);
        }
    }
}
