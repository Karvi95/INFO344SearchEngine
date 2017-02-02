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

            // Default behavior is to add words to the list as soon as possible.
            current.partialWords.Add(pageTitle);


            // But should potentially build standard Trie if capacity is exceeded.
            if (current.partialWords.Count > _capacity || current.children != null)
            {

                // Begin constructing standard trie out of all the previously inserted words
                // in leaf node's list.
                foreach (string s in current.partialWords)
                {

                    // Obtain first letter and initialize a node to potentially associate
                    // said letter to node.
                    char letter = s[0];
                    TrieNode node;

                    // Create Dictionary if none exists.
                    if (current.children == null)
                    {
                        current.children = new Dictionary<char, TrieNode>();

                    }

                    // If dictionary exists but letter is not a key,
                    // make a new node an associate.
                    if (current.children != null && !(current.children.ContainsKey(letter)))
                    {
                        node = new TrieNode();
                        current.children.Add(letter, node);
                    }

                    // Otherwise, traverse to node where letter exists.
                    else
                    {
                        node = current.children[letter];
                    }

                    // Cannot substring a string of length one; will receive C#'s
                    // version of OutOfBounds Exception, so only rebalance when string is
                    // sufficiently long.
                    if (s.Length == 1)
                    {
                        node.isTerminalChar = true;
                    }
                    if (s.Length >= 2)
                    {
                        rebalance(s.Substring(1, s.Length - 1), node);
                    }

                }

                // Clear all words from this level because they've been inserted as nodes.
                current.partialWords.Clear();
            }

        }

        public List<string> getPrefix(string pageTitle)
        {
            // Create pointer to node and empty list.
            TrieNode current = root;
            string potentialPrefix = "";
            List<string> titlesInTrie = new List<string>();

            if (current.partialWords.Count > 0)
            {
                foreach (string inList in root.partialWords)
                {
                    if (inList.StartsWith(pageTitle) && titlesInTrie.Count != 10)
                    {
                        titlesInTrie.Add(inList);
                    }
                }
                return titlesInTrie;
            }

            // Immediately return an empty list if input was an empty string. 
            if (pageTitle == "")
            {
                return titlesInTrie;
            }

            // For as many characters there are in the string,
            for (int index = 0; index < pageTitle.Length; index++)
            {
                char letter = pageTitle[index];

                // traverse input letter-by-letter as long as the current child contains that letter.
                if (current.children.ContainsKey(letter))
                {
                    current = current.children[letter];
                    potentialPrefix += letter;

                    // and only if list of strings at this node is empty,
                    if (!current.partialWords.Any())
                    {
                        continue;
                    }
                    else
                    {
                        titlesInTrie = linearSearch(potentialPrefix, current, titlesInTrie, pageTitle);

                        if (current.children == null)
                        {
                            return titlesInTrie;
                        }
                    }
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

        public List<string> linearSearch(string prefix, TrieNode current, List<string> titlesInTrie, string pageTitle)
        {

            // Iterate through strings in partial words list
            foreach (string inList in current.partialWords)
            {
                if ((prefix + inList).StartsWith(pageTitle))
                {
                    titlesInTrie.Add(prefix + inList);

                    if (titlesInTrie.Count == 10)
                    {
                        break;
                    }
                }
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
                    if (titlesInTrie.Count < 10)
                    {
                        titlesInTrie.Add(prefix);
                    }
                }

                if (prefixEnd.partialWords.Count > 0)
                {
                    titlesInTrie = linearSearch(prefix, prefixEnd, titlesInTrie, prefix);

                    if (titlesInTrie.Count == 10)
                    {
                        return titlesInTrie;
                    }
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