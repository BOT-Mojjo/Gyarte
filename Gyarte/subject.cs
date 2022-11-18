using System;
using System.Collections.Generic;
using Raylib_cs;

public class subject{
    public int Age { get; set; }
    public string Gender { get; set; }
    public double AvgReactionTime { get; set; }
    public string MiscellaneousNotes { get; set; }
    public List<double> PreReactionTimes { get; set; } //phase 1 reactiontime, 
    public List<double> PostReactionTimes {get; set; } //time given to the subject to react?
    public List<int> Choises { get; set; }
    public List<int> Color { get; set; } //in sets of 2, [0] = first foreground color, [1] = first background color, [n] = foreground color, [n+1] background color for that round.
}
