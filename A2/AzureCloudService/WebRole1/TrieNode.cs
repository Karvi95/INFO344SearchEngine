using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebRole1
{
    public class TrieNode
    {
        public Dictionary<char, TrieNode> children { get; set; }
        public bool isTerminalChar { get; set; }
        public List<string> partialWords { get; set; }

        public TrieNode()
        {
            this.children = null;
            this.isTerminalChar = false;
            this.partialWords = new List<string>();
        }
    }
}
