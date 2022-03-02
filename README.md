# GamepadCmd

## Usage
```
GamepadCmd.exe "[parameter file path (.txt)]"
```

## Parameter file path
1 parameter / line, a parameter is represented as follows :
```
[Pattern 1];[Duration 1];[Command 1]
[Pattern 2];[Duration 2];[Command 2]
...
```

With :
* ***Pattern*** : button or combination of buttons pressed at the same time required for matching the pattern. Each buttons must be separated by comma
* ***Duration*** : Pressed buttons duration (in milliseconds) required for matching the pattern
* ***Command*** : Command to execute for this pattern
 
## Examples
Press "A" for 1000 ms will execute the command "calc.exe"
```
A;1000;calc.exe
```
Press "Start" + "Back" for 3000 ms will execute the command "taskkill /F /IM win32calc.exe"
```
Start,Back;3000;taskkill /F /IM win32calc.exe
```

## Supported buttons
Only tested with xbox360 controller
```
Up
Down
Left
Right
Start
Back
LS
RS
LB
RB
A
B
X
Y
```