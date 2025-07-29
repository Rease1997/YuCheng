using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RPG.Sockets
{

    public class Message
    {
        private byte[] readBytes = new byte[4096];
        private int startIndex = 0;
        public int StartIndex
        {
            get { return startIndex; }

        }
        public int RemainCount()
        {
            return readBytes.Length - startIndex;
        }
        public byte[] ReadBytes
        {
            get { return readBytes; }
            set { readBytes = value; }
        }

        /// <summary>
        /// Êý¾Ý½âÎö
        /// </summary>
        /// <param name="dataCount"></param>
        /// <param name="processDataCallBack"></param>
        public void ReadMsg(int dataCount, Action<string> processDataCallBack)
        {
            startIndex += dataCount;
            Debug.Log("ReadMsg startIndex:" + startIndex+ "  dataCount:" + dataCount);
            string msg = System.Text.Encoding.UTF8.GetString(readBytes, 0, startIndex);
            Debug.Log("ReadMsg msg:" + msg);

            string[] msgArray = msg.Split('|');
            Debug.Log("ReadMsg msgArray:" + msgArray.Length);

            int msgCount = msgArray.Length - 1;
            Debug.Log("ReadMsg msgCount:" + msgCount);

            for (int i = 0; i < msgCount; i++)
            {
                processDataCallBack(msgArray[i]);                
            }
            if (msg.LastIndexOf("|") != msgCount)
            {
                byte[] remainCount = System.Text.Encoding.UTF8.GetBytes(msgArray[msgArray.Length-1]);
                startIndex = remainCount.Length;
                return;
            }
            startIndex = 0;
        }

       
    }
}