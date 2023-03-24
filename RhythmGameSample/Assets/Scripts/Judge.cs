using TMPro;
using UnityEngine;

public class Judge : MonoBehaviour {
    //変数の宣言
    [SerializeField] private GameObject[] MessageObj;//プレイヤーに判定を伝えるゲームオブジェクト
    [SerializeField] NotesManager notesManager;//スクリプト「notesManager」を入れる変数
    [SerializeField] TextMeshProUGUI comboText;
    [SerializeField] TextMeshProUGUI scoreText;
    
    void Update() {

        if (GManager.instance.Start) {

            if (Input.GetKeyDown(KeyCode.D))//〇キーが押されたとき
            {
                Debug.Log("notesManager.LaneNum[0] ??? " + notesManager.LaneNum[0]);
                if (notesManager.LaneNum[0] == 0)//押されたボタンはレーンの番号とあっているか？
                {
                    Debug.Log("Time.time : " + Time.time);
                    Debug.Log("notesManager.NotesTime[0] : " + notesManager.NotesTime[0]);
                    Debug.Log("GManager.instance.StartTime : " + GManager.instance.StartTime);
                    Judgement(GetABS(Time.time - (notesManager.NotesTime[0] + GManager.instance.StartTime)), 0);
                    /*
                    本来ノーツをたたく場所と実際にたたいた場所がどれくらいずれているかを求め、
                    その絶対値をJudgement関数に送る
                    */
                } else {
                    if (notesManager.LaneNum[1] == 0) {
                        Judgement(GetABS(Time.time - (notesManager.NotesTime[1] + GManager.instance.StartTime)), 1);
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.F)) {
                if (notesManager.LaneNum[0] == 1) {
                    Judgement(GetABS(Time.time - (notesManager.NotesTime[0] + GManager.instance.StartTime)), 0);
                } else {
                    if (notesManager.LaneNum[1] == 1) {
                        Judgement(GetABS(Time.time - (notesManager.NotesTime[1] + GManager.instance.StartTime)), 1);
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.J)) {
                if (notesManager.LaneNum[0] == 2) {
                    Judgement(GetABS(Time.time - (notesManager.NotesTime[0] + GManager.instance.StartTime)), 0);
                } else {
                    if (notesManager.LaneNum[1] == 2) {
                        Judgement(GetABS(Time.time - (notesManager.NotesTime[1] + GManager.instance.StartTime)), 1);
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.K)) {
                if (notesManager.LaneNum[0] == 3) {
                    Debug.Log("Time.time : " + Time.time);
                    Debug.Log("notesManager.NotesTime[0] : " + notesManager.NotesTime[0]);
                    Debug.Log("GManager.instance.StartTime : " + GManager.instance.StartTime);
                    Debug.Log("res : " + GetABS(Time.time - (notesManager.NotesTime[0] + GManager.instance.StartTime)));
                    Judgement(GetABS(Time.time - (notesManager.NotesTime[0] + GManager.instance.StartTime)), 0);
                } else {
                    if (notesManager.LaneNum[1] == 3) {
                        Judgement(GetABS(Time.time - (notesManager.NotesTime[1] + GManager.instance.StartTime)), 1);
                    }
                }
            }

            if (Time.time > notesManager.NotesTime[0] + 0.2f + GManager.instance.StartTime)//本来ノーツをたたくべき時間から0.2秒たっても入力がなかった場合
            {
                message(3);
                deleteData(0);
                
                GManager.instance.miss++;
                GManager.instance.combo = 0;
                //ミス
            }

        }

        
    }
    void Judgement(float timeLag, int numOffset) {
        if (timeLag <= 0.05)//本来ノーツをたたくべき時間と実際にノーツをたたいた時間の誤差が0.1秒以下だったら
        {
            Debug.Log("Perfect");
            GManager.instance.ratioScore += 5;
            GManager.instance.perfect++;
            GManager.instance.combo++;
            message(0);
            deleteData(numOffset);
        }
        else {
            if (timeLag <= 0.08)//本来ノーツをたたくべき時間と実際にノーツをたたいた時間の誤差が0.15秒以下だったら
            {
                Debug.Log("Great");
                GManager.instance.ratioScore += 3;
                GManager.instance.great++;
                GManager.instance.combo++;
                message(1);
                deleteData(numOffset);
            }
            else {
                if (timeLag <= 0.10)//本来ノーツをたたくべき時間と実際にノーツをたたいた時間の誤差が0.2秒以下だったら
                {
                    Debug.Log("Bad");
                    GManager.instance.ratioScore += 1;
                    GManager.instance.bad++;
                    GManager.instance.combo++;
                    message(2);
                    deleteData(numOffset);
                }
            }
        }
    }
    float GetABS(float num)
    {
        if (num >= 0)
        {
            return num;
        }
        else {
            return -num;
        }
    }
    void deleteData(int numOffset)//すでにたたいたノーツを削除する関数
    {
        notesManager.NotesTime.RemoveAt(numOffset);
        notesManager.LaneNum.RemoveAt(numOffset);
        notesManager.NoteType.RemoveAt(numOffset);

        GManager.instance.score = (int)Mathf.Round(1000000 * Mathf.Floor(GManager.instance.ratioScore / GManager.instance.maxScore));

        comboText.text = GManager.instance.combo.ToString();        
        scoreText.text = GManager.instance.score.ToString();
    }

    void message(int judge)//判定を表示する
    {
        Instantiate(MessageObj[judge], new Vector3(notesManager.LaneNum[0] - 1.5f, 0.76f, 0.15f), Quaternion.Euler(45, 0, 0));
    }
}