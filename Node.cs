using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finalProject
{
    public class Node
    {
        public Node next { get; set; }
        public object data { get; set; }

        public Node(object data)
        {
            this.data = data;
            this.next = null;
        }
    }
}
