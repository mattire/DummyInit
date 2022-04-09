// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
using System.Collections.Generic;

namespace DummyInit.Classes2
{

    public class Test2
    {
        public int Prop1 { get; set; }
    }

    public class GlossDef
    {
        public string para { get; set; }
        public List<string> GlossSeeAlso { get; set; }
        public List<int> Test1List { get; set; }
        public List<Test2> Test2List { get; set; }
        //public List<List<int>> Test3List { get; set; }
    }

    public class GlossEntry
    {
        public string ID { get; set; }
        public int IntProp { get; set; }
        public float FloatProp { get; set; }
        public double DoubleProp { get; set; }
        public string SortAs { get; set; }
        public string GlossTerm { get; set; }
        public string Acronym { get; set; }
        public string Abbrev { get; set; }
        public GlossDef GlossDef { get; set; }
        public string GlossSee { get; set; }
    }

    public class GlossList
    {
        public GlossEntry GlossEntry { get; set; }
    }

    public class GlossDiv
    {
        public string title { get; set; }
        public GlossList GlossList { get; set; }
    }

    public class Glossary
    {
        public string title { get; set; }
        public GlossDiv GlossDiv { get; set; }
    }

    public class Root2
    {
        public Glossary glossary { get; set; }
    }

}