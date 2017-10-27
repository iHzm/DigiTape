using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class mainController : MonoBehaviour {

	Camera cam;
	public GameObject _cube;

	Vector3 marker1;
	Vector3 marker2;
	bool marker1Placed;
	bool marker2Placed;

	void Start () {
		cam = Camera.main;
		Reset();
	}

	void Update ()
	{

		if (marker1Placed == false) {
			
			placeObject(1);
		} else if (marker2Placed == false) {
			placeObject(2);
		} else {
			float inchesDistance = Vector3.Distance(marker1, marker2) * 39.3701f;
			_ShowAndroidToastMessage(inchesDistance.ToString());
		}
	}

	void Reset(){
		marker1 = Vector3.zero;
		marker2 = Vector3.zero;
		marker1Placed = false;
		marker2Placed = false;
	}
	private static void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
                        message, 0);
                    toastObject.Call("show");
                }));
            }
        }
    public void placeObject (int mode)
	{
		Touch touch;
		TrackableHit hit;
		TrackableHitFlag raycastfilter = TrackableHitFlag.PlaneWithinInfinity;
		if (Input.touchCount > 0) {
			touch = Input.GetTouch (0);

			if (touch.phase != TouchPhase.Began) { //for user experience
				return;
			}

			if (Session.Raycast (cam.ScreenPointToRay (touch.position), raycastfilter, out hit)) {
				Anchor anc = Session.CreateAnchor (hit.Point, Quaternion.identity);
				var cubemarker = Instantiate (_cube, hit.Point, Quaternion.identity, anc.transform);
				if (mode == 1) {
					marker1 = cubemarker.transform.position;
					marker1Placed = true;
				} else if (mode == 2) {
					marker2 = cubemarker.transform.position;
					marker2Placed = true;
				}
			}
		}
    }
}
