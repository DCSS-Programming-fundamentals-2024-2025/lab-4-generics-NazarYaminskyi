using generics.Interfaces;
class Program
{
    static void Main()
    {
        Faculty fpm = new Faculty();

        for(int i = 1; i < 4; i++)
        {
            Group g = new Group(i, "kp4" + i);
            for(int j = 1; j < 6; j++)
            {
                Student s = new Student(j, "John" + i + "." + j);
                g.AddStudent(s);
            }
            fpm.AddGroup(g);
        }

        IEnumerable<Student> group = fpm.GetGroup(1).GetAllStudents();

        foreach(Student s in group)
        {
            Console.WriteLine(s.Name);
        }

        IReadOnlyRepository<Student,int> studRepo = new InMemoryRepository<Student,int>();
        IReadOnlyRepository<Person,int>  persRepo = studRepo; 
    }
}

class Person { public int Id; public string Name;}

class Student : Person
{
    public Student() {} 
    public Student(int _Id, string _Name)
    {
        Id = _Id;
        Name = _Name;
    }
    public void SayName()
    {
        Console.WriteLine(Name);
    }

    public void SubmitWork()
    {

    }
}

class Teacher : Person
{
    public void GradeStudent()
    {

    }

    public void ExpelStudent()
    {

    }

    public void ShowPresentStudents()
    {

    }
}

class Group
{
    public int Id;
    public string Name;

    public Group() {} 
    public Group(int _Id, string _Name)
    {
        Id = _Id;
        Name = _Name;
    }
    

    IRepository<Student,int> _students = new InMemoryRepository<Student,int>();
    public void AddStudent(Student s)
    {
        _students.Add(s.Id, s);
    }

    public void RemoveStudent(int studentId)
    {
        _students.Remove(studentId);
    }

    public IEnumerable<Student> GetAllStudents()
    {
        return _students.GetAll();
    }

    public Student FindStudent(int studentId)
    {
        return _students.Get(studentId);
    }
}

class Faculty
{
    public int Id;
    public string Name;
    IRepository<Group,int> _groups = new InMemoryRepository<Group,int>();
    public void AddGroup(Group g)
    {
        _groups.Add(g.Id, g);
    }

    public void RemoveGroup(int id)
    {
        _groups.Remove(id);
    }

    public IEnumerable<Group> GetAllGroups()
    {
        return _groups.GetAll();
    }

    public Group GetGroup(int id)
    {
        return _groups.Get(id);
    }
}

class InMemoryRepository<TEntity,TKey> : IRepository<TEntity,TKey>
where TEntity : class, new()
where TKey    : struct
{
    private readonly Dictionary<TKey, TEntity> _storage = new();

    public void Add(TKey id, TEntity entity)
    {
        if (_storage.ContainsKey(id)) throw new ArgumentException($"An entity with the same key already exists: {id}");
            
        _storage.Add(id, entity);
    }

    public TEntity Get(TKey id)
    {
       return _storage.TryGetValue(id, out var entity) ? entity : null;;
    }

    public IEnumerable<TEntity> GetAll()
    {
        return _storage.Values;
    }

    public void Remove(TKey id)
    {
        _storage.Remove(id);
    }
}