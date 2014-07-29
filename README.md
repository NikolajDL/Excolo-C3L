# Excolo C3L 
### CommandLine Language Library (C3L)
==========


Excolo C3L is a library used to easily add a command console to any C# project. You include the library as a NuGet package, create your own command environment(s), load your environments into the C3L virtual machine and fire up the shell. 

The project is still in its early stages. Here are some planned work:
- Example project.
- Nested-commands.
- Pipelined-commands.
- Environment separator, so that commandnames only needs to be unique to each environment. 
- Better exceptions in the execution-part.
- More default environments - more commands in the system and io environments.
- More unit-tests.
- Clean up messy code.


Installation
==========
_Coming soon_


Tutorial
==========

Here is a short guide to how to use C3L in your own project. More examples can be found in the example project (once it is ready ;-) )

### Create your first environment

First we create an environment. The environment is a collection of commands and is responsible for binding the command methods to commandnames and arguments. 

In this example we create a UserEnvironment, which will bind a command that creates a new user using a IUserService (which is not included in this example).

```CSharp
public class UserEnvironment : BaseEnvironment
{
    private readonly IVirtualMachine _vm;
    private readonly IUserService _userService;
    
    public override string Name
    {
        get { return "user"; }
    }

    public UserEnvironment(IVirtualMachine vm, IUserService userService)
    {
        _vm = vm;
        _userService = userService;
        
        Bind(CreateUser, 
            "createuser email password role", 
            new { role = "guest" },
            new { 
                email = new[] { "e" }, 
                password = new[] { "pass", "p" }, 
                role = new[] { "r" } 
            });
    }

    protected IEnumerable<IArgument> CreateUser(ArgumentValueLookup args)
    {
        string email;
        if(!args["email"].TryGetValue<string>(out email))
            throw new ArgumentException("Expected argument 'email' to be of type 'String'");
        
        string password;
        if(!args["password"].TryGetValue<string>(out password))
            throw new ArgumentException("Expected argument 'password' to be of type 'String'");
        
        string role;
        if(!args["role"].TryGetValue<string>(out role))
            throw new ArgumentException("Expected argument 'role' to be of type 'String'");

        var user = _userService.RegisterUser(email, password, role);

        _vm.Output.WriteLine("Created user with email '" + user.Email  + "' and role '" + user.Role + "'");

        yield break;
    }
}
```

That's it for this environment! Now lets go through the this code. 

First we have the readonly fields with references to the `IVirtualMachine` that uses this environment - this is primarily to enable us to write output to the virtual machine - and to the `IUserService` which is a service that handles all user data-access code (this is not part of this example).

#### Naming the environment
Then we override the abstract property `Name`. This is the name of the environment and must be unique for your application. The names 'system' and 'io' is already reserved. 

#### Binding commands
After overriding the property we define the constructor for our environment. In the constructor we define the binding of our command. If you're familiar with ASP.NET this process is somewhat similar to configuring routes. We bind commands using the `Bind`-method defined in the `BaseEnvironment`. Here is our binding again:

```
Bind(CreateUser, 
    "createuser email password role", 
    new { role = "guest" },
    new { 
        email = new[] { "e" }, 
        password = new[] { "pass", "p" }, 
        role = new[] { "r" } 
    });
```
The first argument is the command-delegate. This is the method that is invoked when a user enters the command in the shell. The command-delegate returns an `IEnumerable<IArgument>` and takes as input an `ArgumentValueLookup`. This is done behind the scenes and isn't something to worry too much about. 

The second argument is the command-template. This is a string that defines the structure of the command. Essentially it is a whitespace-separated list, where the first element is the commandname and the remaining elements are the command arguments. In this case the name of the command is 'createuser' and it expects three arguments 'email', 'password' and 'role'. It is important to note that the commandname should be unique within your application.

The third argument is optional and is the default values for the command arguments. It expects an anonymous object, where each property corresponds to one of the arguments in the command-template. By setting a default value for an argument, you make it optional to the user. In our example above we set the default value of the 'role' argument to be 'guest', thus role is the only optional argument when a user invokes the 'createuser' command.

The last argument is also optional and defines aliases to the commandname and arguments. This also expects an anonymous object, where each property corresponds to one of the argument in the command-template. The value of each argument is a string-array, where each string is an alias for the propertyname. In this example we define single-character aliases for each argument. 

#### Implementing the command
The last part is implementing the command that is invoked. As stated earlier this must must return an `IEnumerable<IArgument>` and take as input an `ArgumentValueLookup`. The rest is pretty much up to you. 

The `ArgumentValueLookup` is a kind of dictionary, that allows the same key to be used for multiple values. The dictionary keys are strings and correspond to the argument names you defined in the command-template in the binding. So for this example the keys in this ArgumentValueLookup would be 'name', 'password' and 'role'. The values of the dictionary is an `IArgument` with contains the value and type of an argument.

In this example command we simply extract and validate the arguments, pass them to our IUserService to register a user and then prints out a message. We finish with `yield break` as we don't really want to return any arguments for this command.

### Using the environment
We have created an environment with out own commands for our virtual machine and now we want to use it. For the purpose of this example we use a windows console project to create a shell where a user can pass in commands. 

```
public class Program
{
    public static void Main()
    {
        StreamReader input = new StreamReader(Console.OpenStandardInput()); ;
        StreamWriter output = new StreamWriter(Console.OpenStandardOutput()); ;
        StreamWriter log = new StreamWriter(new FileStream("log.txt", FileMode.Append)); ;
        
        Shell shell = new Shell(input, output, log);
        
        IUserService service = new UserService();
        IEnvironment userEnvironment = new UserEnvironment(shell.VirtualMachine, service);

        shell.VirtualMachine.LoadEnvironment(userEnvironment);
        shell.Start();
    }
}
```
That's it. When we run this project, we can begin to invoke our commands. Let's explain this code a bit and afterwards show how to invoke commands using C3L. 

First we create some streams for our shell. In this example we simply them based on the System.Console stream, so we can work with the windows console project directly.

Then we create our shell pbejct by passing the streams as arguments. 

After that we create our UserService that is required by our UserEnvironment and we create the UserEnvironment by passing in the shell VirtualMachine and the UserService as constructor arguments. 

In the last two lines we load our user environment into the virtual machine of the shell and start the shell. The shell will then run until it console project is closed (or the exit/quit command from the default system environment is invoked).

### How to invoke commands?
When we startup our console project we are met by the following window:
![](http://i.imgur.com/7EIzqXp.png)

We can then invoke our createuser command using: **createuser email:"fake@ema.il" password:"somepassword" role:"admin"**.

In the following screenshow we have invoked our command. 
![](http://i.imgur.com/SBXqpgI.png)
As you may notice, we've left our the role argument as we defined this to be optional, and we used 'p' instead of 'password' as we defined this as an alias to the password argumentname. 

In fact this command could be invoked in many different ways. We don't even HAVE to specifically write the argument names, as long as we pass the arguments in the order defined in the command-template. The purpose of specifically writing the argument names is that we can write the arguments in any order. 

Here is a list of possible ways to invoke the same command:
```
createuser "email@test.ts" passwordwithoutspecialchars admin
createuser p:thisispass11 role:admin "email@test.ts"
createuser r:admin p:passthisispass11word e:"email@test.ts"
createuser "email@test.ts" thisispass11
....
```

That's it for this tutorial.

Once the example project is ready, it will present more different use-cases, such as the catch-all argument, etc.



License
============
Free for both personal and commercial use.

Copyright (c) 2014 Nikolaj Dam Larsen. Released under the [MITlicense](https://github.com/Excolo/Excolo-C3L/blob/master/LICENSE).
