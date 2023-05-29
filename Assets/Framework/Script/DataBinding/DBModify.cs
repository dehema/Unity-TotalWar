using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB
{
    /// <summary>
    /// ���ݸ�������
    /// </summary>
    public enum DBAction
    {
        Init,
        Update,
        Add,
        Remove,
    }

    /// <summary>
    /// ���ݱ䶯
    /// </summary>
    public class DBModify
    {
        /// <summary>
        /// ���ݸ�������
        /// </summary>
        public object value;
        public DBAction action;

        public T Value<T>()
        {
            return (T)this.value;
        }

        public override string ToString()
        {
            return string.Format("action: {0}, value: {1}", action, value);
        }
    }

    public class DBKeyModify : DBModify
    {
        /// <summary>
        ///  ���ݵ� key
        /// </summary>
        public string key;
        /// <summary>
        /// �Ƿ��ǵ�һ��
        /// </summary>
        public bool isFirst;
        /// <summary>
        /// �Ƿ������һ��
        /// </summary>
        public bool isLast;

        public override string ToString()
        {
            return string.Format("action: {0}, key: {1}, value: {2}", action, key, value);
        }
    }
}