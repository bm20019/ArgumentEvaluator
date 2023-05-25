namespace ArgumentEvaluator;
public class CommandLineParser
{
    private string NameApp {get;set;}
    private string versionApp {get;set;}
    private List<string> arguments { get; set; }
    private List<string> argumentsSmall { get; set; }
    private List<Type> ArgType { get; set; }
    private List<string> Descripcion {get;set;}
    private char flagsChar { get; set; }
    private List<Command> Commands {get;set;}
    public CommandLineParser(string NameApp, string VersionApp)
    {
        arguments = new List<string>();
        argumentsSmall = new List<string>();
        ArgType = new List<Type>();
        Descripcion = new List<string>();
        flagsChar = '-';
        this.NameApp = NameApp;
        this.versionApp = VersionApp;
    }
    public void Add(string argument, Type tipo, string descripcion)
    {
        arguments.Add(argument);
        ArgType.Add(tipo);
        argumentsSmall.Add("");
        Descripcion.Add(descripcion);
    }

    public void Add(string argument, string argSmall,string descripcion, Type tipo)
    {
        arguments.Add(argument);
        argumentsSmall.Add(argSmall);
        ArgType.Add(tipo);
        Descripcion.Add(descripcion);
    }
    public void SetFlagsChar(char flagsChar)
    {
        this.flagsChar = flagsChar;
    }
    public void SetVersion(string version){
        this.versionApp = version;
    }
    public void SetNameApplication(string name){
        this.NameApp = name;
    }
    public void Parse(string[] args)
    {
        List<Command> cmds = new List<Command>();

        if (args.Length == 0)
        {
            PrintHelp();
            throw new ArgumentException("No se especificó ningún comando.");
        }
        List<string> listaArgs = args.ToList();
        for (int i = 0; i < listaArgs.Count; i++)
        {
            string item = listaArgs[i];

            if (item.StartsWith(flagsChar))
            {
                if (arguments.Contains(item) || argumentsSmall.Contains(item))
                {
                    int pos = arguments.IndexOf(item);
                    if (pos == -1)
                        pos = argumentsSmall.IndexOf(item);

                    int contador = i + 1;

                    if (pos > -1)
                    {
                        if (ArgType[pos] == Type.Array)
                        {
                            Command cmd = new Command();
                            List<object> parameters = new List<object>();
                            while (!listaArgs[contador].StartsWith(flagsChar))
                            {
                                string value = listaArgs[contador];
                                parameters.Add(value);
                                contador++;
                            }
                            i = contador - 1;
                            cmd.Add(item, parameters);
                            cmds.Add(cmd);
                        }
                        else if (ArgType[pos] == Type.Single)
                        {
                            if (contador >= listaArgs.Count)
                                throw new Exception(String.Format("El argumento: {0} requiere por lo menos un parametro.", item));

                            string value = listaArgs[contador];
                            if (value.StartsWith("-"))
                                throw new Exception(String.Format("El argumento: {0} requiere por lo menos un parametro.", item));
                            i = contador;
                            Command cmd = new Command(item, new List<object>() { value });
                            cmds.Add(cmd);
                        }
                        else if (ArgType[pos] == Type.Boolean)
                        {
                            cmds.Add(new Command(item, new List<object>() { true }));
                            i = contador;
                        }
                    }
                }
                else
                {
                    throw new Exception($"Arg: {item}. El argumento no es reconocible");
                }
            }
        }
        this.Commands = cmds;
    }

    public void PrintHelp(){
        Console.WriteLine("{0} {1}", this.NameApp, this.versionApp);
        Console.WriteLine(string.Format("Uso: {0} [OPCIONES]\n", this.NameApp, this.versionApp));
        for (int i = 0; i < arguments.Count; i++)
        {
            Console.WriteLine("{0}, {1} \t\t{2}\n",arguments[i],argumentsSmall[i],Descripcion[i]);
        }
        return;
    }

    public Command? GetCommand(string name)
    {
        return this.Commands.Find(e => e.GetName() == name);
    }
}