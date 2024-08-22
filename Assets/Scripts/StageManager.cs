using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoStage : MonoBehaviour
{

    int StageSize = 80; //�X�e�[�W�̉��s�T�C�Y�i��ŕύX�j
    int StageIndex;

    public Transform Target;//�v���C���[�𓖂Ă�
    public GameObject[] stagenum;//�X�e�[�W�̃v���n�u
    public GameObject[] obstacles; // ��Q���̃v���n�u
    public int FirstStageIndex;//�X�^�[�g���ɂǂ̃C���f�b�N�X����X�e�[�W�𐶐�����̂�
    public int aheadStage; //���O�ɐ������Ă����X�e�[�W
    public List<GameObject> StageList = new List<GameObject>();//���������X�e�[�W�̃��X�g
    public List<GameObject> ObstacleList = new List<GameObject>(); // ����������Q���̃��X�g


    public float minObstacleInterval = 1f; // ��Q�������̍ŏ��Ԋu�i��ŕύX�j
    public float maxObstacleInterval = 3f; // ��Q�������̍ő�Ԋu�i��ŕύX�j

    private float nextObstacleTime; // ���ɏ�Q���𐶐����鎞�Ԃ̕ϐ�
    public float obstacleDistance = 40f; // ��Q�����v���C���[����ǂꂾ�������Đ������邩(�e�X�g)

    private int[] xPositions = { 0, -3, 3 }; // ��Q���̐���������x���W�i�������烉���_���Ɂj

    public GameObject goalPrefab; // �S�[���̃v���n�u
    private bool isGoalGenerated = false; // �S�[�����������ꂽ���ǂ�����ǐՂ���t���O
    private float timer = 0f; // �S�[�����o��������^�C�~���O��}��^�C�}�[

    // Start is called before the first frame update
    void Start()
    {
        StageIndex = FirstStageIndex - 1;�@//
        StageManager(aheadStage);�@//�w�肵�������X�e�[�W�����O�ɐ���
        ScheduleNextObstacle();�@//�ŏ��̏�Q���̃^�C�~���O
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime; // �^�C�}�[���X�V

        int targetPosIndex = (int)(Target.position.z / StageSize);�@//�v���C���[���ǂ��ɂ���̂�

        if (timer >= 120f)
        {
            // 120�b�ȏ�o�߂����ꍇ�A��Q���̐������~�߂�
            RemovePassedObstacles(); // ���ɐ�������Ă����Q���������폜����
            return;
        }

        // �����Ԋu�����X�ɒZ������
        DecreaseObstacleIntervalOverTime();

        if (targetPosIndex + aheadStage > StageIndex)�@�@//�K�v���ɉ����ăX�e�[�W�����֐����Ăяo��
        {
            if (isGoalGenerated) //��肭�����Ȃ��̂œ�d�ɂȂ��Ă邯�ǋC�ɂ��Ȃ���
            {
                if (Time.time >= nextObstacleTime) //���݂̎��Ԃ����̏�Q���������Ԃ𒴂��Ă��邩���m�F���Ăяo��
                {
                    GenerateObstacle(); //��Q���̐���
                    ScheduleNextObstacle(); //���̐������Ԃ̌v�Z
                }
                RemovePassedObstacles(); // �v���C���[���ʂ�߂�����Q�����폜
                return;
            }

            StageManager(targetPosIndex + aheadStage);
        }


        if (Time.time >= nextObstacleTime)�@//���݂̎��Ԃ����̏�Q���������Ԃ𒴂��Ă��邩���m�F���Ăяo��
        {
            GenerateObstacle();�@//��Q���̐���
            ScheduleNextObstacle();�@//���̐������Ԃ̌v�Z
        }

        RemovePassedObstacles(); // �v���C���[���ʂ�߂�����Q�����폜

        if (timer >= 95f && !isGoalGenerated) // �^�C����95�ɒB�����Ƃ�
        {
            GenerateGoalStage(); // �S�[���X�e�[�W�𐶐�
            isGoalGenerated = true; // �S�[�����������ꂽ���Ƃ��L�^
        }

    }
    void StageManager(int maps)
    {
        if (maps <= StageIndex)�@//���łɑ���Ă�Ƃ��Ԃ�
        {
            return;
        }

        for (int i = StageIndex + 1; i <= maps; i++)//�w�肵���X�e�[�W�܂ō쐬����
        {
            GameObject stage = MakeStage(i);
            StageList.Add(stage);
        }

        while (StageList.Count > aheadStage + 2)//�Â��X�e�[�W���폜����
        {
            DestroyStage();
        }

        StageIndex = maps;
    }

    GameObject MakeStage(int index)�@//�X�e�[�W�𐶐�����
    {

        int nextStage = Random.Range(0, stagenum.Length);�@//�X�e�[�W�v���n�u���烉���_���ɑI��

        GameObject stageObject = (GameObject)Instantiate(stagenum[nextStage], new Vector3(0, -0.5f, index * StageSize + 40 + 0.05f), Quaternion.identity);

        return stageObject;
    }

void DestroyStage()�@//�X�e�[�W���폜����
    {
    if (StageList[0] != StageList[FirstStageIndex])
    {
        GameObject oldStage = StageList[0];
        StageList.RemoveAt(0);
        Destroy(oldStage); // �C���X�^���X���폜
    }
}

void GenerateObstacle() // ��Q���𐶐�����
{
    int xPos = xPositions[Random.Range(0, xPositions.Length)];�@//�R��x���W���烉���_����
        int obstacleIndex = Random.Range(0, obstacles.Length);�@//��Q���v���n�u���烉���_����

        Vector3 spawnPosition = new Vector3(xPos, -0.5f, Target.position.z + obstacleDistance); // �v���C���[�����苗����ɐ���
    GameObject obstacle = Instantiate(obstacles[obstacleIndex], spawnPosition, Quaternion.identity);

    ObstacleList.Add(obstacle); // ��Q�������X�g�ɒǉ�
}
void ScheduleNextObstacle() // ���̏�Q�������̃^�C�~���O��ݒ�
{
    nextObstacleTime = Time.time + Random.Range(minObstacleInterval, maxObstacleInterval);
}

void RemovePassedObstacles() // �v���C���[���ʂ�߂�����Q�����폜
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

void GenerateGoalStage()�@//�S�[�������X�g�ɒǉ�����
    {
    Vector3 goalPosition = new Vector3(0, -0.5f, StageIndex * StageSize + 120 + 0.05f); //�S�[���̃v���n�u�̃T�C�Y�ɂ����120��40+�T�C�Y�ɕύX����
    GameObject goalStage = Instantiate(goalPrefab, goalPosition, Quaternion.identity);
    StageList.Add(goalStage); // �S�[���X�e�[�W�����X�g�ɒǉ�

}

// ��Q�������Ԋu�����Ԍo�߂ŒZ������
void DecreaseObstacleIntervalOverTime()
{
    // �o�ߎ��Ԃɉ����Đ����Ԋu��i�K�I�ɒZ������
    if (timer >= 0f && timer < 30f)
    {
        minObstacleInterval = 3f;
        maxObstacleInterval = 5f;
    }
    else if (timer >= 30f && timer < 60f)
    {
        minObstacleInterval = 1.5f;
        maxObstacleInterval = 3f;
    }
    else if (timer >= 60f && timer < 90f)
    {
        minObstacleInterval = 1f;
        maxObstacleInterval = 2f;
    }
    else if (timer >= 90f)
    {
        minObstacleInterval = 0.5f;
        maxObstacleInterval = 1.2f;
    }
}

}