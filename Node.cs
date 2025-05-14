using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finalProject
{
    public class Node
    {
        public Node Next { get; set; }
        public object Data { get; set; }

        public Node(object data)
        {
            this.Data = data;
            this.Next = null;
        }
    }
}
