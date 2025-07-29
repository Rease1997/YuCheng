using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonMsgData
{
    public string requestCode;
    public object data;
     public JsonMsgData() { }

    public JsonMsgData(string code, object @object)
    {
        this.requestCode = code;
        this.data = @object;

    }
    
}
