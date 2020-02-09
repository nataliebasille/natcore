namespace Natcore_Entityframework
{
    public class TestEntity
    {
        public int Key { get; set; }

        public int IntProp { get; set; }

        public string StringProp { get; set; }

        public TestEnum EnumProp { get; set; }
    }

    public enum TestEnum
    {
        One,
        Two,
        Three
    }
}
