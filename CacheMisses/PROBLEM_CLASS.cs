using System.Collections.Generic;


namespace Problem
{
    // *****************************************
    // DON'T CHANGE CLASS OR FUNCTION NAME
    // YOU CAN ADD FUNCTIONS IF YOU NEED TO
    // *****************************************
    public static class PROBLEM_CLASS
    {
        #region YOUR CODE IS HERE 

        enum SOLUTION_TYPE { NAIVE, EFFICIENT };
        static SOLUTION_TYPE solType = SOLUTION_TYPE.EFFICIENT;

        //Your Code is Here:
        //==================
        /// <summary>
        /// Implements the Off-line Caching Algorithm to minimize cache misses.
        /// The function processes a sequence of memory requests while managing a cache of limited size.
        /// </summary>
        /// <param name="cacheSize">The maximum number of elements that can be stored in the cache.</param>
        /// <param name="requests">An array representing the sequence of memory requests.</param>
        /// <returns>The Min number of cache misses after processing the entire request sequence.</returns>
        static public int RequiredFunction(int cacheSize, string[] requests)
        {
            if (cacheSize == 0)
            {
                return requests.Length;
            }

            Dictionary<string, List<int>> occurrences = new Dictionary<string, List<int>>();
            for (int i = 0; i < requests.Length; i++)
            {
                string req = requests[i];
                if (!occurrences.ContainsKey(req))
                {
                    occurrences[req] = new List<int>();
                }
                occurrences[req].Add(i);
            }

            HashSet<string> cache = new HashSet<string>();
            int misses = 0;

            for (int i = 0; i < requests.Length; i++)
            {
                string req = requests[i];
                if (cache.Contains(req))
                {
                    continue;
                }

                misses++;
                if (cache.Count < cacheSize)
                {
                    cache.Add(req);
                }
                else
                {
                    string evictElement = null;
                    int farthestNextUse = -1;

                    foreach (string element in cache)
                    {
                        List<int> indices = occurrences[element];
                        int pos = indices.BinarySearch(i);
                        if (pos < 0)
                        {
                            pos = ~pos;
                        }
                        if (pos >= indices.Count)
                        {
                            evictElement = element;
                            break;
                        }
                        else
                        {
                            int nextUse = indices[pos];
                            if (nextUse > farthestNextUse)
                            {
                                farthestNextUse = nextUse;
                                evictElement = element;
                            }
                        }
                    }

                    cache.Remove(evictElement);
                    cache.Add(req);
                }
            }

            return misses;
        }
        
        #endregion
    }
}
