using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WigglyEffect : MonoBehaviour
{
    public TMP_Text textMesh;
    public float waveSpeed = 1.0f;
    public float waveHeight = 1.0f;

    void Start()
    {
        if (textMesh == null)
        {
            textMesh = GetComponent<TMP_Text>();
        }
    }

    void Update()
    {
        TMP_TextInfo textInfo = textMesh.textInfo;
        for (int i = 0; i < textInfo.characterCount; ++i)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible)
                continue;

            float charRotationAngle = Mathf.Sin(Time.time * waveSpeed + i) * waveHeight;
            charRotationAngle = Mathf.Clamp(charRotationAngle, -0.03f, 0.03f);

            
            Vector3 charCenter = (charInfo.bottomLeft + charInfo.topRight) / 2;
            Quaternion rotation = Quaternion.Euler(0, 0, charRotationAngle);

            for (int j = 0; j < 4; ++j)
            {
                Vector3 orig = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices[charInfo.vertexIndex + j];
                Vector3 dir = orig - charCenter;
                dir = rotation * dir;
                textInfo.meshInfo[charInfo.materialReferenceIndex].vertices[charInfo.vertexIndex + j] = charCenter + dir;
            }
        }

        textMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
    }
}
