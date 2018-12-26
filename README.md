Random FizzBuzz
==================

An algorithm that uses brute-force to find the seed for `Random()` that lets the following code:


```csharp
private static readonly Random Random = new Random(seed);

public static IEnumerable<string> FizzBuzz() =>
    Enumerable.Range(1, int.MaxValue).Select( i =>
        new[] {"fizz", "buzz", "fizz buzz", i.ToString()}[Random.Next(4)]);
```

generate the longest, valid sequence for the Fizz Buzz problem.


## Result
So far, the best result is a 14-item sequence, generated with the seed `40043843`.

```bash
New record! 4 generated a length of 2
New record! 75 generated a length of 3
New record! 358 generated a length of 4
New record! 1520 generated a length of 5
New record! 7188 generated a length of 6
New record! 9686 generated a length of 8
New record! 41160 generated a length of 9
New record! 322584 generated a length of 11
New record! 35263236 generated a length of 12
New record! 40043843 generated a length of 14
```
