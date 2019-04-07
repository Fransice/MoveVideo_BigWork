using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChange : MonoBehaviour
{
    public static UIChange instance;
    public string Path;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        instance = this;
#if UNITY_ANDROID
        Path = Application.persistentDataPath;
#endif

#if  UNITY_EDITOR
        Path = Application.streamingAssetsPath;

#endif
    }
    public Text Title;
    public Transform HotMoveList;
    public Transform ComingMoveList;
    public Transform TopMoveList;
    public GameObject DetailedData;
    public GameObject QuitUI;

    public void Top()
    {
        Title.text = "电影TOP250";
        TopMoveList.gameObject.SetActive(true);
        ComingMoveList.gameObject.SetActive(false);
        HotMoveList.gameObject.SetActive(false);
    }
    public void Hot()
    {
        Title.text = "正在热映";
        TopMoveList.gameObject.SetActive(false);
        ComingMoveList.gameObject.SetActive(false);
        HotMoveList.gameObject.SetActive(true);
    }
    public void Coming()
    {
        Title.text = "即将上映";
        TopMoveList.gameObject.SetActive(false);
        ComingMoveList.gameObject.SetActive(true);
        HotMoveList.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!DetailedData.activeSelf && !QuitUI.activeSelf)
            {
                QuitUI.SetActive(true);
            }
            else if (!DetailedData.activeSelf && QuitUI.activeSelf)
            {
                QuitUI.SetActive(false);
            }
            else if (DetailedData.activeSelf)
            {
                DetailedData.SetActive(false);
            }
        }
    }
}