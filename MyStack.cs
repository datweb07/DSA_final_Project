using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finalProject
{
    public class MyStack<T>
    {
        private Node top;

        public bool IsEmpty()
        {
            return top == null;
        }

        public void Push(object ele)
        {
            Node n = new Node(ele);
            n.data = ele;
            n.next = top;
            top = n;
        }

        public Node Pop()
        {
            if (top == null)
                return null;
            Node d = top;
            top = top.next;
            return d;
        }

        public object Peek()
        {
            return top.data;
        }

        public int Count()
        {
            int count = 0;
            Node current = top;
            while (current != null)
            {
                count++;
                current = current.next;
            }
            return count;
        }
    }
}
