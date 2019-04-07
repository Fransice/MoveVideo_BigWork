using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DownJosn : MonoBehaviour
{
    private string html = "http://api.douban.com/v2/movie/top250?apikey=0df993c66c0c636e29ecbb5344252a4a&count=250";
    void Start()
    {
        StartCoroutine(GetMoveData(html));
    }
    public IEnumerator GetMoveData(string Url)
    {
        WWW www = new WWW(Url);
        yield return www;
        if (www.isDone)
        {
            string JsonText = www.text;
            print("Json获取完成");
            //找到当前路径
            FileInfo file = new FileInfo("C:/Users/13290/Documents/WeChat Files/TOP.Json");
            //判断有没有文件，有则打开文件，，没有创建后打开文件
            StreamWriter sw = file.CreateText();
            //ToJson接口将你的列表类传进去，，并自动转换为string类型
            //将转换好的字符串存进文件，
            sw.WriteLine(JsonText);
            //注意释放资源
            sw.Close();
            sw.Dispose();
			print("写入完成");
        }
    }
}
