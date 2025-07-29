using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonMsgArrayData
{
    public string requestCode;
    public List<object> data;
    public JsonMsgArrayData() { }

    public JsonMsgArrayData(string code, List<object> @object)
    {
        this.requestCode = code;
        this.data = @object;

    }
    
}

