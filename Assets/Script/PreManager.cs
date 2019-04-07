using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class PreManager : MonoBehaviour
{
    public Text Nmae;
    public Button but_alt;
    public string alt_http;
    public string path;
    FileInfo file;

    public void AddOnclick()
    {
        Application.OpenURL(alt_http);
    }
    public IEnumerator DownImag(string Url)
    {
        string Name = Url.Split('/')[Url.Split('/').Length - 1];
        path = UIChange.instance.Path;
        string file_SaveUrl = path + "/" + Name;
        file = new FileInfo(file_SaveUrl);
        if (File.Exists(file_SaveUrl))//判断一下本地是否有了 如果有就不需下载
        {
            print(file_SaveUrl + "===>> 已经存在不在下载！");
            StartCoroutine(LoadImag(file_SaveUrl));
        }
        else if (Url != "")
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
            GetComponent<RawImage>().texture = texture;
        }
    }
}
