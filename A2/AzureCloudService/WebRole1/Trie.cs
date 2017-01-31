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

        public string rebalance(string pageTitle, TrieNode current)
        {
            current.partialWords.Add(pageTitle);
            //if passed in word's length is 1, then mark node as end of word
            if (pageTitle.Length == 1)
            {
                current.isTerminalChar = true;
                return "done";
            }

            if (current.partialWords.Count <= _capacity)
            {
                return "done";
            }
            else
            {
                //List<String> tempList = current.HybridList;
                //current.HybridList.Clear();
                foreach (string s in current.partialWords)
                {
                    //get first letter in item
                    char letter = s[0];
                    TrieNode node;

                    //check that first letter is in the dictionary key
                    if (current.children.ContainsKey(letter))
                    {
                        //if it does, pass that word into that node
                        node = current.children[letter];
                        //return rearrange(node, tempWord);
                    }
                    else
                    {
                        //if it doesn't, create a new node with that letter and add the rest of the letters into the hybrid
                        node = new TrieNode();
                        current.children.Add(letter, node);
                        //String test = tempWord.Substring(1, tempWord.Length - 1);
                    }
                    if (s.Length != 1)
                    {
                        rebalance(s.Substring(1, s.Length - 1), node);
                    }
                }
                current.partialWords.Clear();
                return "done";
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