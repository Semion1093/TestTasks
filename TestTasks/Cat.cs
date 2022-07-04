namespace TestTasks
{
    public abstract class Cat
    {
        public Cat(int age, string name, int tailCount)
        {
            Age = age;
            Name = name;
            TailCount = tailCount;
        }

        public int Age { get; set; }
        public string Name { get; set; }
        public int TailCount { get; set; }
    }
}
