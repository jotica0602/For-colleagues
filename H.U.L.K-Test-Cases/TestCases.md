# Test Cases Inputs

## Valid Expression Inputs

```js
print("Hello World"); 
```

```js
print((((1 + 2) ^ 3) * 4) / 5);  
```

```js
print(sin(2 * PI) ^ 2 + cos(3 * PI / log(4, 64)));,  
```

```js
function tan(x) => sin(x)/cos(x); 
```

```js
print(tan(PI/2));  
```

```js
let x = PI/2 in print(tan(x));  
```

```js
let number = 42, text = \The meaning of life is \ in print(text @ number);  
```

```js
The meaning of life is 42
```

```js
let number = 42 in (let text = \The meaning of life is \ in (print(text @ number))); 
```

```js
print(7 + (let x = 2 in x * x));  
```

```js
let a = 42 in if (a % 2 == 0) print(\Even\) else print(\odd\);  
```

```js
let a = 42 in print(if (a % 2 == 0) \even\ else \odd\);  
```

```js
function fib(n) => if (n > 1) fib(n-1) + fib(n-2) else 1; 
```

```js
let x = 42 in print(x);  
```

```js
fib(5);  
```

```js
print(fib(6));  
```

```js
function fact(n) => if(n>1) n*fact(n-1) else 1;
```

### Outputs

Mathematical Expressions were tested in C#

```js
Hello World
21.6
-1
1.2246467991473532E-16
16331239353195370
16331239353195370
The meaning of life is 42
The meaning of life is 42
11
Even
even
42
8
13
```

## Invalid Expression Inputs

```js
let 14a = 5 in print(14a);              
```

```js
let a = 5 in print(a;                   
```

```js
let a = 5 inn print(a);                 
```

```js
let a = in print(a);                    
```

```js
let a = "hello world" in print(a + 5);  
```

```js
print(fib("hello world"));              
```

```js
print(fib(4,3));                        
```

### Outputs
This are possible error messages.

```js
>!lexical error: "14a" is not a valid token.
>!syntax error: unexpected token: "Semicolon: ;" at index: 8 expected: "RightParenthesis".
>!syntax error: unexpected token: "Identifier: inn" at index: 4 expected: "InKeyWord".
>!syntax error: unexpected token "InKeyWord" after "Equals: =" at index: 3.
>!semantic error: "Addition" cannot be applied between "String" and "Number".
>!semantic error: "GreatherThan" cannot be applied between "String" and "Number".
>!semantic error: function "fib" recieves 1 argument(s), but 2 were given.
```

## Import
Some people may have implemented an "Import" functionality, this is for them.

## Copy the code in a .txt file or whatever and run it.
### Valid Expression Inputs

```js
print("Hello World");
print((((1 + 2) ^ 3) * 4) / 5);
print(sin(2 * PI) ^ 2 + cos(3 * PI / log(4, 64)));
print(sin(PI));
function tan(x) => sin(x)/cos(x);
print(tan(PI/2));
let x = PI/2 in print(tan(x));
let number = 42, text = "The meaning of life is" in print(text @ number);
let number = 42 in (let text = "The meaning of life is" in (print(text @ number)));
print(7 + (let x = 2 in x * x));
let a = 42 in if (a % 2 == 0) print("Even") else print("odd");
let a = 42 in print(if (a % 2 == 0) "even" else "odd");
function fib(n) => if (n > 1) fib(n-1) + fib(n-2) else 1;
let x = 42 in print(x);
fib(5);
print(fib(6));
let x = 3 in print(fib(x+3));
function sum(n) => if(n <= 1) 1 else n + sum(n-1);
sum(100);
function fact(n) => if(n>1) n * fact(n-1) else 1;
fact(10);
let a = (let b = (let c = 30 in c) in b) in a > 2*5 & false;
2^(3+2)^2;
```

### Invalid Expression Inputs

```js
let 14a = 5 in print(14a);
let a = (let b = (let c = 21 in c + 1) in b) in b - 22;
let a = 5 in print(a;
function f(a,2) => 3;
let a = 5 inn print(a);
let a = in print(a);
let a = "hello world" in print(a + 5);
print(fib("hello world"));
print(fib(4,3));
```
