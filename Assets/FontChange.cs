using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteInEditMode]
public class FontChange : MonoBehaviour
{
    [SerializeField] private TMP_FontAsset _fontAsset;
    private void Update()
    {
        if (_fontAsset == null) return;
        foreach(var t in FindObjectsOfType<TextMeshProUGUI>(true))
        {
            t.font = _fontAsset;
            t.UpdateFontAsset();
        }
    }
}
