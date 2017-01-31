using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1
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
            // Create pointer to node and empty list.
            TrieNode current = root;
            string potentialPrefix = "";
            List<string> titlesInTrie = new List<string>();

            // Immediately return an empty list if input was an empty string. 
            if (pageTitle == "")
            {
                return titlesInTrie;
            }

            // Traverse input letter-by-letter as long as the current child contains that letter
            for (int index = 0; index < pageTitle.Length; index++)
            {
                char letter = pageTitle[index];
                if (current.children.ContainsKey(letter))
                {
                    current = current.children[letter];
                    potentialPrefix += letter;
                }
                else
                {
                    break;
                }
            }

            // Submit node of the ending character in prefix to helper 
            // function along with input and the empty list to actually 
            // traverse the trie and obtain 10 results.
            if (potentialPrefix == pageTitle)
            {
                titlesInTrie = traverseTrie(pageTitle, current, titlesInTrie);
            }
            return titlesInTrie;
        }
        public List<string> traverseTrie(string prefix, TrieNode prefixEnd, List<string> titlesInTrie)
        {
            // Base case; if there are 10 titles return immediately.
            if (titlesInTrie.Count == 10)
            {
                return titlesInTrie;
            }
            else
            {
                // If this is a complete word, need to add title to list.
                if (prefixEnd.isTerminalChar)
                {
                    titlesInTrie.Add(prefix);
                }

                // Just because it's a complete word doesn't mean it can't have children;
                // recurse on all children apending the key to the string and the value as new node.
                if (prefixEnd.children != null)
                {
                    foreach (KeyValuePair<char, TrieNode> child in prefixEnd.children)
                    {
                        titlesInTrie = traverseTrie(prefix + child.Key, child.Value, titlesInTrie);
                    }
                }
                return titlesInTrie;
            }
        }
    }
}