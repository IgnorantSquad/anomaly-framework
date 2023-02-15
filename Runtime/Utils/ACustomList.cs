using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anomaly.Utils
{
    public class ACustomList<T>
    {
        public T data;
        public ACustomList<T> next = null;
        public bool isHead = false;

        public bool IsNull => ReferenceEquals(data, null);

        public static ACustomList<T> Create(T initData = default(T))
        {
            ACustomList<T> head = new ACustomList<T>();
            head.isHead = true;
            head.data = initData;
            return head;
        }

        public void Add(ACustomList<T> other)
        {
            if (other == null) return;
            var last = this;
            while (last.next != null) last = last.next;
            last.next = other;
            other.isHead = false;
        }

        public void Add(T other)
        {
            ACustomList<T> node = new ACustomList<T>();
            node.data = other;
            Add(node);
        }

        public static void Remove(ACustomList<T> head, ACustomList<T> node)
        {
            if (head == null || node == null) return;

            ACustomList<T> iter = head.next;

            if (ReferenceEquals(head, node))
            {
                head.next.isHead = true;
                head = null;
                return;
            }

            while (iter != null)
            {
                if (ReferenceEquals(iter.next, node))
                {
                    iter.next = node.next;
                    node = null; node.next = null;
                    break;
                }
                iter = iter.next;
            }
        }
    }

}