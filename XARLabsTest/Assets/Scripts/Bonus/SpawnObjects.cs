using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SpawnObjects : MonoBehaviour
{
    [SerializeField] protected ARRaycastManager aRRaycastManager;
    [SerializeField] protected GameObject spawnPrefab;

    [SerializeField] protected float scaleSpeed = 0.5f;
    [SerializeField] protected float minScale = 0.5f;
    [SerializeField] protected float maxScale = 3.0f;

    List<ARRaycastHit> aRRaycastHits = new List<ARRaycastHit>();

    private GameObject spawnGameObject;
    private Touch firstTouch, secondTouch;
    private float initialDistance = 0;
    private float currentDistance = 0;
    private bool isScaling = false;
    private Vector3 initialScale;

    protected void Update()
    {
        if (Input.touchCount == 0) return;

        if (Input.touchCount >= 2)
        {
            if (!spawnGameObject) return;

            firstTouch = Input.GetTouch(0);
            secondTouch = Input.GetTouch(1);
            currentDistance = Vector2.Distance(firstTouch.position, secondTouch.position);

            if (firstTouch.phase == TouchPhase.Began || secondTouch.phase == TouchPhase.Began && !isScaling)
            {
                isScaling = true;
                initialDistance = currentDistance;
                initialScale = spawnGameObject.transform.localScale;

            }
            else if (isScaling && (firstTouch.phase == TouchPhase.Moved || secondTouch.phase == TouchPhase.Moved))
            {
                if (Mathf.Abs(currentDistance - initialDistance) > 0.01f)
                {
                    float scaleFactor = currentDistance / initialDistance;
                    Vector3 newScale = initialScale * scaleFactor;

                    newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
                    newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
                    newScale.z = Mathf.Clamp(newScale.z, minScale, maxScale);

                    spawnGameObject.transform.localScale = Vector3.Lerp(spawnGameObject.transform.localScale, newScale, Time.deltaTime * scaleSpeed);
                }
            }
            else if (isScaling)
            {
                isScaling = false;
                initialScale = spawnGameObject.transform.localScale;
            }
            return;
        }

        if (aRRaycastManager.Raycast(Input.GetTouch(0).position, aRRaycastHits, UnityEngine.XR.ARSubsystems.TrackableType.Planes))
        {

            if (spawnGameObject)
            {
                spawnGameObject.transform.position = aRRaycastHits[0].pose.position;
                spawnGameObject.transform.rotation = aRRaycastHits[0].pose.rotation;
                return;
            }

            spawnGameObject = Instantiate(spawnPrefab, aRRaycastHits[0].pose.position, aRRaycastHits[0].pose.rotation);
        }
    }
}