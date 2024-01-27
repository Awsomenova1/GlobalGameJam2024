using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextWave : TextEffect
{
    public float intensity = 1;
    public float speed = 1;
    public float waveLength = 1;

    public TextWave(float intensity = 1, float speed = 1, float waveLength = 1)
    {
        this.intensity = intensity;
        this.speed = speed;
        this.waveLength = waveLength;
        type = "Wave";
    }

    //Effects seem to be applied to index 0 a ton, and it gets worse the more text there is. It may be because it is there first, and so it gets the most hits
    public override void ApplyEffectToMesh(TMP_TextInfo textMesh)
    {
        int endPoint = end;
        if (endPoint == -1 || endPoint > textMesh.characterCount)
            endPoint = textMesh.characterCount;

        for(int i = start; i < endPoint; i++)
        {
            //Sets up indexes needed to find appropriate vertices
            TMP_CharacterInfo info = textMesh.characterInfo[i];
            int index = info.materialReferenceIndex;
            int vertexIndex = info.vertexIndex;

            //Skips empty characters as they reapply their affects to index 0
            //Also works to have:
            //if(vertexIndex == 0 && i != 0)
            if (info.character == '\r' || info.character == '\n' || info.character == '\t' || info.character == ' ')
                continue;

            //Calculates offset
            float offset = Mathf.Sin(info.vertex_BL.position.x/waveLength + (Time.time * speed)) * intensity;
            Vector3 offsetVector = new Vector3(0, offset, 0);

            //Applies changes made to the vertices
            textMesh.meshInfo[index].vertices[vertexIndex + 0] += offsetVector;
            textMesh.meshInfo[index].vertices[vertexIndex + 1] += offsetVector;
            textMesh.meshInfo[index].vertices[vertexIndex + 2] += offsetVector;
            textMesh.meshInfo[index].vertices[vertexIndex + 3] += offsetVector;

        }
    }
}
