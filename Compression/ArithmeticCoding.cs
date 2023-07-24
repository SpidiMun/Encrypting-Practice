using System;
using System.Collections.Generic;
using System.Linq;

namespace Compression
{
    internal struct Segment // Характеризует интервал [Left; Right)
    {
        public double Left { get; set; } // Левая граница интервала [
        public double Right { get; set; } // Правая граница интервала )
    }

    internal class ArithmeticCoding
    {
        public double Algorithm(string originText)
        {
            Dictionary<char, int> symbolsCount = new Dictionary<char, int>();
            foreach (char symbol in originText)
            {
                if (!symbolsCount.ContainsKey(symbol))
                {
                    symbolsCount.Add(symbol, 1);
                }
                else
                {
                    symbolsCount[symbol]++;
                }
            }
            List<double> counter = new List<double>();
            foreach (KeyValuePair<char, int> symbol in symbolsCount)
            {
                counter.Add(Convert.ToDouble(symbol.Value) / Convert.ToDouble(originText.Length));
            }
            return Encode(symbolsCount.Keys.ToArray(), counter.ToArray(), originText);
        }

        private Dictionary<char, Segment> DefineSegments(char[] symbols, double[] probability)
        {
            Dictionary<char, Segment> segment = new Dictionary<char, Segment>();
            double l = 0;
            for (int i = 0; i < symbols.Length; i++)
            {
                segment.Add(symbols[i], new Segment() { Left = l, Right = l + probability[i] });
                l = segment[symbols[i]].Right;
            }
            return segment;
        }

        public double Encode(char[] symbols, double[] probability, string originText)
        {
            Dictionary<char, Segment> segment = DefineSegments(symbols, probability);
            double left = 0,
                right = 1;
            for (int i = 0; i < originText.Length; i++)
            {
                char symb = originText[i];
                double newLeft = left + (right - left) * segment[symb].Left,
                    newRight = left + (right - left) * segment[symb].Right;
                left = newLeft;
                right = newRight;
            }
            return (left + right) / 2;
        }
    }
}
