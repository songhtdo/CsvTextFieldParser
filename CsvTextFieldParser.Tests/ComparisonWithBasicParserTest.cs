﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotVisualBasic.FileIO
{
    public class ComparisonWithBasicParserTest
    {
        [TestCase("1234567890,\n", 100)]
        [TestCase("2,\n\r", 100)]
        [TestCase("abcdefgh,\n\r\t ", 100)]
        public void RandomInput(string inputCharsString, int iterations, int seed = 0)
        {
            var inputChars = inputCharsString.ToArray();
            var random = new Random(seed);
            for (var i = 0; i < iterations; i++)
            {
                var inputLength = random.Next(minValue: 1, maxValue: 1000);
                var input = string.Join(string.Empty, Enumerable.Range(0, inputLength).Select(_ => inputChars[random.Next(0, inputChars.Length)]));

                var expected = ParseBasicCsv(input).ToList();
                var actual = ParseFancyCsv(input).ToList();
                CollectionAssert.AreEqual(expected, actual);
            }
        }

        public IEnumerable<string[]> ParseBasicCsv(string input)
        {
            using (var reader = new StringReader(input))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        yield return line.Split(',');
                    }
                }
            }
        }

        public IEnumerable<string[]> ParseFancyCsv(string input)
        {
            using (var parser = new CsvTextFieldParser(new StringReader(input)))
            {
                while (!parser.EndOfData)
                {
                    yield return parser.ReadFields();
                }
            }
        }
    }
}