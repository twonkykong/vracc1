using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PropMenuButton : MonoBehaviour
{
    public Spawner spawner;
    public SpawnerOnline spawnerOnline;
    public GameObject prop;

    private void OnEnable()
    {
        if (!GetComponentInChildren<Text>().enabled) return;
        
        RuntimePreviewGenerator.Padding = 0.25f;
        Texture2D tex = RuntimePreviewGenerator.GenerateModelPreview(prop.transform, 256, 256);
        Sprite spr = GetComponent<Image>().sprite;
        spr = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        GetComponent<Image>().sprite = spr;
        GetComponent<Image>().color = new Color(1, 1, 1);
        GetComponentInChildren<Text>().enabled = false;
    }

    public void Press()
    {
        if (spawner != null)
        {
            spawner.prop = prop;

            if (spawner.duplicateProp != null)
            {
                Destroy(spawner.duplicateProp);
            }
        }
        else
        {
            spawnerOnline.prop = prop;

            if (spawnerOnline.duplicateProp != null)
            {
                Destroy(spawnerOnline.duplicateProp);
            }
        }
    }
}
