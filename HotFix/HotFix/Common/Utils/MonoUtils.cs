using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix.Common.Utils
{
    internal class MonoUtils
    {
        public void CreateButton(Transform parent,Action action)
        {
            Button cloneBtn = new GameObject().AddComponent<Button>();
            cloneBtn.gameObject.name = "testBtn";
            cloneBtn.transform.parent = parent;
            cloneBtn.gameObject.AddComponent<Image>();
            cloneBtn.transform.localScale = Vector3.one;
            cloneBtn.onClick.AddListener(() => {
                Debug.Log("点击测试按钮");
                if(action != null)
                {
                    action();
                }
            });
        }
    }
}
