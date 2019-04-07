using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
public class DetailedCtrl : MonoBehaviour
{
    public Text Title;
    public Text Titles;
    public RawImage Image;
    public Text Average;
    public Text Time;
    public Text Type;
    public Text longTime;
    public Transform directors;
    public Transform casts;
    public string path;
    FileInfo file;
    public string URL;
    public VideoPlayer video;
    public List<GameObject> OBJList;
    public Image VideoBG;
    public List<Sprite> VideoBGT;
    public void Directors(string Url, string Name, string directors_Alt)
    {
        GameObject dir = (GameObject)Instantiate(Resources.Load("Pre"));
        dir.transform.parent = directors;
        PreManager pre = dir.GetComponent<PreManager>();
        pre.Nmae.text = Name;
        pre.alt_http = directors_Alt;
        pre.but_alt.onClick.RemoveAllListeners();
        pre.but_alt.onClick.AddListener(pre.AddOnclick);
        StartCoroutine(pre.DownImag(Url));
    }
    public void Casts(string Url, string Name, string casts_Alt)
    {
        GameObject dir = (GameObject)Instantiate(Resources.Load("Pre"));
        dir.transform.parent = casts;
        PreManager pre = dir.GetComponent<PreManager>();
        pre.Nmae.text = Name;
        pre.alt_http = casts_Alt;
        pre.but_alt.onClick.RemoveAllListeners();
        pre.but_alt.onClick.AddListener(pre.AddOnclick);
        StartCoroutine(pre.DownImag(Url));
    }
    private void OnDisable()
    {
        for (int i = 0; i < directors.childCount; i++)
        {
            Destroy(directors.GetChild(i).gameObject);
        }


        for (int i = 0; i < casts.childCount; i++)
        {
            Destroy(casts.GetChild(i).gameObject);
        }
        Title.text = "";
        Titles.text = "";
        Average.text = "";
        Time.text = "";
        Type.text = "";
        longTime.text = "";
        for (int i = 0; i < OBJList.Count; i++)
        {
            OBJList[i].SetActive(true);
        }
    }
    public void AddOnclick()
    {
        Image.gameObject.GetComponent<Button>().onClick.AddListener(Open);
    }
    public void Open()
    {
        Application.OpenURL(URL);
    }
    public IEnumerator DownImag(string Url, int numType)
    {
        if (numType == 0)
        {
            for (int i = 0; i < OBJList.Count; i++)
            {
                OBJList[i].SetActive(false);
            }
        }
        string Name = Url.Split('/')[Url.Split('/').Length - 1];
        path = UIChange.instance.Path;
        string file_SaveUrl = path + "/" + Name;
        file = new FileInfo(file_SaveUrl);
        if (File.Exists(file_SaveUrl))//判断一下本地是否有了 如果有就不需下载
        {
            print(file_SaveUrl + "===>> 已经存在不在下载！");
            StartCoroutine(LoadImag(file_SaveUrl));
        }
        else
        {
            WWW www = new WWW(Url);
            yield return www;
            if (www.isDone)
            {
                byte[] bytes = www.bytes;
                File.WriteAllBytes(file_SaveUrl, bytes);
                StartCoroutine(LoadImag(file_SaveUrl));
            }
        }
    }

    public IEnumerator LoadImag(string imgPath)
    {
        WWW www = new WWW("file://" + imgPath);
        yield return www;
        if (www.isDone)
        {
            Texture2D texture = www.texture;
            Image.texture = texture;
        }
    }
    bool IS_Click;
    public void VideoButton()
    {
        if (!IS_Click)
        {
            video.Play();
            IS_Click = true;
            VideoBG.gameObject.SetActive(false);
        }
        else
        {
            video.Pause();
            IS_Click = false;
            VideoBG.sprite = VideoBGT[1];
            VideoBG.gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        IS_Click = false;
        VideoBG.gameObject.SetActive(true);
        VideoBG.sprite = VideoBGT[0];
    }
}
