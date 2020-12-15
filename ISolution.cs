using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Immutable;

namespace AdventOfCode
{
  public interface ISolution {

    public string Name {get;}

    long PartOne(string input);
    long PartTwo(string input);

  }
}

