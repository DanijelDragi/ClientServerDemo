using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour {
    public GameObject targetPrefab;
    
    public static TargetManager Instance;

    private static Dictionary<int, GameObject> _targets = new Dictionary<int, GameObject>();

    public void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else if (Instance != this) {
            Debug.Log("Instance already exists! Destroying this object!");
            Destroy(this);
        }
    }

    public void SpawnTarget(int id, Vector3 spawn, int clickCapacity) {
        if (_targets.ContainsKey(id)) {
            Debug.Log("Target already exists!");
        }
        else {
            GameObject target = Instantiate(targetPrefab, spawn, Quaternion.identity);
            _targets.Add(id, target);
            Target targetScript = target.GetComponent<Target>();
            targetScript.clickCapacity = clickCapacity;
            targetScript.id = id;
        }
    }

    public void MoveTarget(Vector3 newPosition, int id) {
        StartCoroutine(MoveToPosition(_targets[id].transform, newPosition, Constants.BOX_POSITION_UPDATE_INTERVAL));
    }

    public void DestroyTarget(int id) {
        Destroy(_targets[id]);
        _targets.Remove(id);
    }

    private IEnumerator MoveToPosition(Transform trans, Vector3 position, int timeToMove)
    {
        Vector3 currentPos = trans.position;
        float t = 0f;
        while(t < 1)
        {
            t += (Time.deltaTime * 1000) / timeToMove;
            try {
                trans.position = Vector3.Lerp(currentPos, position, t);
            }
            catch (MissingReferenceException) {
                break;
            }
            yield return null;
        }
    }

    public static void GameOver() {
        foreach (GameObject target in _targets.Values) {
            Destroy(target);
        }
    }
}
