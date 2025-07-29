using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
    internal class TipsWindow:Window
    {
        int time = 0;
        Text text;
        public override void Awake(object param1 = null, object param2 = null, object param3 = null)
        {
            text = m_Transform.Find("Image/Text").GetComponent<Text>();
        }

        public override void OnShow(object param1 = null, object param2 = null, object param3 = null)
        {
            text.text = param1.ToString();
            time = int.Parse(param2.ToString());
            IEnumeratorTool.instance.StartCoroutineNew(showCM());
        }
        private IEnumerator showCM()
        {
            yield return new WaitForSecondsRealtime(time);
            UIManager.instance.CloseWnd(this);
        }
    }
}
