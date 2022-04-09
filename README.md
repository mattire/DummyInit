# DummyInit
C# code. Just very simple copy paste method to create dummy simple initialization for jsonlike class structure, with properties. See Program.cs code file inside DummyInit folder. Uses reflection to discover props.

Usage: Call Process method in console program like:

Process(typeof(Root2));

Output is something like:

var myRoot2 = new Root2() {
  glossary = new Glossary()
  {
    title = "blaa",
    GlossDiv = new GlossDiv()
    {
      title = "blaa",
      GlossList = new GlossList()
      {
...


List of List properties not supported.

