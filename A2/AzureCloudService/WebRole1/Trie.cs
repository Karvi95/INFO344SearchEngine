using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1
{
    class Trie
    {
        public TrieNode root { get; private set; }
        private readonly int _capacity = 20;

        public Trie()
        {
            root = new TrieNode();
        }

        public void insert(string pageTitle)
        {
            if ((pageTitle != null) && (pageTitle.Length != 0))
            {
                rebalance(pageTitle, root);
            }
        }

        public void rebalance(string pageTitle, TrieNode current)
        {
            current.partialWords.Add(pageTitle);
            
            if (pageTitle.Length == 1)
            {
                current.isTerminalChar = true;
            }

            if (current.partialWords.Count > _capacity)
            {
                foreach (string s in current.partialWords)
                {
            
                    char letter = s[0];
                    TrieNode node;

    
                    if (current.children.ContainsKey(letter))
                    {
    
                        node = current.children[letter];
    
                    }
                    else
                    {
    
                        node = new TrieNode();
                        current.children.Add(letter, node);
    
                    }
                    if (s.Length != 1)
                    {
                        rebalance(s.Substring(1, s.Length - 1), node);
                    }
                }
                current.partialWords.Clear();
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