using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureConsole
{
    class Trie
    {
        public TrieNode root { get; private set; }

        public Trie()
        {
            root = new TrieNode();
        }

        public void insert(string pageTitle)
        {
            TrieNode current = root;
            for (int i = 0; i < pageTitle.Length; i++)
            {
                if (current.children == null)
                {
                    current.children = new Dictionary<char, TrieNode>();
                }

                char letter = pageTitle[i];

                if (!current.children.ContainsKey(letter))
                {
                    current.children.Add(letter, new TrieNode());
                }

                current = current.children[letter];

                if (i == pageTitle.Length - 1)
                {
                    current.isTerminalChar = true;
                }
            }
        }
        
        public List<string> getPrefix(string pageTitle)
        {
            
            TrieNode current = root;
            List<string> titlesInTrie = new List<string>();

            if (pageTitle == "")
            {
                return titlesInTrie;
            }

//          string result = "";

            int index = 0;
            char letter = pageTitle[index];

            while ( index < pageTitle.Length && current.children.ContainsKey(letter))
            {
                current = current.children[letter];
//              result += letter;

                index++;
                if (index < pageTitle.Length) {
                    letter = pageTitle[index];
                } else
                {
                    break;
                }
            }

//          getResults(prefix, current, titlesInTrie, 10);
            return titlesInTrie;
        }
        public List<string> traverseTrie(string prefix, TrieNode prefixEnd, List<string> titlesInTrie)
        {
            if (titlesInTrie.Count == 10)
            {
                return titlesInTrie;
            } else
            {
                if (prefixEnd.isTerminalChar == true)
                {
                    titlesInTrie.Add(prefix);
                }
            }
            return null;
        }
    }
}