
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using static TreeEditor.TreeEditorHelper;

[Serializable]
public class Data {
    public string name;
    public int maxBlock;
    public int BPM;
    public int offset;
    public Note[] notes;

}
[Serializable]
public class Note {
    public int type;
    public int num;
    public int block;
    public int LPB;
}

public class NotesManager : MonoBehaviour {
    public int noteNum;
    private string path = "Notes/";
    public string songName;

    public List<int> LaneNum = new List<int>(); // ������ 4���� �׶��� ���γѹ� : 0, 1, 2, 3 
    public List<int> NoteType = new List<int>();    // ��Ʈ�� Ÿ�� - �ճ�Ʈ �����
    public List<float> NotesTime = new List<float>();   // 
    public List<GameObject> NotesObj = new List<GameObject>();

    [SerializeField] private float NotesSpeed;
    [SerializeField] GameObject noteObj;

    void OnEnable() {
        NotesSpeed = GManager.instance.noteSpeed;
        noteNum = 0;        
        Load(songName);
    }

    private void Load(string SongName) {
        string inputString = Resources.Load<TextAsset>(path+SongName).ToString();
        Data inputJson = JsonUtility.FromJson<Data>(inputString);

        noteNum = inputJson.notes.Length;   // ��Ʈ����
        GManager.instance.maxScore = noteNum * 5;

        // ����
        for (int i = 0; i < inputJson.notes.Length; i++) {
            float kankaku = 60 / (inputJson.BPM * (float)inputJson.notes[i].LPB);   // ex 60��/120bmp * LPB(lengh per beat : �ѹ��ڴ� �ɰ����� ����, 4/4���� �Ҷ� �и���4, �Ѹ��� � �ֳ�)
            float beatSec = kankaku * (float)inputJson.notes[i].LPB;    // �ѹ���
            float time = (beatSec * inputJson.notes[i].num / (float)inputJson.notes[i].LPB) + inputJson.offset + 0.01f;
            NotesTime.Add(time);
            LaneNum.Add(inputJson.notes[i].block);
            NoteType.Add(inputJson.notes[i].type);

            float z = NotesTime[i] * NotesSpeed;
            NotesObj.Add(Instantiate(noteObj, new Vector3(inputJson.notes[i].block - 1.5f, 0.55f, z), Quaternion.identity));
        }
    }
}