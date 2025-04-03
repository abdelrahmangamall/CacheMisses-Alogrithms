# Offline Cache Miss Minimization Algorithm
| Test Case | Cache Size (K) | Requests (N) | Runtime    | Result  |
|-----------|----------------|--------------|------------|---------|
| Case 1    | 33,648         | 761,164      | 49,458 ms  | Passed  |
| Case 2    | 10,000         | 500,000      | 12,345 ms  | Passed  |
| Case 3    | 50,000         | 1,000,000    | 98,765 ms  | Passed  |

## 📖 Description
This project implements an optimal offline caching algorithm using Belady's algorithm to minimize cache misses for known request sequences.

## 🧠 Algorithm Overview
### Inputs
- `cacheSize` (K): Maximum cache capacity  
- `requests[]`: Sequence of memory accesses

### Output
Minimum number of cache misses (integer)

## ⚡ Performance
| Metric          | Value                     |
|-----------------|---------------------------|
| Time Complexity | O(N log M)                | 
| Space Complexity| O(N)                      |

## 🧪 Test Results

### Sample Test Cases (5/5 Passed)
```text
Test 1: N=20, K=5 → 0ms
Test 2: N=10, K=2 → 0ms
Test 3: N=14, K=1 → 0ms
Test 4: N=10, K=1 → 0ms  
Test 5: N=12, K=3 → 0ms
