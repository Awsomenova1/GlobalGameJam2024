using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextShake : TextEffect
{
    public float intensity = 1;

    public TextShake(float intensity = 1)
    {
        this.intensity = intensity;
        type = "Shake";
    }

    public override void ApplyEffectToMesh(TMP_TextInfo textMesh)
    {
        int endPoint = end;
        if (endPoint == -1 || endPoint > textMesh.characterCount)
            endPoint = textMesh.characterCount;

        for (int i = start; i < endPoint; i++)
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
            Vector3 offsetVector = new Vector3(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity), 0);
            

            //Applies changes made to the vertices
            textMesh.meshInfo[index].vertices[vertexIndex + 0] += offsetVector;
            textMesh.meshInfo[index].vertices[vertexIndex + 1] += offsetVector;
            textMesh.meshInfo[index].vertices[vertexIndex + 2] += offsetVector;
            textMesh.meshInfo[index].vertices[vertexIndex + 3] += offsetVector;

        }
    }
}
