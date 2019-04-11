using UnityEngine;
using System.Collections;

public class ApplicationManager : MonoBehaviour {

    public GameObject[] panels;

	public void Quit () 
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}

    public void SetPanel(int index) {
        // called by the panel selector buttons
        // first hide all panels
        for (int i = 0; i < panels.Length; i += 1) {
            panels[i].SetActive(false);
        }
        // now make the selected panel visible
        panels[index].SetActive(true);
    } 
}
