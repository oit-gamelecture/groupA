using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoStage : MonoBehaviour
{

    int StageSize = 80; //ステージの奥行サイズ（後で変更可）
    int StageIndex;

    public Transform Target;//プレイヤーを当てる
    public GameObject[] stagenum;//ステージのプレハブ
    public GameObject[] obstacles; // 障害物のプレハブ
    public int FirstStageIndex;//スタート時にどのインデックスからステージを生成するのか
    public int aheadStage; //事前に生成しておくステージ
    public List<GameObject> StageList = new List<GameObject>();//生成したステージのリスト
    public List<GameObject> ObstacleList = new List<GameObject>(); // 生成した障害物のリスト


    public float minObstacleInterval = 1f; // 障害物生成の最小間隔（後で変更可）
    public float maxObstacleInterval = 3f; // 障害物生成の最大間隔（後で変更可）

    private float nextObstacleTime; // 次に障害物を生成する時間の変数
    public float obstacleDistance = 40f; // 障害物をプレイヤーからどれだけ離して生成するか(テスト)

    private int[] xPositions = { 0, -3, 3 }; // 障害物の生成をするx座標（ここからランダムに）

    public GameObject goalPrefab; // ゴールのプレハブ
    private bool isGoalGenerated = false; // ゴールが生成されたかどうかを追跡するフラグ
    private float timer = 0f; // ゴールを出現させるタイミングを図るタイマー

    // Start is called before the first frame update
    void Start()
    {
        StageIndex = FirstStageIndex - 1;　//
        StageManager(aheadStage);　//指定した数分ステージを事前に生成
        ScheduleNextObstacle();　//最初の障害物のタイミング
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime; // タイマーを更新

        int targetPosIndex = (int)(Target.position.z / StageSize);　//プレイヤーがどこにいるのか

        if (targetPosIndex + aheadStage > StageIndex)　　//必要性に応じてステージ生成関数を呼び出し
        {
            if (isGoalGenerated) //上手くいかないので二重になってるけど気にしないで
            {
                if (Time.time >= nextObstacleTime) //現在の時間が次の障害物生成時間を超えているかを確認し呼び出し
                {
                    GenerateObstacle(); //障害物の生成
                    ScheduleNextObstacle(); //次の生成時間の計算
                }
                RemovePassedObstacles(); // プレイヤーが通り過ぎた障害物を削除
                return;
            }

            StageManager(targetPosIndex + aheadStage);
        }


        if (Time.time >= nextObstacleTime)　//現在の時間が次の障害物生成時間を超えているかを確認し呼び出し
        {
            GenerateObstacle();　//障害物の生成
            ScheduleNextObstacle();　//次の生成時間の計算
        }

        RemovePassedObstacles(); // プレイヤーが通り過ぎた障害物を削除

        if (timer >= 95f && !isGoalGenerated) // タイムが95に達したとき
        {
            GenerateGoalStage(); // ゴールステージを生成
            isGoalGenerated = true; // ゴールが生成されたことを記録
        }

    }
    void StageManager(int maps)
    {
        if (maps <= StageIndex)　//すでに足りてるとき返す
        {
            return;
        }

        for (int i = StageIndex + 1; i <= maps; i++)//指定したステージまで作成する
        {
            GameObject stage = MakeStage(i);
            StageList.Add(stage);
        }

        while (StageList.Count > aheadStage +2)//古いステージを削除する
        {
            DestroyStage();
        }

        StageIndex = maps;
    }

    GameObject MakeStage(int index)　//ステージを生成する
    {

        int nextStage = Random.Range(0, stagenum.Length);　//ステージプレハブからランダムに選ぶ

        GameObject stageObject = (GameObject)Instantiate(stagenum[nextStage], new Vector3(0, -0.5f, index * StageSize + 40 + 0.05f), Quaternion.identity);

        return stageObject;
    }

    void DestroyStage()　//ステージを削除する
    {
        if (StageList[0] != StageList[FirstStageIndex])
        {
            GameObject oldStage = StageList[0];
            StageList.RemoveAt(0);
            Destroy(oldStage); // インスタンスを削除
        }
    }

    void GenerateObstacle() // 障害物を生成する
    {
        int xPos = xPositions[Random.Range(0, xPositions.Length)];　//３つのx座標からランダムに
        int obstacleIndex = Random.Range(0, obstacles.Length);　//障害物プレハブからランダムに

        Vector3 spawnPosition = new Vector3(xPos, -0.5f, Target.position.z + obstacleDistance); // プレイヤーから一定距離先に生成
        GameObject obstacle = Instantiate(obstacles[obstacleIndex], spawnPosition, Quaternion.identity);

        ObstacleList.Add(obstacle); // 障害物をリストに追加
    }
    void ScheduleNextObstacle() // 次の障害物生成のタイミングを設定
    {
        nextObstacleTime = Time.time + Random.Range(minObstacleInterval, maxObstacleInterval);
    }

    void RemovePassedObstacles() // プレイヤーが通り過ぎた障害物を削除
    {
        for (int i = ObstacleList.Count - 1; i >= 0; i--)
        {
            if (ObstacleList[i].transform.position.z < Target.position.z - StageSize)
            {
                GameObject passedObstacle = ObstacleList[i];
                ObstacleList.RemoveAt(i);
                Destroy(passedObstacle);
            }
        }
    }

    void GenerateGoalStage()　//ゴールをリストに追加する
    {
        Vector3 goalPosition = new Vector3(0, -0.5f, StageIndex * StageSize + 120 + 0.05f); //ゴールのプレハブのサイズによって120を40+サイズに変更して
        GameObject goalStage = Instantiate(goalPrefab, goalPosition, Quaternion.identity);
        StageList.Add(goalStage); // ゴールステージをリストに追加

    }

   

}