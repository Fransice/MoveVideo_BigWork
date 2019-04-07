using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
public class ImgCtrl : MonoBehaviour
{
    //http://api.douban.com/v2/movie/subject/26865690?apikey=0b2bdeda43b5688921839c8ecb20399b&city=%E5%8C%97%E4%BA%AC&client=&udid=
    public Text Title;
    public RawImage Cover;
    public List<string> genres;//类型
    public string average;//评分
    public List<string> casts_img;//演员图片路径
    public List<string> casts_name;//演员名称
    public List<string> casts_alt;//演员详细信息
    public string durations;//时长
    public string year;//上映时间
    public List<string> directors_img;//导演图片路径
    public List<string> directors_name;//导演名字
    public List<string> directors_alt;//导演详细信息
    public string images;
    public string alt;
    public GameObject DetailedData;
    public string id;
    private string VideoData;
    private string VideoURL;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(Click);
    }
    public void Click()
    {
        DetailedData = UIChange.instance.DetailedData;
        DetailedData.SetActive(true);
        DetailedCtrl detailedData = DetailedData.GetComponent<DetailedCtrl>();
        StartCoroutine(GetVideo("http://api.douban.com/v2/movie/subject/" + id + "?apikey=0b2bdeda43b5688921839c8ecb20399b&city=%E5%8C%97%E4%BA%AC&client=&udid=", detailedData));
        detailedData.Title.text = Title.text;
        detailedData.Titles.text = Title.text;
        detailedData.Time.text = year;
        for (int i = 0; i < genres.Count; i++)
        {
            detailedData.Type.text += genres[i] + "  ";
        }
        detailedData.longTime.text = durations;
        detailedData.URL = alt;
        detailedData.Average.text = average;
        detailedData.AddOnclick();
        int numType = 0;
        print(casts_name.Count);
        if (casts_name.Count == 1)
        {
            numType = 0;
        }
        else
        {
            numType = 1;
        }
        StartCoroutine(detailedData.DownImag(images, numType));

        for (int i = 0; i < casts_img.Count; i++)
        {
            detailedData.Casts(casts_img[i], casts_name[i], casts_alt[i]);
        }
        for (int i = 0; i < directors_name.Count; i++)
        {
            detailedData.Directors(directors_img[i], directors_name[i], directors_alt[i]);
        }

    }
    IEnumerator GetVideo(string URL, DetailedCtrl detailedData)
    {
        WWW www = new WWW(URL);
        yield return www;
        if (www.isDone)
        {
            VideoData = www.text;
            JsonVideo(VideoData, detailedData);
        }
    }
    public void JsonVideo(string Data, DetailedCtrl detailedData)
    {
        JsonData jsonData = JsonMapper.ToObject(Data);
        VideoURL = jsonData["trailers"][0]["resource_url"].ToString();
        detailedData.video.url = VideoURL;
    }
}
