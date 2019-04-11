using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using UnityEngine.UI;
public class DownImg : MonoBehaviour
{
    private string HotMoveUrl = "https://api.douban.com/v2/movie/in_theaters?apikey=0b2bdeda43b5688921839c8ecb20399b&start=0&count=100";
    private string ComingMoveUrl = "http://api.douban.com/v2/movie/coming?apikey=0df993c66c0c636e29ecbb5344252a4a&count=100";
    private string TopMoveUrl = "http://api.douban.com/v2/movie/top250?apikey=0df993c66c0c636e29ecbb5344252a4a&count=250";
    private string JsonText;
    private string ImgUrl;
    private string ImgName;
    FileInfo file;
    public Transform HotMoveParent;
    public Transform ComingMoveParent;
    public Transform TopMoveParent;
    private string title;
    void Start()
    {
        print(UIChange.instance.Path);
        StartCoroutine(GetMoveData(HotMoveUrl, MoveType.HotMOve));//正在热映
        // GetData();
    }
    public void GetData()
    {
        StartCoroutine(GetMoveData(HotMoveUrl, MoveType.HotMOve));//正在热映
        StartCoroutine(GetMoveData(ComingMoveUrl, MoveType.ComingMove));//即将上映
        StartCoroutine(GetMoveData(TopMoveUrl, MoveType.TopMove));//TOP250
    }
    public IEnumerator GetMoveData(string Url, MoveType moveType)
    {
        WWW www = new WWW(Url);
        yield return www;
        if (www.isDone)
        {
            JsonText = www.text;
            print("Json获取完成");
            Json(JsonText, moveType);
        }
    }
    IEnumerator Downimg(string Url, string name, string title, MoveType moveType, List<string> Genres, string Average, List<string> Casts_img, List<string> Casts_name, List<string> Casts_alt, string Durations, string Year, List<string> Directors_img, List<string> Directors_name, List<string> Directors_alt, string ALt, string images, string id)
    {
        string file_SaveUrl = UIChange.instance.Path + "/" + name;
        file = new FileInfo(file_SaveUrl);
        if (File.Exists(file_SaveUrl))//判断一下本地是否有了 如果有就不需下载
        {
            // print(file_SaveUrl);
            StartCoroutine(LoadImg(file_SaveUrl, title, moveType, Genres, Average, Casts_img, Casts_name, Casts_alt, Durations, Year, Directors_img, Directors_name, Directors_alt, ALt, images, id));
            // print(name + "===>> 已经存在不在下载！");
        }
        else
        {
            WWW www = new WWW(Url);
            yield return www;
            if (www.isDone)
            {
                // print(name + "==>> 下载完成！");
                byte[] bytes = www.bytes;
                File.WriteAllBytes(file_SaveUrl, bytes);
                StartCoroutine(LoadImg(UIChange.instance.Path + @"\" + name, title, moveType, Genres, Average, Casts_img, Casts_name, Casts_alt, Durations, Year, Directors_img, Directors_name, Directors_alt, ALt, images, id));
            }
        }
    }
    public void Json(string Data, MoveType moveType)
    {
        JsonData jsonData = JsonMapper.ToObject(Data);
        switch (moveType)
        {
            case MoveType.ComingMove:
                for (int i = 0; i < jsonData["entries"].Count; i++)
                {
                    List<string> genres = new List<string>();//类型
                    string average = "";//评分
                    List<string> casts_img = new List<string>();//演员图片
                    List<string> casts_name = new List<string>();//演员名称
                    List<string> casts_alt = new List<string>();//演员详细信息
                    string durations = "";//时长
                    string year = "";//上映时间
                    List<string> directors_img = new List<string>();//导演图片
                    List<string> directors_name = new List<string>();//导演名字
                    List<string> directors_alt = new List<string>();//导演详细信息
                    string images = "";
                    string alt = "";
                    string id = "";


                    ImgUrl = jsonData["entries"][i]["images"]["medium"].ToString();
                    title = jsonData["entries"][i]["title"].ToString();
                    ImgName = ImgUrl.Split('/')[ImgUrl.Split('/').Length - 1];

                    id = jsonData["entries"][i]["id"].ToString();

                    //详细信息
                    images = ImgUrl;
                    genres.Add("");
                    average = jsonData["entries"][i]["rating"].ToString();
                    casts_img.Add("");
                    casts_name.Add("");
                    casts_alt.Add("");
                    durations = "";
                    year = jsonData["entries"][i]["pubdate"].ToString();
                    directors_img.Add("");
                    directors_name.Add("");
                    directors_alt.Add("");
                    alt = "";

                    StartCoroutine(Downimg(ImgUrl, ImgName, title, moveType, genres, average, casts_img, casts_name, casts_alt, durations, year, directors_img, directors_name, directors_alt, alt, images, id));
                }
                break;
            case MoveType.HotMOve:
            case MoveType.TopMove:
                for (int i = 0; i < jsonData["subjects"].Count; i++)
                {

                    List<string> genres = new List<string>();//类型
                    string average = "";//评分
                    List<string> casts_img = new List<string>();//演员图片
                    List<string> casts_name = new List<string>();//演员名称
                    List<string> casts_alt = new List<string>();//演员详细信息
                    string durations = "";//时长
                    string year = "";//上映时间
                    List<string> directors_img = new List<string>();//导演图片
                    List<string> directors_name = new List<string>();//导演名字
                    List<string> directors_alt = new List<string>();//导演详细信息
                    string images = "";
                    string alt = "";
                    string id = "";



                    ImgUrl = jsonData["subjects"][i]["images"]["medium"].ToString();
                    title = jsonData["subjects"][i]["title"].ToString();
                    ImgName = ImgUrl.Split('/')[ImgUrl.Split('/').Length - 1];
                    id = jsonData["subjects"][i]["id"].ToString();

                    //详细信息
                    images = ImgUrl;
                    for (int j = 0; j < jsonData["subjects"][i]["genres"].Count; j++)//类型
                    {
                        genres.Add(jsonData["subjects"][i]["genres"][j].ToString());
                    }
                    average = jsonData["subjects"][i]["rating"]["average"].ToString();

                    for (int j = 0; j < jsonData["subjects"][i]["casts"].Count; j++)//演员信息
                    {
                        try
                        {
                            casts_img.Add(jsonData["subjects"][i]["casts"][j]["avatars"]["medium"].ToString());
                        }
                        catch (System.Exception e)
                        {
                            print("medium:" + id);
                        }
                        try
                        {
                            casts_name.Add(jsonData["subjects"][i]["casts"][j]["name"].ToString());
                        }
                        catch (System.Exception e)
                        {
                            print("name:" + id);
                        }
                        try
                        {
                            casts_alt.Add(jsonData["subjects"][i]["casts"][j]["alt"].ToString());
                        }
                        catch (System.Exception e)
                        {
                            print("alt:" + id);
                        }
                    }
                    if (jsonData["subjects"][i]["durations"].Count == 0)
                    {
                        durations = "暂无数据";
                    }
                    else
                    {
                        durations = jsonData["subjects"][i]["durations"][0].ToString();
                    }

                    year = jsonData["subjects"][i]["year"].ToString();

                    for (int j = 0; j < jsonData["subjects"][i]["directors"].Count; j++)//导演信息
                    {
                        directors_img.Add(jsonData["subjects"][i]["directors"][j]["avatars"]["medium"].ToString());
                        directors_name.Add(jsonData["subjects"][i]["directors"][j]["name"].ToString());
                        directors_alt.Add(jsonData["subjects"][i]["directors"][j]["alt"].ToString());
                    }
                    alt = jsonData["subjects"][i]["alt"].ToString();

                    StartCoroutine(Downimg(ImgUrl, ImgName, title, moveType, genres, average, casts_img, casts_name, casts_alt, durations, year, directors_img, directors_name, directors_alt, alt, images, id));

                }
                break;
        }

    }
    public IEnumerator LoadImg(string recordPath, string title, MoveType moveType, List<string> Genres, string average, List<string> Casts_img, List<string> Casts_name, List<string> Casts_alt, string Durations, string Year, List<string> Directors_img, List<string> Directors_name, List<string> Directors_alt, string ALt, string images, string id)
    {
        WWW www = new WWW("file://" + recordPath);
        yield return www;
        if (www.isDone)
        {
            Texture2D texture = www.texture;
            Transform obj = AddImg(moveType);
            ImgCtrl imgCtrl = obj.GetComponent<ImgCtrl>();
            imgCtrl.Cover.texture = texture;
            imgCtrl.Title.text = title;
            imgCtrl.genres = Genres;
            imgCtrl.average = average;
            imgCtrl.casts_img = Casts_img;
            imgCtrl.casts_name = Casts_name;
            imgCtrl.casts_alt = Casts_alt;
            imgCtrl.durations = Durations;
            imgCtrl.year = Year;
            imgCtrl.directors_img = Directors_img;
            imgCtrl.directors_name = Directors_name;
            imgCtrl.directors_alt = Directors_alt;
            imgCtrl.alt = ALt;
            imgCtrl.images = images;
            imgCtrl.id = id;
        }

    }
    public Transform AddImg(MoveType moveType)
    {
        GameObject img = (GameObject)Instantiate(Resources.Load("Image"));
        switch (moveType)
        {
            case MoveType.HotMOve:
                img.transform.parent = HotMoveParent;
                break;
            case MoveType.TopMove:
                img.transform.parent = TopMoveParent;
                break;
            case MoveType.ComingMove:
                img.transform.parent = ComingMoveParent;
                break;

        }
        return img.transform;
    }
    public enum MoveType
    {
        HotMOve,
        ComingMove,
        TopMove
    }
}
