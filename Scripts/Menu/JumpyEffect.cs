using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JumpyEffect : MonoBehaviour
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

        // Calculate bounce offset using a sine wave for a smooth, continuous bounce
        float bounceOffset = Mathf.Sin(Time.time * waveSpeed + i) * waveHeight;

        // Calculate color from the rainbow spectrum
        Color32 color = Color.HSVToRGB(((Time.time * 0.5f) + (i * 0.1f)) % 1, 1, 1);

        for (int j = 0; j < 4; ++j)
        {
            int vertexIndex = charInfo.vertexIndex + j;
            Vector3 orig = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices[vertexIndex];
            // Apply the bounce offset to the y-coordinate of each vertex
            orig.y += bounceOffset;
            textInfo.meshInfo[charInfo.materialReferenceIndex].vertices[vertexIndex] = orig;

            // Apply the rainbow color to each vertex
            textInfo.meshInfo[charInfo.materialReferenceIndex].colors32[vertexIndex] = color;
        }
    }

    // Update the vertex data to apply the changes, including colors
    textMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
    }
}
