using System;
using System.Collections.Generic;
using System.Text;

namespace CbrHackaton.Models
{
    public class StatisticsModel
    {
        public int AnswerId { get; set; }
        public double Percentage { get; set; }
        public double PercentageSto => Percentage * 100;
        public string Info => $@"За вопрос '{Name}' проголосовало {Math.Round(PercentageSto,2)}% человек";
        public string Name { get; set; }
    }
}
