using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public NoteData data;

    public void UpdatePosition()
    {
        Vector3 pos = new Vector3();
        pos.x = data.line - 2 + 0.5f;
        pos.y = MusicSheet.MilliSecToHeight(data.milliSec);
        pos.z = 0;

        transform.localPosition = pos;
    }
}
