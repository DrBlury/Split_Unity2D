using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {

    public enum State {
        spawning, delay, start
    }

    [System.Serializable]
	public class Wave {
        public string name;
        public GameObject[] enemyPrefabs;
        public int[] howMany;
    }

    public State state;
    public Wave[] waves;
    public GameObject spawnPointsObject;
    public List<Transform> spawnPoints = new List<Transform>();
    int spindex;
    public int waveIndex = 0;
    public float wavedelay;
    public float countdown;
    public float checkTime = 1f;



    void Start() {
        InitializeSpawnPoints();
        countdown = wavedelay;
        state = State.start;
    }


    void Update() {
        if (countdown <= 0) {
            if (state == State.start) {
                StartCoroutine(CreateWave(waves[waveIndex]));
            }
        }
        else {
            countdown -= Time.deltaTime;
        }

        if (state == State.delay) {
            if (checkForEnemies()) {
                return;
            } else {
                WaveCompleted();
            }
        }
    }

    bool checkForEnemies () {
        checkTime -= Time.deltaTime;
        if (checkTime <= 0) {
            checkTime = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null) {
                return false;
            } 
        }
        return true;
    }

    void WaveCompleted () {
        countdown = wavedelay;
        state = State.start;
        Debug.Log(waveIndex);
        if (waveIndex + 1 >= waves.Length) {
            waveIndex = 0;
        } else {
            waveIndex++;
        }
        
    }

    IEnumerator CreateWave (Wave wave) {
        state = State.spawning;
        for (int i = 0; i < wave.enemyPrefabs.Length; i++) {
            CreateEnemy(wave.enemyPrefabs[i], wave.howMany[i]);
        }
        countdown = wavedelay;
        state = State.delay;

        yield return new WaitForSeconds(wavedelay);
    }

    void CreateEnemy (GameObject enemyPrefab, int howMany) {
        for (int i = 0; i < howMany; i++) {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        }
    }

    void OnDrawGizmos() {

        if (spawnPoints.Count > 0) {

            Gizmos.color = Color.red;

            foreach (Transform t in spawnPoints)
                Gizmos.DrawSphere(t.position, 0.5f);

            Gizmos.color = Color.red;
        }
    }

    void InitializeSpawnPoints() {
        Debug.Log("Loaded.");
        Transform[] tem = spawnPointsObject.GetComponentsInChildren<Transform>();
        spindex = 0;
        foreach (Transform t in tem) {
            t.name = "SpawnPoint: " + spindex.ToString();
            spawnPoints.Add(t);
            spindex++;
        }
    }
}
