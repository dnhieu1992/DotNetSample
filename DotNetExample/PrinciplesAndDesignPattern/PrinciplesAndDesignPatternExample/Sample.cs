namespace PrinciplesAndDesignPatternExample
{
    public class Sample : ISample
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string LoadText()
        {
            throw new NotImplementedException();
        }
    }
}
