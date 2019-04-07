using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.IO;
public class MenuCtrl : MonoBehaviour
{
    public DownImg downlmg;
    public DownImg.MoveType moveType;
    public string Url;
    public bool IS_OnceClick;
    public List<TextAsset> textAsset;
    public void Click()
    {
        if (!IS_OnceClick)
        {
            StartDown();
        }
    }
    public void StartDown()
    {
        IS_OnceClick = true;
        StartCoroutine(downlmg.GetMoveData(Url, moveType));
    }
    public void TOP()
    {
        if (!IS_OnceClick)
        {
            IS_OnceClick = true;
            StartCoroutine(TOPMove());
        }
    }
    IEnumerator TOPMove()
    {
        for (int i = 0; i < textAsset.Count; i++)
        {
            downlmg.Json(textAsset[i].text, moveType);
            yield return new WaitForSeconds(0.6f);
        }
    }
}
