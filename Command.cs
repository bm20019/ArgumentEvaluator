namespace ArgumentEvaluator;

public class Command
{
    private string Name { get; set; }
    private List<object> Parameters { get; set; }

    public Command(){}

    public Command(string name, List<object> parameters)
    {
        Name = name;
        Parameters = parameters;
    }

    public void Add(string name, List<object> parameters){
        this.Name = name;
        this.Parameters =parameters;
    }

    public string GetName(){
        return Name;
    }
    public List<object> GetParameters(){
        return this.Parameters;
    }

    public override string ToString(){

        string Title = string.Format("Name: {0}\n", Name), content="Parameters:\n";
        foreach (object item in Parameters)
        {
            content+=string.Format("\t{0}\n",item);
        }
        return Title + content;
    }
}