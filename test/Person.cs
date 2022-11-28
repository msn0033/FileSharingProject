using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    public class Person
    {
        public List<string>? States { get; set; }
    }
     void PossibleDereferenceNullExamples(string? message)
    {
        Console.WriteLine(message?.Length); // CS8602

        var c = new Person { States = { "Red", "Yellow", "Green" } }; // CS8670
    }
    

}

