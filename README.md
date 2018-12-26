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
