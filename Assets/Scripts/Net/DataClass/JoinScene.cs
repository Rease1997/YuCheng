using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinScene
{//{"requestCode":"JoinScene","data":{"sceneId":"0","userId":"503356083155509248"}}
    public string sceneId = string.Empty;
    public string userId = string.Empty;
    public string time;

    public JoinScene(string sceneId, string userId,string time=null)
    {
        this.sceneId = sceneId;
        this.userId = userId;
        this.time = time;
    }
}
