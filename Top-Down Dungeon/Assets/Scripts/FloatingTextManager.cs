using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextManager : MonoBehaviour
{
    public GameObject textPrefab;

    private List<FloatingText> floatingTexts = new();

    public void DeleteAll()
    {
        foreach (var t in floatingTexts)
            Destroy(t.textMeshPro);
        floatingTexts.Clear();
    }

    private void Update()
    {
        foreach (FloatingText txt in floatingTexts)
            txt.UpdateFloatingText();
    }

    public void Show(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        FloatingText txt = GetFloatingText();

        txt.textMeshPro.text = msg;
        txt.textMeshPro.fontSize = fontSize;
        txt.textMeshPro.color = color;
        txt.go.transform.position = position;
        txt.motion = motion;
        txt.duration = duration;

        txt.Show();
    }

    private FloatingText GetFloatingText()
    {
        FloatingText txt = floatingTexts.Find(t => !t.active);

        if (txt == null)
        {
            txt = new();
            txt.go = Instantiate(textPrefab);
            txt.textMeshPro = txt.go.GetComponent<TextMeshPro>();

            floatingTexts.Add(txt);
        }

        return txt;
    }
}
