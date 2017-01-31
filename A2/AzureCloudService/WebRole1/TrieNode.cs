﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1
{
    public class TrieNode
    {
        public Dictionary<char, TrieNode> children { get; set; }
        public bool isTerminalChar { get; set; }

        public TrieNode()
        {
            this.children = null;
            this.isTerminalChar = false;
        }
    }
}